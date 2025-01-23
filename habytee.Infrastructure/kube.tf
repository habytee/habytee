terraform {
  required_version = ">= 1.5.0"
  required_providers {
    hcloud = {
      source  = "hetznercloud/hcloud"
      version = ">= 1.49.1"
    }
    cloudflare = {
      source  = "cloudflare/cloudflare"
      version = "~> 4.0"
    }
    sops = {
      source = "carlpett/sops"
      version = ">= 1.1.1"
    }
    local = {
      source  = "hashicorp/local"
      version = ">= 2.5.2"
    }
    helm = {
      source = "hashicorp/helm"
      version = "~> 2.4"
    }
  }
}

module "talos" {
  source  = "trinami/talos/hcloud"
  version = "v1.0.7"

  talos_version = "v1.9.1"

  hcloud_token = data.sops_file.secrets.data["hcloud_token"]
  
  extra_firewall_rules = [
    {
      description = "Allow All IPv4 Traffic"
      direction   = "in"
      protocol    = "tcp"
      port        = "any"
      source_ips  = ["0.0.0.0/0"]
    }
  ]

  cluster_name    = "habytee.com"
  datacenter_name = "fsn1-dc14"

  control_plane_count       = 1
  control_plane_server_type = "cax11"

  worker_count       = 2
  worker_server_type = "cx22"
  sysctls_extra_args = {
    "user.max_user_namespaces" = "11255"
  }
}
