using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Utility;
using TabletopTweaks.Core.NewUnitParts;
using TabletopTweaks.Core.Utilities;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [AllowMultipleComponents]
    [TypeId("ae92531991984946a1708bb00f1f48b1")]
    public class AbilityTargetNoSplitHexRepeat : BlueprintComponent, IAbilityTargetRestriction {
        [InitializeStaticString]
        private static readonly LocalizedString SplitHex = Helpers.CreateString(modContext: Main.TTTContext, "AbilityTargetNoSplitHexRepeat.UI", "Cannot target the same unit twice with Split Hex.");

        public string GetAbilityTargetRestrictionUIText(UnitEntityData caster, TargetWrapper target) {
            return SplitHex;
        }

        public bool IsTargetRestrictionPassed(UnitEntityData caster, TargetWrapper target) {
            var SplitHexPart = caster.Get<UnitPartSplitHex>();
            var Ability = base.OwnerBlueprint as BlueprintAbility;
            var Unit = target.Unit;
            if (SplitHexPart == null || Ability == null || Unit == null) { return true; }
            return SplitHexPart.ValidTarget(Ability, Unit);
        }
    }
}
