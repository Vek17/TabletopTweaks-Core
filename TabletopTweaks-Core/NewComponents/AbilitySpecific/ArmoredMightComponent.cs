using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.NewRules;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    /// <summary>
    /// Grants a focus bonus to AC based on 50% of your armor's AC bonus capped at (your mythic rank + 1) / 2.
    /// </summary>
    [TypeId("e90c706b6fd84f90b4dcd35ef2699483")]
    public class ArmoredMightComponent : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateArmorAC>,
        IInitiatorRulebookSubscriber,
        ISubscriber {

        /*
        public override void OnTurnOn() {
            base.OnTurnOn();
            UpdateModifier();
        }

        public override void OnTurnOff() {
            base.OnTurnOff();
            DeactivateModifier();
        }

        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem) {
            if (slot.Owner != Owner) {
                return;
            }
            UpdateModifier();
        }

        public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit) {
            UpdateModifier();
        }

        private void UpdateModifier() {
            DeactivateModifier();
            ActivateModifier();
        }

        private void ActivateModifier() {
            if (Owner.Body.Armor.HasArmor && Owner.Body.Armor.Armor.Blueprint.IsArmor) {
                Owner.Stats.AC.AddModifierUnique(CalculateModifier(), base.Runtime, Descriptor);
            }
        }

        private void DeactivateModifier() {
            Owner.Stats.AC.RemoveModifiersFrom(base.Runtime);
        }

        private int CalculateModifier() {
            int itemEnhancementBonus = GameHelper.GetItemEnhancementBonus(Owner.Body.Armor.Armor);
            int focusBonus = Owner.Stats.AC.Modifiers
                .Where(m => m.Source != this.Fact)
                .Where(m => m.ModDescriptor == ModifierDescriptor.ArmorFocus)
                .Sum(m => m.ModValue);
            int mythicBonus = (Owner.Body.Armor.Armor.Blueprint.ArmorBonus + itemEnhancementBonus + focusBonus) / 2;
            int limit = (Owner.Progression.MythicLevel + 1) / 2;
            return Math.Min(mythicBonus, limit);
        }

        public void HandleUnitGainFact(EntityFact fact) {
            UpdateModifier();
        }

        public void HandleUnitLostFact(EntityFact fact) {
            UpdateModifier();
        }

        public ModifierDescriptor Descriptor = ModifierDescriptor.ArmorFocus;
        */

        public override void OnTurnOn() {
            base.OnTurnOn();
            base.Owner.Body.Armor?.Armor?.RecalculateStats();
        }

        public override void OnTurnOff() {
            base.OnTurnOff();
            base.Owner.Body.Armor?.Armor?.RecalculateStats();
        }

        public void OnEventAboutToTrigger(RuleCalculateArmorAC evt) {
            if (evt.IsShield) { return; }
            evt.EnableArmoredMight(base.Fact);
        }

        public void OnEventDidTrigger(RuleCalculateArmorAC evt) {
        }
    }
}
