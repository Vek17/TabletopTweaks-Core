using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Mechanics.Conditions;

namespace TabletopTweaks.Core.NewComponents {
    [TypeId("1012b102496f4bfd98c0b854bfe5f98e")]
    public class ContextConditionSizeDifferneceFromCaster : ContextCondition {
        public override bool CheckCondition() {
            var casterSize = base.Context.MaybeCaster?.State.Size ?? 0;
            var targetSize = base.Target?.Unit?.State.Size ?? 0;

            if (delta == 0) {
                return casterSize == targetSize;
            }
            if (delta > 0) {
                return (targetSize - casterSize) <= delta;
            }
            return (casterSize - targetSize) <= delta;
        }

        public override string GetConditionCaption() {
            return "Check if target has size delta in range of caster";
        }

        public int delta;
    }
}
