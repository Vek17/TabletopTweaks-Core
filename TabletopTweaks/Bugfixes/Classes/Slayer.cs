﻿using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace TabletopTweaks.Bugfixes.Classes {
    class Slayer {
        [HarmonyPatch(typeof(ResourcesLibrary), "InitializeLibrary")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;
            static bool Prefix() {
                if (Initialized) {
                    // When wrath first loads into the main menu InitializeLibrary is called by Kingmaker.GameStarter.
                    // When loading into maps, Kingmaker.Runner.Start will call InitializeLibrary which will
                    // clear the ResourcesLibrary.s_LoadedBlueprints cache which causes loaded blueprints to be garbage collected.
                    // Return false here to prevent ResourcesLibrary.InitializeLibrary from being called twice 
                    // to prevent blueprints from being garbage collected.
                    return false;
                }
                else {
                    return true;
                }
            }
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (Resources.Settings.Slayer.DisableAllFixes) { return; }
                Main.LogHeader("Patching Slayer Resources");
                patchSlayerStudiedTarget();
                Main.LogHeader("Slayer Resource Patch Complete");

                //Do Stuff
            }
        }

        public static void patchSlayerStudiedTarget() {
            if (!Resources.Settings.Slayer.Fixes["StudiedTarget"]) { return; }
            BlueprintBuff SlayerStudiedTargetBuff = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("45548967b714e254aa83f23354f174b0");
            SlayerStudiedTargetBuff.GetComponent<ContextRankConfig>().m_Progression = ContextRankProgression.OnePlusDivStep;
            Main.LogPatch("Patched", SlayerStudiedTargetBuff);
        }
    }
}