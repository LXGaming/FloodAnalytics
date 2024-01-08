using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace LXGaming.FloodAnalytics.Tests.Services;

public abstract class ServiceTestBase {

    protected readonly IServiceCollection Services = new ServiceCollection();
    protected ServiceProvider Provider = null!;

    [OneTimeSetUp]
    public async Task StartAsync() {
        Provider = Services.BuildServiceProvider();
        foreach (var service in Provider.GetServices<IHostedService>()) {
            await service.StartAsync(CancellationToken.None);
        }
    }

    [OneTimeTearDown]
    public async Task StopAsync() {
        foreach (var service in Provider.GetServices<IHostedService>()) {
            await service.StopAsync(CancellationToken.None);
        }

        await Provider.DisposeAsync();
    }
}