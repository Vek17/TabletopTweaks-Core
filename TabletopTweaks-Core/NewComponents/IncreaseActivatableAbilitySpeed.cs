using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Commands.Base;
using System.Linq;
using TabletopTweaks.Core.NewEvents;

namespace TabletopTweaks.Core.NewComponents {
    [AllowMultipleComponents]
    [TypeId("6615ed84b34245629adf3da8e5ef1c47")]
   
    public class IncreaseActivatableAbilitySpeed : UnitFactComponentDelegate, IActivatableAbilityGetCommandTypeHandler {

        public ReferenceArrayProxy<BlueprintActivatableAbility, BlueprintActivatableAbilityReference> Abilities => this.m_Abilities;

        public void HandleGetCommandType(ActivatableAbility ability, ref UnitCommand.CommandType commandType) {
            if (Abilities.Contains(ability.Blueprint)) {
                if (OriginalIsFaster(commandType)) { return; }
                if (this.Owner.CombatState.HasCooldownForCommand(NewCommandType)
                    && !this.Owner.CombatState.HasCooldownForCommand(commandType)) {
                    return;
                }
                commandType = NewCommandType;
            }
        }

        private bool OriginalIsFaster(UnitCommand.CommandType commandType) {
            switch (commandType) {
                case UnitCommand.CommandType.Free:
                    return true;
                case UnitCommand.CommandType.Swift:
                    return NewCommandType != UnitCommand.CommandType.Free;
                case UnitCommand.CommandType.Move:
                    return NewCommandType != UnitCommand.CommandType.Free
                        && NewCommandType != UnitCommand.CommandType.Swift;
                case UnitCommand.CommandType.Standard:
                    return NewCommandType != UnitCommand.CommandType.Free
                        && NewCommandType != UnitCommand.CommandType.Swift
                        && NewCommandType != UnitCommand.CommandType.Move;
                default:
                    return true;

            }
        }

        public UnitCommand.CommandType NewCommandType;
        public BlueprintActivatableAbilityReference[] m_Abilities = new BlueprintActivatableAbilityReference[0];
    }
}
