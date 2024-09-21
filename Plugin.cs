using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HealthDrink.Behavior;
using LethalLib.Modules;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace HealthDrink
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string modGUID = "Nono.HealthDrink";
        private const string modName = "Health Drink";
        private const string modVersion = "1.0.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static Plugin Instance;

        internal new static ManualLogSource Logger;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            // Spawning item stuff
            string assetDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "healthitem");
            AssetBundle bundle = AssetBundle.LoadFromFile(assetDir);

            Item healthDrink = bundle.LoadAsset<Item>("Assets/HealthDrink.asset");
            HealPlayer healScript = healthDrink.spawnPrefab.AddComponent<HealPlayer>();
            healScript.grabbable = true;
            healScript.grabbableToEnemies = true;
            healScript.itemProperties = healthDrink; 

            NetworkPrefabs.RegisterNetworkPrefab(healthDrink.spawnPrefab);
            Utilities.FixMixerGroups(healthDrink.spawnPrefab);
            Items.RegisterScrap(healthDrink, 50, Levels.LevelTypes.All); 


            Logger = base.Logger;
            Logger.LogInfo("It's healing time!!!!");

            harmony.PatchAll(typeof(Plugin));
        }
    }
}
