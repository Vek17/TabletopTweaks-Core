using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using TabletopTweaks.Core.NewUnitParts;
using TabletopTweaks.Core.Utilities;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    /// <summary>
    /// Ability Restriction that blocks usage unless the caster has a black blade equiped.
    /// </summary>
    [TypeId("a0ff3623a0154448a082b1c5ea9898fc")]
    public class AbilityRequirementHasBlackBlade : BlueprintComponent, IAbilityRestriction {
        [InitializeStaticString]
        private static readonly LocalizedString UIText = Helpers.CreateString("AbilityRequirementHasBlackBlade.UI", "You must be wielding your Black Blade");
        public string GetAbilityRestrictionUIText() {
            return UIText;
        }

        public bool IsAbilityRestrictionPassed(AbilityData ability) {
            var BlackBlade = ability.Caster.Get<UnitPartBlackBlade>();
            var primaryWeapon = ability.Caster.Body.PrimaryHand?.MaybeWeapon;
            var secondaryWeapon = ability.Caster.Body.SecondaryHand?.MaybeWeapon;
            if (BlackBlade == null || (primaryWeapon == null && secondaryWeapon == null)) { return false; }
            return BlackBlade.IsBlackBlade(primaryWeapon) || BlackBlade.IsBlackBlade(secondaryWeapon);
        }
    }
}
