using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.Utilities;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [TypeId("5f0b94b3bdaf464bbefc9869e39804c2")]
    public class MaximizedCriticalComponent : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateDamage>,
        IRulebookHandler<RuleCalculateDamage>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateDamage evt) {
            var attackRoll = evt.ParentRule?.AttackRoll;
            if (attackRoll == null) { return; }
            if (!attackRoll.IsCriticalConfirmed && !attackRoll.FortificationNegatesCriticalHit) { return; }

            evt.DamageBundle.WeaponDamage.TemporaryContext(damage => {
                damage.CalculationType.Set(DamageCalculationType.Maximized, base.Fact);
            });
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt) {
        }
    }
}
