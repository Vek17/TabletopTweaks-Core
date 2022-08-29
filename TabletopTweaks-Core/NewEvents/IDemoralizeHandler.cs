using HarmonyLib;
using Kingmaker.Controllers.Dialog;
using Kingmaker.Designers;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.NewEvents {
    /// <summary>
    /// Event handler for the Demoralize action.
    /// </summary>
    public interface IInitiatorDemoralizeHandler : IUnitSubscriber {
        /// <summary>
        /// Fires at the end of Demoralize.RunAction() if the intimidate check is successful.
        /// </summary>
        /// 
        /// <remarks>
        /// <para>
        /// The <paramref name="appliedBuff"/> will be either GreaterBuff or Buff. It does not represent other buffs
        /// applied, e.g. ShatterConfidenceBuff. If GreaterBuff is applied by FrighteningThug, appliedBuff will be
        /// Buff.
        /// </para>
        /// 
        /// <para>
        /// If <paramref name="appliedBuff"/> is null it likely means the target is immune to GreaterBuff or Buff.
        /// </para>
        /// </remarks>
        /// 
        /// <param name="intimidateCheck">The triggered skill check</param>
        /// <param name="appliedBuff">The buff applied, or null if none was applied</param>
        void AfterIntimidateSuccess(Demoralize action, RuleSkillCheck intimidateCheck, Buff appliedBuff);

        [HarmonyPatch(typeof(Demoralize))]
        internal static class Demoralize_RunAction {
            private static void NotifyIntimidateSuccess(
                Demoralize action, RuleSkillCheck intimidateCheck, Buff appliedBuff) {
                EventBus.RaiseEvent<IInitiatorDemoralizeHandler>(
                    action.Context?.MaybeCaster,
                    h => h.AfterIntimidateSuccess(action, intimidateCheck, appliedBuff));
            }

            static readonly MethodInfo RuleStatCheck_Success =
                AccessTools.PropertyGetter(typeof(RuleStatCheck), nameof(RuleStatCheck.Success));
            static readonly MethodInfo Buff_StoreFact = AccessTools.Method(typeof(Buff), nameof(Buff.StoreFact));

            [HarmonyPatch(nameof(Demoralize.RunAction)), HarmonyTranspiler]
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il) {
                try {
                    var code = new List<CodeInstruction>(instructions);
                    Label newJumpTarget = il.DefineLabel();

                    // Search backwards for the last Leave_S instruction which is the insertion point.
                    var index = code.Count - 1;
                    var insertionIndex = 0;
                    var leaveLabel = new List<Label>();
                    for (; index >= 0; index--) {
                        if (code[index].opcode == OpCodes.Leave_S) {
                            insertionIndex = index;
                            leaveLabel = code[index].labels;
                            break;
                        }
                    }
                    if (insertionIndex == 0) {
                        throw new InvalidOperationException("Missing demoralize transpiler insertion index.");
                    }

                    CodeInstruction loadIntimidateCheck = null;
                    CodeInstruction loadAppliedBuff = null;

                    // Keep searching backwards to find the load statements and redirect jumps to leaveLabel or
                    // retLabel to newJumpTarget.
                    index--; // Make sure not to change the last Leave_S jump or it will be an infinite loop
                    for (; index >= 0; index--) {
                        if (code[index].Calls(RuleStatCheck_Success)) {
                            // Statement before Success must load the skill check
                            loadIntimidateCheck = code[index - 1].Clone();
                            break; // Don't mess w/ jumps before the skill check result is generated
                        }

                        if (code[index].Calls(Buff_StoreFact)) {
                            // Statement before StoreFact is loading the fact being stored, statement before that is
                            // loading the buff
                            loadAppliedBuff = code[index - 2].Clone();
                        }

                        // Any operand which is a Label is a jump
                        if (code[index].operand is Label jumpTarget) {
                            if (leaveLabel.Contains(jumpTarget)) {
                                code[index].operand = newJumpTarget;
                            }
                        }
                    }

                    if (loadIntimidateCheck is null) {
                        throw new InvalidOperationException("Missing intimidate check load instruction.");
                    }
                    if (loadAppliedBuff is null) {
                        throw new InvalidOperationException("Missing applied buff load instruction.");
                    }

                    var newCode = new List<CodeInstruction>() {
                        new CodeInstruction(OpCodes.Ldarg_0).WithLabels(newJumpTarget), // Loads this (action) 
                        loadIntimidateCheck,
                        loadAppliedBuff,
                        CodeInstruction.Call(
                            typeof(Demoralize_RunAction), nameof(Demoralize_RunAction.NotifyIntimidateSuccess)),
                    };
                    code.InsertRange(insertionIndex, newCode);
                    return code;
                } catch (Exception e) {
                    TTTContext.Logger.LogError(e, "Demoralize transpiler failed.");
                    return instructions;
                }
            }
        }
    }
}
