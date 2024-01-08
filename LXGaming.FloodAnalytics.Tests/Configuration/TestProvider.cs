using System.Reflection;
using LXGaming.Configuration.Generic;
using Microsoft.Extensions.Configuration;

namespace LXGaming.FloodAnalytics.Tests.Configuration;

public class TestProvider<T> : IProvider<T> {

    public T? Value { get; }

    public TestProvider() {
        Value = Activator.CreateInstance<T>();
        new ConfigurationBuilder()
            .AddUserSecrets(Assembly.GetExecutingAssembly())
            .Build()
            .Bind(Value);
    }

    public Task LoadAsync(CancellationToken cancellationToken = default) {
        return Task.CompletedTask;
    }

    public Task SaveAsync(CancellationToken cancellationToken = default) {
        return Task.CompletedTask;
    }

    public void Dispose() {
        GC.SuppressFinalize(this);
    }
}