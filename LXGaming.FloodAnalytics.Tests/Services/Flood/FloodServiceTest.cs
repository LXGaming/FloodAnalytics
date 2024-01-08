using LXGaming.Common.Hosting;
using LXGaming.Configuration;
using LXGaming.Configuration.Generic;
using LXGaming.FloodAnalytics.Configuration;
using LXGaming.FloodAnalytics.Services.Flood;
using LXGaming.FloodAnalytics.Tests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LXGaming.FloodAnalytics.Tests.Services.Flood;

[Parallelizable]
public class FloodServiceTest : ServiceTestBase {

    public FloodServiceTest() {
        Services.AddService<FloodService>();
        Services.AddSchedulerFactory();
        Services.AddWebService();
    }

    [OneTimeSetUp]
    public void Setup() {
        var config = Provider.GetRequiredService<IConfiguration>().GetRequiredProvider<IProvider<Config>>();
        var category = config.Value?.FloodCategory;
        if (string.IsNullOrEmpty(category?.Username) || string.IsNullOrEmpty(category?.Password)) {
            Assert.Ignore("Flood credentials have not been configured");
        }
    }

    [Test]
    [Order(1)]
    public Task DeserializeAuthenticateAsync() => Provider.GetRequiredService<FloodService>().AuthenticateAsync();

    [Test]
    public Task DeserializeTorrentsAsync() => Provider.GetRequiredService<FloodService>().GetTorrentsAsync();
}