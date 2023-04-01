using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    /// <summary>
    /// Applies an enchantment bonus to armor or shields based on the EnchantLevel context value.
    /// </summary>
    [AllowMultipleComponents]
    [TypeId("4db6644b48ed43a69e16d8c9b60dd775")]
    public class MagicalVestmentComponent : UnitBuffComponentDelegate<BuffEnchantWornItemData>,
        IAreaLoadingStagesHandler,
        IUnitEquipmentHandler {
        private BlueprintItemEnchantment Enchantment {
            get {
                return Enchantments[Math.Max(0, Math.Min(EnhancementBonus - 1, Enchantments.Length - 1))];
            }
        }

        private ReferenceArrayProxy<BlueprintItemEnchantment, BlueprintItemEnchantmentReference> Enchantments {
            get {
                return this.m_EnchantmentBlueprints;
            }
        }

        private int EnhancementBonus => Math.Min(m_EnchantmentBlueprints.Length, EnchantLevel.Calculate(base.Context));

        private void UpdateEffect() {
            base.Owner?.Stats?.GetStat(StatType.AC)?.RemoveModifiersFrom(base.Runtime);
            if (!base.Data.Enchantments.Empty()) { return; }
            base.Owner.Stats.GetStat(StatType.AC).RemoveModifiersFrom(base.Runtime);
            ItemEntity itemEntity = Shield ? base.Owner.Body.SecondaryHand.MaybeShield?.ArmorComponent : base.Owner.Body.Armor.MaybeArmor;
            if (itemEntity != null) {
                base.Data.Enchantments.Add(itemEntity.AddEnchantment(this.Enchantment, base.Context, null));
            } else {
                if (!Shield) {
                    base.Owner.Stats.GetStat(StatType.AC).AddModifierUnique(EnhancementBonus, base.Runtime, ModifierDescriptor.Armor);
                }
            }
        }

        public override void OnActivate() {
            UpdateEffect();
        }

        public override void OnDeactivate() {
            foreach (ItemEnchantment itemEnchantment in base.Data.Enchantments) {
                ItemEntity owner = itemEnchantment.Owner;
                if (owner != null) {
                    owner.RemoveEnchantment(itemEnchantment);
                }
            }
            base.Data.Enchantments.Clear();
            base.Owner?.Stats?.GetStat(StatType.AC)?.RemoveModifiersFrom(base.Runtime);
        }

        public override void OnTurnOff() {
            base.OnTurnOff();
            OnDeactivate();
        }

        public override void OnRemoved() {
            base.OnRemoved();
            OnDeactivate();
        }

        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem) {
            if (slot.Owner != Owner) {
                return;
            }
            OnActivate();
        }

        public void OnAreaScenesLoaded() { }

        public void OnAreaLoadingComplete() {
            UpdateEffect();
        }


        /// <summary>
        /// List of Enchantments to apply to equipment. Index corresponds to EnchantLevel - 1.
        /// </summary>
        [SerializeField]
        [FormerlySerializedAs("Enchantment")]
        public BlueprintItemEnchantmentReference[] m_EnchantmentBlueprints = new BlueprintItemEnchantmentReference[5];
        /// <summary>
        /// Enhancment bonus to apply. This is capped at the number of provided m_EnchantmentBlueprints.
        /// </summary>
        public ContextValue EnchantLevel;
        /// <summary>
        /// Apply to shield instead of armor.
        /// </summary>
        public bool Shield;
    }
}
