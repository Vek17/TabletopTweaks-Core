using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    /// <summary>
    /// Applies extra damage to natural weapons when using shifter claws and attacking with dexterity and using strength for damage.
    /// </summary>
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintUnitFact))]
    [TypeId("17ba8e7a5a584c2882cf6b7862509f1f")]
    public class ShiftersEdgeComponent : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
        IRulebookHandler<RuleCalculateWeaponStats>,
        ISubscriber,
        IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
            if (IsSuitable(evt)) {
                evt.AddDamageModifier(DamageBonus.Calculate(base.Context), base.Fact, Descriptor);
                evt.DamageDescription[0].AddModifier(new Modifier(DamageBonus.Calculate(base.Context), base.Fact, Descriptor));
            }
        }

        private bool IsSuitable(RuleCalculateWeaponStats evt) {
            var weapon = evt.Weapon;
            var ruleCalculateAttackBonus = new RuleCalculateAttackBonusWithoutTarget(evt.Initiator, weapon, 0);

            ruleCalculateAttackBonus.WeaponStats.m_Triggered = true;
            Rulebook.Trigger(ruleCalculateAttackBonus);

            return (evt.DamageBonusStat == StatType.Strength)
                && ruleCalculateAttackBonus.AttackBonusStat == StatType.Dexterity
                && (weapon.Blueprint.Category == WeaponCategory.Claw
                    || PolymorphDamageTransfer.IsApplicableToWeapon(weapon, evt.Initiator));
        }
        /// <summary>
        /// Value of the damage bonus to add.
        /// </summary>
        public ContextValue DamageBonus;
        /// <summary>
        /// Descriptor of the damage bonus.
        /// </summary>
        public ModifierDescriptor Descriptor = ModifierDescriptor.UntypedStackable;
    }
}
