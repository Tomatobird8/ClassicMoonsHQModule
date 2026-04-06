using System.Collections.Generic;
using OPI = ClassicMoonsHQModule.OtherPluginInfos;

namespace ClassicMoonsHQModule
{
    internal class PackDefinition
    {
        // --- MOD VERSION DEFINITIONS ---
        internal static Dictionary<string, string> commonRequiredMods = new Dictionary<string, string>
        {
        };

        internal static Dictionary<string, string> commonOptionalMods = new Dictionary<string, string>
        {
            {OPI.FREEMOONS_GUID, "1.0.2" } // 1.2.0 on TS
        };

        internal static Dictionary<string, string> v56Mods = new Dictionary<string, string>
        {
            {OPI.LLL_GUID, "1.3.8" },
            {OPI.PATHFINDINGLAGFIX_GUID, "1.2.2" },
            {OPI.STARLANCERAIFIX_GUID, "3.8.4" },
            {OPI.LETHALLIB_GUID, "0.16.2" },
            {OPI.LOADSTONE_GUID, "0.1.18" },
            {OPI.CULLFACTORY_GUID, "1.7.0" }
        };

        internal static Dictionary<string, string> v62Mods = new Dictionary<string, string>
        {
            {OPI.LLL_GUID, "1.3.13" },
            {OPI.PATHFINDINGLAGFIX_GUID, "2.1.1" },
            {OPI.PATHFINDINGLIB_GUID, "0.0.14" },
            {OPI.STARLANCERAIFIX_GUID, "3.9.0" }, // 3.9.1 on TS
            {OPI.LETHALLIB_GUID, "1.0.1" },
            {OPI.LOADSTONE_GUID, "0.1.23" },
            {OPI.CULLFACTORY_GUID, "2.0.4" }
        };

        internal static Dictionary<string, string> v69Mods = new Dictionary<string, string>
        {
            {OPI.LLL_GUID, "1.4.11" },
            {OPI.PATHFINDINGLAGFIX_GUID, "2.2.4" },
            {OPI.PATHFINDINGLIB_GUID, "2.3.2" },
            {OPI.STARLANCERAIFIX_GUID, "3.9.0" }, // 3.9.1 on TS
            {OPI.LETHALLIB_GUID, "1.0.1" },
            {OPI.LOADSTONE_GUID, "0.1.23" },
            {OPI.CULLFACTORY_GUID, "2.0.4" }
        };

        internal static Dictionary<string, string> v72Mods = new Dictionary<string, string>
        {
            {OPI.LLL_GUID, "1.4.11" },
            {OPI.PATHFINDINGLAGFIX_GUID, "2.2.4" },
            {OPI.PATHFINDINGLIB_GUID, "2.3.2" },
            {OPI.STARLANCERAIFIX_GUID, "3.11.1" },
            {OPI.LETHALLIB_GUID, "1.1.1" },
            {OPI.LOADSTONE_GUID, "0.1.23" },
            {OPI.CULLFACTORY_GUID, "2.0.4" }
        };

        internal static Dictionary<string, string> v73Mods = new Dictionary<string, string>
        {
            {OPI.LLL_GUID, "1.6.8" },
            {OPI.PATHFINDINGLAGFIX_GUID, "2.2.5" },
            {OPI.PATHFINDINGLIB_GUID, "2.4.1" },
            {OPI.STARLANCERAIFIX_GUID, "3.11.1" },
            {OPI.LETHALLIB_GUID, "1.1.1" },
            {OPI.LOADSTONE_GUID, "0.1.23" },
            {OPI.CULLFACTORY_GUID, "2.0.4" }
        };
    }
}
