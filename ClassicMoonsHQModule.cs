using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using ClassicMoonsHQModule.Patches;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using OPI = ClassicMoonsHQModule.OtherPluginInfos;

namespace ClassicMoonsHQModule;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(OPI.LLL_GUID)]
public class ClassicMoonsHQModule : BaseUnityPlugin
{
    public static ClassicMoonsHQModule Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;
    internal static Harmony? Harmony { get; set; }

    internal readonly static List<PackDefinition> packDefinitions =
        [
        new PackDefinition(
            versionRange: [Versions.v56],
            requiredMods: new Dictionary<string, Version>{
                {OPI.LLL_GUID, new Version("1.3.8") },
                {OPI.PATHFINDINGLAGFIX_GUID, new Version("1.2.2") },
                {OPI.STARLANCERAIFIX_GUID, new Version("3.8.0") },
                {OPI.LETHALLIB_GUID, new Version("0.16.2") },
                {OPI.LOADSTONE_GUID, new Version("0.1.18") },
                {OPI.CULLFACTORY_GUID, new Version("1.7.0") },
                {OPI.DERELICT_MOON_PLUGIN_GUID, new Version("1.0.0") } },
            optionalMods: [],
            disallowedMods: [],
            patches: [
                typeof(LLLConfigLoaderPatcher_v1),
                typeof(SoundManagerPatcher)
                ]
            ),
        new PackDefinition(
            versionRange: [Versions.v62],
            requiredMods: new Dictionary<string, Version>{
                {OPI.LLL_GUID, new Version("1.3.13") },
                {OPI.PATHFINDINGLAGFIX_GUID, new Version("2.1.1") },
                {OPI.PATHFINDINGLIB_GUID, new Version("0.0.14") },
                {OPI.STARLANCERAIFIX_GUID, new Version("3.8.4") },
                {OPI.LETHALLIB_GUID, new Version("1.0.1") },
                {OPI.LOADSTONE_GUID, new Version("0.1.23") },
                {OPI.CULLFACTORY_GUID, new Version("2.0.4") },
                {OPI.DERELICT_MOON_PLUGIN_GUID, new Version("1.2.0") } },
            optionalMods: [],
            disallowedMods: [],
            patches: [
                typeof(LLLConfigLoaderPatcher_v1),
                typeof(SoundManagerPatcher)
                ]
            ),
        new PackDefinition(
            versionRange: [Versions.v64, Versions.v69],
            requiredMods: new Dictionary<string, Version>{
                {OPI.LLL_GUID, new Version("1.4.11") },
                {OPI.PATHFINDINGLAGFIX_GUID, new Version("2.2.4") },
                {OPI.PATHFINDINGLIB_GUID, new Version("2.3.2") },
                {OPI.STARLANCERAIFIX_GUID, new Version("3.9.0") },
                {OPI.LETHALLIB_GUID, new Version("1.0.1") },
                {OPI.LOADSTONE_GUID, new Version("0.1.23") },
                {OPI.CULLFACTORY_GUID, new Version("2.0.4") },
                {OPI.DERELICT_MOON_PLUGIN_GUID, new Version("1.2.0") } },
            optionalMods: [],
            disallowedMods: [],
            patches: [
                typeof(LLLConfigLoaderPatcher_v1),
                typeof(SoundManagerPatcher)
                ]
            ),
        new PackDefinition(
            versionRange: [Versions.v72],
            requiredMods: new Dictionary<string, Version>{
                {OPI.LLL_GUID, new Version("1.4.11") },
                {OPI.PATHFINDINGLAGFIX_GUID, new Version("2.2.4") },
                {OPI.PATHFINDINGLIB_GUID, new Version("2.3.2") },
                {OPI.STARLANCERAIFIX_GUID, new Version("3.11.1") },
                {OPI.LETHALLIB_GUID, new Version("1.1.1") },
                {OPI.LOADSTONE_GUID, new Version("0.1.23") },
                {OPI.CULLFACTORY_GUID, new Version("2.0.4") },
                {OPI.DERELICTFIX_GUID, new Version("1.0.0") },
                {OPI.DERELICT_MOON_PLUGIN_GUID, new Version("1.2.0") } },
            optionalMods: [],
            disallowedMods: [],
            patches: [
                typeof(LLLConfigLoaderPatcher_v1),
                typeof(SoundManagerPatcher)
                ]
            ),
        new PackDefinition(
            versionRange: [Versions.v73],
            requiredMods: new Dictionary<string, Version>{
                {OPI.LLL_GUID, new Version("1.6.8") },
                {OPI.PATHFINDINGLAGFIX_GUID, new Version("2.2.5") },
                {OPI.PATHFINDINGLIB_GUID, new Version("2.4.1") },
                {OPI.STARLANCERAIFIX_GUID, new Version("3.11.1") },
                {OPI.LETHALLIB_GUID, new Version("1.1.1") },
                {OPI.LOADSTONE_GUID, new Version("0.1.23") },
                {OPI.CULLFACTORY_GUID, new Version("2.0.4") },
                {OPI.DERELICT_MOON_PLUGIN_GUID, new Version("1.2.1") } },
            optionalMods: [],
            disallowedMods: [],
            patches: [
                typeof(LLLConfigLoaderPatcher_v2),
                typeof(SoundManagerPatcher)
                ]
            ),
        new PackDefinition(
            versionRange: [Versions.v81],
            requiredMods: new Dictionary<string, Version>{
                {OPI.LLL_GUID, new Version("1.7.13") },
                {OPI.PATHFINDINGLAGFIX_GUID, new Version("2.4.2") },
                {OPI.PATHFINDINGLIB_GUID, new Version("2.4.1") },
                {OPI.STARLANCERAIFIX_GUID, new Version("3.13.2") },
                {OPI.LETHALLIB_GUID, new Version("1.2.0") },
                {OPI.CULLFACTORY_GUID, new Version("2.0.11") },
                {OPI.DERELICT_MOON_PLUGIN_GUID, new Version("1.2.1") } },
            optionalMods: [],
            disallowedMods: [],
            patches: [
                typeof(LLLConfigLoaderPatcher_v2)
                ]
            )
        ];

