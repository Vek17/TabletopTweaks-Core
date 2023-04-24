using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.NewUnitParts;
using Kingmaker.UnitLogic;

namespace TabletopTweaks.Core.NewComponents.OwlcatReplacements {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowMultipleComponents]
    [TypeId("9fb8efb1b84a413798c12ffb1e515eea")]
    public class ClassLevelsForPrerequisitesTTT : UnitFactComponentDelegate {

        public override void OnTurnOn() {
            base.Owner.Ensure<UnitPartFakeClassLevels>().AddPrerequisiteEntry(
                base.Fact,
                m_FakeClass,
                m_ActualClass,
                Modifier,
                Summand,
                CheckedGroups
            );
        }

        public override void OnTurnOff() {
            var unitPart = base.Owner.Get<UnitPartFakeClassLevels>();
            if (unitPart != null) {
                unitPart.RemovePrerequisiteEntry(base.Fact);
            }
        }

        public BlueprintCharacterClassReference m_FakeClass;
        public BlueprintCharacterClassReference m_ActualClass;
        public double Modifier = 1;
        public int Summand = 0;
        public FeatureGroup[] CheckedGroups = new FeatureGroup[0];
    }
}
