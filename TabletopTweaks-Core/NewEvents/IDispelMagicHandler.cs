using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace TabletopTweaks.Core.NewEvents {
    internal interface IDispelMagicHandler : IUnitSubscriber {
        void OnDidDispelEffects(UnitEntityData target);

        [HarmonyPatch(typeof(ContextActionDispelMagic), nameof(ContextActionDispelMagic.RunAction))]
        private class ContextActionDispelMagic_InstallHandlers_Patch {
            static readonly MethodInfo ContextActionDispelMagic_TriggerHandlers = AccessTools.Method(typeof(ContextActionDispelMagic_InstallHandlers_Patch), "TriggerHandlers");

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                var codes = new List<CodeInstruction>(instructions);
                var labels = codes[codes.Count - 1].labels.ToArray().ToList();
                codes[codes.Count - 1].labels.Clear();
                codes.InsertRange(codes.Count - 1,
                    new CodeInstruction[] {
                        new CodeInstruction(OpCodes.Ldarg_0){
                            labels = labels
                        },
                        new CodeInstruction(OpCodes.Ldloc_0),
                        new CodeInstruction(OpCodes.Call, ContextActionDispelMagic_TriggerHandlers)
                    }
                );
                return codes.AsEnumerable();
            }

            static void TriggerHandlers(ContextActionDispelMagic action, int dispeledItems) {
                var caster = action.Context.MaybeCaster;
                if (caster == null) { return; }
                if (action.Target?.Unit == null) { return; }
                if (dispeledItems > 0) {
                    EventBus.RaiseEvent<IDispelMagicHandler>(caster, h => h.OnDidDispelEffects(action.Target.Unit));
                }
            }
        }
    }
}
