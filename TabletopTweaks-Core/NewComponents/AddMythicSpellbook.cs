using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;

namespace TabletopTweaks.Core.NewComponents {
    [TypeId("b5dccdaa75dd4cc28343d9cb08b468db")]
    public class AddMythicSpellbook : UnitFactComponentDelegate, IUnitReapplyFeaturesOnLevelUpHandler, IUnitSubscriber, ISubscriber {
        private BlueprintSpellbook Spellbook => m_Spellbook?.Get();

        public override void OnActivate() {
            if (base.IsReapplying) {
                return;
            }
            this.UpdateSpellbook();
        }

        public override void OnDeactivate() {
            if (base.IsReapplying) {
                return;
            }
            base.Owner.Descriptor.DeleteSpellbook(this.Spellbook);
        }

        public void HandleUnitReapplyFeaturesOnLevelUp() {
            this.UpdateSpellbook();
        }

        private void UpdateSpellbook() {
            Spellbook spellbook = base.Owner.DemandSpellbook(this.Spellbook);
            int num = this.m_CasterLevel.Calculate(base.Context);
            while (spellbook.CasterLevel < num) {
                int casterLevel = spellbook.CasterLevel;
                spellbook.AddMythicLevel();
                if (casterLevel == spellbook.CasterLevel) {
                    break;
                }
            }
        }

        public BlueprintSpellbookReference m_Spellbook;
        public ContextValue m_CasterLevel;
    }
}
