using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.MechanicsChanges {
    class MountedCombatModifiers {

        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (TTTContext.Fixes.BaseFixes.IsDisabled("FixMountedLongspearModifer")) { return; }
                TTTContext.Logger.LogHeader("Patching Mounted Effects");
                FixModifers();

            }
            static void FixModifers() {
                var ChargeBuff = Resources.GetBlueprint<BlueprintBuff>("f36da144a379d534cad8e21667079066");
                var MountedBuff = Resources.GetBlueprint<BlueprintBuff>("b2d13e8f3bb0f1d4c891d71b4d983cf7");
                var LongspearChargeBuff = Resources.GetModBlueprint<BlueprintBuff>("LongspearChargeBuff");

                MountedBuff.AddComponent(Helpers.Create<BuffExtraEffectsRequirements>(c => {
                    c.CheckedBuff = ChargeBuff.ToReference<BlueprintBuffReference>();
                    c.CheckWeaponCategory = true;
                    c.WeaponCategory = WeaponCategory.Longspear;
                    c.ExtraEffectBuff = LongspearChargeBuff.ToReference<BlueprintBuffReference>();
                }));
                TTTContext.Logger.LogPatch("Patched", MountedBuff);
            }
        }
    }
}
