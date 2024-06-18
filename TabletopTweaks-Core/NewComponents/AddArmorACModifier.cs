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
            evt.AddModifier(CalculateBaseValue(base.Fact.MaybeContext), base.Fact, Descriptor);
        }

        public void OnEventDidTrigger(RuleCalculateArmorAC evt) {
        }

        public int CalculateBaseValue(MechanicsContext context) {
            if (context == null) {
                PFLog.Default.ErrorWithReport("Context is missing", Array.Empty<object>());
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
