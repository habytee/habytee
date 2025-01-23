provider "cloudflare" {
  api_token = data.sops_file.secrets.data["cloudflare_api_token_inner"]
}

provider "helm" {
  kubernetes {
    host = yamldecode(module.talos.kubeconfig).clusters[0].cluster.server
    cluster_ca_certificate = base64decode(yamldecode(module.talos.kubeconfig).clusters[0].cluster.certificate-authority-data)

    client_certificate = base64decode(yamldecode(module.talos.kubeconfig).users[0].user.client-certificate-data)
    client_key = base64decode(yamldecode(module.talos.kubeconfig).users[0].user.client-key-data)
  }
}

provider "hcloud" {
  token = data.sops_file.secrets.data["hcloud_token"]
}
