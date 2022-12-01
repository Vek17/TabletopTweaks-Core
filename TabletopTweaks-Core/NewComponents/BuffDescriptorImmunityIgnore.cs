using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.NewUnitParts;

namespace TabletopTweaks.Core.NewComponents {
    [TypeId("ebd1d39947344e15a478755d551b2a46")]
    public class BuffDescriptorImmunityIgnore : UnitFactComponentDelegate {

        public SpellDescriptorWrapper Descriptor;
        public override void OnTurnOn() {
            base.Owner.Ensure<UnitPartIgnoreBuffDescriptorImmunity>().AddEntry(Descriptor, base.Fact);
        }

        public override void OnTurnOff() {
            base.Owner.Ensure<UnitPartIgnoreBuffDescriptorImmunity>().RemoveEntry(base.Fact);
        }

    }
}
