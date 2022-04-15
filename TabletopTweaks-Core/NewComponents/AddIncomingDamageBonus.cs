using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics;

namespace TabletopTweaks.Core.NewComponents {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("da89a30a249443eebaa1cefe10dabf32")]
    public class AddIncomingDamageBonus : UnitBuffComponentDelegate, 
        ITargetRulebookHandler<RuleCalculateDamage>, 
        IRulebookHandler<RuleCalculateDamage>, 
        ISubscriber, ITargetRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateDamage evt) {
            if (evt.Target.Descriptor == base.Owner) {
                BaseDamage weaponDamage = evt.DamageBundle.WeaponDamage;
                if (weaponDamage == null) {
                    return;
                }
                AddModifierTargetRelated(weaponDamage, Value.Calculate(base.Context), Descriptor);
            }
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt) {
        }

        private void AddModifierTargetRelated(BaseDamage damage, int value, ModifierDescriptor desc) {
            if (damage.m_Modifiers.Add(new Modifier(value, base.Fact, desc))) {
                damage.BonusTargetRelated += value;
            }
        }
        public ContextValue Value;
        public ModifierDescriptor Descriptor = ModifierDescriptor.UntypedStackable;
    }
}
