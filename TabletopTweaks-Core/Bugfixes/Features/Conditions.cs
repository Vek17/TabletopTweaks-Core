using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.Bugfixes.Features {
    class Conditions {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Conditions");
                PatchStaggered();
                PatchNauseated();

                static void PatchStaggered() {
                    if (TTTContext.Fixes.BaseFixes.IsDisabled("StaggeredDescriptors")) { return; }
                    var Staggered = Resources.GetBlueprint<BlueprintBuff>("df3950af5a783bd4d91ab73eb8fa0fd3");
                    Staggered.GetComponent<SpellDescriptorComponent>().Descriptor = SpellDescriptor.Staggered;
                    TTTContext.Logger.LogPatch("Patched", Staggered);
                }

                static void PatchNauseated() {
                    if (TTTContext.Fixes.BaseFixes.IsDisabled("NauseatedDescriptors")) { return; }
                    var Nauseated = Resources.GetBlueprint<BlueprintBuff>("956331dba5125ef48afe41875a00ca0e");
                    Nauseated.GetComponent<SpellDescriptorComponent>().Descriptor = SpellDescriptor.Nauseated;
                    TTTContext.Logger.LogPatch("Patched", Nauseated);
                }
            }
        }
    }
}
