using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    /// <summary>
    /// Applies the weapon training bonus damage twice if using dex to attack and strength for damage.
    /// </summary>
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintUnitFact))]
    [TypeId("a07e963bb1e74da1a615e3a426004c47")]
    public class TrainedGraceComponent : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
        IRulebookHandler<RuleCalculateWeaponStats>,
        ISubscriber,
        IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
            UnitPartWeaponTraining unitPartWeaponTraining = Owner.Get<UnitPartWeaponTraining>();
            if (IsSuitable(evt, unitPartWeaponTraining)) {
                evt.AddDamageModifier(unitPartWeaponTraining.GetWeaponRank(evt.Weapon), base.Fact, Descriptor);
            }
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
        }

        private bool IsSuitable(RuleCalculateWeaponStats evt, UnitPartWeaponTraining unitPartWeaponTraining) {
            var weapon = evt.Weapon;
            var ruleCalculateAttackBonus = new RuleCalculateAttackBonusWithoutTarget(evt.Initiator, weapon, 0);
            ruleCalculateAttackBonus.WeaponStats.m_Triggered = true;
            Rulebook.Trigger(ruleCalculateAttackBonus);

            return unitPartWeaponTraining.IsSuitableWeapon(weapon)
                && (!MeleeOnly || !weapon.Blueprint.IsRanged)
                && (!EnforceGroup || weapon.Blueprint.FighterGroup.Contains(WeaponGroup))
                && (evt.DamageBonusStat == StatType.Strength)
                && ruleCalculateAttackBonus.AttackBonusStat == StatType.Dexterity;
        }
        /// <summary>
        /// Restrict to specific groups.
        /// </summary>
        public bool EnforceGroup;
        /// <summary>
        /// Group to restrict bonuses for.
        /// </summary>
        public WeaponFighterGroup WeaponGroup;
        /// <summary>
        /// Restrict to only melee weapons.
        /// </summary>
        public bool MeleeOnly;
        /// <summary>
        /// Descriptor of the second weapon training bonus.
        /// </summary>
        public ModifierDescriptor Descriptor = ModifierDescriptor.UntypedStackable;
    }
}
