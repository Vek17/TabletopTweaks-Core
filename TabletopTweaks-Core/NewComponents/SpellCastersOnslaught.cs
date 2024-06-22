using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaks.Core.NewComponents {
    [TypeId("b5385c03dd58459aa33cc7ef5dde7648")]
    public class SpellCastersOnslaught : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCastSpell> {

        private BlueprintItemEnchantment FireEnchantment => m_FireEnchantment;
        private BlueprintItemEnchantment ColdEnchantment => m_ColdEnchantment;
        private BlueprintItemEnchantment AcidEnchantment => m_AcidEnchantment;
        private BlueprintItemEnchantment ElectricityEnchantment => m_ElectricityEnchantment;
        private BlueprintItemEnchantment SonicEnchantment => m_SonicEnchantment;
        private BlueprintItemEnchantment DivineEnchantment => m_DivineEnchantment;
        private BlueprintItemEnchantment[] AllEnchantments => new BlueprintItemEnchantment[] {
            FireEnchantment,
            ColdEnchantment,
            AcidEnchantment,
            ElectricityEnchantment,
            SonicEnchantment,
            DivineEnchantment
        };

        public void OnEventAboutToTrigger(RuleCastSpell evt) {
        }

        public void OnEventDidTrigger(RuleCastSpell evt) {
            if (!evt.Success || !(evt.Spell.Blueprint.Type == AbilityType.Spell)) { return; }

            if (evt.Spell.SpellDescriptor.HasFlag(SpellDescriptor.Fire)) {
                EnchantAllWeapons(1.Rounds(), base.Context, FireEnchantment);
            } else if (evt.Spell.SpellDescriptor.HasFlag(SpellDescriptor.Cold)) {
                EnchantAllWeapons(1.Rounds(), base.Context, ColdEnchantment);
            } else if (evt.Spell.SpellDescriptor.HasFlag(SpellDescriptor.Acid)) {
                EnchantAllWeapons(1.Rounds(), base.Context, AcidEnchantment);
            } else if (evt.Spell.SpellDescriptor.HasFlag(SpellDescriptor.Electricity)) {
                EnchantAllWeapons(1.Rounds(), base.Context, ElectricityEnchantment);
            } else if (evt.Spell.SpellDescriptor.HasFlag(SpellDescriptor.Sonic)) {
                EnchantAllWeapons(1.Rounds(), base.Context, SonicEnchantment);
            } else {
                EnchantAllWeapons(1.Rounds(), base.Context, DivineEnchantment);
            }
        }

        public void EnchantAllWeapons(Rounds duration, MechanicsContext context, BlueprintItemEnchantment enchantment) {
            if (Owner.Body.HandsAreEnabled && Owner.Body.PrimaryHand.MaybeWeapon != null && !Owner.Body.PrimaryHand.MaybeWeapon.Blueprint.IsNatural) {
                EnchantWeapon(Owner.Body.PrimaryHand.MaybeWeapon, duration, context, enchantment);
            }
            if (Owner.Body.HandsAreEnabled && Owner.Body.SecondaryHand.MaybeWeapon != null && !Owner.Body.PrimaryHand.MaybeWeapon.Blueprint.IsNatural) {
                EnchantWeapon(Owner.Body.SecondaryHand.MaybeWeapon, duration, context, enchantment);
            }
        }

        public void EnchantWeapon(ItemEntityWeapon item, Rounds duration, MechanicsContext context, BlueprintItemEnchantment enchantment) {
            if (item == null) {
                return;
            }
            PurgeEnchantments(item);
            item.AddEnchantment(enchantment, context, new Rounds?(duration));
        }

        public void PurgeEnchantments(ItemEntityWeapon item) {
            if (item == null) {
                return;
            }
            List<ItemEnchantment> enchantments = item.Enchantments;
            foreach (var enchantment in AllEnchantments) {
                ItemEnchantment itemEnchantment = enchantments.GetFact(enchantment);
                if (!itemEnchantment?.IsTemporary ?? true) {
                    continue;
                }
                item.RemoveEnchantment(itemEnchantment);
            }
        }

        public BlueprintWeaponEnchantmentReference m_FireEnchantment;
        public BlueprintWeaponEnchantmentReference m_ColdEnchantment;
        public BlueprintWeaponEnchantmentReference m_AcidEnchantment;
        public BlueprintWeaponEnchantmentReference m_ElectricityEnchantment;
        public BlueprintWeaponEnchantmentReference m_SonicEnchantment;
        public BlueprintWeaponEnchantmentReference m_DivineEnchantment;
    }
}
