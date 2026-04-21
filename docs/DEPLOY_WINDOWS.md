<div align="center" style="margin: 20px 0; padding: 10px; background: #1c1917; border-radius: 10px;">
  <strong>🌐 Language: </strong>
  
  <a href="./DEPLOY_WINDOWS.ru.md" style="color: #F5F752; margin: 0 10px;">
    🇷🇺 Russian
  </a>
  | 
  <span style="color: #0891b2; margin: 0 10px;">
    ✅ 🇺🇸 English (current)
  </span>
</div>

# Windows Server Deployment Guide

Deploy **Lizerium Restarter Server API** to the public internet using **Windows Server 2022 + NGINX + HTTPS**.

---

# Architecture

```text
Internet
   ↓
https://users.lizup.ru
   ↓
NGINX (Windows Server)
   ↓
http://127.0.0.1:52349/users
   ↓
Lizerium.Restarter.Server
```

---

# Requirements

- Windows Server 2022
- Administrator access
- Public IP address
- Domain name
- .NET 8 Runtime
- Open ports: 80 / 443

---

# Step 1 — DNS Configuration

Create A record:

```text
Type: A
Host: users
Value: YOUR_PUBLIC_IP
TTL: 300
```

Example:

```text
users.lizup.ru -> 89.110.96.215
```

Verify:

```powershell
nslookup users.lizup.ru
```

---

# Step 2 — Open Firewall Ports

Run PowerShell as Administrator:

```powershell
New-NetFirewallRule -DisplayName "NGINX HTTP" -Direction Inbound -Protocol TCP -LocalPort 80 -Action Allow

New-NetFirewallRule -DisplayName "NGINX HTTPS" -Direction Inbound -Protocol TCP -LocalPort 443 -Action Allow
```

Optional API direct port:

```powershell
New-NetFirewallRule -DisplayName "Lizerium API" -Direction Inbound -Protocol TCP -LocalPort 52349 -Action Allow
```

---

# Step 3 — Install NGINX for Windows

Download:

https://nginx.org/en/download.html

Recommended path:

```text
C:\nginx\
```

Structure:

```text
C:\nginx\nginx.exe
C:\nginx\conf\nginx.conf
C:\nginx\logs\
```

Start:

```powershell
cd C:\nginx
.\nginx.exe
```

Test:

```text
http://YOUR_PUBLIC_IP
```

---

# Step 4 — Install SSL Certificate (Let's Encrypt)

Recommended tool:

https://www.win-acme.com/

Install:

```text
C:\win-acme\
```

Run:

```powershell
cd C:\win-acme
.\wacs.exe
```

Choose certificate for:

```text
users.lizup.ru
```

Export PEM files for NGINX.

Expected files:

```text
C:\nginx\ssl\users.lizup.ru-chain.pem
C:\nginx\ssl\users.lizup.ru-key.pem
```

---

# Step 5 — Configure nginx.conf

Open:

```text
C:\nginx\conf\nginx.conf
```

Use:

```nginx
worker_processes 1;

events {
    worker_connections 1024;
}

http {
    include mime.types;
    default_type application/octet-stream;
    sendfile on;

    server {
        listen 80;
        server_name users.lizup.ru;

        location / {
            return 301 https://$host$request_uri;
        }
    }

    server {
        listen 443 ssl;
        server_name users.lizup.ru;

        ssl_certificate     C:/nginx/ssl/users.lizup.ru-chain.pem;
        ssl_certificate_key C:/nginx/ssl/users.lizup.ru-key.pem;

        ssl_protocols TLSv1.2 TLSv1.3;

        location / {
            proxy_pass http://127.0.0.1:52349;
            proxy_http_version 1.1;

            proxy_set_header Host $host;
            proxy_set_header X-Real-IP $remote_addr;
            proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header X-Forwarded-Proto https;
        }
    }
}
```

---

# Step 6 — Reload NGINX

```powershell
cd C:\nginx
.\nginx.exe -s reload
```

---

# Step 7 — Configure Lizerium API

Your app should listen only locally:

```csharp
builder.WebHost.UseUrls("http://127.0.0.1:52349");
```

Do **not** bind public interfaces when using reverse proxy.

---

# Step 8 — Final Test

Open:

```text
https://users.lizup.ru/users
```

Expected result:

- HTTPS valid certificate
- JSON response
- Public internet access

---

# Security Recommendations

## Restrict by API Key

Use:

```text
X-Api-Key
```

## Limit Access by IP (NGINX)

```nginx
location /users {
    allow YOUR_IP;
    deny all;
}
```

## Hide Direct Port 52349

Only expose 80 / 443 publicly.

---

# Auto Start NGINX

Use Windows Task Scheduler or NSSM service wrapper.

---

# Troubleshooting

## 404 Not Found

Open:

```text
https://users.lizup.ru/users
```

not root `/`

## 502 Bad Gateway

Check if Lizerium server is running on:

```text
127.0.0.1:52349
```

## SSL Errors

Re-run win-acme certificate generation.

## Firewall Issues

Verify ports:

```powershell
netstat -ano | findstr :443
```

---

# Production Recommendation

Use:

- Cloudflare DNS
- Strong API keys
- NGINX logs
- Automatic certificate renewal

---

# Related Docs

- [README.md](../README.md)
- [BUILD.md](BUILD.md)
- [API.md](API.md)
