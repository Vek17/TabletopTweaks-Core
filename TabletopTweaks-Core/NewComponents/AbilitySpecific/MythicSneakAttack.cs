using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UI.ServiceWindow.CharacterScreen;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System.Linq;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    /// <summary>
    /// Increases the die size of sneak attack damage by one step.
    /// </summary>
    [TypeId("aabbfeda974c455aafe14d05efca4f67")]
    public class MythicSneakAttack : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RulePrepareDamage>,
        IRulebookHandler<RulePrepareDamage>,
        ISubscriber, IInitiatorRulebookSubscriber {
        public void OnEventAboutToTrigger(RulePrepareDamage evt) {
        }

        public void OnEventDidTrigger(RulePrepareDamage evt) {
            evt?.ParentRule?.m_DamageBundle?
                .Where(damage => damage.Sneak)
                .ForEach(damage => {
                    var rolls = damage.Dice.ModifiedValue.Rolls;
                    var originalDice = damage.Dice.ModifiedValue.Dice;

                    DiceFormula? formula = originalDice switch {
                        DiceType.D3 => new DiceFormula(rolls, DiceType.D4),
                        DiceType.D4 => new DiceFormula(rolls, DiceType.D6),
                        DiceType.D6 => new DiceFormula(rolls, DiceType.D8),
                        DiceType.D8 => new DiceFormula(rolls, DiceType.D10),
                        DiceType.D10 => new DiceFormula(rolls, DiceType.D12),
                        _ => null
                    };

                    if (formula is not null) {
                        damage.Dice.Modify(formula.Value, base.Fact);
                    }
                });

        }
    }
}
