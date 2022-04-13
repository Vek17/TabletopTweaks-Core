using HarmonyLib;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.Commands.Base;
using System;

namespace TabletopTweaks.Core.NewEvents {
    public interface IActivatableAbilityGetCommandTypeHandler : IUnitSubscriber {
        void HandleGetCommandType(ActivatableAbility ability, ref UnitCommand.CommandType commandType);

        [HarmonyPatch(typeof(UnitActivateAbility), nameof(UnitActivateAbility.GetCommandType), new Type[] { typeof(ActivatableAbility) })]
        static class UnitActivateAbility_GetCommandType_IActivatableAbilityGetCommandTypeHandler_Patch {
            static void Postfix(ActivatableAbility ability, ref UnitCommand.CommandType __result) {
                var result = __result;
                EventBus.RaiseEvent<IActivatableAbilityGetCommandTypeHandler>(ability.Owner, h => h.HandleGetCommandType(ability, ref result));
                __result = result;
            }
        }
    }
}
