<div align="center" style="margin: 20px 0; padding: 10px; background: #1c1917; border-radius: 10px;">
  <strong>🌐 Язык: </strong>
  
  <span style="color: #F5F752; margin: 0 10px;">
    ✅ 🇷🇺 Русский (текущий)
  </span>
  | 
  <a href="./DEPLOY_WINDOWS.md" style="color: #0891b2; margin: 0 10px;">
    🇺🇸 English
  </a>
</div>

# Руководство по развёртыванию на Windows Server

Развёртывание **Lizerium Restarter Server API** в публичный интернет с использованием **Windows Server 2022 + NGINX + HTTPS**.

---

# Архитектура

```text id="v7k2ra"
Интернет
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

# Требования

- Windows Server 2022
- Права администратора
- Публичный IP-адрес
- Доменное имя
- .NET 8 Runtime
- Открытые порты: 80 / 443

---

# Шаг 1 — Настройка DNS

Создайте запись типа A:

```text id="o1y7ta"
Тип: A
Имя: users
Значение: YOUR_PUBLIC_IP
TTL: 300
```

Пример:

```text id="n5h3sq"
users.lizup.ru -> 89.110.96.215
```

Проверка:

```powershell id="p3e2yt"
nslookup users.lizup.ru
```

---

# Шаг 2 — Открытие портов в Firewall

Запустите PowerShell от имени администратора:

```powershell id="j8k1xe"
New-NetFirewallRule -DisplayName "NGINX HTTP" -Direction Inbound -Protocol TCP -LocalPort 80 -Action Allow

New-NetFirewallRule -DisplayName "NGINX HTTPS" -Direction Inbound -Protocol TCP -LocalPort 443 -Action Allow
```

Дополнительно (если нужен прямой доступ к API):

```powershell id="m2d9vr"
New-NetFirewallRule -DisplayName "Lizerium API" -Direction Inbound -Protocol TCP -LocalPort 52349 -Action Allow
```

---

# Шаг 3 — Установка NGINX для Windows

Скачать:

https://nginx.org/en/download.html

Рекомендуемый путь установки:

```text id="u4t0ab"
C:\nginx\
```

Структура:

```text id="g5x6rc"
C:\nginx\nginx.exe
C:\nginx\conf\nginx.conf
C:\nginx\logs\
```

Запуск:

```powershell id="v6w4jn"
cd C:\nginx
.\nginx.exe
```

Проверка:

```text id="w0f8xs"
http://YOUR_PUBLIC_IP
```

---

# Шаг 4 — Установка SSL сертификата (Let's Encrypt)

Рекомендуемый инструмент:

https://www.win-acme.com/

Установка:

```text id="r1p7zu"
C:\win-acme\
```

Запуск:

```powershell id="f3h2ta"
cd C:\win-acme
.\wacs.exe
```

Выберите сертификат для домена:

```text id="k9x1mq"
users.lizup.ru
```

Экспортируйте PEM файлы для NGINX.

Ожидаемые файлы:

```text id="b4u7he"
C:\nginx\ssl\users.lizup.ru-chain.pem
C:\nginx\ssl\users.lizup.ru-key.pem
```

---

# Шаг 5 — Настройка nginx.conf

Откройте:

```text id="x3q6ny"
C:\nginx\conf\nginx.conf
```

Используйте:

```nginx id="n8m2ru"
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

# Шаг 6 — Перезагрузка NGINX

```powershell id="q2w8eh"
cd C:\nginx
.\nginx.exe -s reload
```

---

# Шаг 7 — Настройка Lizerium API

Приложение должно слушать только локальный интерфейс:

```csharp id="d7m1kt"
builder.WebHost.UseUrls("http://127.0.0.1:52349");
```

Не открывайте публичные интерфейсы при использовании reverse proxy.

---

# Шаг 8 — Финальная проверка

Откройте:

```text id="t5y9pq"
https://users.lizup.ru/users
```

Ожидаемый результат:

- Валидный HTTPS сертификат
- JSON ответ
- Доступ из интернета

---

# Рекомендации по безопасности

## Защита API ключом

Используйте заголовок:

```text id="h6s4zx"
X-Api-Key
```

## Ограничение доступа по IP (NGINX)

```nginx id="e9f2vm"
location /users {
    allow YOUR_IP;
    deny all;
}
```

## Скрыть прямой порт 52349

Публично открывайте только 80 / 443.

---

# Автозапуск NGINX

Используйте:

- Планировщик задач Windows
- NSSM (служба Windows)

---

# Решение проблем

## 404 Not Found

Открывайте:

```text id="k8v3rp"
https://users.lizup.ru/users
```

а не корень `/`

## 502 Bad Gateway

Проверьте, работает ли приложение на:

```text id="y2u4ec"
127.0.0.1:52349
```

## Ошибки SSL

Повторно выпустите сертификат через win-acme.

## Проблемы Firewall

Проверьте открытые порты:

```powershell id="p9n1xt"
netstat -ano | findstr :443
```

---

# Рекомендации для Production

Используйте:

- Cloudflare DNS
- Надёжные API ключи
- Логи NGINX
- Автообновление сертификатов

---

# Связанная документация

- [README.md](../README.ru.md)
- [BUILD.md](BUILD.ru.md)
- [API.md](API.ru.md)
