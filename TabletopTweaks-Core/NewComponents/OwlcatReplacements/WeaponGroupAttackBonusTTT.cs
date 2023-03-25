using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;

namespace TabletopTweaks.Core.NewComponents.OwlcatReplacements {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowMultipleComponents]
    [TypeId("8e2425fe221c4c189983b8fe4add4f59")]
    public class WeaponGroupAttackBonusTTT : UnitFactComponentDelegate, 
        IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, 
        IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, 
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt) {
            if (evt.Weapon != null && evt.Weapon.Blueprint.FighterGroup.Contains(this.WeaponGroup)) {
                evt.AddModifier(AttackBonus.Calculate(base.Context) * base.Fact.GetRank(), base.Fact, this.Descriptor);
            }
        }

        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt) {
        }

        public WeaponFighterGroup WeaponGroup;
        public ContextValue AttackBonus;
        public ModifierDescriptor Descriptor;
    }
}
