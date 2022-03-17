using HarmonyLib;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.NewEvents {
    public interface IStatBonusCalculatedHandler : IGlobalSubscriber {

        void StatBonusCalculated(ref int value, StatType stat, ModifierDescriptor descriptor, MechanicsContext context);

        private class EventTriggers{

            [HarmonyPatch(typeof(AddStatBonus), nameof(AddStatBonus.OnTurnOn))]
            static class AddStatBonus_Idealize_Patch {
                static readonly MethodInfo Modifier_AddModifierUnique = AccessTools.Method(typeof(ModifiableValue), "AddModifierUnique", new Type[] {
                typeof(int),
                typeof(EntityFactComponent),
                typeof(ModifierDescriptor)
            });
                static readonly MethodInfo Idealize_AddIdealizeBonus = AccessTools.Method(
                    typeof(EventTriggers),
                    nameof(EventTriggers.AddIdealizeBonus),
                    new Type[] { typeof(int), typeof(AddStatBonus) }
                );
                //Add Idealize calculations
                static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                    if (TTTContext.AddedContent.WizardArcaneDiscoveries.IsDisabled("Idealize")) { return instructions; }

                    var codes = new List<CodeInstruction>(instructions);
                    int target = FindInsertionTarget(codes);
                    //Utilities.ILUtils.LogIL(codes);
                    codes.InsertRange(target, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, Idealize_AddIdealizeBonus)
                });
                    //Utilities.ILUtils.LogIL(codes);
                    return codes.AsEnumerable();
                }
                private static int FindInsertionTarget(List<CodeInstruction> codes) {
                    int target = 0;
                    for (int i = 0; i < codes.Count; i++) {
                        //Find where the modifier is added and grab the load of the value varriable
                        if (codes[i].opcode == OpCodes.Ldloc_0) { target = i + 1; }
                        if (codes[i].Calls(Modifier_AddModifierUnique)) {
                            return target;
                        }
                    }
                    TTTContext.Logger.Log("ADD STAT IDEALIZE PATCH - AddStatBonus: COULD NOT FIND TARGET");
                    return -1;
                }
            }
            [HarmonyPatch(typeof(AddContextStatBonus), nameof(AddContextStatBonus.OnTurnOn))]
            static class AddContextStatBonus_Idealize_Patch {
                static readonly MethodInfo Modifier_AddModifier = AccessTools.Method(typeof(ModifiableValue), "AddModifier", new Type[] {
                typeof(int),
                typeof(EntityFactComponent),
                typeof(ModifierDescriptor)
            });
                static readonly MethodInfo Idealize_AddIdealizeBonus = AccessTools.Method(
                    typeof(EventTriggers),
                    nameof(EventTriggers.AddIdealizeBonus),
                    new Type[] { typeof(int), typeof(AddContextStatBonus) }
                );
                //Add Idealize calculations
                static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                    if (TTTContext.AddedContent.WizardArcaneDiscoveries.IsDisabled("Idealize")) { return instructions; }

                    var codes = new List<CodeInstruction>(instructions);
                    int target = FindInsertionTarget(codes);
                    //Utilities.ILUtils.LogIL(codes);
                    codes.InsertRange(target, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, Idealize_AddIdealizeBonus)
                });
                    target = FindInsertionTarget(codes, target);
                    codes.InsertRange(target, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, Idealize_AddIdealizeBonus)
                });
                    //Utilities.ILUtils.LogIL(codes);
                    return codes.AsEnumerable();
                }
                private static int FindInsertionTarget(List<CodeInstruction> codes, int startingIndex = 0) {
                    int target = startingIndex;
                    for (int i = startingIndex; i < codes.Count; i++) {
                        //Find where the modifier is added and grab the load of the value varriable
                        if (codes[i].opcode == OpCodes.Ldloc_1) { target = i + 1; }
                        if (codes[i].Calls(Modifier_AddModifier) && target != startingIndex) {
                            return target;
                        }
                    }
                    TTTContext.Logger.Log("ADD STAT IDEALIZE PATCH - AddContextStatBonus: COULD NOT FIND TARGET");
                    return -1;
                }
            }
            [HarmonyPatch(typeof(AddGenericStatBonus), nameof(AddStatBonus.OnTurnOn))]
            static class AddGenericStatBonus_Idealize_Patch {
                static readonly MethodInfo Modifier_AddModifierUnique = AccessTools.Method(typeof(ModifiableValue), "AddModifierUnique", new Type[] {
                typeof(int),
                typeof(EntityFactComponent),
                typeof(ModifierDescriptor)
            });
                static readonly MethodInfo Idealize_AddIdealizeBonus = AccessTools.Method(
                    typeof(EventTriggers),
                    nameof(EventTriggers.AddIdealizeBonus),
                    new Type[] { typeof(int), typeof(AddGenericStatBonus) }
                );
                //Add Idealize calculations
                static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                    if (TTTContext.AddedContent.WizardArcaneDiscoveries.IsDisabled("Idealize")) { return instructions; }

                    var codes = new List<CodeInstruction>(instructions);
                    int target = FindInsertionTarget(codes);
                    //Utilities.ILUtils.LogIL(codes);
                    codes.InsertRange(target, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, Idealize_AddIdealizeBonus)
                });
                    //Utilities.ILUtils.LogIL(codes);
                    return codes.AsEnumerable();
                }
                private static int FindInsertionTarget(List<CodeInstruction> codes) {
                    int target = 0;
                    for (int i = 0; i < codes.Count; i++) {
                        //Find where the modifier is added and grab the load of the value varriable
                        if (codes[i].opcode == OpCodes.Ldloc_1) { target = i + 1; }
                        if (codes[i].Calls(Modifier_AddModifierUnique)) {
                            return target;
                        }
                    }
                    TTTContext.Logger.Log("ADD STAT IDEALIZE PATCH - AddGenericStatBonus: COULD NOT FIND TARGET");
                    return -1;
                }
            }

            private static int AddIdealizeBonus(int value, AddStatBonus component) {
                return CallEvent(value, component.Stat, component.Descriptor, component.Context);
            }
            private static int AddIdealizeBonus(int value, AddContextStatBonus component) {
                return CallEvent(value, component.Stat, component.Descriptor, component.Context);
            }
            private static int AddIdealizeBonus(int value, AddGenericStatBonus component) {
                return CallEvent(value, component.Stat, component.Descriptor, component.Context);
            }
            private static int CallEvent(int value, StatType stat, ModifierDescriptor descriptor, MechanicsContext context) {
                EventBus.RaiseEvent<IStatBonusCalculatedHandler>(h => h.StatBonusCalculated(ref value, stat, descriptor, context));
                return value;
            }
        }
    }
}
