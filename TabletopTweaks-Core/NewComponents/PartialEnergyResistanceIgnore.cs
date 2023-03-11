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
                    if (Reduction > 0  && source.EnergyType == EnergyType) {
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
