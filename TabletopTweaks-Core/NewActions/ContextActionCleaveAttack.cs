using System;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items.Slots;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;

namespace TabletopTweaks.Core.NewActions {
    [TypeId("d083a4cefb794764ba79eb921e322fd9")]
    public class ContextActionCleaveAttack : ContextAction {

        public BlueprintFeature MythicFeature => this.m_MythicFeature?.Get();

        public override string GetCaption() {
            return "Caster melee cleave attack";
        }

        public override void RunAction() {
            var maybeCaster = base.Context.MaybeCaster;
            if (maybeCaster == null) {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            var threatHandMelee = maybeCaster.GetThreatHandMelee();
            if (threatHandMelee == null) {
                PFLog.Default.Error("Caster can't make melee attack", Array.Empty<object>());
                return;
            }
            var initialTarget = base.Target?.Unit;
            if (initialTarget == null) {
                PFLog.Default.Error("Target is missing", Array.Empty<object>());
                return;
            }
            var attackTarget = SelectTarget(maybeCaster, initialTarget);
            if (attackTarget != null) {
                this.RunAttackRule(maybeCaster, attackTarget, threatHandMelee, 0, 0, 1);
            }
        }

        private void RunAttackRule(UnitEntityData caster, UnitEntityData target, WeaponSlot hand, int attackBonusPenalty = 0, int attackNumber = 0, int attacksCount = 1) {
            RuleAttackWithWeapon ruleAttackWithWeapon = new RuleAttackWithWeapon(caster, target, hand.Weapon, attackBonusPenalty) {
                Reason = base.Context,
                AutoHit = this.AutoHit,
                AutoCriticalThreat = this.AutoCritThreat,
                AutoCriticalConfirmation = this.AutoCritConfirmation,
                ExtraAttack = this.ExtraAttack,
                IsFullAttack = false,
                AttackNumber = attackNumber,
                AttacksCount = attacksCount
            };
            if (this.IgnoreStatBonus) {
                ruleAttackWithWeapon.WeaponStats.OverrideDamageBonusStatMultiplier(0f);
            }
            base.Context.TriggerRule<RuleAttackWithWeapon>(ruleAttackWithWeapon);
        }

        private UnitEntityData SelectTarget(UnitEntityData caster, UnitEntityData initialTarget) {
            var isMythic = MythicFeature != null && caster.HasFact(MythicFeature);
            UnitEntityData newTarget = null;
            foreach (UnitGroupMemory.UnitInfo unitInfo in caster.Memory.Enemies) {
                UnitEntityData unit = unitInfo.Unit;
                if (unit != null && !(unit.View == null) && (unit != initialTarget)
                    && caster.IsReach(unit, caster.GetThreatHandMelee())
                    && unit.Descriptor.State.IsConscious
                    && (isMythic || initialTarget.DistanceTo(unit) <= (initialTarget.View.Corpulence + TargetRadius.Meters + unit.View.Corpulence))
                    && (newTarget == null || unit.DistanceTo(initialTarget.Position) < newTarget.DistanceTo(initialTarget.Position))) 
                {
                    newTarget = unit;
                }
            }
            return newTarget;
        }
        public BlueprintFeatureReference m_MythicFeature;
        public Feet TargetRadius = 5.Feet();
        public bool AutoHit;
        public bool IgnoreStatBonus;
        public bool AutoCritThreat;
        public bool AutoCritConfirmation;
        public bool ExtraAttack = true;
    }
}
