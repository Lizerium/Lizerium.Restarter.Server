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

```text
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

```text
Тип: A
Имя: users
Значение: YOUR_PUBLIC_IP
TTL: 300
```

Пример:

```text
users.lizup.ru -> 89.110.96.215
```

Проверка:

```powershell
nslookup users.lizup.ru
```

---

# Шаг 2 — Открытие портов в Firewall

Запустите PowerShell от имени администратора:

```powershell
New-NetFirewallRule -DisplayName "NGINX HTTP" -Direction Inbound -Protocol TCP -LocalPort 80 -Action Allow

New-NetFirewallRule -DisplayName "NGINX HTTPS" -Direction Inbound -Protocol TCP -LocalPort 443 -Action Allow
```

Дополнительно (если нужен прямой доступ к API):

```powershell
New-NetFirewallRule -DisplayName "Lizerium API" -Direction Inbound -Protocol TCP -LocalPort 52349 -Action Allow
```

---

# Шаг 3 — Установка NGINX для Windows

Скачать:

https://nginx.org/en/download.html

Рекомендуемый путь установки:

```text
C:\nginx\
```

Структура:

```text
C:\nginx\nginx.exe
C:\nginx\conf\nginx.conf
C:\nginx\logs\
```

Запуск:

```powershell
cd C:\nginx
.\nginx.exe
```

Проверка:

```text
http://YOUR_PUBLIC_IP
```

---

# Шаг 4 — Установка SSL сертификата (Let's Encrypt)

Рекомендуемый инструмент:

https://www.win-acme.com/

Установка:

```text
C:\win-acme\
```

Запуск:

```powershell
cd C:\win-acme
.\wacs.exe
```

Выберите сертификат для домена:

```text
users.lizup.ru
```

Экспортируйте PEM файлы для NGINX.

Ожидаемые файлы:

```text
C:\nginx\ssl\users.lizup.ru-chain.pem
C:\nginx\ssl\users.lizup.ru-key.pem
```

---

# Шаг 5 — Настройка nginx.conf

Откройте:

```text
C:\nginx\conf\nginx.conf
```

Используйте:

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

# Шаг 6 — Перезагрузка NGINX

```powershell
cd C:\nginx
.\nginx.exe -s reload
```

---

# Шаг 7 — Настройка Lizerium API

Приложение должно слушать только локальный интерфейс:

```csharp
builder.WebHost.UseUrls("http://127.0.0.1:52349");
```

Не открывайте публичные интерфейсы при использовании reverse proxy.

---

# Шаг 8 — Финальная проверка

Откройте:

```text
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

```text
X-Api-Key
```

## Ограничение доступа по IP (NGINX)

```nginx
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

```text
https://users.lizup.ru/users
```

а не корень `/`

## 502 Bad Gateway

Проверьте, работает ли приложение на:

```text
127.0.0.1:52349
```

## Ошибки SSL

Повторно выпустите сертификат через win-acme.

## Проблемы Firewall

Проверьте открытые порты:

```powershell
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
