using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using System.Linq;
using TabletopTweaks.Core.NewEvents;

namespace TabletopTweaks.Core.NewComponents {
    [TypeId("6ae68d43906f4ccfba4f98e9f267353a")]
    public class AddFlatCriticalRangeIncrease : UnitFactComponentDelegate, ICriticalRangeCalculatedHandler {

        public void OnAfterCriticalRangeCalculated(RuleCalculateWeaponStats sourceRule, int value, ref int adjustment) {
            if (AllWeapons || (sourceRule.Weapon != null && WeaponCategories.Contains(sourceRule.Weapon.Blueprint.Category))) {
                adjustment += CriticalRangeIncrease.Calculate(base.Context);
            }
        }
        public bool AllWeapons;
        public WeaponCategory[] WeaponCategories = new WeaponCategory[0];
        public ContextValue CriticalRangeIncrease;
    }
}
