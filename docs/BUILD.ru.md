<div align="center" style="margin: 20px 0; padding: 10px; background: #1c1917; border-radius: 10px;">
  <strong>🌐 Язык: </strong>
  
  <span style="color: #F5F752; margin: 0 10px;">
    ✅ 🇷🇺 Русский (текущий)
  </span>
  | 
  <a href="./BUILD.md" style="color: #0891b2; margin: 0 10px;">
    🇺🇸 English
  </a>
</div>

# Сборка

```bash
dotnet restore
dotnet build -c Release
```

---

# `appsettings.json`

- Создайте файл `appsettings.json` в директории [app/Lizerium.Restarter.Server](app/Lizerium.Restarter.Server)

```json
{
	"StatsFilePath": "полный_путь_до_stats.json",
	"ServerExecutablePath": "полный_путь_до_flserver.exe",
	"RestartIfNoPlayersAfterMinutes": 360,
	"CheckIntervalSeconds": 1,
	"ApiSecretKey": "секретный_ключ",
	"ApiPort": 52349,
	"Language": "Ru"
}
```

---

# Описание параметров

- `StatsFilePath` — полный путь до файла `stats.json`

Пример:

```text
C:\Freelancer\EXE\stats.json
```

Файл `stats.json` формируется вашим сервером через LizeriumFLHook с плагином статистики.

Пример тестового файла:

[stats.json](app/TestData/stats.json)

---

- `ServerExecutablePath` — полный путь до запускаемого файла сервера `flserver.exe`

Пример:

```text
C:\Freelancer\EXE\flserver.exe
```

---

- `RestartIfNoPlayersAfterMinutes` — через сколько минут без игроков выполнить автоматический рестарт сервера

Пример:

```json
360
```

---

- `CheckIntervalSeconds` — частота проверки состояния сервера в секундах

Пример:

```json
1
```

---

- `ApiSecretKey` — секретный ключ для внешнего доступа к API

Используется в заголовке:

```text
X-Api-Key
```

Пример:

```text
342adzscaw
```

---

- `ApiPort` — порт встроенного HTTP API

Пример:

```json
52349
```

---

- `Language` — язык интерфейса консоли и логов

Поддерживается:

```text
Ru
En
```

---

# Важное замечание

> [!IMPORTANT]
> Файл `stats.json` должен быть сохранён в кодировке `windows-1251`.

Это важно для корректного отображения кириллицы в названиях систем, кораблей и игроков на Windows Server.

---

# Запуск

После настройки:

```bash
Lizerium.Restarter.Server.exe
```

---

# Результат работы

Программа автоматически:

- следит за процессом `flserver.exe`
- перезапускает сервер при падении
- перезапускает сервер при долгом отсутствии игроков
- показывает онлайн игроков в консоли
- отдаёт статистику через API
- поддерживает русский и английский языки

---

# Связанная документация

- [README.md](../README.ru.md)
- [DEPLOY_WINDOWS.md](DEPLOY_WINDOWS.ru.md)
- [API.md](API.ru.md)
