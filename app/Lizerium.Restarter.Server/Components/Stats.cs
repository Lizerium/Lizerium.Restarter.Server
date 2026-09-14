/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 14 сентября 2026 09:51:43
 * Version: 1.0.148
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