using HarmonyLib;
using TabletopTweaks.Core.Utilities;
using UnityModManagerNet;

namespace TabletopTweaks.Homebrew {
    static class Main {
        public static bool Enabled;
        static bool Load(UnityModManager.ModEntry modEntry) {
            var harmony = new Harmony(modEntry.Info.Id);
            //ModSettings.ModEntry = modEntry;
            //ModSettings.LoadAllSettings();
            //ModSettings.ModEntry.OnSaveGUI = OnSaveGUI;
            //ModSettings.ModEntry.OnGUI = UMMSettingsUI.OnGUI;
            harmony.PatchAll();
            PostPatchInitializer.Initialize();
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            //ModSettings.SaveSettings("Fixes.json", ModSettings.Fixes);
            //ModSettings.SaveSettings("AddedContent.json", ModSettings.AddedContent);
            //ModSettings.SaveSettings("Homebrew.json", ModSettings.Homebrew);
        }
    }
}
