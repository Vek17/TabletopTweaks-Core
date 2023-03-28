using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using TabletopTweaks.Core.NewEvents;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    public class BewitchingReflexComponent : UnitBuffComponentDelegate,
        IInitiatorRulebookHandler<RuleCastSpell>,
        IRulebookHandler<RuleCastSpell>,
        IAbilityGetCommandTypeHandler {

        public override void OnTurnOn() {
        }

        public override void OnTurnOff() {
        }

        public void OnEventAboutToTrigger(RuleCastSpell evt) {
        }

        public void OnEventDidTrigger(RuleCastSpell evt) {
            if (!isValidTrigger(evt.Spell)) { return; }
            Buff.Remove();
        }

        public void HandleGetCommandType(AbilityData ability, ref UnitCommand.CommandType commandType) {
            if (isValidTrigger(ability)) {
                if (OriginalIsFaster(commandType, UnitCommand.CommandType.Swift)) { return; }
                if (this.Owner.CombatState.HasCooldownForCommand(UnitCommand.CommandType.Swift)
                    && !this.Owner.CombatState.HasCooldownForCommand(commandType)) {
                    return;
                }
                commandType = UnitCommand.CommandType.Swift;
            }

            bool OriginalIsFaster(UnitCommand.CommandType commandType, UnitCommand.CommandType newCommandType) {
                switch (commandType) {
                    case UnitCommand.CommandType.Free:
                        return true;
                    case UnitCommand.CommandType.Swift:
                        return newCommandType != UnitCommand.CommandType.Free;
                    case UnitCommand.CommandType.Move:
                        return newCommandType != UnitCommand.CommandType.Free
                            && newCommandType != UnitCommand.CommandType.Swift;
                    case UnitCommand.CommandType.Standard:
                        return newCommandType != UnitCommand.CommandType.Free
                            && newCommandType != UnitCommand.CommandType.Swift
                            && newCommandType != UnitCommand.CommandType.Move;
                    default:
                        return true;
                }
            }
        }

        private bool isValidTrigger(AbilityData spell) {
            return spell.Blueprint.SpellDescriptor.HasFlag(SpellDescriptor.Hex);
        }
    }
}
