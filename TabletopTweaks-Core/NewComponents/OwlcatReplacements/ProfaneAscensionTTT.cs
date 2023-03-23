using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System.Collections.Generic;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic;
using Kingmaker.EntitySystem.Entities;

namespace TabletopTweaks.Core.NewComponents.OwlcatReplacements {
    [AllowedOn(typeof(BlueprintFeature), false)]
    [AllowedOn(typeof(BlueprintBuff), false)]
    [AllowMultipleComponents]
    [TypeId("3bc52621bb2d42ae9233b55e38882b21")]
    public class ProfaneAscensionTTT : UnitFactComponentDelegate {

        public override void OnTurnOn() {
            var attributes = new List<StatType> {
                StatType.Strength,
                StatType.Dexterity,
                StatType.Constitution,
                StatType.Intelligence,
                StatType.Wisdom,
                StatType.Charisma
            };
            this.m_HighestStat = getHighestStat(base.Owner, attributes);
            attributes.Remove(m_HighestStat);
            this.m_SecondHighestStat = getHighestStat(base.Owner, attributes);
            int primaryBonus = this.HighestStatBonus.Calculate(base.Context);
            int secondaryBonus = this.SecondHighestStatBonus.Calculate(base.Context);
            base.Owner.Stats
                .GetStat(this.m_HighestStat)
                .AddModifier(primaryBonus, base.Runtime, this.Descriptor);
            base.Owner.Stats
                .GetStat(this.m_SecondHighestStat)
                .AddModifier(secondaryBonus, base.Runtime, this.Descriptor);
        }

        public override void OnTurnOff() {
            ModifiableValue stat = base.Owner.Stats.GetStat(this.m_HighestStat);
            if (stat != null) {
                stat.RemoveModifiersFrom(base.Runtime);
            }
            ModifiableValue stat2 = base.Owner.Stats.GetStat(this.m_SecondHighestStat);
            if (stat2 == null) {
                return;
            }
            stat2.RemoveModifiersFrom(base.Runtime);
        }

        static private StatType getHighestStat(UnitEntityData unit, IEnumerable<StatType> stats) {
            StatType highestStat = StatType.Unknown;
            int highestBaseValue = -1;
            int highestModValue = -1;
            foreach (StatType stat in stats) {
                var attribute = unit.Stats.GetStat(stat);
                var baseValue = attribute.BaseValue;
                var modValue = attribute.ModifiedValue;
                if (baseValue >= highestBaseValue) {
                    if (baseValue == highestBaseValue) { 
                        if(modValue > highestModValue) {
                            highestModValue = modValue;
                            highestStat = stat;
                        }
                    } else { 
                        highestBaseValue = baseValue;
                        highestStat = stat;
                    }
                }
            }
            return highestStat;
        }

        public ModifierDescriptor Descriptor;
        public ContextValue HighestStatBonus;
        public ContextValue SecondHighestStatBonus;
        private StatType m_HighestStat;
        private StatType m_SecondHighestStat;
    }
}
