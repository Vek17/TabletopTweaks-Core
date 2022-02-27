using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.NewUnitParts;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    /// <summary>
    /// Allows mythic spellbooks to work with spell strike and spell combat.
    /// </summary>
    [TypeId("02978cf43c6c4959883c9e31734f183c")]
    public class BroadStudyMythicComponent : UnitFactComponentDelegate {
        public override void OnTurnOn() {
            base.Owner.Ensure<UnitPartBroadStudy>().AddMythicSource(base.Fact);
            foreach (var book in Spellbooks) {
                base.Owner.Ensure<UnitPartBroadStudy>().AddMythicSpellbook(book, base.Fact);
            }
        }

        public override void OnTurnOff() {
            base.Owner.Ensure<UnitPartBroadStudy>().RemoveEntry(base.Fact);
        }
        /// <summary>
        /// Additional spell books to consider mythic spellbooks.
        /// </summary>
        public BlueprintSpellbookReference[] Spellbooks;
    }
}
