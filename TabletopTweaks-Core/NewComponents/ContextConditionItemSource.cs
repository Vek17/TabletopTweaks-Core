using Kingmaker.UnitLogic.Mechanics.Conditions;

namespace TabletopTweaks.Core.NewComponents {
    public class ContextConditionItemSource : ContextCondition {

        public override string GetConditionCaption() {
            return $"Check if context has source item";
        }

        public override bool CheckCondition() {
            return base.Context?.SourceAbilityContext?.Ability?.SourceItem != null;
        }
    }
}
