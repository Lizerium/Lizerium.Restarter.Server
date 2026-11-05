/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 05 ноября 2026 06:53:00
 * Version: 1.0.199
 */

using System.Text.Json.Serialization;

namespace Lizerium.Restarter.Server.Components
{
    public class Player
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("rank")]
        public string Rank { get; set; }

        [JsonPropertyName("group")]
        public string Group { get; set; }

        [JsonPropertyName("ship")]
        public string Ship { get; set; }

        [JsonPropertyName("system")]
        public string System { get; set; }
    }
}