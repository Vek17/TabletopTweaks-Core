using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Owlcat.Runtime.UI.Utility;
using System;

namespace TabletopTweaks.Core.NewComponents {
    [TypeId("a4d48fc6e20e42bb8226e279d730d918")]
    public class IncreaseSpellDiceSize : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>,
        ISubscriber, IInitiatorRulebookSubscriber {

        private EntityFact DisplayFact {
            get {
                EntityFact displayFact = null;
                if (UseFalseFact) {
                    if (m_FalseFact?.IsEmpty() ?? true) { return this.Fact; }
                    if (FalseFactFromCaster) {
                        displayFact = this.Context.MaybeCaster?.GetFact(m_FalseFact) ?? null;
                    } else {
                        displayFact = this.Owner.GetFact(m_FalseFact);
                    }
                }
                return displayFact ?? this.Fact;
            }
        }

        public void OnEventAboutToTrigger(RuleCalculateDamage evt) {
            foreach (BaseDamage baseDamage in evt.DamageBundle) {
                MechanicsContext context = evt.Reason.Context;
                if (context == null) { return; }
                if (!context.SourceAbility?.IsSpell ?? true) { return; }

                var AdjustedDice = StepUpDice(baseDamage.Dice.ModifiedValue.Dice, DiceIncreases.Calculate(this.Context));
                if (AdjustedDice <= this.MaxDice) {
                    baseDamage.Dice.Modify(new DiceFormula(baseDamage.Dice.ModifiedValue.Rolls, AdjustedDice), DisplayFact);
                } else {
                    baseDamage.Dice.Modify(new DiceFormula(baseDamage.Dice.ModifiedValue.Rolls, this.MaxDice), DisplayFact);
                    baseDamage.AddModifier(new Modifier(OvercapBonusPerStep.Calculate(this.Context) * DiceSizeDifference(AdjustedDice, MaxDice), DisplayFact, ModifierDescriptor.UntypedStackable));
                }
            }
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt) {
        }

        private static DiceType[] DiceIndexs = new DiceType[] {
            DiceType.One,
            DiceType.D2,
            DiceType.D3,
            DiceType.D4,
            DiceType.D6,
            DiceType.D8,
            DiceType.D10,
            DiceType.D12,
            DiceType.D20,
            DiceType.D100
        };
        DiceType StepUpDice(DiceType dice, int sizes) {
            var diceIndex = DiceIndexs.IndexOf(dice);
            var newIndex = Math.Min(diceIndex + sizes, DiceIndexs.Length - 1);
            return DiceIndexs[newIndex];
        }
        int DiceSizeDifference(DiceType dice1, DiceType dice2) {
            var diceIndex1 = DiceIndexs.IndexOf(dice1);
            var diceIndex2 = DiceIndexs.IndexOf(dice2);
            return Math.Abs(diceIndex1 - diceIndex2);
        }

        public DiceType MaxDice = DiceType.D12;
        public ContextValue DiceIncreases;
        public ContextValue OvercapBonusPerStep = 1;
        public bool UseFalseFact;
        public bool FalseFactFromCaster;
        public BlueprintUnitFactReference m_FalseFact;
    }
}
