using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.FactLogic;

namespace TabletopTweaks.Core.NewComponents.OwlcatReplacements.DamageResistance {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowedOn(typeof(BlueprintUnit), false)]
    [AllowMultipleComponents]
    [TypeId("c1b871587fd24447a770da78f1319445")]
    public class TTAddDamageResistanceHardness : TTAddDamageResistanceBase {

        public override bool IsHardness => true;

        public override bool IsSameDRTypeAs(TTAddDamageResistanceBase other) {
            return other is TTAddDamageResistanceHardness;
        }

        protected override void AdditionalInitFromVanillaDamageResistance(AddDamageResistanceBase vanillaResistance) {
        }

        protected override bool Bypassed(ComponentRuntime runtime, BaseDamage damage, ItemEntityWeapon weapon) {
            return (damage.Type > DamageType.Physical)
                || (damage is PhysicalDamage physicalDamage && physicalDamage.MaterialsMask.HasFlag(PhysicalDamageMaterial.Adamantite) && this.CalculateValue(runtime) <= 20);
        }
    }
}
