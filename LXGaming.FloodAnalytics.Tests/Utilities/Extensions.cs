﻿using LXGaming.Common.Hosting;
using LXGaming.Configuration;
using LXGaming.Configuration.Hosting;
using LXGaming.FloodAnalytics.Configuration;
using LXGaming.FloodAnalytics.Tests.Configuration;
using LXGaming.FloodAnalytics.Tests.Services.Quartz;
using LXGaming.FloodAnalytics.Tests.Services.Web;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace LXGaming.FloodAnalytics.Tests.Utilities;

public static class Extensions {

    public static IServiceCollection AddConfiguration(this IServiceCollection services) {
        var configuration = new DefaultConfiguration();
        configuration.Register(nameof(Config), new TestProvider<Config>());
        return services.AddConfiguration(configuration);
    }

    public static IServiceCollection AddSchedulerFactory(this IServiceCollection services) {
        return services.AddSingleton<ISchedulerFactory, TestSchedulerFactory>();
    }

    public static IServiceCollection AddWebService(this IServiceCollection services) {
        return services
            .AddConfiguration()
            .AddLogging()
            .AddService<TestWebService>();
    }
}