using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace TabletopTweaks.Core.NewComponents {
    [TypeId("5b61a8c461f941f497abf23f24abd66a")]
    public class SecondaryWeaponOverride : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
        IRulebookHandler<RuleCalculateWeaponStats>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
            if (CheckWeaponCategory && evt.Weapon.Blueprint.Category != WeaponCategory) { return; }
            evt.IsSecondaryOverride = IsSecondary;
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
        }

        public bool IsSecondary;
        public bool CheckWeaponCategory;
        public WeaponCategory WeaponCategory;
    }
}