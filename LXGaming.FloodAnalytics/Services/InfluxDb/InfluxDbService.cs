using InfluxDB.Client;
using LXGaming.Common.Hosting;
using LXGaming.Configuration;
using LXGaming.Configuration.Generic;
using LXGaming.FloodAnalytics.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LXGaming.FloodAnalytics.Services.InfluxDb;

[Service(ServiceLifetime.Singleton)]
public class InfluxDbService(IConfiguration configuration) : IHostedService {

    public InfluxDBClient Client { get; private set; } = null!;
    public string? Bucket { get; private set; }
    public string? Organization { get; private set; }

    private readonly IProvider<Config> _config = configuration.GetRequiredProvider<IProvider<Config>>();

    public Task StartAsync(CancellationToken cancellationToken) {
        var influxDbCategory = _config.Value?.InfluxDbCategory;
        if (influxDbCategory == null) {
            throw new InvalidOperationException("InfluxDbCategory is unavailable");
        }

        if (string.IsNullOrEmpty(influxDbCategory.Url)) {
            throw new InvalidOperationException("Url has not been configured for InfluxDb");
        }

        if (string.IsNullOrEmpty(influxDbCategory.Token)) {
            throw new InvalidOperationException("Token has not been configured for InfluxDb");
        }

        Client = new InfluxDBClient(influxDbCategory.Url, influxDbCategory.Token);
        Bucket = influxDbCategory.Bucket;
        Organization = influxDbCategory.Organization;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        Client.Dispose();
        return Task.CompletedTask;
    }
}