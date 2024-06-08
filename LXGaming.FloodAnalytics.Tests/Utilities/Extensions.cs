using LXGaming.Configuration;
using LXGaming.Configuration.Hosting;
using LXGaming.FloodAnalytics.Configuration;
using LXGaming.FloodAnalytics.Services.Web;
using LXGaming.FloodAnalytics.Services.Web.Utilities;
using LXGaming.FloodAnalytics.Tests.Configuration;
using LXGaming.FloodAnalytics.Tests.Services.Quartz;
using LXGaming.FloodAnalytics.Tests.Services.Web;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace LXGaming.FloodAnalytics.Tests.Utilities;

public static class Extensions {

    public static IServiceCollection AddConfiguration(this IServiceCollection services) {
        if (services.Any(descriptor => descriptor.ServiceType == typeof(IConfiguration))) {
            throw new InvalidOperationException("Configuration is already registered");
        }

        var configuration = new DefaultConfiguration();
        configuration.Register(nameof(Config), new TestProvider<Config>());
        return services.AddConfiguration(configuration);
    }

    public static IServiceCollection AddSchedulerFactory(this IServiceCollection services) {
        if (services.Any(descriptor => descriptor.ServiceType == typeof(ISchedulerFactory))) {
            throw new InvalidOperationException("SchedulerFactory is already registered");
        }

        return services.AddSingleton<ISchedulerFactory, TestSchedulerFactory>();
    }

    public static IServiceCollection AddWebService(this IServiceCollection services) {
        if (services.Any(descriptor => descriptor.ServiceType == typeof(WebService))) {
            throw new InvalidOperationException("WebService is already registered");
        }

        return services
            .AddConfiguration()
            .AddWebService<WebService, TestWebService>();
    }
}