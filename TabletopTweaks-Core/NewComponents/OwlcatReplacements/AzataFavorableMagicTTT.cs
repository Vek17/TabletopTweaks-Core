using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;

namespace TabletopTweaks.Core.NewComponents.OwlcatReplacements {
    [TypeId("e3b2dcf430cb449684c76fd854e732ea")]
    public class AzataFavorableMagicTTT : UnitFactComponentDelegate,
        IWasRoll,
        IGlobalSubscriber,
        ISubscriber {

        public override void OnTurnOn() {
            base.Owner.State.Features.AzataFavorableMagic.Retain();
        }

        public override void OnTurnOff() {
            base.Owner.State.Features.AzataFavorableMagic.Release();
        }

        public void WasRoll(RulebookEvent ruleEvent, RuleRollD20 ruleRoll) {
            if (ruleEvent != null) {
                if (ruleEvent is RuleCheckConcentration || ruleEvent is RuleSpellResistanceCheck || ruleEvent is RuleCheckCastingDefensively) {
                    CheckReroll(ruleEvent, ruleRoll);
                    return;
                }
                RuleSavingThrow ruleSavingThrow;
                if ((ruleSavingThrow = (ruleEvent as RuleSavingThrow)) != null) {
                    RuleSavingThrow ruleEvent2 = ruleSavingThrow;
                    CheckReroll(ruleEvent2, ruleRoll);
                }
            }
        }

        private void CheckReroll(RuleSavingThrow ruleEvent, RuleRollD20 ruleRoll) {
            if (ruleRoll.Reason.Caster == Owner) {
                if (OnlySpells) {
                    AbilityType? abilityType = ruleEvent.Reason.Ability?.Blueprint?.Type ?? ruleEvent.Reason.Context?.SourceAbility?.Type;
                    if (abilityType != AbilityType.Spell) {
                        return;
                    }
                }
                ruleRoll.Reroll(Fact, false);
            }
        }

        private void CheckReroll(RulebookEvent ruleEvent, RuleRollD20 ruleRoll) {
            if (ruleEvent.Initiator == base.Owner) {
                ruleRoll.Reroll(Fact, true);
            }
        }

        public bool OnlySpells;
    }
}
