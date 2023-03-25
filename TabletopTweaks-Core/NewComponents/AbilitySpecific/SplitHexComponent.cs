using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using TabletopTweaks.Core.NewEvents;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Blueprints.Classes.Spells;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("00d2d2c9def641d3844a98a5a95f238e")]
    public class SplitHexComponent : UnitFactComponentDelegate<SplitHexComponent.SplitHexData>,
        IInitiatorRulebookHandler<RuleCastSpell>,
        IRulebookHandler<RuleCastSpell>,
        IAbilityGetCommandTypeHandler, 
        IUnitNewCombatRoundHandler {

        public override void OnTurnOn() {
        }

        public override void OnTurnOff() {
        }

        public void OnEventAboutToTrigger(RuleCastSpell evt) {
        }

        public void OnEventDidTrigger(RuleCastSpell evt) {
            if (!isValidTrigger(evt)) { return; }
            if (Data.HasStoredHex) {
                if (Data.StoredHex == evt.Spell.Blueprint) {
                    Data.Clear();
                }
            } else if(evt.Spell.Blueprint.SpellDescriptor.HasFlag(SpellDescriptor.Hex) && !evt.Spell.IsAOE)  {
                Data.Store(evt.Spell.Blueprint);
            }
        }

        public void HandleGetCommandType(AbilityData ability, ref UnitCommand.CommandType commandType) {
            if (Data.HasStoredHex && ability.Blueprint.AssetGuid == Data.StoredHex.AssetGuid) {
                commandType = UnitCommand.CommandType.Free;
            }
        }

        public void HandleNewCombatRound(UnitEntityData unit) {
            Data.Clear();
        }

        private bool isValidTrigger(RuleCastSpell evt) {
            return evt.Success
                && evt.Spell.Blueprint.SpellDescriptor.HasFlag(SpellDescriptor.Hex)
                && !evt.IsDuplicateSpellApplied
                && !evt.Spell.IsAOE;
        }

        public class SplitHexData {
            private BlueprintAbilityReference m_StoredHex;

            public bool HasStoredHex => !m_StoredHex?.IsEmpty() ?? false;
            public BlueprintAbility StoredHex => m_StoredHex?.Get();


            public void Store(BlueprintAbility hex) {
                m_StoredHex = hex.ToReference<BlueprintAbilityReference>();
            }

            public void Clear() {
                m_StoredHex = null;
            }
        }
    }
}
