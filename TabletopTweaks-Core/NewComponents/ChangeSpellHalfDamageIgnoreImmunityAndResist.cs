using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;

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
            var toAdd = new List<BaseDamage>();
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
                        toAdd.Add(newDamage);
                    }
                }
            }
            foreach (var newDamage in toAdd) {
                evt.Add(newDamage);
            }
        }
        public DamageEnergyType EnergyType;
        public bool CheckAbilityType = false;
        public AbilityType[] ValidAbilityTypes = new AbilityType[0];
    }
}
