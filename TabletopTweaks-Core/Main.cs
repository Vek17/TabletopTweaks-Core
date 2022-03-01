using HarmonyLib;
using Kingmaker.EntitySystem.Persistence.Versioning;
using TabletopTweaks.Core.ModLogic;
using TabletopTweaks.Core.SaveUpgrades;
using TabletopTweaks.Core.Utilities;
using UnityModManagerNet;

namespace TabletopTweaks.Core {
    internal static class Main {
        private static bool Enabled;
        public static ModContextTTTCore TTTContext;
        static bool Load(UnityModManager.ModEntry modEntry) {
            var harmony = new Harmony(modEntry.Info.Id);
            TTTContext = new ModContextTTTCore(modEntry);
            TTTContext.LoadAllSettings();
            TTTContext.ModEntry.OnSaveGUI = OnSaveGUI;
            TTTContext.ModEntry.OnGUI = UMMSettingsUI.OnGUI;
            harmony.PatchAll();
            PostPatchInitializer.Initialize(TTTContext);
            RegisterUpgrades();
            return true;
        }
        private static void RegisterUpgrades() {
            JsonUpgradeSystem.Register(-117, "Migrate TabletopTweaks to TabletopTweaks-Core", new TabletopTweaksMigration(TTTContext));
        }
        private static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            TTTContext.SaveSettings("Fixes.json", TTTContext.Fixes);
            TTTContext.SaveSettings("AddedContent.json", TTTContext.AddedContent);
            TTTContext.SaveSettings("Homebrew.json", TTTContext.Homebrew);
        }
    }
}