using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using System.Linq;
using TabletopTweaks.Core.Utilities;

namespace TabletopTweaks.Core.NewComponents {
    [AllowMultipleComponents]
    [TypeId("9ea33bacd9fb466e996d243274f84f9a")]
    public class WeaponExtraDamageDice : WeaponEnchantmentLogic,
        IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
        IRulebookHandler<RuleCalculateWeaponStats>,
        ISubscriber, IInitiatorRulebookSubscriber {
        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
            if (evt.Weapon == base.Owner) {
                DamageDescription Damage = new DamageDescription {
                    TypeDescription = DamageType,
                    Dice = Value,
                    SourceFact = base.Fact
                };
                var weaponDamageTypeDescription = evt.DamageDescription.FirstOrDefault()?.TypeDescription;
                if (weaponDamageTypeDescription != null) {
                    Damage.TypeDescription.Physical.TemporaryContext(d => {
                        d.Enhancement = weaponDamageTypeDescription.Physical.Enhancement;
                        d.EnhancementTotal = weaponDamageTypeDescription.Physical.EnhancementTotal;
                    });
                    Damage.TypeDescription.Common.TemporaryContext(d => {
                        var weaponDamage = evt.DamageDescription.First().TypeDescription.Common;
                        d.Reality = weaponDamageTypeDescription.Common.Reality;
                        d.Alignment = weaponDamageTypeDescription.Common.Alignment;
                    });
                }
                evt.DamageDescription.Add(Damage);
            }
        }

        public DamageTypeDescription DamageType;
        public DiceFormula Value;
    }
}
