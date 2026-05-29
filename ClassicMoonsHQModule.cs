using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using ClassicMoonsHQModule.Patches;
using HarmonyLib;
using System;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClassicMoonsHQModule
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class ClassicMoonsHQModule : BaseUnityPlugin
    {
        public static ClassicMoonsHQModule Instance { get; private set; } = null!;
        internal new static ManualLogSource Logger { get; private set; } = null!;
        internal static Harmony? Harmony { get; set; }

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

            if (Chainloader.PluginInfos.TryGetValue(OtherPluginInfos.LLL_GUID, out PluginInfo pluginInfo))
            {
                if (pluginInfo.Metadata.Version >= new Version(PackDefinition.v73Mods[OtherPluginInfos.LLL_GUID]))
                {
                    Harmony.PatchAll(typeof(LLLConfigLoaderPatcher_v2));
                }
                else
                {
                    Harmony.PatchAll(typeof(LLLConfigLoaderPatcher_v1));
                }
                if (pluginInfo.Metadata.Version == new Version(PackDefinition.v81Mods[OtherPluginInfos.LLL_GUID]))
                {
                    SceneManager.sceneLoaded += FixLevelGenerationRoot; // don't patch LLL v1.7.0 onwards
                }
                Harmony.PatchAll(typeof(SoundManagerPatcher)); // don't patch LLL v1.7.0 onwards
            }

            Logger.LogDebug("Finished patching!");
        }

        internal static void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "TutorialMoonScene")
            {
                EditVerdanceScene(scene);
            }
        }

        internal static void FixLevelGenerationRoot(Scene scene, LoadSceneMode mode) // Temp fix for v81 (LLL v1.6.9)
        {
            GameObject? Systems = FindByName(scene.GetRootGameObjects(), "Systems");
            if (Systems == null)
            {
                return;
            }
            Transform? LevelGeneration = Systems.transform.Find("LevelGeneration");
            if (LevelGeneration == null)
            {
                return;
            }
            Transform? firstChild = LevelGeneration.GetChild(0);
            if (firstChild == null)
            {
                return;
            }
            if (firstChild.name == "LevelGenerationRoot")
            {
                firstChild.SetAsLastSibling();
                Logger.LogDebug("Reordered LevelGeneration child objects.");
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
    }
}
