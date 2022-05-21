using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using System;

namespace TabletopTweaks.Core.NewComponents {
    [TypeId("a364529376b34037b59a71b9306e1365")]
    public class ContextConditionCasterHasResource : ContextCondition {

        public override bool CheckCondition() {
            if (base.Context.MaybeCaster == null) {
                PFLog.Default.Error(this, "Caster is missing", Array.Empty<object>());
                return false;
            }
            return base.Context.MaybeCaster.Descriptor.Resources.HasEnoughResource(m_Resource, Amount);
        }

        public override string GetConditionCaption() {
            return "";
        }
        public BlueprintAbilityResourceReference m_Resource;
        public int Amount = 1;
    }
}
