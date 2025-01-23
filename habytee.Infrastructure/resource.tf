
resource "hcloud_load_balancer" "habytee_lb" {
  depends_on = [module.talos]
  name = "habytee-lb"
  load_balancer_type = "lb11"
  location = "fsn1"
}

resource "hcloud_load_balancer_service" "load_balancer_service_http" {
  load_balancer_id = hcloud_load_balancer.habytee_lb.id
  protocol         = "tcp"

  listen_port = 80
  destination_port = 30080
}

resource "hcloud_load_balancer_service" "load_balancer_service_https" {
  load_balancer_id = hcloud_load_balancer.habytee_lb.id
  protocol         = "tcp"

  listen_port = 443
  destination_port = 30443
}

resource "hcloud_load_balancer_target" "worker_targets" {
  depends_on = [ hcloud_load_balancer.habytee_lb ]
  for_each          = module.talos.talos_worker_ids
  type              = "server"
  load_balancer_id  = hcloud_load_balancer.habytee_lb.id
  server_id         = each.value
}

resource "cloudflare_record" "root_a" {
  depends_on = [hcloud_load_balancer.habytee_lb]
  zone_id = data.sops_file.secrets.data["domain_zone_id"]
  name    = "@"
  content   = hcloud_load_balancer.habytee_lb.ipv4
  type    = "A"
  proxied = false
  allow_overwrite = true
}

# ipv6 currently not supported by module
# resource "cloudflare_record" "root_aaaa" {
#   depends_on = [hcloud_load_balancer.habytee_lb]
#   zone_id = data.sops_file.secrets.data["domain_zone_id"]
#   name    = "@"
#   content   = hcloud_load_balancer.habytee_lb.ipv6
#   type    = "AAAA"
#   proxied = false
#   allow_overwrite = true
# }

resource "cloudflare_record" "wildcard_a" {
  depends_on = [hcloud_load_balancer.habytee_lb]
  zone_id = data.sops_file.secrets.data["domain_zone_id"]
  name    = "*"
  content   = hcloud_load_balancer.habytee_lb.ipv4
  type    = "A"
  proxied = false
  allow_overwrite = true
}

# ipv6 currently not supported by module
# resource "cloudflare_record" "wildcard_aaaa" {
#   depends_on = [hcloud_load_balancer.habytee_lb]
#   zone_id = data.sops_file.secrets.data["domain_zone_id"]
#   name    = "*"
#   content   = hcloud_load_balancer.habytee_lb.ipv6
#   type    = "AAAA"
#   proxied = false
#   allow_overwrite = true
# }

resource "helm_release" "ingress-nginx" {
  depends_on = [ 
    cloudflare_record.wildcard_a,
    cloudflare_record.root_a
  ]
  name       = "ingress-nginx"
  repository = "https://kubernetes.github.io/ingress-nginx"
  chart      = "ingress-nginx"
  version    = "4.12.0"
  namespace  = "ingress-nginx"
  create_namespace = true
  wait = false
  values = [
    file("${path.module}/nginx-values.yaml")
  ]
}

resource "helm_release" "cert-manager" {
  depends_on = [ 
    cloudflare_record.wildcard_a,
    cloudflare_record.root_a
  ]
  name       = "cert-manager"
  repository = "https://charts.jetstack.io"
  chart      = "cert-manager"
  version    = "1.16.2"
  namespace  = "cert-manager"
  create_namespace = true
  values = [
    file("${path.module}/cert-manager-values.yaml")
  ]
}

resource "local_file" "kubeconfig" {
  depends_on = [
    module.talos,
    helm_release.cert-manager
  ]

  content  = module.talos.kubeconfig
  filename = "${path.module}/kubeconfig"
}

resource "null_resource" "apply_kustomize" {
  depends_on = [
    cloudflare_record.wildcard_a,
    cloudflare_record.root_a,
    helm_release.cert-manager,
    local_file.kubeconfig
  ]

  provisioner "local-exec" {
    command = <<EOT
      echo '${data.template_file.yaml_with_replacements.rendered}' > tmp/habytee.tmp.yaml
      kustomize build ./ | kubectl apply --kubeconfig=${local_file.kubeconfig.filename} -f -
    EOT
  }
}
