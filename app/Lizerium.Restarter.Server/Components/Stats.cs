/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 09 августа 2026 15:31:48
 * Version: 1.0.112
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