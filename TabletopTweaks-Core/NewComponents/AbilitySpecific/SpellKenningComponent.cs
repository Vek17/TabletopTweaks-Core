using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using TabletopTweaks.Core.NewUnitParts;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [TypeId("74f00427d6194decb20524bc0d284a1a")]
    public class SpellKenningComponent : UnitFactComponentDelegate {
        public override void OnActivate() {
            var spellKenning = Owner.Ensure<UnitPartSpellKenning>();
            if (m_Resource != null) {
                spellKenning.SetKenningResource(m_Resource);
            }
            spellKenning.AddKenningSpellbook(m_Spellbook, this.Fact);
            m_SpellLists.ForEach(list => {
                spellKenning.AddKenningSpellList(list, this.Fact);
            });
        }
        public override void OnDeactivate() {
            var spellKenning = Owner.Get<UnitPartSpellKenning>();
            if (spellKenning != null) {
                spellKenning.RemoveEntry(this.Fact);
            }
        }

        public BlueprintSpellbookReference m_Spellbook;
        public BlueprintSpellListReference[] m_SpellLists;
        public BlueprintAbilityResourceReference m_Resource;
    }
}
