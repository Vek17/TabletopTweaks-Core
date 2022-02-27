using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.NewUnitParts;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    /// <summary>
    /// Allows specified class's spellbook to use spell combat and spell strike.
    /// </summary>
    [TypeId("594783c6f70b4eb3bc848902976d215b")]
    public class BroadStudyComponent : UnitFactComponentDelegate {
        public override void OnTurnOn() {
            base.Owner.Ensure<UnitPartBroadStudy>().AddEntry(CharacterClass, base.Fact);
        }

        public override void OnTurnOff() {
            base.Owner.Ensure<UnitPartBroadStudy>().RemoveEntry(base.Fact);
        }
        /// <summary>
        /// Class to allow spellbooks for spell combat and spell strike.
        /// </summary>
        public BlueprintCharacterClassReference CharacterClass;
    }
}
