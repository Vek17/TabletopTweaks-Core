using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;

namespace TabletopTweaks.Core.NewComponents {
    [TypeId("66a9a23164554207bf8245d3417f3dec")]
    public class WeaponChangeSizeParametrized : UnitFactComponentDelegate,
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
            if (weapon.Category != base.Param) { return false; }

            return true;
        }

        public ContextValue SizeChange = 1;
    }
}
