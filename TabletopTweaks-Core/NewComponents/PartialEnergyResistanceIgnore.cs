using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System.Linq;
using UnityEngine;

namespace TabletopTweaks.Core.NewComponents {
    [AllowMultipleComponents]
    [TypeId("b25a641d55694b5eb9bf5293be79292f")]
    public class PartialEnergyResistanceIgnore : EntityFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateDamage>,
        IRulebookHandler<RuleCalculateDamage>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateDamage evt) {
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt) {
            var Type = evt.ParentRule?.Reason?.Ability?.Blueprint.Type ?? (AbilityType)(-1);
            if (CheckAbilityType && !ValidAbilityTypes.Contains(Type)) {
                return;
            }
            foreach (DamageValue damageValue in evt.CalculatedDamage) {
                EnergyDamage source = damageValue.Source as EnergyDamage;
                if (source != null) {
                    int Reduction = source.ReductionBecauseResistance;
                    if (Reduction > 0 && source.EnergyType == EnergyType) {
                        if (ByHalf) {
                            source.ReductionPenalty.Add(new Modifier(Mathf.CeilToInt((float)Reduction / 2f), this.Fact, ModifierDescriptor.UntypedStackable));
                        } else {
                            source.ReductionPenalty.Add(new Modifier(Value.Calculate(this.Context), this.Fact, ModifierDescriptor.UntypedStackable));
                        }
                    }
                }
            }
        }
        public DamageEnergyType EnergyType;
        public bool ByHalf = false;
        public ContextValue Value = new ContextValue();
        public bool CheckAbilityType = false;
        public AbilityType[] ValidAbilityTypes = new AbilityType[0];
    }
}