    // Loaded plugins
    internal static Dictionary<string, PluginInfo> pluginInfos = [];

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

        pluginInfos = Chainloader.PluginInfos;

        Patch();

        SceneManager.sceneLoaded += OnSceneLoad;

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }

    internal static void Patch()
    {
        Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

        Logger.LogDebug("Patching...");

        Harmony.PatchAll(typeof(MenuManagerPatcher));

        if (Chainloader.PluginInfos.TryGetValue(OPI.LLL_GUID, out PluginInfo lllInfo))
        {
            foreach (PackDefinition pack in packDefinitions)
            {
                if (lllInfo.Metadata.Version == pack.RequiredMods[OPI.LLL_GUID])
                {
                    PatchType(pack.Patches);
                    break;
                }
            }
        }

        Logger.LogDebug("Finished patching!");
    }

    internal static void PatchType(Type[] typeArray)
    {
        foreach (Type type in typeArray)
        {
            PatchType(type);
        }
    }

    internal static void PatchType(Type type)
    {
        Logger.LogDebug($"Patching {type.Name}");
        Harmony?.PatchAll(type);
    }

    internal static void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "TutorialMoonScene")
        {
            EditVerdanceScene(scene);
        }
    }

    internal static GameObject FindByName(GameObject[] gameObjects, string name)
    {
        return Array.Find(gameObjects, obj => obj.name == name);
    }

    internal static void EditVerdanceScene(Scene scene)
    {
        Logger.LogInfo("Editing Verdance Scene.");
        Transform dropshipSpawnSpot = scene.GetRootGameObjects()[2].transform.Find("ItemShipAnimContainer/ItemShip/ItemSpawnPositions/DropshipSpawn (1)");
        dropshipSpawnSpot.position += new Vector3(0f, 0.15f, 0f);
    }
    internal enum Versions
    {
        v56 = 56,
        v62 = 62,
        v64 = 64,
        v69 = 69,
        v72 = 72,
        v73 = 73,
        v81 = 81
    }
}
