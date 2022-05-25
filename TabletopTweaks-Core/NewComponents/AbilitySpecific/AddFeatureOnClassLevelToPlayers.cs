using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {

    [TypeId("fee1dc0c040845ba8abcdca526d0f52e")]
    public class AddFeatureOnClassLevelToPlayers : AddFeatureOnClassLevel {

        private new bool IsFeatureShouldBeApplied() {
            return IsValidUnit(base.Owner);
        }

        private bool IsValidUnit(UnitDescriptor unit) {
            if (unit == null) { return false; }
            var result = unit.Unit.IsMainCharacter
                || unit.Unit.IsCloneOfMainCharacter
                || unit.Unit.IsStoryCompanion()
                || unit.Unit.IsCustomCompanion()
                || (unit.Blueprint?.IsCompanion ?? false);
            return Not ? !result : result;
        }
        public bool Not = false;

        [HarmonyPatch(typeof(AddFeatureOnClassLevel), nameof(AddFeatureOnClassLevel.IsFeatureShouldBeApplied))]
        static class LocalizationManager_OnLocaleChanged_Handler {
            static void Postfix(AddFeatureOnClassLevel __instance, ref bool __result) {
                var componet = __instance as AddFeatureOnClassLevelToPlayers;
                if (componet != null) { __result &= componet.IsFeatureShouldBeApplied(); }
            }
        }
    }
}
