using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System.Linq;

namespace TabletopTweaks.Core.NewComponents {
    [ClassInfoBox("This component applies only for polymorphed form. It expected to be placed on the same fact with components AddOutgoingPhysicalDamageProperty present.")]
    [TypeId("b0f80e02154a46c5aacafbb776064b3e")]
    public class PolymorphDamagePropertyTransfer : UnitFactComponentDelegate,
        ISubscriber, IInitiatorRulebookSubscriber,
        IInitiatorRulebookHandler<RulePrepareDamage>, IRulebookHandler<RulePrepareDamage> {

        public void OnEventDidTrigger(RulePrepareDamage evt) {
            if (PolymorphDamageTransfer.IsApplicableToWeapon(evt.ParentRule.DamageBundle.Weapon, base.Owner)) {
                this.TransferPhysicalProperties(evt);
            }
        }

        private void TransferPhysicalProperties(RulePrepareDamage evt) {
            AddOutgoingPhysicalDamageProperty component = base.Fact.Blueprint.GetComponent<AddOutgoingPhysicalDamageProperty>();
            if (component == null) {
                return;
            }
            evt.DamageBundle.OfType<PhysicalDamage>().ForEach(damage => {
                component.ApplyProperties(damage);
            });
        }

        public void OnEventAboutToTrigger(RulePrepareDamage evt) {
        }
    }
}
