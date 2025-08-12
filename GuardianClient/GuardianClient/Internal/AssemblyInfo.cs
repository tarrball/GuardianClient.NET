using System.Reflection;

namespace GuardianClient.Internal;

internal static class AssemblyInfo
{
    internal static string GetPackageVersion()
    {
        var packageVersion = Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
            .InformationalVersion;

        return packageVersion;
    }
}