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

```text id="a1q0ks"
http://127.0.0.1:52349
```

Публичный доступ (пример):

```text id="n7vxh2"
https://users.lizup.ru
```

---

# Авторизация

Все защищённые запросы требуют заголовок:

```text id="f6n3zo"
X-Api-Key: YOUR_SECRET_KEY
```

Настраивается в:

```text id="v1xw7e"
appsettings.json
```

Пример:

```json id="j2mc0n"
{
	"ApiSecretKey": "my_super_secret_key"
}
```

---

# Content-Type

Ответы используют:

```text id="ptn1xv"
application/json; charset=utf-8
```

---

# Endpoints

---

# GET /users

Возвращает текущую статистику сервера и список активных игроков.

## Запрос

```http id="m3a8vb"
GET /users HTTP/1.1
Host: users.lizup.ru
X-Api-Key: YOUR_SECRET_KEY
```

---

## Успешный ответ

```json id="w9z2tp"
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

```json id="r4v0zt"
{
	"serverload": "0",
	"players": []
}
```

---

## Ошибка авторизации

Статус:

```text id="s6d1xq"
401 Unauthorized
```

Тело ответа:

```json id="m8n0yw"
{
	"error": "Unauthorized"
}
```

---

## Внутренняя ошибка сервера

Статус:

```text id="f2j9ac"
500 Internal Server Error
```

Тело ответа:

```json id="d7m1ke"
{
	"error": "Failed reading stats file"
}
```

---

# Примеры использования

---

# curl

```bash id="z5v2ou"
curl -H "X-Api-Key: YOUR_SECRET_KEY" ^
https://users.lizup.ru/users
```

---

# PowerShell

```powershell id="h4n2ze"
Invoke-RestMethod `
-Uri "https://users.lizup.ru/users" `
-Headers @{ "X-Api-Key" = "YOUR_SECRET_KEY" }
```

---

# C#

```csharp id="x9c7fr"
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

```javascript id="p1r7ta"
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

```text id="g8t1yr"
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

```text id="u5m8ze"
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
