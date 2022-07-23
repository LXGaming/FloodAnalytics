using System.Reflection;
using System.Text.Json;

namespace LXGaming.FloodAnalytics.Utilities;

public static class Toolbox {

    public static readonly JsonSerializerOptions JsonSerializerOptions = new() {
        IncludeFields = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public static string GetAssemblyVersion(Assembly assembly) {
        return (assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                ?? assembly.GetCustomAttribute<AssemblyVersionAttribute>()?.Version
                ?? "null").Split('+', '-')[0];
    }
}