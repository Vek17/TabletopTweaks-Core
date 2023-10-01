using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;

using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Blueprints;

namespace TabletopTweaks.Core.NewComponents {
    [TypeId("4494a51dbdfe493b9e9a95bafb4b1a4c")]
    public class AddContextCasterLevelBonus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber {
        public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt) {
            if (this.SpellsOnly && evt.Spellbook == null) {
                return;
            }
            SpellDescriptorComponent component = evt.Spell.GetComponent<SpellDescriptorComponent>();
            if (component == null) {
                return;
            }
            SpellDescriptor spellDescriptor = component.Descriptor.Value;
            spellDescriptor = UnitPartChangeSpellElementalDamage.ReplaceSpellDescriptorIfCan<UnitEntityData>(base.Owner, spellDescriptor);
            if (spellDescriptor.HasAnyFlag(this.Descriptor)) {
                evt.AddBonusCasterLevel(this.BonusCasterLevel.Calculate(this.Context), this.ModifierDescriptor);
            }
        }

        public void OnEventDidTrigger(RuleCalculateAbilityParams evt) {
        }

        public SpellDescriptorWrapper Descriptor;
        public ContextValue BonusCasterLevel;
        public ModifierDescriptor ModifierDescriptor = ModifierDescriptor.UntypedStackable;
        public bool SpellsOnly;
    }
}
