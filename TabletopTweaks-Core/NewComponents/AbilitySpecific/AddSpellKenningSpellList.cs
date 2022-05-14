using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using TabletopTweaks.Core.NewUnitParts;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [TypeId("bcb34c0bffd94a8b85b97c0865a30e23")]
    public  class AddSpellKenningSpellList : UnitFactComponentDelegate {
        public override void OnActivate() {
            var spellKenning = Owner.Ensure<UnitPartSpellKenning>();
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

        public BlueprintSpellListReference[] m_SpellLists;
    }
}
