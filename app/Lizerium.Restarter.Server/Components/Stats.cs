/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 02 мая 2026 19:17:18
 * Version: 1.0.13
 */

using System.Text.Json.Serialization;

namespace Lizerium.Restarter.Server.Components
{
    public class Stats
    {
        [JsonPropertyName("serverload")]
        public string ServerLoad { get; set; }

        [JsonPropertyName("players")]
        public Player[] Players { get; set; }
    }
}