/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 15 мая 2026 07:50:35
 * Version: 1.0.26
 */

namespace Lizerium.Restarter.Server.Services.Localizer
{
    public static class LangService
    {
        public static LanguageType Current = LanguageType.En;

        private static readonly Dictionary<string, (string ru, string en)> _dict = new()
        {
            ["starting_monitor"] = (
                "Запуск мониторинга FLServer...",
                "Starting FLServer monitor..."
            ),

            ["shutdown"] = (
                "Завершение работы...",
                "Shutting down..."
            ),

            ["no_players"] = (
                "Нет активных игроков.",
                "No active players."
            ),

            ["restarting_server"] = (
                "Перезапуск сервера...",
                "Restarting server..."
            ),

            ["file_not_found"] = (
                "Файл не найден!",
                "File not found!"
            ),

            ["server_not_running"] = (
                "{0} не запущен. Перезапуск...",
                "{0} not running. Restarting..."
            ),

            ["config_not_found"] = (
                "appsettings.json не найден!\nСоздаю...",
                "appsettings.json not found!\nCreating..."
            ),

            ["fill_values"] = (
                "Заполните ваши значения",
                "Fill in your values"
            ),

            ["config_invalid"] = (
                "appsettings.json не заполнен!\nЗаполните...",
                "appsettings.json is not configured!\nFill it..."
            ),

            ["json_deserialize_failed"] = (
                "[{0}] Ошибка десериализации JSON: {1}",
                "[{0}] JSON deserialization failed: {1}"
            ),

            ["corrupted_json"] = (
                "Исходный (возможно повреждённый) JSON:",
                "Original (possibly corrupted) JSON:"
            ),

            ["error_reading_file"] = (
                "[{0}] Ошибка чтения {1}",
                "[{0}] Error reading {1}"
            ),

            ["restart_due_idle"] = (
                "[{0}] Нет игроков, uptime {1:F1} мин. Перезапуск сервера...",
                "[{0}] No players, uptime {1:F1} min. Restarting server..."
            ),

            ["log_write_error"] = (
                "Ошибка записи лога: {0}",
                "Log write error: {0}"
            ),

            ["server_file_missing"] = (
                "{0} - файл не существует, укажите полный путь до flserver.exe!",
                "{0} - file does not exist, specify full path to flserver.exe!"
            ),

            ["monitor_restart_in"] = (
                "[Мониторинг] До рестарта: {0:D2}:{1:D2}:{2:D2}",
                "[Monitoring] Restart in: {0:D2}:{1:D2}:{2:D2}"
            ),

            ["unknown_player"] = (
                "<неизвестен>",
                "<unknown>"
            ),

            ["unknown_system"] = (
                "<неизвестно где>",
                "<unknown location>"
            ),

            ["unknown_ship"] = (
                "<неизвестный корабль>",
                "<unknown ship>"
            ),

            ["flying_in_system"] = (
                "летает в системе",
                "is flying in system"
            ),

            ["on_ship"] = (
                "на корабле",
                "on ship"
            ),

            ["api_disabled"] = (
                "Мини API не запущено (порт {0}).",
                "Mini API not started (port {0})."
            ),

            ["api_started"] = (
                "Мини API (Kestrel) запущено на http://<ip>:{0}/users",
                "Mini API (Kestrel) started at http://<ip>:{0}/users"
            ),

            ["request_in"] = (
                "Входящий запрос",
                "Incoming request"
            ),

            ["unauthorized"] = (
                "Неавторизован",
                "Unauthorized"
            ),

            ["stats_read_failed"] = (
                "Ошибка чтения stats файла: {0}",
                "Failed reading stats file: {0}"
            ),

            ["api_stopping"] = (
                "Остановка API...",
                "Stopping API..."
            ),

            ["api_stopped"] = (
                "API остановлено.",
                "API stopped."
            ),

            ["api_stop_error"] = (
                "Ошибка остановки API: {0}",
                "API stop error: {0}"
            ),
        };

        public static string T(string key)
        {
            if (!_dict.ContainsKey(key))
                return key;

            var item = _dict[key];

            return Current == LanguageType.Ru
                ? item.ru
                : item.en;
        }

        public static string T(string key, params object[] args)
        {
            return string.Format(T(key), args);
        }
    }
}
