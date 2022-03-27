using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace TabletopTweaks.Core.NewRules {
    public class RuleFortificationCheck : RulebookTargetEvent {

        public RuleFortificationCheck([NotNull]RuleAttackRoll evt) : base(evt.Initiator, evt.Target) {
            this.Roll = new RuleRollD100(Initiator);
            this.ForCritical = evt.IsCriticalConfirmed;
            this.ForSneakAttack = evt.IsSneakAttack;
            this.ForPreciseStrike = evt.PreciseStrike > 0;
            this.AttackRoll = evt;
        }

        public readonly RuleRollD100 Roll;
        public int FortificationChance =>
            Math.Max(0, Math.Min(100, (this.Target.Get<UnitPartFortification>()?.Value ?? 0) + Bonuses.DefaultIfEmpty().Max() - Penalties.DefaultIfEmpty().Min()));
        public bool UseFortification => FortificationChance > 0;
        public bool AutoPass { get; set; }
        public bool IsPassed {
            get {
                if (!this.AutoPass) {
                    return Roll > FortificationChance;
                }
                return true;
            }
        }
        public RuleAttackRoll AttackRoll { get; }
        public bool ForCritical { get; }
        public bool ForSneakAttack { get; }
        public bool ForPreciseStrike { get; }

        public override void OnTrigger(RulebookEventContext context) {
            Rulebook.Trigger<RuleRollD100>(this.Roll);
        }

        public void AddBonus(int bonus) {
            Bonuses.Add(bonus);
        }
        public void AddPenalty(int bonus) {
            Penalties.Add(bonus);
        }

        private readonly List<int> Bonuses = new List<int>();
        private readonly List<int> Penalties = new List<int>();

        // Replace all fortification logic with new rule based logic
        [HarmonyPatch(typeof(RuleAttackRoll), "OnTrigger", new Type[] { typeof(RulebookEventContext) })]
        private class RuleFortificationRoll_Implementation {
            static readonly MethodInfo get_TargetUseFortification = AccessTools.PropertyGetter(typeof(RuleAttackRoll), "TargetUseFortification");
            static readonly MethodInfo method_CheckFortification = AccessTools.Method(typeof(RuleFortificationRoll_Implementation), "CheckFortification");

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                var codes = new List<CodeInstruction>(instructions);
                var target = FindInsertionTarget(codes);
                //Main.TTTContext.Logger.Log($"Target({target.Index}, {target.Start}, {target.End})");
                //Utilities.ILUtils.LogIL(Main.TTTContext, codes);
                var labels = codes.GetRange(target.Start, (target.End - target.Start))
                    .SelectMany(c => c.labels)
                    .Where(labels => labels != null)
                    .Distinct()
                    .ToList();
                codes.RemoveRange(target.Start, 1 + (target.End - target.Start));
                codes.InsertRange(target.Start, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, method_CheckFortification),
                });
                codes[target.Start].labels = labels;
                //Utilities.ILUtils.LogIL(Main.TTTContext, codes);
                return codes.AsEnumerable();
            }
            private static TargetInfo FindInsertionTarget(List<CodeInstruction> codes) {
                for (int i = 0; i < codes.Count; i++) {
                    if (codes[i].opcode == OpCodes.Call && codes[i].Calls(get_TargetUseFortification)) {
                        return new TargetInfo(i, i - 12, i + 24);
                    }
                }
                Main.TTTContext.Logger.Log("RuleFortificationCheck: COULD NOT FIND TARGET");
                return new TargetInfo(-1,-1,-1);
            }
            private struct TargetInfo {
                public int Index;
                public int Start;
                public int End;

                public TargetInfo(int index, int start, int end) {
                    this.Index = index;
                    this.Start = start;
                    this.End = end;
                }
            }

            private static void CheckFortification(RuleAttackRoll evt) {
                var FortificationCheck = Rulebook.Trigger<RuleFortificationCheck>(new RuleFortificationCheck(evt));
                if (!FortificationCheck.UseFortification) { return; }
                evt.FortificationChance = FortificationCheck.FortificationChance;
                evt.FortificationRoll = FortificationCheck.Roll;
                if (!FortificationCheck.IsPassed) {
                    evt.FortificationNegatesSneakAttack = evt.IsSneakAttack;
                    evt.FortificationNegatesCriticalHit = evt.IsCriticalConfirmed;
                    evt.IsSneakAttack = false;
                    evt.IsCriticalConfirmed = false;
                    evt.PreciseStrike = 0;
                }
            }
        }
    }
}
