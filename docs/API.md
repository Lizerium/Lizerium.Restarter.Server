<div align="center" style="margin: 20px 0; padding: 10px; background: #1c1917; border-radius: 10px;">
  <strong>🌐 Language: </strong>
  
  <a href="./API.ru.md" style="color: #F5F752; margin: 0 10px;">
    🇷🇺 Russian
  </a>
  | 
  <span style="color: #0891b2; margin: 0 10px;">
    ✅ 🇺🇸 English (current)
  </span>
</div>

# Lizerium Restarter Server API

HTTP API for bots, monitoring systems, dashboards, and remote integrations.

---

# Base URL

Local:

```text
http://127.0.0.1:52349
```

Public (example):

```text
https://users.lizup.ru
```

---

# Authentication

All protected endpoints require header:

```text
X-Api-Key: YOUR_SECRET_KEY
```

Configure in:

```text
appsettings.json
```

Example:

```json
{
	"ApiSecretKey": "my_super_secret_key"
}
```

---

# Content Type

Responses use:

```text
application/json; charset=utf-8
```

---

# Endpoints

---

# GET /users

Returns current server statistics and active players.

## Request

```http
GET /users HTTP/1.1
Host: users.lizup.ru
X-Api-Key: YOUR_SECRET_KEY
```

---

## Success Response

```json
{
	"serverload": "6",
	"players": [
		{
			"name": "NubotiV",
			"rank": "142",
			"group": "0",
			"ship": "Vestnik",
			"system": "Aiur"
		},
		{
			"name": "Lizerium",
			"rank": "1",
			"group": "0",
			"ship": "Shadow Blade",
			"system": "New York"
		}
	]
}
```

---

## Empty Server Example

```json
{
	"serverload": "0",
	"players": []
}
```

---

## Unauthorized Response

Status:

```text
401 Unauthorized
```

Body:

```json
{
	"error": "Unauthorized"
}
```

---

## Internal Error Response

Status:

```text
500 Internal Server Error
```

Body:

```json
{
	"error": "Failed reading stats file"
}
```

---

# Usage Examples

---

# curl

```bash
curl -H "X-Api-Key: YOUR_SECRET_KEY" ^
https://users.lizup.ru/users
```

---

# PowerShell

```powershell
Invoke-RestMethod `
-Uri "https://users.lizup.ru/users" `
-Headers @{ "X-Api-Key" = "YOUR_SECRET_KEY" }
```

---

# C#

```csharp
using var client = new HttpClient();

client.DefaultRequestHeaders.Add(
    "X-Api-Key",
    "YOUR_SECRET_KEY");

string json =
    await client.GetStringAsync(
        "https://users.lizup.ru/users");
```

---

# JavaScript

```javascript
const res = await fetch('https://users.lizup.ru/users', {
	headers: {
		'X-Api-Key': 'YOUR_SECRET_KEY',
	},
})

const data = await res.json()
```

---

# Response Fields

| Field      | Description          |
| ---------- | -------------------- |
| serverload | Current server load  |
| players    | Active players array |
| name       | Player nickname      |
| rank       | Player rank          |
| group      | Group identifier     |
| ship       | Current ship         |
| system     | Current system       |

---

# Security Notes

- Never expose API key publicly
- Prefer HTTPS deployment
- Restrict IP access if possible
- Rotate keys periodically

---

# Rate Limiting

Recommended polling interval:

```text
5-30 seconds
```

Avoid excessive requests.

---

# Monitoring Integration Ideas

- Discord bot online stats
- Telegram bot player list
- Web dashboard
- Prometheus exporter
- Server control panel

---

# Planned Endpoints

Future optional endpoints:

```text
/status
/restart
/start
/stop
/metrics
/version
```

---

# Related Docs

- [README.md](../README.md)
- [BUILD.md](BUILD.md)
- [DEPLOY_WINDOWS.md](DEPLOY_WINDOWS.md)
