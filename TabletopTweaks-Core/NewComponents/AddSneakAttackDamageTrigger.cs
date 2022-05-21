using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using System.Linq;

namespace TabletopTweaks.Core.NewComponents {
    [AllowMultipleComponents]
    [TypeId("ab899b2357d14767bb091548f9cee543")]
    public class AddSneakAttackDamageTrigger : EntityFactComponentDelegate,
        IInitiatorRulebookHandler<RulePrepareDamage>,
        IRulebookHandler<RulePrepareDamage>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RulePrepareDamage evt) {
        }

        public void OnEventDidTrigger(RulePrepareDamage evt) {
            if (evt.ParentRule.DamageBundle.Any(baseDamage => baseDamage.Sneak) && !evt.ParentRule.DisablePrecisionDamage) {
                base.Fact.RunActionInContext(Action, evt.Target);
            }
        }

        public ActionList Action;
    }
}
