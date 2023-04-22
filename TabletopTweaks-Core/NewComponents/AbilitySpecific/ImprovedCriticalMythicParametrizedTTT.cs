using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.RuleSystem;
using TabletopTweaks.Core.Utilities;
using Kingmaker.UnitLogic.Mechanics;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [TypeId("5f0b94b3bdaf464bbefc9869e39804c2")]
    public class ImprovedCriticalMythicParametrizedTTT : UnitFactComponentDelegate, 
        IInitiatorRulebookHandler<RuleCalculateDamage>, 
        IRulebookHandler<RuleCalculateDamage>, 
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateDamage evt) {
            var attackRoll = evt.ParentRule?.AttackRoll;
            if (attackRoll == null) { return; }
            if (!attackRoll.IsCriticalConfirmed && !attackRoll.FortificationNegatesCriticalHit) { return; }
            if (attackRoll.Weapon?.Blueprint?.Category != base.Param) { return; }

            evt.DamageBundle.WeaponDamage.TemporaryContext(damage => { 
                damage.Dice.Modify(
                    new DiceFormula() {
                        m_Dice = evt.DamageBundle.WeaponDamage.Dice.ModifiedValue.Dice,
                        m_Rolls = evt.DamageBundle.WeaponDamage.Dice.ModifiedValue.Rolls * DiceMultiplier.Calculate(base.Context),
                    },
                    base.Fact
                );
            });
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt) {
        }

        public ContextValue DiceMultiplier = 2;
    }
}
