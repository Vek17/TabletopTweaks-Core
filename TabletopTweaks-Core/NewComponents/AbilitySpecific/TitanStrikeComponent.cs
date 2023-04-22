using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System.Linq;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    /// <summary>
    /// Increases the damage of unarmed strikes by one dice step. Additionally increases the DC of combat maneuvers and stunning fist based on the target's size.
    /// </summary>
    [TypeId("4a2247bdf0cf4b139863f0136abd4af8")]
    public class TitanStrikeComponent : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateWeaponSizeBonus>,
        IRulebookHandler<RuleCalculateWeaponSizeBonus>,
        IInitiatorRulebookHandler<RuleCalculateCMB>,
        IRulebookHandler<RuleCalculateCMB>,
        IGlobalRulebookHandler<RuleSavingThrow>,
        IRulebookHandler<RuleSavingThrow>,
        ISubscriber, IInitiatorRulebookSubscriber, IGlobalSubscriber {

        public ReferenceArrayProxy<BlueprintBuff, BlueprintBuffReference> StunningFistBuffs => m_StunningFistBuffs;

        public void OnEventAboutToTrigger(RuleCalculateWeaponSizeBonus evt) {

        }

        public void OnEventAboutToTrigger(RuleCalculateCMB evt) {
            int bonus = (int)evt?.Target?.State?.Size - (int)evt?.Initiator?.State?.Size;
            if (bonus > 0 && (
                evt.Type == CombatManeuver.BullRush
                || evt.Type == CombatManeuver.Pull
                || evt.Type == CombatManeuver.Grapple
                || evt.Type == CombatManeuver.Overrun
                || evt.Type == CombatManeuver.SunderArmor
                || evt.Type == CombatManeuver.Trip
            )) {
                evt.AddModifier(new Modifier(bonus, this.Fact, ModifierDescriptor.UntypedStackable));
            }
        }

        public void OnEventAboutToTrigger(RuleSavingThrow evt) {
            var context = evt.Reason.Context;
            if (context.MaybeCaster != base.Owner || !StunningFistBuffs.Any(b => b == context.AssociatedBlueprint)) { return; }
            int bonus = (int)evt.Initiator?.State?.Size - (int)context.MaybeCaster?.State?.Size;
            if (bonus > 0) {
                evt.AddBonusDC(bonus);
            }
        }

        public void OnEventDidTrigger(RuleCalculateWeaponSizeBonus evt) {
            if (evt.Weapon.Blueprint.Type.IsUnarmed) {
                var preSize = evt.WeaponSize;
                evt.m_SizeShift += 1;
                evt.WeaponDamageDice.Modify(
                   WeaponDamageScaleTable.Scale(evt.WeaponDamageDice.ModifiedValue, evt.WeaponSize, preSize, evt.Weapon.Blueprint), base.Fact
                );
            }
        }

        public void OnEventDidTrigger(RuleCalculateCMB evt) {
        }

        public void OnEventDidTrigger(RuleSavingThrow evt) {
        }
        /// <summary>
        /// Stunning fist buff to increase DC of.
        /// </summary>
        public BlueprintBuffReference[] m_StunningFistBuffs;
    }
}
