using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Owlcat.QA.Validation;
using System.Linq;

namespace TabletopTweaks.Core.NewComponents.Properties {
    [TypeId("5f193022788a43d28c0bdaa913a21117")]
    public class BuffCountGetter : PropertyValueGetter {
        public override int GetBaseValue(UnitEntityData unit) {
            return unit.Buffs.Enumerable.Where(b => b.IsFromSpell && !b.IsNotDispelable).Count();
        }

        public override void ApplyValidation(ValidationContext context, int parentIndex) {
            base.ApplyValidation(context, parentIndex);
        }
    }
}
