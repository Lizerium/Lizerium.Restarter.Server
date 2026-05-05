/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 05 мая 2026 07:02:00
 * Version: 1.0.16
 */

using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

using Lizerium.Restarter.Server.Components;
using Lizerium.Restarter.Server.Services;
using Lizerium.Restarter.Server.Services.Localizer;

internal class MainPoint
{
    static DateTime lastPlayerTime = DateTime.Now;
    static Config config;

    static void Main()
    {
        var exitsConfig = File.Exists("appsettings.json");
        if (!exitsConfig)
        {
            Console.WriteLine(LangService.T("config_not_found"));
            File.WriteAllText("appsettings.json", JsonSerializer.Serialize<Config>(new Config()));
            Console.WriteLine(LangService.T("fill_values"));
            Console.ReadLine();
            return;
        }

        var text = File.ReadAllText("appsettings.json");

        config = JsonSerializer.Deserialize<Config>(text);

        if (config == null || !config.IsValid)
        {
            Console.WriteLine(LangService.T("config_invalid"));
            Console.ReadLine();
            return;
        }
        LangService.Current = config.Language;

        Console.WriteLine("[Monitor] " + LangService.T("starting_monitor")); ;
        var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (s, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
            Console.WriteLine(LangService.T("shutdown")); ;
        };

        var apiPoint = new ApiPoint(config);
        apiPoint.ActionLog += (msg) =>
        {
            Console.WriteLine("[API]" + msg);
        };
        
        var apiTask = Task.Run(async () => await apiPoint.Init(cts.Token));

        while (!cts.Token.IsCancellationRequested)
        {
            if (!IsProcessRunning("flserver"))
            {
                var msg = LangService.T("server_not_running", "flserver.exe");
                Console.WriteLine(msg);
                Log(msg);
                StartServer();
                lastPlayerTime = DateTime.Now; // сброс таймера после рестарта
            }

            if (File.Exists(config.StatsFilePath))
            {
                try
                {
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                    var json = File.ReadAllText(config.StatsFilePath, Encoding.GetEncoding("windows-1251"));

                    // 1. Чиним невалидные \uXXXX (например, \uK123 → \\uK123)
                    json = Regex.Replace(json, @"\\u(?![0-9A-Fa-f]{4})", @"\\u");

                    // 2. Чиним все другие неэкранированные слэши
                    json = Regex.Replace(json, @"(?<!\\)\\(?![\\\/bfnrtu""])", @"\\");

                    // 3. Пробуем десериализацию
                    Stats? stats = null;
                    try
                    {
                        stats = JsonSerializer.Deserialize<Stats>(json);
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine(
                            LangService.T("json_deserialize_failed", DateTime.Now, ex.Message));
                        Console.WriteLine(LangService.T("corrupted_json"));
                        Console.WriteLine(json.Substring(0, Math.Min(500, json.Length)) + "...");
                    }

                    if (stats == null)
                    {
                        Console.WriteLine(
                            LangService.T("error_reading_file", DateTime.Now, config.StatsFilePath));
                        return;
                    }

                    bool noPlayersNow = stats?.Players?.Length == 0;
                    double totalUptimeMinutes = (DateTime.Now - lastPlayerTime).TotalMinutes;
                    TimeSpan remaining = TimeSpan.FromMinutes(config.RestartIfNoPlayersAfterMinutes) - (DateTime.Now - lastPlayerTime);

                    DrawPlayerBoard(stats.Players, remaining);

                    if (noPlayersNow && totalUptimeMinutes >= config.RestartIfNoPlayersAfterMinutes)
                    {
                        Console.WriteLine(
                            LangService.T("restart_due_idle",
                            DateTime.Now,
                            totalUptimeMinutes));
                        RestartServer();
                        lastPlayerTime = DateTime.Now;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        LangService.T("error_reading_file", DateTime.Now, ex.Message));
                }
            }

            Thread.Sleep(config.CheckIntervalSeconds * 1000);
        }
    }

    static bool IsProcessRunning(string name)
    {
        return Process.GetProcessesByName(name).Length > 0;
    }

    static void Log(string message)
    {
        Console.WriteLine(message);
        try
        {
            File.AppendAllText("flserver_monitor.log", message + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                LangService.T("log_write_error", ex.Message));
        }
    }

    static void StartServer()
    {
        var filExist = File.Exists(config.ServerExecutablePath);
        if (!filExist)
        {
            Console.WriteLine(
                LangService.T("server_file_missing",
                config.ServerExecutablePath));
            Console.ReadLine();
            return;
        }
        var startInfo = new ProcessStartInfo
        {
            FileName = config.ServerExecutablePath,
            Arguments = "-c",
            WorkingDirectory = Path.GetDirectoryName(config.ServerExecutablePath),
            UseShellExecute = true, // важно для GUI-приложений
            WindowStyle = ProcessWindowStyle.Normal
        };

        Process.Start(startInfo);
    }

    static void DrawPlayerBoard(Player[] players, TimeSpan remaining)
    {
        Console.Clear();

        int hours = remaining.Hours;
        int minutes = remaining.Minutes;
        int seconds = remaining.Seconds;

        Console.WriteLine(
            LangService.T("monitor_restart_in",
            hours, minutes, seconds));

        if (players == null || players.Length == 0)
        {
            Console.WriteLine(LangService.T("no_players"));
            return;
        }

        if (players?.Length > 0)
        {
            for (int i = 0; i < players.Length; i++)
            {
                var p = players[i];
                string rank = string.IsNullOrWhiteSpace(p.Rank) ? "?" : p.Rank;
                string name = string.IsNullOrWhiteSpace(p.Name) ? LangService.T("unknown_player") : p.Name;
                string system = string.IsNullOrWhiteSpace(p.System) ? LangService.T("unknown_system") : p.System;
                string ship = string.IsNullOrWhiteSpace(p.Ship) ? LangService.T("unknown_ship") : p.Ship;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"[{rank}] ");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"{name}");

                Console.ResetColor();
                Console.Write(" " + LangService.T("flying_in_system") + " ");

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"{system}");

                Console.ResetColor();
                Console.Write(" " + LangService.T("on_ship") + " ");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{ship}");

                Console.ResetColor();
            }
        }
        else
        {
            Console.WriteLine(LangService.T("no_players"));
        }
    }

    static void RestartServer()
    {
        foreach (var proc in Process.GetProcessesByName("flserver"))
        {
            proc.Kill(true);
        }

        Thread.Sleep(2000); // Ждём пока полностью выгрузится
        StartServer();
    }
}
