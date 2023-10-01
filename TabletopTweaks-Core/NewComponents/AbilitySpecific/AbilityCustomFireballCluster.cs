using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers.Projectiles;
using Kingmaker.Controllers;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.Utility;
using Owlcat.Runtime.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [TypeId("eff703a2d201448eadc9c18570e49149")]
    public class AbilityDeliverCluster : AbilityDeliverEffect {

        public ReferenceArrayProxy<BlueprintProjectile, BlueprintProjectileReference> Projectiles {
            get {
                return this.m_Projectiles;
            }
        }

        public override IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, TargetWrapper target) {
            Vector3 castPosition = context.Caster.EyePosition;

            int count = this.UseMaxProjectilesCount ? context[this.MaxProjectilesCountRank] : this.Projectiles.Length;
            List<IEnumerator<AbilityDeliveryTarget>> deliveryProcesses = this.Projectiles.Take(count).Select((BlueprintProjectile proj, int index) => 
            this.Deliver(context, castPosition, target, proj, index, false)).ToList<IEnumerator<AbilityDeliveryTarget>>();
            while (deliveryProcesses.Count > 0) {
                int num;
                for (int j = 0; j < deliveryProcesses.Count; j = num) {
                    IEnumerator<AbilityDeliveryTarget> p2 = deliveryProcesses[j];
                    bool flag;
                    while ((flag = p2.MoveNext()) && p2.Current != null) {
                        yield return p2.Current;
                    }
                    if (!flag) {
                        deliveryProcesses[j] = null;
                    }
                    p2 = null;
                    num = j + 1;
                }
                deliveryProcesses.RemoveAll((IEnumerator<AbilityDeliveryTarget> i) => i == null);
                yield return null;
            }
            yield break;
        }

        protected IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, Vector3 castPosition, TargetWrapper target, BlueprintProjectile projectile, int index, bool isControlledProjectile) {
            TimeSpan startTime = Game.Instance.TimeController.GameTime;
            UnitEntityData caster = context.MaybeCaster;
            if (caster == null) {
                PFLog.Default.Error(this, "Caster is missing", Array.Empty<object>());
                yield break;
            }
            Vector3 vector = target.IsUnit ? target.Point : AbilityDeliverProjectile.CorrectPosition(target.Point, caster.Position);
            while (Game.Instance.TimeController.GameTime - startTime < (this.DelayBetweenProjectiles * (float)index).Seconds()) {
                yield return null;
            }
            TargetWrapper launcher = isControlledProjectile ? castPosition : context.Caster;

            Projectile proj = null; 
            if (index == 0) {
                proj = Game.Instance.ProjectileController.Launch(launcher, target, projectile, null);
                proj.IsFirstProjectile = true;
            } else {
                proj = Game.Instance.ProjectileController.Launch(launcher, target, projectile, AttackResult.Miss);
            }
            bool reach = context.HasMetamagic(Metamagic.Reach);
            proj.MaxRange = this.GetProjectileRange(context, reach);

            yield return null;
            while (!proj.IsHit && !proj.Cleared) {
                yield return null;
            }
            if (proj.Cleared) {
                yield break;
            }
            if (proj.IsFirstProjectile) {
                target = proj.Target;
                yield return new AbilityDeliveryTarget(target) {
                    Projectile = proj
                };
            }
            yield break;
        }

        protected virtual float GetProjectileRange(AbilityExecutionContext context, bool reach) {
            return context.AbilityBlueprint.GetRange(reach, context.Ability).Meters;
        }

        // Token: 0x04008A24 RID: 35364
        private const float ConeAngle = 90f;

        // Token: 0x04008A25 RID: 35365
        [SerializeField]
        [FormerlySerializedAs("Projectiles")]
        public BlueprintProjectileReference[] m_Projectiles;

        // Token: 0x04008A26 RID: 35366
        public AbilityProjectileType Type;

        // Token: 0x04008A27 RID: 35367
        [HideIf("IsPierceOrCone")]
        public bool IsHandOfTheApprentice;

        // Token: 0x04008A28 RID: 35368
        [ShowIf("UseFeet")]
        [SerializeField]
        [FormerlySerializedAs("Length")]
        public Feet m_Length;

        // Token: 0x04008A29 RID: 35369
        [ShowIf("UseFeet")]
        [SerializeField]
        [FormerlySerializedAs("LineWidth")]
        public Feet m_LineWidth = 5.Feet();

        // Token: 0x04008A2A RID: 35370
        [HideIf("IsPierceOrCone")]
        [FormerlySerializedAs("IsRangedTouch")]
        public bool NeedAttackRoll;

        // Token: 0x04008A2B RID: 35371
        [ShowIf("ShowWeapon")]
        [FormerlySerializedAs("TouchWeapon")]
        [SerializeField]
        [FormerlySerializedAs("Weapon")]
        public BlueprintItemWeaponReference m_Weapon;

        // Token: 0x04008A2C RID: 35372
        [ShowIf("NeedAttackRoll")]
        public bool ReplaceAttackRollBonusStat;

        // Token: 0x04008A2D RID: 35373
        [ShowIf("ShowAttackRollBonusStat")]
        public StatType AttackRollBonusStat;

        // Token: 0x04008A2E RID: 35374
        public bool UseMaxProjectilesCount;

        // Token: 0x04008A2F RID: 35375
        [ShowIf("UseMaxProjectilesCount")]
        public AbilityRankType MaxProjectilesCountRank;

        // Token: 0x04008A30 RID: 35376
        [ShowIf("IsMultipleProjectiles")]
        public float DelayBetweenProjectiles;

        // Token: 0x04008A31 RID: 35377
        [SerializeField]
        [FormerlySerializedAs("ControlledProjectileHolderBuff")]
        public BlueprintBuffReference m_ControlledProjectileHolderBuff;
    }
}
