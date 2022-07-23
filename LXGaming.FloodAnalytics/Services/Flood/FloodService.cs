using System.Net;
using System.Text.Json;
using LXGaming.Common.Hosting;
using LXGaming.FloodAnalytics.Configuration;
using LXGaming.FloodAnalytics.Services.Flood.Jobs;
using LXGaming.FloodAnalytics.Services.Flood.Models;
using LXGaming.FloodAnalytics.Services.Quartz;
using LXGaming.FloodAnalytics.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;

namespace LXGaming.FloodAnalytics.Services.Flood;

[Service(ServiceLifetime.Singleton)]
public class FloodService : IHostedService {

    private const uint DefaultReconnectDelay = 2;
    private const uint DefaultMaximumReconnectDelay = 300; // 5 Minutes
    private readonly IConfiguration _configuration;
    private readonly ILogger<FloodService> _logger;
    private readonly ISchedulerFactory _schedulerFactory;
    private HttpClient? _httpClient;

    public FloodService(IConfiguration configuration, ILogger<FloodService> logger, ISchedulerFactory schedulerFactory) {
        _configuration = configuration;
        _logger = logger;
        _schedulerFactory = schedulerFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken) {
        var floodCategory = _configuration.Config?.FloodCategory;
        if (floodCategory == null) {
            _logger.LogWarning("FloodCategory is unavailable");
            return;
        }

        if (string.IsNullOrEmpty(floodCategory.Address)) {
            _logger.LogWarning("Flood address has not been configured");
            return;
        }

        if (string.IsNullOrEmpty(floodCategory.Username) || string.IsNullOrEmpty(floodCategory.Password)) {
            _logger.LogWarning("Flood credentials have not been configured");
            return;
        }

        if (string.IsNullOrEmpty(floodCategory.Schedule)) {
            _logger.LogWarning("Flood schedule has not been configured");
            return;
        }

        _httpClient = new HttpClient(new HttpClientHandler {
            CookieContainer = new CookieContainer(),
            UseCookies = true
        });
        _httpClient.BaseAddress = new Uri(floodCategory.Address);
        _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", Constants.Application.UserAgent);

        var reconnectDelay = DefaultReconnectDelay;
        while (true) {
            try {
                var authenticate = await AuthenticateAsync(floodCategory.Username, floodCategory.Password);
                if (authenticate is not { Success: true }) {
                    _logger.LogWarning("Flood authentication failed");
                    return;
                }

                _logger.LogInformation("Connected to Flood as {Username} ({Level})", authenticate.Username, authenticate.Level);
                break;
            } catch (HttpRequestException ex) {
                if (ex is { StatusCode: HttpStatusCode.Unauthorized }) {
                    throw;
                }

                var delay = TimeSpan.FromSeconds(reconnectDelay);
                reconnectDelay = Math.Min(reconnectDelay << 1, DefaultMaximumReconnectDelay);

                _logger.LogWarning("Connection failed! Next attempt in {Delay}: {Message}", delay, ex.Message);
                await Task.Delay(delay, cancellationToken);
            }
        }

        var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
        await scheduler.ScheduleJobAsync<FloodJob>(FloodJob.JobKey, TriggerBuilder.Create().WithCronSchedule(floodCategory.Schedule).Build());
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        _httpClient?.Dispose();
        return Task.CompletedTask;
    }

    public async Task<T> EnsureAuthenticatedAsync<T>(Func<Task<T>> task) {
        try {
            return await task();
        } catch (HttpRequestException ex) {
            if (ex is not { StatusCode: HttpStatusCode.Unauthorized }) {
                throw;
            }
        }

        var authenticate = await AuthenticateAsync(_configuration.Config?.FloodCategory.Username ?? "", _configuration.Config?.FloodCategory.Password ?? "");
        if (authenticate is { Success: true }) {
            _logger.LogInformation("Reconnected to Flood as {Username} ({Level})", authenticate.Username, authenticate.Level);
        } else {
            _logger.LogWarning("Reconnection failed!");
        }

        return await task();
    }

    public async Task<Authenticate?> AuthenticateAsync(string username, string password) {
        if (_httpClient == null) {
            throw new InvalidOperationException("HttpClient is unavailable");
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, "api/auth/authenticate") {
            Content = new FormUrlEncodedContent(new Dictionary<string, string> {
                {"username", username},
                {"password", password}
            })
        };
        using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();
        await using var stream = await response.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<Authenticate>(stream, Toolbox.JsonSerializerOptions);
    }

    public async Task<TorrentProperties?> GetTorrentAsync(string hash) {
        var torrentListSummary = await GetTorrentsAsync();
        if (torrentListSummary?.Torrents?.TryGetValue(hash, out var torrentProperties) ?? false) {
            return torrentProperties;
        }

        return null;
    }

    public async Task<TorrentListSummary?> GetTorrentsAsync() {
        if (_httpClient == null) {
            throw new InvalidOperationException("HttpClient is unavailable");
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, "api/torrents");
        using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();
        await using var stream = await response.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<TorrentListSummary>(stream, Toolbox.JsonSerializerOptions);
    }
}