using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Utility;
using UnityEngine;

namespace TabletopTweaks.Core.NewActions {
    [TypeId("24573debba5648bab789542144871d98")]
    public class AbilityCustomCleaveTTT : AbilityCustomLogic {

        public override bool IsEngageUnit => true;
        public BlueprintFeature GreaterFeature => this.m_GreaterFeature?.Get();
        public BlueprintFeature MythicFeature => this.m_MythicFeature?.Get();

        public override IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, TargetWrapper target) {
            var caster = context.MaybeCaster;
            var previousTarget = target.Unit;
            if (caster == null) {
                PFLog.Default.Error(this, "Caster is missing", Array.Empty<object>());
                yield break;
            }
            bool isGreater = GreaterFeature != null && caster.Descriptor.Progression.Features.HasFact(GreaterFeature);
            bool isMythic = MythicFeature != null && caster.Descriptor.Progression.Features.HasFact(MythicFeature);
            var threatHand = caster.GetThreatHandMelee();
            if (threatHand == null) {
                PFLog.Default.Error("Caster can't attack", Array.Empty<object>());
                yield break;
            }
            var targetUnit = target.Unit;
            if (targetUnit == null) {
                PFLog.Default.Error("Can't be applied to point", Array.Empty<object>());
                yield break;
            }
            List<UnitEntityData> targetList = new List<UnitEntityData>
            {
                targetUnit
            };
            foreach (UnitGroupMemory.UnitInfo unitInfo in caster.Memory.Enemies) {
                UnitEntityData unit = unitInfo.Unit;
                if (unit != targetUnit && unit.Descriptor.State.IsConscious && caster.IsReach(unit, threatHand)) {
                    targetList.Add(unit);
                }
            }
            targetList.Sort((UnitEntityData u1, UnitEntityData u2) => u1.DistanceTo(targetUnit).CompareTo(u2.DistanceTo(targetUnit)));
            List<UnitEntityData> hitTargets = new List<UnitEntityData>();
            List<UnitEntityData> validTargets = new List<UnitEntityData>(targetList);
            while (validTargets.Count > 0) {
                Main.TTTContext.Logger.Log($"IsMythic: {isMythic}");
                if (!isMythic) {
                    Main.TTTContext.Logger.Log("Non Mythic Cleave");
                    validTargets
                        .Sort((UnitEntityData u1, UnitEntityData u2) => u1.DistanceTo(previousTarget).CompareTo(u2.DistanceTo(previousTarget)));
                    validTargets.ForEach(t => {
                        Main.TTTContext.Logger.Log($"Distance from Previous Target: {t.DistanceTo(previousTarget)} - {t.View.Corpulence + previousTarget.View.Corpulence + 5.Feet().Meters}");
                    });
                    validTargets = validTargets
                        .Where(t => t.DistanceTo(previousTarget) <= t.View.Corpulence + previousTarget.View.Corpulence + 5.Feet().Meters)
                        .ToList();
                }
                var currentTarget = validTargets.FirstOrDefault();
                if (currentTarget == null) { break; }
                Main.TTTContext.Logger.Log($"Active Target: {currentTarget.DistanceTo(previousTarget)} - {currentTarget.View.Corpulence + previousTarget.View.Corpulence + 5.Feet().Meters}");
                if (!context.TriggerRule(new RuleAttackWithWeapon(caster, currentTarget, threatHand.Weapon, 0) {
                    IsFirstAttack = hitTargets.Any()
                }).AttackRoll.IsHit) {
                    break;
                }
                hitTargets.Add(currentTarget);
                previousTarget = currentTarget;
                yield return new AbilityDeliveryTarget(currentTarget);
                if (!isGreater && hitTargets.Count > 1) {
                    break;
                }
                validTargets = targetList.Where(t => !hitTargets.Contains(t)).ToList();
            }
            yield break;
        }

        public override void Cleanup(AbilityExecutionContext context) {
        }

        [SerializeField]
        public BlueprintFeatureReference m_GreaterFeature;
        [SerializeField]
        public BlueprintFeatureReference m_MythicFeature;
    }
}
