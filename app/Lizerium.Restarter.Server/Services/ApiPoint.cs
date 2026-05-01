/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 01 мая 2026 06:52:59
 * Version: 1.0.12
 */

using System.Text;

using Lizerium.Restarter.Server.Components;
using Lizerium.Restarter.Server.Services.Localizer;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Lizerium.Restarter.Server.Services;

public sealed class ApiPoint
{
    public Config AppConfig { get; }
    private WebApplication? _app;
    
    public Action<string>? ActionLog { get; set; }

    public ApiPoint(Config appConfig) => AppConfig = appConfig;

    public async Task Init(CancellationToken token)
    {
        if (AppConfig.ApiPort <= 0)
        {
            ActionLog?.Invoke(
                LangService.T("api_disabled", AppConfig.ApiPort));
            return;
        }

        var builder = WebApplication.CreateBuilder();

        // Слушаем все интерфейсы (включая внешний IP)
        builder.WebHost.UseUrls($"http://0.0.0.0:{AppConfig.ApiPort}");

        var app = builder.Build();
        _app = app;

        // CORS (для браузера)
        app.Use(async (ctx, next) =>
        {
            ctx.Response.Headers["Access-Control-Allow-Origin"] = "*";
            ctx.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, OPTIONS";
            ctx.Response.Headers["Access-Control-Allow-Headers"] = "X-Api-Key, Content-Type";

            if (HttpMethods.IsOptions(ctx.Request.Method))
            {
                ctx.Response.StatusCode = StatusCodes.Status200OK;
                return;
            }

            await next();
        });

        // Маршрут /users
        app.MapGet("/users", async (ctx) =>
        {
            ActionLog?.Invoke(
                LangService.T("request_in"));

            var key = ctx.Request.Headers["X-Api-Key"].ToString();
            if (key != AppConfig.ApiSecretKey)
            {
                ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                ctx.Response.ContentType = "application/json; charset=utf-8";
                await ctx.Response.WriteAsync(
                    "{\"error\":\"" +
                    LangService.T("unauthorized") +
                    "\"}");
                return;
            }

            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                var json = await File.ReadAllTextAsync(
                    AppConfig.StatsFilePath,
                    Encoding.GetEncoding("windows-1251"),
                    token
                );

                ctx.Response.StatusCode = StatusCodes.Status200OK;
                ctx.Response.ContentType = "application/json; charset=utf-8";
                await ctx.Response.WriteAsync(json);
            }
            catch (Exception ex)
            {
                ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
                ctx.Response.ContentType = "application/json; charset=utf-8";
                var error = LangService.T("stats_read_failed", ex.Message)
                    .Replace("\"", "'");

                await ctx.Response.WriteAsync($"{{\"error\":\"{error}\"}}");
            }
        });

        ActionLog?.Invoke(
            LangService.T("api_started", AppConfig.ApiPort));

        // Запуск + корректная остановка по токену
        await app.StartAsync(token);
        try
        {
            await app.WaitForShutdownAsync(token);
        }
        catch (OperationCanceledException)
        {
            // ok
        }
        finally
        {
            await StopAsync();
        }
    }

    public async Task StopAsync()
    {
        if (_app is null) return;
        try
        {
            await _app.StopAsync();
        }
        catch {
            ActionLog?.Invoke(LangService.T("api_stopping"));

            try
            {
                await _app.StopAsync();
            }
            catch (Exception ex)
            {
                ActionLog?.Invoke(
                    LangService.T("api_stop_error", ex.Message));
            }

            await _app.DisposeAsync();
            _app = null;

            ActionLog?.Invoke(LangService.T("api_stopped"));
        }
        await _app.DisposeAsync();
        _app = null;
    }
}

