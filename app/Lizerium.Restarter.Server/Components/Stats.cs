/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 12 июля 2026 06:53:02
 * Version: 1.0.84
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