using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using LXGaming.Common.Hosting;
using LXGaming.Common.Text.Json;
using LXGaming.Configuration;
using LXGaming.Configuration.Generic;
using LXGaming.FloodAnalytics.Configuration;
using LXGaming.FloodAnalytics.Configuration.Categories;
using LXGaming.FloodAnalytics.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LXGaming.FloodAnalytics.Services.Web;

[Service(ServiceLifetime.Singleton)]
public class WebService(IConfiguration configuration, ILogger<WebService> logger) : IHostedService {

    public JsonSerializerOptions JsonSerializerOptions { get; private set; } = null!;
    protected IProvider<Config> Config { get; } = configuration.GetRequiredProvider<IProvider<Config>>();
    protected ILogger<WebService> Logger { get; } = logger;

    public virtual Task StartAsync(CancellationToken cancellationToken) {
        var webCategory = Config.Value?.WebCategory;
        if (webCategory == null) {
            throw new InvalidOperationException("WebCategory is unavailable");
        }

        if (webCategory.Timeout <= 0) {
            Logger.LogWarning("Timeout is out of bounds. Resetting to {Value}", WebCategory.DefaultTimeout);
            webCategory.Timeout = WebCategory.DefaultTimeout;
        }

        JsonSerializerOptions = new JsonSerializerOptions {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver()
                .WithOrderPropertiesModifier()
                .WithRequiredPropertiesModifier()
        };

        return Task.CompletedTask;
    }

    public virtual Task StopAsync(CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }

    public virtual HttpClient CreateHttpClient(HttpMessageHandler handler) {
        var httpClient = new HttpClient(handler) {
            Timeout = TimeSpan.FromMilliseconds(Config.Value?.WebCategory.Timeout ?? WebCategory.DefaultTimeout)
        };
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", Constants.Application.UserAgent);
        return httpClient;
    }

    public virtual async Task<T> DeserializeAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken = default) {
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        return await JsonSerializer.DeserializeAsync<T>(stream, JsonSerializerOptions, cancellationToken)
               ?? throw new JsonException($"Failed to deserialize {nameof(T)}");
    }
}