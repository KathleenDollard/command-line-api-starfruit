namespace Playground
{
    public enum RollForward
    {
        Patch,   // Uses the specified version. If not found, rolls forward to the latest patch level. If not found, fails. This value is the legacy behavior from the earlier versions of the SDK.
        Feature, // Uses the latest patch level for the specified major, minor, and feature band. If not found, rolls forward to the next higher feature band within the same major/minor and uses the latest patch level for that feature band. If not found, fails.
        Minor,  // Uses the latest patch level for the specified major, minor, and feature band. If not found, rolls forward to the next higher feature band within the same major/minor version and uses the latest patch level for that feature band. If not found, rolls forward to the next higher minor and feature band within the same major and uses the latest patch level for that feature band. If not found, fails.
        Major,  // Uses the latest patch level for the specified major, minor, and feature band. If not found, rolls forward to the next higher feature band within the same major/minor version and uses the latest patch level for that feature band. If not found, rolls forward to the next higher minor and feature band within the same major and uses the latest patch level for that feature band. If not found, rolls forward to the next higher major, minor, and feature band and uses the latest patch level for that feature band. If not found, fails.
        LatestPatch, // Uses the latest installed patch level that matches the requested major, minor, and feature band with a patch level and that is greater or equal than the specified value. If not found, fails.
        LatestFeature,//  Uses the highest installed feature band and patch level that matches the requested major and minor with a feature band that is greater or equal than the specified value. If not found, fails.
        LatestMinor, // Uses the highest installed minor, feature band, and patch level that matches the requested major with a minor that is greater or equal than the specified value. If not found, fails.
        LatestMajor, // Uses the highest installed .NET Core SDK with a major that is greater or equal than the specified value. If not found, fail.
        Disable, // Doesn't roll forward. Exact match required.
    }
}
