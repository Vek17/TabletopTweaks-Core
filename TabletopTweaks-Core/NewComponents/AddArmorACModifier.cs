using Kingmaker;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.QA;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Linq;
using TabletopTweaks.Core.NewRules;

namespace TabletopTweaks.Core.NewComponents {
    [TypeId("886fbb8c19184266ad117d62b17a20ad")]
    public class AddArmorACModifier : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateArmorAC>,
        IInitiatorRulebookSubscriber,
        ISubscriber {

        public override void OnTurnOn() {
            base.OnTurnOn();
            if (IsShield) {
                base.Owner.Body.CurrentHandsEquipmentSet?.SecondaryHand?.MaybeShield?.ArmorComponent?.RecalculateStats();
            } else {
                base.Owner.Body.Armor?.Armor?.RecalculateStats();
            }
        }

        public override void OnTurnOff() {
            base.OnTurnOff();
            if (IsShield) {
                base.Owner.Body.CurrentHandsEquipmentSet?.SecondaryHand?.MaybeShield?.ArmorComponent?.RecalculateStats();
            } else {
                base.Owner.Body.Armor?.Armor?.RecalculateStats();
            }
        }

        public void OnEventAboutToTrigger(RuleCalculateArmorAC evt) {
            if (CheckArmorType && !ArmorTypes.Any(t => t == evt.ArmorItem.Blueprint.ProficiencyGroup)) { return; }
            Main.TTTContext.Logger.Log("AddArmorACModifier: Trigger");
            evt.AddModifier(CalculateBaseValue(base.Fact.MaybeContext), base.Fact, Descriptor);
            Main.TTTContext.Logger.Log($"AddArmorACModifier: StatBonus {CalculateBaseValue(base.Fact.MaybeContext)} ");
            Main.TTTContext.Logger.Log($"AddArmorACModifier: ContextOwner {base.Fact.MaybeContext.MaybeOwner?.CharacterName} ");
        }

        public void OnEventDidTrigger(RuleCalculateArmorAC evt) {
        }

        public int CalculateBaseValue(MechanicsContext context) {
            if (context == null) {
                PFLog.Default.ErrorWithReport("Context is missing", Array.Empty<object>());
                Main.TTTContext.Logger.Log("AddArmorACModifier: Context is missing");
                return 0;
            }
            return this.Value.Calculate(context);
        }

        public ContextValue Value = 0;
        public ModifierDescriptor Descriptor = ModifierDescriptor.ArmorFocus;
        public bool IsShield;
        public bool CheckArmorType;
        public ArmorProficiencyGroup[] ArmorTypes = new ArmorProficiencyGroup[0];
    }
}
