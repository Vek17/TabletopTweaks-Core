﻿using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using System.Linq;
using TabletopTweaks.Core.NewRules;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [TypeId("cf98f0d59230406d99ec6d88fc639d41")]
    public class MythicMediumArmorFocusEndurance : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateArmorAC>,
        IInitiatorRulebookSubscriber,
        ISubscriber {

        public override void OnTurnOn() {
            base.OnTurnOn();
            base.Owner.Body.Armor?.Armor?.RecalculateStats();
        }

        public override void OnTurnOff() {
            base.OnTurnOff();
            base.Owner.Body.Armor?.Armor?.RecalculateStats();
        }

        public void OnEventAboutToTrigger(RuleCalculateArmorAC evt) {
            if (CheckArmorType && !ArmorTypes.Any(t => t == evt.ArmorItem.Blueprint.ProficiencyGroup)) { return; }
            evt.EnableMythicMediumArmorEndurance(base.Fact);
        }

        public void OnEventDidTrigger(RuleCalculateArmorAC evt) {
        }

        public ModifierDescriptor Descriptor = ModifierDescriptor.ArmorFocus;
        public bool CheckArmorType;
        public ArmorProficiencyGroup[] ArmorTypes = new ArmorProficiencyGroup[0];
    }
}
