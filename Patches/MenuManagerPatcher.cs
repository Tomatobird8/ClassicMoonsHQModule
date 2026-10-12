using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ClassicMoonsHQModule.Patches;

[HarmonyPatch(typeof(MenuManager))]
internal class MenuManagerPatcher
{
    private static Action<MenuManager, string, string, bool>? MenuNotif3;
    private static Action<MenuManager, string, string>? MenuNotif2;

    static MenuManagerPatcher()
    {
        MethodInfo? method = AccessTools.Method(typeof(MenuManager), nameof(MenuManager.DisplayMenuNotification));
        if (method != null)
        {
            if (method.GetParameters().Length == 3)
            {
                MenuNotif3 = AccessTools.MethodDelegate<Action<MenuManager, string, string, bool>>(method);
            }
            else
            {
                MenuNotif2 = AccessTools.MethodDelegate<Action<MenuManager, string, string>>(method);
            }
        }
    }

    [HarmonyPatch("Start")]
    [HarmonyPrefix]
    [HarmonyBefore(OtherPluginInfos.VLOG_GUID)]
    internal static void Start_Prefix(MenuManager __instance)
    {
        if (__instance.isInitScene)
        {
            return;
        }
        string invalidSessionReason = "";

        int verNum = GameNetworkManager.Instance.gameVersionNum;

        ClassicMoonsHQModule.Logger.LogInfo("Game version " + verNum);

        // Global required mods
        invalidSessionReason += CheckModValidity(PackDefinition.commonRequiredMods, true);

        bool packFound = false;

        // Version specific required mods
        foreach (PackDefinition pack in ClassicMoonsHQModule.packDefinitions)
        {
            foreach (ClassicMoonsHQModule.Versions version in pack.VersionRange)
            {
                if ((int)version == verNum)
                {
                    invalidSessionReason += CheckModValidity(pack.RequiredMods, true);
                    invalidSessionReason += CheckForBannedMods(pack.DisallowedMods);
                    packFound = true;
                    break;
                }
            }
        }

        if (!packFound)
        {
            invalidSessionReason += "Unsupported game version, ";
        }

        // Global optional mods
        invalidSessionReason += CheckModValidity(PackDefinition.commonOptionalMods, false);

        // VLog special check
        if (!ClassicMoonsHQModule.pluginInfos.ContainsKey(OtherPluginInfos.VLOG_GUID))
        {
            invalidSessionReason += "VLog missing, ";
        }

        // Display warning
        if (!invalidSessionReason.IsNullOrWhiteSpace())
        {
            invalidSessionReason = invalidSessionReason.TrimEnd(',', ' ');
            ClassicMoonsHQModule.Logger.LogWarning($"WARNING! Modpack misconfiguration: {invalidSessionReason}");
            if (MenuNotif3 != null)
            {
                MenuNotif3(__instance, $"WARNING! Modpack misconfiguration: {invalidSessionReason}", "[ OK ]", true);
            }
            else if (MenuNotif2 != null)
            {
                MenuNotif2(__instance, $"WARNING! Modpack misconfiguration: {invalidSessionReason}", "[ OK ]");
            }
            else
            {
                ClassicMoonsHQModule.Logger.LogError("Displaying menu notification failed.");
            }
        }
    }

    internal static string CheckModValidity(Dictionary<string, Version> dict, bool required)
    {
        string invalidSessionReason = "";
        foreach (KeyValuePair<string, Version> entry in dict)
        {
            if (!ClassicMoonsHQModule.pluginInfos.ContainsKey(entry.Key))
            {
                if (required) invalidSessionReason += $"{entry.Key} v{entry.Value} is misssing, ";
                continue;
            }
            else if (ClassicMoonsHQModule.pluginInfos[entry.Key].Metadata.Version != entry.Value)
            {
                if (!required && ClassicMoonsHQModule.pluginInfos[entry.Key].Metadata.Version <= entry.Value)
                {
                    continue;
                }
                invalidSessionReason += $"{ClassicMoonsHQModule.pluginInfos[entry.Key].Metadata.GUID} v{ClassicMoonsHQModule.pluginInfos[entry.Key].Metadata.Version.ToString()} didnt match required version v{entry.Value}, ";
            }
        }
        return invalidSessionReason;
    }

    internal static string CheckForBannedMods(string[] names)
    {
        foreach (string s in names)
        {
            if (ClassicMoonsHQModule.pluginInfos.ContainsKey(s)) return "Invalid mods found - please reinstall the pack, ";
        }
        return "";
    }
}
