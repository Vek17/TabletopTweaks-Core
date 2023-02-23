using HarmonyLib;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Parts;
using System;
using TabletopTweaks.Core.NewUnitParts;

namespace TabletopTweaks.Core.MechanicsChanges {
    public static class AdditionalActivatableAbilityGroups {
        public enum ExtentedActivatableAbilityGroup : int {
            CavalierCharge = 1000
        }

        private static bool IsTTTGroup(this ActivatableAbilityGroup group) {
            return Enum.IsDefined(typeof(ExtentedActivatableAbilityGroup), (int)group);
        }

        [HarmonyPatch(typeof(UnitPartActivatableAbility), nameof(UnitPartActivatableAbility.IncreaseGroupSize), new Type[] { typeof(ActivatableAbilityGroup) })]
        static class UnitPartActivatableAbility_IncreaseGroupSize_Patch {

            static bool Prefix(UnitPartActivatableAbility __instance, ActivatableAbilityGroup group) {
                if (group.IsTTTGroup()) {
                    var extensionPart = __instance.Owner.Ensure<UnitPartActivatableAbilityGroupExtension>();
                    extensionPart.IncreaseGroupSize((ExtentedActivatableAbilityGroup)group);
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(UnitPartActivatableAbility), nameof(UnitPartActivatableAbility.DecreaseGroupSize), new Type[] { typeof(ActivatableAbilityGroup) })]
        static class UnitPartActivatableAbility_DecreaseGroupSize_Patch {

            static bool Prefix(UnitPartActivatableAbility __instance, ActivatableAbilityGroup group) {
                if (group.IsTTTGroup()) {
                    var extensionPart = __instance.Owner.Ensure<UnitPartActivatableAbilityGroupExtension>();
                    extensionPart.DecreaseGroupSize((ExtentedActivatableAbilityGroup)group);
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(UnitPartActivatableAbility), nameof(UnitPartActivatableAbility.GetGroupSize), new Type[] { typeof(ActivatableAbilityGroup) })]
        static class UnitPartActivatableAbility_GetGroupSize_Patch {

            static bool Prefix(UnitPartActivatableAbility __instance, ActivatableAbilityGroup group, ref int __result) {
                if (group.IsTTTGroup()) {
                    var extensionPart = __instance.Owner.Ensure<UnitPartActivatableAbilityGroupExtension>();
                    __result = extensionPart.GetGroupSize((ExtentedActivatableAbilityGroup)group);
                    return false;
                }
                return true;
            }
        }
 
        //[HarmonyPatch(typeof(UnitPartActivatableAbility), nameof(UnitPartActivatableAbility.PersistentGroupsSizeIncreases), MethodType.Getter)]
        static class UnitPartActivatableAbility_PersistentGroupsSizeIncreases_Getter_Patch {

            static void Postfix(UnitPartActivatableAbility __instance, ref int[] __result) {

            }
        }

        //[HarmonyPatch(typeof(UnitPartActivatableAbility), nameof(UnitPartActivatableAbility.PersistentGroupsSizeIncreases), MethodType.Setter)]
        static class UnitPartActivatableAbility_PersistentGroupsSizeIncreases_Setter_Patch {

            static void Postfix(UnitPartActivatableAbility __instance) {

            }
        }
    }
}
