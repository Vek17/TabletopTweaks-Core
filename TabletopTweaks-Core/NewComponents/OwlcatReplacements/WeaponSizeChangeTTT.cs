using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaks.Core.NewComponents.OwlcatReplacements {
    [TypeId("b64469c566b94f2d9b83c66d777ad5df")]
    public class WeaponSizeChangeTTT : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateWeaponSizeBonus>,
        IRulebookHandler<RuleCalculateWeaponSizeBonus> {

        public void OnEventAboutToTrigger(RuleCalculateWeaponSizeBonus evt) {
        }

        public void OnEventDidTrigger(RuleCalculateWeaponSizeBonus evt) {
            if (IsValidWeapon(evt.Weapon.Blueprint)) {
                var preSize = evt.WeaponSize;
                evt.m_SizeShift += SizeChange.Calculate(base.Context);
                evt.WeaponDamageDice.Modify(
                   WeaponDamageScaleTable.Scale(evt.WeaponDamageDice.ModifiedValue, evt.WeaponSize, preSize, evt.Weapon.Blueprint), base.Fact
                );
            }
        }

        private bool IsValidWeapon(BlueprintItemWeapon weapon) {
            if (CheckRangeType && !RangeType.IsSuitableWeapon(weapon)) { return false; }
            if (CheckWeaponGroup && !weapon.FighterGroup.Contains(WeaponGroup)) { return false; }
            if (CheckWeaponCategory && !Categories.Contains(weapon.Category)) { return false; }

            return true;
        }

        public bool CheckRangeType;
        public WeaponRangeType RangeType;
        public bool CheckWeaponGroup;
        public WeaponFighterGroup WeaponGroup;
        public bool CheckWeaponCategory;
        public WeaponCategory[] Categories = new WeaponCategory[0];
        public ContextValue SizeChange = 1;
    }
}
