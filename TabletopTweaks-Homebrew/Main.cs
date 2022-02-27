using HarmonyLib;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
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
            Helpers.CreateString("test", "");
            new AddStatBonus() { 
                Descriptor = ModifierDescriptor.UntypedStackable,
                Stat = StatType.Dexterity,
                Value = 2
            };
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            //ModSettings.SaveSettings("Fixes.json", ModSettings.Fixes);
            //ModSettings.SaveSettings("AddedContent.json", ModSettings.AddedContent);
            //ModSettings.SaveSettings("Homebrew.json", ModSettings.Homebrew);
        }
    }
}
