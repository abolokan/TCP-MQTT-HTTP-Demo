using System.Text.RegularExpressions;

namespace Solid.Common.Utils;

public class VersionUtils
{
    private VersionUtils()
    {

    }

    private static readonly string _versionReg = @"^(\d+)\.?(\d+)?\.?(\d+)?$";
    public static string CoerceVersion(string version)
    {
        Match match = Regex.Match(version, _versionReg);

        // Extract major, minor, and patch components
        int major = int.Parse(match.Groups[1].Value);
        int minor = match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : 0;
        int patch = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : 0;

        string coercedVersion = $"{major}.{minor}.{patch}";

        return coercedVersion;
    }

    public static bool IsValid(string version) => Regex.IsMatch(version, _versionReg);

    public static bool IsSatisfied(string currentVersion, string latestVersion)
    {
        Version current = new Version(currentVersion);
        Version latest = new Version(latestVersion);

        return current >= latest;
    }
}
