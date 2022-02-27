using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using static Kingmaker.Designers.Mechanics.Facts.ModifyD20;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    /// <summary>
    /// Customized version of ModifyD20 to support reroll mechanics with power of law Aeon gaze.
    /// </summary>
    [TypeId("df847aa8e2f94af2a4efe8c73228dc97")]
    public class AeonPowerOfLaw : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleSavingThrow>,
        IRulebookHandler<RuleSavingThrow>,
        IInitiatorRulebookHandler<RuleRollD20>,
        IRulebookHandler<RuleRollD20>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleSavingThrow evt) {
        }
        public void OnEventDidTrigger(RuleSavingThrow evt) {
        }

        public void OnEventAboutToTrigger(RuleRollD20 evt) {

        }

        public void OnEventDidTrigger(RuleRollD20 evt) {
            if (ShouldReroll(evt)) {
                evt.Override(RollResult.Calculate(base.Context));
            }
        }

        private bool ShouldReroll(RuleRollD20 rollRule) {
            switch (this.RollCondition) {
                case RollConditionType.ShouldBeMoreThan:
                    return rollRule.Result > this.ValueToCompareRoll.Calculate(base.Context);
                case RollConditionType.ShouldBeLessThan:
                    return rollRule.Result < this.ValueToCompareRoll.Calculate(base.Context);
                case RollConditionType.ShouldBeLessOrEqualThan:
                    return rollRule.Result <= this.ValueToCompareRoll.Calculate(base.Context);
                case RollConditionType.ShouldBeMoreOrEqualThan:
                    return rollRule.Result >= this.ValueToCompareRoll.Calculate(base.Context);
                case RollConditionType.Equal:
                    return rollRule.Result == this.ValueToCompareRoll.Calculate(base.Context);
                default:
                    return true;
            }
        }
        /// <summary>
        /// Value to override roll with if the required conditions are met.
        /// </summary>
        public ContextValue RollResult;
        /// <summary>
        /// Value to compare to the base roll to determine if it should be overriden.
        /// </summary>
        public ContextValue ValueToCompareRoll;
        /// <summary>
        /// Condition for overriding the base roll. Is used to compare with RollResult.
        /// <para>
        /// Comparision takes place as: Original Roll : RollCondition : RollResult
        /// </para>
        /// </summary>
        public RollConditionType RollCondition;
    }
}
