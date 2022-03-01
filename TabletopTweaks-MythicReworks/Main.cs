using HarmonyLib;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.MythicReworks.ModLogic;
using UnityModManagerNet;

namespace TabletopTweaks.MythicReworks {
    static class Main {
        public static bool Enabled;
        public static ModContextTTTMythicReworks TTTContext;
        static bool Load(UnityModManager.ModEntry modEntry) {
            var harmony = new Harmony(modEntry.Info.Id);
            TTTContext = new ModContextTTTMythicReworks(modEntry);
            TTTContext.LoadAllSettings();
            TTTContext.ModEntry.OnSaveGUI = OnSaveGUI;
            TTTContext.ModEntry.OnGUI = UMMSettingsUI.OnGUI;
            harmony.PatchAll();
            PostPatchInitializer.Initialize(TTTContext);
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            TTTContext.Blueprints.RemoveUnused();
            TTTContext.SaveSettings(TTTContext.BlueprintsFile, TTTContext.Blueprints);
        }
    }
}
