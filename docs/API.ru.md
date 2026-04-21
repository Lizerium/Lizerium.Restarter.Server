<div align="center" style="margin: 20px 0; padding: 10px; background: #1c1917; border-radius: 10px;">
  <strong>🌐 Язык: </strong>
  
  <span style="color: #F5F752; margin: 0 10px;">
    ✅ 🇷🇺 Русский (текущий)
  </span>
  | 
  <a href="API.md" style="color: #0891b2; margin: 0 10px;">
    🇺🇸 English
  </a>
</div>

# Lizerium Restarter Server API

HTTP API для ботов, систем мониторинга, веб-панелей и удалённых интеграций.

---

# Базовый URL

Локально:

```text
http://127.0.0.1:52349
```

Публичный доступ (пример):

```text
https://users.lizup.ru
```

---

# Авторизация

Все защищённые запросы требуют заголовок:

```text
X-Api-Key: YOUR_SECRET_KEY
```

Настраивается в:

```text
appsettings.json
```

Пример:

```json
{
	"ApiSecretKey": "my_super_secret_key"
}
```

---

# Content-Type

Ответы используют:

```text
application/json; charset=utf-8
```

---

# Endpoints

---

# GET /users

Возвращает текущую статистику сервера и список активных игроков.

## Запрос

```http
GET /users HTTP/1.1
Host: users.lizup.ru
X-Api-Key: YOUR_SECRET_KEY
```

---

## Успешный ответ

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

## Пустой сервер

```json
{
	"serverload": "0",
	"players": []
}
```

---

## Ошибка авторизации

Статус:

```text
401 Unauthorized
```

Тело ответа:

```json
{
	"error": "Unauthorized"
}
```

---

## Внутренняя ошибка сервера

Статус:

```text
500 Internal Server Error
```

Тело ответа:

```json
{
	"error": "Failed reading stats file"
}
```

---

# Примеры использования

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

# Поля ответа

| Поле       | Описание                 |
| ---------- | ------------------------ |
| serverload | Текущая нагрузка сервера |
| players    | Массив активных игроков  |
| name       | Ник игрока               |
| rank       | Ранг игрока              |
| group      | Идентификатор группы     |
| ship       | Текущий корабль          |
| system     | Текущая система          |

---

# Рекомендации по безопасности

- Не публикуйте API ключ
- Используйте HTTPS
- Ограничивайте доступ по IP при необходимости
- Периодически меняйте ключ

---

# Ограничение запросов

Рекомендуемый интервал опроса:

```text
5-30 секунд
```

Избегайте слишком частых запросов.

---

# Идеи интеграции

- Discord бот со статистикой онлайна
- Telegram бот со списком игроков
- Веб-панель мониторинга
- Prometheus exporter
- Панель управления сервером

---

# Планируемые endpoints

Возможные будущие маршруты:

```text
/status
/restart
/start
/stop
/metrics
/version
```

---

# Связанная документация

- [README.md](../README.ru.md)
- [BUILD.md](BUILD.ru.md)
- [DEPLOY_WINDOWS.md](DEPLOY_WINDOWS.ru.md)
