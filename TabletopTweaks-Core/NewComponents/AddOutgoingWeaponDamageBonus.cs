using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using System;
using TabletopTweaks.Core.Utilities;

namespace TabletopTweaks.Core.NewComponents {
    [TypeId("03f55b5c7cb0445ab32ce2c8d44704ec")]
    public class AddOutgoingWeaponDamageBonus : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateDamage>,
        IRulebookHandler<RuleCalculateDamage>,
        ISubscriber,
        IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateDamage evt) {
            if (evt.DamageBundle.First == null) { return; }

            var WeaponDamage = evt.DamageBundle.First;
            DamageTypeDescription description = GenerateTypeDescriptiron(WeaponDamage);
            DamageDescription damageDescriptor = description.GetDamageDescriptor(Helpers.CreateCopy(WeaponDamage.Dice.ModifiedValue), 0);
            damageDescriptor.TemporaryContext(dd => {
                dd.TypeDescription.Physical.Enhancement = description.Physical.Enhancement;
                dd.TypeDescription.Physical.EnhancementTotal = description.Physical.EnhancementTotal;
                dd.TypeDescription.Common.Alignment = WeaponDamage.AlignmentsMask;
                dd.SourceFact = this.Fact;
                if (BonusDamageMultiplier > 1) {
                    dd.ModifyDice(new DiceFormula(damageDescriptor.Dice.Rolls * BonusDamageMultiplier, damageDescriptor.Dice.Dice), this.Fact);
                }
                dd.AddModifier(new Modifier(WeaponDamage.Bonus * Math.Max(1, BonusDamageMultiplier), this.Fact, ModifierDescriptor.UntypedStackable));
            });
            evt.ParentRule.m_DamageBundle.m_Chunks.Insert(1, damageDescriptor.CreateDamage());
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
