using Kingmaker.UnitLogic;
using System.Collections.Generic;
using static TabletopTweaks.Core.MechanicsChanges.AdditionalActivatableAbilityGroups;

namespace TabletopTweaks.Core.NewUnitParts {
    public class UnitPartActivatableAbilityGroupExtension : OldStyleUnitPart {

        public void IncreaseGroupSize(ExtentedActivatableAbilityGroup group) {
            if (m_GroupsSizeIncreases.ContainsKey(group)) {
                this.m_GroupsSizeIncreases[group] += 1;
            } else {
                m_GroupsSizeIncreases.Add(group, 1);
            }
        }

        public void DecreaseGroupSize(ExtentedActivatableAbilityGroup group) {
            if (m_GroupsSizeIncreases.ContainsKey(group)) {
                this.m_GroupsSizeIncreases[group] -= 1;
            }
        }

        public int GetGroupSize(ExtentedActivatableAbilityGroup group) {
            this.m_GroupsSizeIncreases.TryGetValue(group, out int result);
            return result + 1;
        }

        private SortedDictionary<ExtentedActivatableAbilityGroup, int> m_GroupsSizeIncreases = new SortedDictionary<ExtentedActivatableAbilityGroup, int>();
    }
}
