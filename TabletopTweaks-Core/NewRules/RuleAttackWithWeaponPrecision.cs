using JetBrains.Annotations;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UI.Models.Log;
using Kingmaker.UI.Models.Log.Events;
using System;
using TabletopTweaks.Core.Utilities;

namespace TabletopTweaks.Core.NewRules {
    class RuleAttackWithWeaponPrecision : RuleAttackWithWeapon {

        [PostPatchInitialize]
        static void AddGameLogEventCreator() {
            if (!GameLogEventsFactory.Creators.ContainsKey(typeof(RuleAttackWithWeaponPrecision))) {
                Type gameLogEventType = typeof(GameLogRuleEvent<>).MakeGenericType(new Type[]
                {
                        typeof(RuleAttackWithWeaponPrecision)
                });
                GameLogEventsFactory.Creators.Add(typeof(RuleAttackWithWeaponPrecision), (RulebookEvent rule) => (GameLogEvent)Activator.CreateInstance(gameLogEventType, new object[]
                {
                        rule
                }));
            }
        }

        public bool ForceSneakAttack { get; set; }
        public RuleAttackWithWeaponPrecision([NotNull] UnitEntityData attacker, [NotNull] UnitEntityData target, [NotNull] ItemEntityWeapon weapon, int attackBonusPenalty) : base(attacker, target, weapon, attackBonusPenalty) {
        }

        public override void OnTrigger(RulebookEventContext context) {
            Rulebook.Trigger<RuleCalculateWeaponStats>(this.WeaponStats);
            this.AttackRoll = Rulebook.Trigger<RuleAttackRoll>(new RuleAttackRoll(this.Initiator, this.Target, this.WeaponStats, this.AttackBonusPenalty) {
                AutoHit = this.AutoHit,
                AutoCriticalThreat = this.AutoCriticalThreat,
                AutoCriticalConfirmation = (TacticalCombatHelper.IsActive || this.AutoCriticalConfirmation),
                SuspendCombatLog = this.Weapon.Blueprint.IsRanged,
                RuleAttackWithWeapon = this,
                DoNotProvokeAttacksOfOpportunity = this.IsAttackOfOpportunity,
                ForceFlatFooted = this.ForceFlatFooted,
                IsSneakAttack = this.ForceSneakAttack,
            });
            BlueprintProjectileReference[] projectiles = this.Weapon.Blueprint.VisualParameters.Projectiles;
            if (projectiles.Length != 0) {
                this.LaunchProjectiles(projectiles);
                return;
            }
            RuleDealDamage damage = this.Weapon.Blueprint.HasNoDamage ? null : this.CreateRuleDealDamage(true);
            RuleAttackWithWeaponResolve ruleAttackWithWeaponResolve = new RuleAttackWithWeaponResolve(this, damage);
            this.MeleeDamage = ruleAttackWithWeaponResolve.Damage;
            this.ResolveRules.Add(ruleAttackWithWeaponResolve);
            context.Trigger<RuleAttackWithWeaponResolve>(ruleAttackWithWeaponResolve);
        }

        [PostPatchInitialize]
        private static void SetupGameLogEvents() {
            Type RuleType = typeof(RuleAttackWithWeaponPrecision);
            if (!GameLogEventsFactory.Creators.ContainsKey(RuleType)) {
                Type gameLogEventType = typeof(GameLogRuleEvent<>).MakeGenericType(new Type[]
                {
                    RuleType
                });
                GameLogEventsFactory.Creators.Add(RuleType, (RulebookEvent rule) => (GameLogEvent)Activator.CreateInstance(gameLogEventType, new object[]
                {
                    rule
                }));
            }
        }
    }
}
