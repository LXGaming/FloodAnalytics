using System.Net;
using System.Text.Json;
using LXGaming.Configuration;
using LXGaming.Configuration.Generic;
using LXGaming.FloodAnalytics.Configuration;
using LXGaming.FloodAnalytics.Utilities;

namespace LXGaming.FloodAnalytics.Services.Web;

public class WebService(IConfiguration configuration, JsonSerializerOptions jsonSerializerOptions) {

    public JsonSerializerOptions JsonSerializerOptions { get; } = jsonSerializerOptions;

    private readonly IProvider<Config> _config = configuration.GetRequiredProvider<IProvider<Config>>();

    public virtual HttpClient CreateClient(HttpMessageHandler handler) {
        var category = _config.Value?.WebCategory;
        if (category == null) {
            throw new InvalidOperationException("WebCategory is unavailable");
        }

        var client = new HttpClient(handler);
        client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", Constants.Application.UserAgent);
        client.Timeout = TimeSpan.FromMilliseconds(category.Timeout);
        return client;
    }

    public SocketsHttpHandler CreateHandler() {
        var category = _config.Value?.WebCategory;
        if (category == null) {
            throw new InvalidOperationException("WebCategory is unavailable");
        }

        return new SocketsHttpHandler {
            AutomaticDecompression = DecompressionMethods.All,
            PooledConnectionLifetime = TimeSpan.FromMilliseconds(category.PooledConnectionLifetime),
            UseCookies = false
        };
    }

    public virtual async Task<T> DeserializeAsync<T>(HttpResponseMessage response,
        CancellationToken cancellationToken = default) {
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        return await JsonSerializer.DeserializeAsync<T>(stream, JsonSerializerOptions, cancellationToken)
               ?? throw new JsonException($"Failed to deserialize {nameof(T)}");
    }
}