using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using TabletopTweaks.Core.NewUnitParts;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    /// <summary>
    /// Overides focuses weapon's damage dice based on the focused weapon damage formula.
    /// </summary>
    [AllowedOn(typeof(BlueprintUnitFact))]
    [TypeId("a945c1d2b2d44247bd37d651665d4f54")]
    public class FocusedWeaponDamageComponent : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
        IRulebookHandler<RuleCalculateWeaponStats>,
        ISubscriber,
        IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
            if (IsValidWeapon(evt.Weapon)) {
                var classLevel = FighterWeaponTrainingProperty.Get().GetInt(this.Owner);
                DiceFormula? formula = classLevel switch {
                    >= 1 and < 5 => new DiceFormula(1, DiceType.D6),
                    >= 5 and < 10 => new DiceFormula(1, DiceType.D8),
                    >= 10 and < 15 => new DiceFormula(1, DiceType.D10),
                    >= 15 and < 20 => new DiceFormula(2, DiceType.D6),
                    >= 20 => new DiceFormula(2, DiceType.D8),
                    _ => null
                };
                if (formula is not null) {
                    evt.WeaponDamageDice.Modify(formula.Value, base.Fact);
                }
            }
        }

        public bool HasWeaponTraining(ItemEntityWeapon weapon) {
            var weaponTaining = this.Owner.Get<UnitPartWeaponTraining>();
            return (weaponTaining?.GetWeaponRank(weapon) > 0);
        }

        public bool IsValidWeapon(ItemEntityWeapon weapon) {
            var focusedWeaponPart = base.Owner.Ensure<UnitPartFocusedWeapon>();
            return HasWeaponTraining(weapon) && focusedWeaponPart.HasEntry(weapon.Blueprint.Category);
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
        }

        public BlueprintUnitPropertyReference FighterWeaponTrainingProperty;
    }
}
