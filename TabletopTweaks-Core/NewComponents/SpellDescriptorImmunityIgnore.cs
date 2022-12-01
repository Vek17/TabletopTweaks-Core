using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;

namespace TabletopTweaks.Core.NewComponents {

    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("fa39b200501a498c95c16d36612f5bf7")]
    public class SpellDescriptorImmunityIgnore :
    UnitFactComponentDelegate {
        public SpellDescriptorWrapper Descriptor;

        public override void OnTurnOn() {
            Owner.Ensure<UnitPartSpellResistance>().IgnoreImmunity(Descriptor);
        }

        public override void OnTurnOff() {
            Owner.Ensure<UnitPartSpellResistance>().RestoreImmunity(Descriptor);
        }
    }
}
