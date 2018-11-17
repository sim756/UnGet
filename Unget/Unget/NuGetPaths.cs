using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Unget
{
    public static class NuGetPaths
    {
        public static readonly Dictionary<OSPlatform, string> NuGetCachePaths = new Dictionary<OSPlatform, string>()
        {
            {OSPlatform.Windows, Environment.ExpandEnvironmentVariables("%UserProfile%\\.nuget\\packages")},
            {OSPlatform.Linux, "~/.nuget/packages"},
            {OSPlatform.OSX, "~/.nuget/packages"}
        };

        public static string GetNugetCachePathByPlatform()
        {
            return (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? NuGetCachePaths[OSPlatform.Windows]
                : (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                    ? NuGetCachePaths[OSPlatform.Linux]
                    : ((RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                        ? NuGetCachePaths[OSPlatform.OSX]
                        : null)));
        }
    }
}