using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics;

namespace TabletopTweaks.Core.NewComponents {
    [AllowMultipleComponents]
    [TypeId("59f389ba1bad4451a4e1286fb463cf82")]
    public class AddConditionalWeaponDamageBonus : EntityFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
        IRulebookHandler<RuleCalculateWeaponStats>,
        ISubscriber, IInitiatorRulebookSubscriber {



        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
            RuleAttackWithWeapon attackWithWeapon = evt.AttackWithWeapon;
            if (attackWithWeapon != null) {
                var context = attackWithWeapon.Reason?.Context;
                if (TargetConditions != null) {
                    if (context != null) {
                        using (context.GetDataScope(attackWithWeapon.Target)) {
                            if (!TargetConditions.Check()) { return; }
                        }
                    }
                }
                if (CasterConditions != null) {
                    if (context != null) {
                        using (context.GetDataScope(attackWithWeapon.Initiator)) {
                            if (!CasterConditions.Check()) { return; }
                        }
                    }
                }
            }
            if (CheckWeaponRangeType && !RangeType.IsSuitableWeapon(evt.Weapon)) {
                return;
            }
            if (CheckWeaponCatergoy && evt.Weapon.Blueprint.Category != Category) {
                return;
            }
            evt.AddDamageModifier(Value.Calculate(this.Context), this.Fact, Descriptor);
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
        }

        public ModifierDescriptor Descriptor;
        public ContextValue Value;
        public ConditionsChecker TargetConditions = new ConditionsChecker();
        public ConditionsChecker CasterConditions = new ConditionsChecker();
        public bool CheckWeaponRangeType;
        public WeaponRangeType RangeType;
        public bool CheckWeaponCatergoy;
        public WeaponCategory Category;
    }
}
