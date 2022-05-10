using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using TabletopTweaks.Core.Utilities;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    /// <summary>
    /// Converts the outgoing weapon damage to the supplied type.
    /// </summary>
    [TypeId("2f13fd3858d642d3ba16351be5168254")]
    public class WeaponBlackBladeElementalAttunement : WeaponEnchantmentLogic, 
        IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
        IRulebookHandler<RuleCalculateWeaponStats>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
            if (evt.Weapon != this.Owner) { return; }
            var originalDescription = evt.DamageDescription[0].TypeDescription;
            evt.DamageDescription[0].TypeDescription = Type;
            evt.DamageDescription[0].TypeDescription.TemporaryContext(d => {
                d.Physical.Enhancement = originalDescription.Physical.Enhancement;
                d.Physical.EnhancementTotal = originalDescription.Physical.EnhancementTotal;
                d.Common.Alignment = originalDescription.Common.Alignment;
            });
        }
        /// <summary>
        /// Type to convert damage to.
        /// </summary>
        public DamageTypeDescription Type;
    }
}
