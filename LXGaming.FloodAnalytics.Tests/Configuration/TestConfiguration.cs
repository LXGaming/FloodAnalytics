using System.Reflection;
using LXGaming.Configuration.Memory;
using Microsoft.Extensions.Configuration;

namespace LXGaming.FloodAnalytics.Tests.Configuration;

public class TestConfiguration<T> : MemoryConfiguration<T> where T : new() {

    public TestConfiguration() {
        new ConfigurationBuilder()
            .AddUserSecrets(Assembly.GetExecutingAssembly())
            .Build()
            .Bind(Value);
    }
}