# I. Infrastructure as Code
The entire infrastructure of Habytee is described as code. Specific requirements are needed for this:

## 1. Setup

You will need:

- Helm, SOPS, kubectl, Kustomize, and Packer installed
- A top-level domain (TLD) such as [habytee.com](https://habytee.com)
- A [Cloudflare](https://cloudflare.com) account
- The domain’s nameservers set to the Cloudflare zones as described [here](https://developers.cloudflare.com/dns/zone-setups/full-setup/setup/)
- ISO images generated for modified Talos Linux (run [this](https://github.com/trinami/terraform-hcloud-talos/blob/main/_packer/create.sh))
- A [PGP key pair](https://blog.gitguardian.com/a-comprehensive-guide-to-sops/) created, and your own `secrets.yaml` file prepared:
  
```yaml
cloudflare_api_token: <>
cloudflare_api_token_inner: <>
domain_zone_id: <>
github_client_secret: <>
github_client_id: <>
cookie_secret: <>
hcloud_token: <>
domain: <>
app_domain: <>
```
<details>
  <summary>Example secrets.yaml</summary>
  
  ```yaml
cloudflare_api_token: 7tZ1FgQ9LGirBYLgoAStFTK7ow3G7irBYLgotFTK7w3HKXtXgzNcnQ8g0XgzNQg0
cloudflare_api_token_inner: 7tZ1FgQ9LG7irBYLoAStFTK7ow3HKXtXzNcnQ8g0
domain_zone_id: e36ff1a7a11e1fb782a0952169d40552
github_client_secret: 61f2c35ce46375b7610bfe676c06280f170022d5
github_client_id: 1v13liiBUwIfm9mAbblx
cookie_secret: vO4D3J4dBRRQb3gFtLJdWnOl8YlsOfok
hcloud_token: 7t1FgQ9LG7irStFTKow3HKXtXgzNcnQ807tZ1gQ9LG7irBYLKow3HKXtgzNcnQ80
domain: habytee.com
app_domain: app.habytee.com
  ```

  [cloudflare_api_token](https://developers.cloudflare.com/fundamentals/api/get-started/create-token/) is used to link the domain and Hetzner load balancer.

  [cloudflare_api_token_inner](https://cert-manager.io/docs/configuration/acme/dns01/cloudflare/#api-tokens) is a token that exists in the inner cluster (Kubernetes secret) for cert-manager.

  [domain_zone_id](https://developers.cloudflare.com/fundamentals/setup/find-account-and-zone-ids/) is the Cloudflare domain zone ID.

  [github_client_secret](https://docs.github.com/en/apps/oauth-apps/building-oauth-apps/creating-an-oauth-app) is the OAuth secret.

  [github_client_id](https://docs.github.com/en/apps/oauth-apps/building-oauth-apps/creating-an-oauth-app) is the client ID.

  [cookie_secret](https://oauth2-proxy.github.io/oauth2-proxy/configuration/overview/#generating-a-cookie-secret) is a random cookie secret for OAuth2 Proxy.

  [hcloud_token](https://docs.hetzner.com/cloud/api/getting-started/generating-api-token/) is the Hetzner API token.

  [domain]() is where the (start) page is hosted.

  [app_domain]() is where the app is hosted.

</details>
<br>
After creating the secrets.yaml file, set the public key for SOPS to use and encrypt the file, then commit it:

```bash
export SOPS_PGP_FP="9AE9C737C244499F8A90062F3E2DED859413E198" && \
sops -e secrets.yaml > secrets.enc.yaml && \
git add . && \
git commit -m "chore: new secrets" && \
git push
```

Once the setup is complete, the entire infrastructure can be deployed with a single command on Linux:

```bash
terraform init && terraform apply
```
<br>

## 2. What the Terraform script does

The [Packer script](https://github.com/trinami/terraform-hcloud-talos/blob/main/_packer/create.sh) in the hcloud-talos module rents AMD64 and ARM servers, and builds [Talos Linux](https://www.talos.dev/) ISO images, including [gVisor](https://gvisor.dev/), a rewritten Linux kernel in the memory-safe language [Go](https://go.dev/).

By default, the script sets up:

- 1 Controlling Plane

- 2 Worker Nodes

- 1 Load Balancer

<br>

Talos Linux is installed on the Controlling Plane and the workers.

The script then sets up ingress-nginx and cert-manager to manage signed TLS certificates.

The Load Balancer is configured to use a [round robin](https://kemptechnologies.com/load-balancer/round-robin-load-balancing) strategy to direct traffic to the ingress-nginx.

DNS entries for the domain and subdomain are set to point to the Load Balancer.

A 4096-bit RSA root certificate is requested and configured.

The API and the client are deployed.

An OAuth2 Proxy, using GitHub as the identity provider, is used to protect the API and client from unauthenticated users.
<br>

## 3. Why it is so cool

### Security

The base system, Talos Linux, is built with the [Kernel Self-Protection Project](https://www.kernel.org/doc/html/latest/security/self-protection.html). It is a read-only system running in memory and is ephemeral. It is not accessible through SSH, only via [mTLS](https://www.myrasecurity.com/de/knowledge-hub/mtls/).

On the container layer, gVisor strongly protects Kubernetes. Any code executed is isolated from the host system. Infected containers (e.g., due to zero-day exploits) can simply be restarted and will never affect the host system.

The OAuth2 Proxy prevents unauthorized access to the application. Since downloading the Blazor DLLs can take up to 20MB per user, it acts as a form of DoS protection. GitHub, as the identity provider, requires accounts with email providers that ask for phone numbers, making bot traffic unlikely.

GitHub as the identity provider also prevents usernames and emails from being stolen, as we don’t store them.

The cert-manager root certificate is 4096 bits, which is larger than the [BSI recommendation](https://www.bsi.bund.de/SharedDocs/Downloads/DE/BSI/Publikationen/TechnischeRichtlinien/TR02102/BSI-TR-02102-2.pdf?__blob=publicationFile&v=6). The ingress uses TLS v1.3 and has an A+ rating on [securityheaders.com](https://securityheaders.com/), which only 2.4% of web pages achieve.

The containers are based on [Alpine Linux](https://alpinelinux.org/about/), a minimal, security-focused distribution with a tiny 8MB image size.
### Scalability

The infrastructure is complex but highly scalable. We can scale the system in seconds to handle millions of users. We can simply increase the number of servers in the Terraform files and reapply the configuration.
