using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Mechanics;
using System.Runtime.Remoting.Contexts;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Enums;
using Kingmaker.Settings;
using static Kingmaker.Blueprints.BlueprintUnitTemplate;
using UnityEngine;
using static LayoutRedirectElement;

namespace TabletopTweaks.Core.NewComponents {
    [AllowMultipleComponents]
    [TypeId("b6ae817008344774b06eeef4364fe6c4")]
    public class ChangeSpellHalfDamageIgnoreImmunityAndResist : EntityFactComponentDelegate,
        IInitiatorRulebookHandler<RulePrepareDamage>,
        IRulebookHandler<RulePrepareDamage>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RulePrepareDamage evt) {
        }

        public void OnEventDidTrigger(RulePrepareDamage evt) {
            var Type = evt.ParentRule?.Reason?.Ability?.Blueprint.Type ?? (AbilityType)(-1);
            if (CheckAbilityType && !ValidAbilityTypes.Contains(Type)) {
                return;
            }
            foreach (BaseDamage baseDamage in evt.DamageBundle) {
                if (baseDamage.Type == DamageType.Energy) {
                    EnergyDamage energyDamage = baseDamage as EnergyDamage;
                    if (energyDamage == null) { continue; }
                    if (energyDamage.EnergyType == EnergyType && !baseDamage.Precision) {
                        EnergyDamage newDamage = new EnergyDamage(baseDamage.Dice.ModifiedValue, EnergyType);
                        if (!baseDamage.Half) {
                            baseDamage.Half.Set(true, this.Fact);
                        } else {
                            baseDamage.Durability *= 0.5f; 
                        }
                        newDamage.CopyFrom(baseDamage);
                        newDamage.SourceFact = this.Fact;
                        newDamage.IgnoreImmunities = true;
                        newDamage.IgnoreReduction = true;
                        evt.Add(newDamage);
                    }
                }
            }
        }
        public DamageEnergyType EnergyType;
        public bool CheckAbilityType = false;
        public AbilityType[] ValidAbilityTypes = new AbilityType[0];
    }
}
