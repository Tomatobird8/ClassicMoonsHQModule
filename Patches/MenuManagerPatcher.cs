using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.IL2CPP.CompilerServices;

namespace ClassicMoonsHQModule.Patches
{
    [HarmonyPatch(typeof(MenuManager))]
    internal class MenuManagerPatcher
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        internal static void Start_Postfix(MenuManager __instance)
        {
            string invalidSessionReason = "";

            int verNum = GameNetworkManager.Instance.gameVersionNum;

            // Global required mods
            invalidSessionReason += CheckModValidity(PackDefinition.commonRequiredMods, true);

            // Version specific required mods
            switch (verNum)
            {
                case 56:
                    invalidSessionReason += CheckModValidity(PackDefinition.v56Mods, true);
                    break;

                case 62:
                    invalidSessionReason += CheckModValidity(PackDefinition.v62Mods, true);
                    break;

                case 64:
                    invalidSessionReason += CheckModValidity(PackDefinition.v69Mods, true);
                    break;

                case 69:
                    invalidSessionReason += CheckModValidity(PackDefinition.v69Mods, true);
                    break;

                case 72:
                    invalidSessionReason += CheckModValidity(PackDefinition.v72Mods, true);
                    break;

                case 73:
                    invalidSessionReason += CheckModValidity(PackDefinition.v73Mods, true);
                    break;
                default:
                    invalidSessionReason += "Unsupported game version";
                    break;
            }

            // Global optional mods
            invalidSessionReason += CheckModValidity(PackDefinition.commonOptionalMods, false);

            // Vlog special check
            if (!ClassicMoonsHQModule.pluginInfos.ContainsKey(OtherPluginInfos.VLOG_GUID))
            {
                invalidSessionReason += "Vlog missing, ";
            }

            // Display warning
            if (!invalidSessionReason.IsNullOrWhiteSpace())
            {
                invalidSessionReason = invalidSessionReason.TrimEnd(',', ' ');
                __instance.DisplayMenuNotification($"WARNING! Modpack misconfiguration: {invalidSessionReason}", "[ OK ]");
                ClassicMoonsHQModule.Logger.LogWarning($"WARNING! Modpack misconfiguration: {invalidSessionReason}");
            }
        }

        internal static string CheckModValidity(Dictionary<string, string> dict, bool required)
        {
            string invalidSessionReason = "";
            foreach (KeyValuePair<string, string> entry in dict)
            {
                if (!ClassicMoonsHQModule.pluginInfos.ContainsKey(entry.Key))
                {
                    if (required) invalidSessionReason += $"{entry.Key} v{entry.Value} is misssing, ";
                    continue;
                }
                else if (ClassicMoonsHQModule.pluginInfos[entry.Key].Metadata.Version.ToString() != entry.Value)
                {
                    invalidSessionReason += $"{ClassicMoonsHQModule.pluginInfos[entry.Key].Metadata.GUID} v{ClassicMoonsHQModule.pluginInfos[entry.Key].Metadata.Version.ToString()} didnt match required version v{entry.Value}, ";
                }
            }
            return invalidSessionReason;
        }
    }
}
