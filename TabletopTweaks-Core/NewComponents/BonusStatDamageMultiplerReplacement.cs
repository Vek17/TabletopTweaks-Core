using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace TabletopTweaks.Core.NewComponents {
    [TypeId("eceed4ce1340485195bb13ce1f0ac06c")]
    public class BonusStatDamageMultiplerReplacement : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
        IRulebookHandler<RuleCalculateWeaponStats>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
            if (CheckWeaponCategory && evt.Weapon.Blueprint.Category != WeaponCategory) { return; }
            evt.OverrideDamageBonusStatMultiplier(this.Multiplier);
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
        }

        public float Multiplier;
        public bool CheckWeaponCategory;
        public WeaponCategory WeaponCategory;
    }
}