using HarmonyLib;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Abilities;

namespace TabletopTweaks.Core.NewEvents {
    public interface IAbilityGetCommandTypeHandler : IUnitSubscriber {
        void HandleGetCommandType(AbilityData ability, ref UnitCommand.CommandType commandType);

        [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.ActionType), MethodType.Getter)]
        static class UnitActivateAbility_GetCommandType_IActivatableAbilityGetCommandTypeHandler_Patch {
            static void Postfix(AbilityData __instance, ref UnitCommand.CommandType __result) {
                var result = __result;
                EventBus.RaiseEvent<IAbilityGetCommandTypeHandler>(__instance.Caster, h => h.HandleGetCommandType(__instance, ref result));
                __result = result;
            }
        }
    }
}
