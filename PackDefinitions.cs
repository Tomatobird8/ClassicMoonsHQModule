using System;
using System.Collections.Generic;
using OPI = ClassicMoonsHQModule.OtherPluginInfos;

namespace ClassicMoonsHQModule;

internal class PackDefinition(ClassicMoonsHQModule.Versions[] versionRange, Dictionary<string, Version> requiredMods, Dictionary<string, Version> optionalMods, string[] disallowedMods, Type[] patches)
{
    public ClassicMoonsHQModule.Versions[] VersionRange = versionRange;
    public Dictionary<string, Version> RequiredMods = requiredMods;
    public Dictionary<string, Version> OptionalMods = optionalMods;
    public string[] DisallowedMods = disallowedMods;
    public Type[] Patches = patches;

    // --- MOD VERSION DEFINITIONS ---
    internal static Dictionary<string, Version> commonRequiredMods = new()
    {
        {OPI.BLURANCEFIX_GUID, new Version("1.0.0") },
        {OPI.LERTZFIX_GUID, new Version("1.0.0") },
        {OPI.TANDRAUSFIX_GUID, new Version("1.1.0") }
    };

    internal static Dictionary<string, Version> commonOptionalMods = new()
    {
        {OPI.FREEMOONS_GUID, new Version("1.0.2") } // 1.2.0 on TS
    };
}
