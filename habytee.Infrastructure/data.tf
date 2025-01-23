data "sops_file" "secrets" {
  source_file = "secrets.enc.yaml"
}

data "template_file" "yaml_with_replacements" {
  template = file("habytee.yaml")

  vars = {
    GITHUB_CLIENT_SECRET = data.sops_file.secrets.data["github_client_secret"]
    GITHUB_CLIENT_ID = data.sops_file.secrets.data["github_client_id"]
    COOKIE_SECRET = data.sops_file.secrets.data["cookie_secret"]
    DOMAIN = data.sops_file.secrets.data["domain"]
    APP_DOMAIN = data.sops_file.secrets.data["app_domain"]
    CLOUDFLARE_API_TOKEN_INNER = base64encode(data.sops_file.secrets.data["cloudflare_api_token_inner"])
    POSTGRES_PASSWORD = data.sops_file.secrets.data["postgres_password"]
  }
}
