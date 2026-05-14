/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 14 мая 2026 10:37:48
 * Version: 1.0.25
 */

using System.Text.Json.Serialization;

using Lizerium.Restarter.Server.Services.Localizer;

namespace Lizerium.Restarter.Server.Components
{
    public class Config
    {
        public string StatsFilePath { get; set; }
        public string ServerExecutablePath { get; set; }
        public int RestartIfNoPlayersAfterMinutes { get; set; }
        public int CheckIntervalSeconds { get; set; }
        public string ApiSecretKey { get; set; }
        public int ApiPort { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LanguageType Language { get; set; }

        [JsonIgnore]
        public bool IsValid => !string.IsNullOrEmpty(StatsFilePath)
            && !string.IsNullOrEmpty(ServerExecutablePath)
            && RestartIfNoPlayersAfterMinutes > 0 && CheckIntervalSeconds > 0;
    }
}