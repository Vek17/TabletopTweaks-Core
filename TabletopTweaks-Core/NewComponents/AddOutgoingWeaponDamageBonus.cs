using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using System;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace TabletopTweaks.Core.NewComponents {
    [TypeId("03f55b5c7cb0445ab32ce2c8d44704ec")]
    public class AddOutgoingWeaponDamageBonus : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateDamage>,
        IRulebookHandler<RuleCalculateDamage>,
        ISubscriber,
        IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateDamage evt) {

            BaseDamage weaponDamage = evt.DamageBundle.WeaponDamage;
            if (weaponDamage == null) {
                return;
            }
            DiceFormula modifiedValue = weaponDamage.Dice.ModifiedValue;
            int extraDice = modifiedValue.Rolls * BonusDamageMultiplier;
            if (extraDice > 0) {
                weaponDamage.PostCritIncrements.AddDiceModifier(extraDice, this.Fact);
            }
            int bonusDamage = weaponDamage.TotalBonus * BonusDamageMultiplier;
            if (bonusDamage > 0) {
                weaponDamage.PostCritIncrements.AddBonusModifier(bonusDamage, this.Fact);
            }
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt) {
            if (RemoveAfterTrigger) {
                base.Owner.RemoveFact(base.Fact);
            }
        }

        private static DamageTypeDescription GenerateTypeDescriptiron(BaseDamage WeaponDamage) {
            DamageTypeDescription description = WeaponDamage.CreateTypeDescription();

            switch (WeaponDamage.Type) {
                case DamageType.Physical: {
                        var physical = WeaponDamage as PhysicalDamage;
                        description.Physical.Enhancement = physical.Enchantment;
                        description.Physical.EnhancementTotal = physical.EnchantmentTotal;
                        description.Physical.Form = physical.Form;
                        description.Physical.Material = physical.MaterialsMask;
                        return description;
                    }
                case DamageType.Energy:
                    var energy = WeaponDamage as EnergyDamage;
                    description.Energy = energy.EnergyType;
                    return description;
                case DamageType.Force:
                    var force = WeaponDamage as ForceDamage;
                    return description;
                case DamageType.Direct:
                    var direct = WeaponDamage as DirectDamage;
                    return description;
                case DamageType.Untyped:
                    var untyped = WeaponDamage as UntypedDamage;
                    return description;
                default:
                    return description;
            }
        }

        public int BonusDamageMultiplier = 1;
        public bool RemoveAfterTrigger = false;
    }
}
