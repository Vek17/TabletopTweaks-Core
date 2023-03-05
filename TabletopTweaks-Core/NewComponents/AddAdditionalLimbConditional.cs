using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using UnityEngine;

namespace TabletopTweaks.Core.NewComponents {
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("5f5a69e7ffc0460a97d25d8daf8065b5")]
    public class AddAdditionalLimbConditional : UnitFactComponentDelegate<AddAdditionalLimb.ComponentData> {

        private BlueprintItemWeapon Weapon => m_Weapon?.Get();
        private BlueprintUnitFact CheckedFact => m_CheckedFact?.Get();

        public override void OnTurnOn() {
            base.OnTurnOn();
            if (Owner.HasFact(CheckedFact)) {
                base.Data.LimbIndex = base.Owner.Body.AddAdditionalLimb(this.Weapon, false);
            }
        }

        public override void OnTurnOff() {
            base.OnTurnOff();
            if (base.Data.LimbIndex > -1) {
                base.Owner.Body.RemoveAdditionalLimb(base.Data.LimbIndex);
            }
            base.Data.LimbIndex = -1;
        }

        [SerializeField]
        public BlueprintItemWeaponReference m_Weapon;
        public BlueprintUnitFactReference m_CheckedFact;

        public class ComponentData {
            public int LimbIndex = -1;
        }
    }
}
