/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 20 августа 2026 09:37:14
 * Version: 1.0.123
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