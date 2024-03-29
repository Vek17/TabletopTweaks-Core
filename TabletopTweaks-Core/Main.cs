﻿using HarmonyLib;
using Kingmaker.EntitySystem.Persistence.Versioning;
using TabletopTweaks.Core.ModLogic;
using TabletopTweaks.Core.SaveUpgrades;
using TabletopTweaks.Core.Utilities;
using UnityModManagerNet;

namespace TabletopTweaks.Core {
    internal static class Main {
        public static ModContextTTTCore TTTContext;
        static bool Load(UnityModManager.ModEntry modEntry) {
            var harmony = new Harmony(modEntry.Info.Id);
            TTTContext = new ModContextTTTCore(modEntry);
            TTTContext.LoadAllSettings();
            TTTContext.ModEntry.OnSaveGUI = OnSaveGUI;
            harmony.PatchAll();
            PostPatchInitializer.Initialize(TTTContext);
            RegisterUpgrades();
            return true;
        }
        private static void RegisterUpgrades() {
            JsonUpgradeSystem.Register(-117, "Migrate TabletopTweaks to TabletopTweaks-Core", new TabletopTweaksMigration(TTTContext));
        }
        private static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
        }
    }
}