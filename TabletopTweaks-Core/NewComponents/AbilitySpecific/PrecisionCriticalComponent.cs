using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.NewUnitParts;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    /// <summary>
    /// Enables critical damage for precision damage with the specified multiplier.
    /// </summary>
    public class PrecisionCriticalComponent : UnitFactComponentDelegate, 
        IInitiatorRulebookHandler<RuleCalculateDamage>, 
        IRulebookHandler<RuleCalculateDamage>, 
        ISubscriber, IInitiatorRulebookSubscriber {

        public override void OnTurnOn() {
            base.Owner.Ensure<UnitPartPrecisionCritical>().AddEntry(CriticalMultiplier, Additional, base.Fact);
        }

        public override void OnTurnOff() {
            base.Owner.Ensure<UnitPartPrecisionCritical>().RemoveEntry(base.Fact);
        }

        public void OnEventAboutToTrigger(RuleCalculateDamage evt) {
            if ((evt.ParentRule?.DamageBundle?.WeaponDamage?.CriticalModifier ?? 1) < 2) { return; }
            var PrecisionCritical = base.Owner.Get<UnitPartPrecisionCritical>();

            foreach (var baseDamage in evt.ParentRule.DamageBundle) {
                if (baseDamage.Precision || baseDamage.Sneak) {
                    baseDamage.CriticalModifier = PrecisionCritical?.GetMultiplier();
                }
            }
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt) {
        }
        /// <summary>
        /// Critical multiplier to use for precision damage.
        /// </summary>
        public int CriticalMultiplier = 2;
        /// <summary>
        /// Allow the multiplier to stack with other Precision Critical increases.
        /// </summary>
        public bool Additional;
    }
}
