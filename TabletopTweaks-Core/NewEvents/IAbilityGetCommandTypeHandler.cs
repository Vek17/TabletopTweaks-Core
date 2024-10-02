using HarmonyLib;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Commands.Base;

namespace TabletopTweaks.Core.NewEvents {
    public interface IAbilityGetCommandTypeHandler : IUnitSubscriber {
        void HandleGetCommandType(AbilityData ability, ref UnitCommand.CommandType commandType);

        [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.ActionType), MethodType.Getter)]
        static class AbilityData_ActionType_IAbilityGetCommandTypeHandler_Patch {
            static void Postfix(AbilityData __instance, ref UnitCommand.CommandType __result) {
                var result = __result;
                EventBus.RaiseEvent<IAbilityGetCommandTypeHandler>(__instance.Caster, h => h.HandleGetCommandType(__instance, ref result));
                __result = result;
            }
        }
    }
}
