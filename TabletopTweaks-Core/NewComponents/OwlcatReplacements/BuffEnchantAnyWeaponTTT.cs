using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UI.GenericSlot;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.Core.NewComponents.OwlcatReplacements {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("04083e8bd6fa4c39b1a5e7590e0ce9bd")]
    public class BuffEnchantAnyWeaponTTT : UnitBuffComponentDelegate<BuffEnchantAnyWeaponData>, IUnitActiveEquipmentSetHandler, IGlobalSubscriber, ISubscriber, IUnitEquipmentHandler {

        public BlueprintItemEnchantment Enchantment {
            get {
                BlueprintItemEnchantmentReference enchantmentBlueprint = this.m_EnchantmentBlueprint;
                if (enchantmentBlueprint == null) {
                    return null;
                }
                return enchantmentBlueprint.Get();
            }
        }

        public override void OnTurnOn() {
            this.OnDeactivate();
            this.OnActivate();
        }

        public override void OnActivate() {
            if (base.Data.GetAll().Count() > 0) {
                return;
            }

            if (this.Slot == EquipSlotBase.SlotType.PrimaryHand) {
                ItemEntity itemEntity = base.Owner.Body.PrimaryHand.HasWeapon ? base.Owner.Body.PrimaryHand.MaybeWeapon : base.Owner.Body.EmptyHandWeapon;
                if (itemEntity == null) {
                    return;
                }
                base.Data.AddEnchantment(itemEntity.AddEnchantment(this.Enchantment, base.Context, null));
                return;
            } else {
                if (this.Slot != EquipSlotBase.SlotType.SecondaryHand) {
                    if (this.Slot == EquipSlotBase.SlotType.AdditionalLimb) {
                        if (base.Owner.Body.AdditionalLimbs.Count > 0) {
                            List<WeaponSlot> additionalLimbs = base.Owner.Body.AdditionalLimbs;
                            for (int i = 0; i < additionalLimbs.Count; i++) {
                                if (additionalLimbs[i] != null) {
                                    ItemEntityWeapon maybeWeapon = additionalLimbs[i].MaybeWeapon;
                                    if (maybeWeapon != null) {
                                        base.Data.AddEnchantment(maybeWeapon.AddEnchantment(this.Enchantment, base.Context, null));
                                    }
                                }
                            }
                            return;
                        }
                        BuffEnchantAnyWeaponData data = base.Data;
                        ItemEntityWeapon emptyHandWeapon = base.Owner.Body.EmptyHandWeapon;
                        data.AddEnchantment((emptyHandWeapon != null) ? emptyHandWeapon.AddEnchantment(this.Enchantment, base.Context, null) : null);
                    }
                    return;
                }
                ItemSlot itemSlot = EquipSlotBase.ExtractSlot(this.Slot, base.Owner.Body);
                if (!itemSlot.HasItem) {
                    return;
                }
                ItemEntity itemEntity2 = itemSlot.Item;
                ItemEntityShield itemEntityShield = itemEntity2 as ItemEntityShield;
                if (itemEntityShield != null) {
                    if (itemEntityShield.WeaponComponent == null) {
                        return;
                    }
                    itemEntity2 = itemEntityShield.WeaponComponent;
                } else if (!(itemEntity2 is ItemEntityWeapon)) {
                    return;
                }
                base.Data.AddEnchantment(itemEntity2.AddEnchantment(this.Enchantment, base.Context, null));
                return;
            }
        }

        public override void OnDeactivate() {
            if (base.Data.GetAll().Count() == 0) {
                return;
            }
            base.Data.RemoveAndClear();
        }

        public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit) {
            if (unit != base.Owner) {
                return;
            }
            if (this.Slot == EquipSlotBase.SlotType.PrimaryHand || this.Slot == EquipSlotBase.SlotType.SecondaryHand || this.Slot == EquipSlotBase.SlotType.AdditionalLimb) {
                base.Data.RemoveAndClear();
                ItemEntity enchantmentInItem;
                if (this.Slot == EquipSlotBase.SlotType.PrimaryHand) {
                    enchantmentInItem = (base.Owner.Body.PrimaryHand.HasWeapon ? base.Owner.Body.PrimaryHand.MaybeWeapon : base.Owner.Body.EmptyHandWeapon);
                } else if (this.Slot == EquipSlotBase.SlotType.SecondaryHand) {
                    enchantmentInItem = (base.Owner.Body.SecondaryHand.HasWeapon ? base.Owner.Body.SecondaryHand.MaybeWeapon : base.Owner.Body.EmptyHandWeapon);
                } else {
                    enchantmentInItem = base.Owner.Body.AdditionalLimbs.Find(delegate (WeaponSlot limb) {
                        SimpleBlueprint o;
                        if (limb == null) {
                            o = null;
                        } else {
                            ItemEntity item = limb.Item;
                            o = ((item != null) ? item.Blueprint : null);
                        }
                        return o == this.Enchantment;
                    }).Item;
                }
                this.SetEnchantmentInItem(enchantmentInItem);
            }
        }

        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem) {
            if (slot.Owner != base.Owner) {
                return;
            }
            if (this.Slot == EquipSlotBase.SlotType.AdditionalLimb) {
                List<WeaponSlot> list = base.Owner.Body.AdditionalLimbs.FindAll(delegate (WeaponSlot limb) {
                    SimpleBlueprint o;
                    if (limb == null) {
                        o = null;
                    } else {
                        ItemEntity maybeItem = limb.MaybeItem;
                        o = ((maybeItem != null) ? maybeItem.Blueprint : null);
                    }
                    return o == this.Enchantment;
                });
                for (int i = 0; i < list.Count; i++) {
                    this.SetEnchantmentInItem(list[i].MaybeWeapon);
                }
                return;
            }
            if (EquipSlotBase.ExtractSlot(this.Slot, base.Owner.Body) == slot) {
                base.Data.RemoveAndClear();
                if (this.Slot == EquipSlotBase.SlotType.PrimaryHand) {
                    ItemEntity enchantmentInItem = base.Owner.Body.PrimaryHand.HasWeapon ? base.Owner.Body.PrimaryHand.MaybeWeapon : base.Owner.Body.EmptyHandWeapon;
                    this.SetEnchantmentInItem(enchantmentInItem);
                    return;
                }
                if (this.Slot == EquipSlotBase.SlotType.SecondaryHand) {
                    ItemEntity enchantmentInItem2 = base.Owner.Body.SecondaryHand.HasWeapon ? base.Owner.Body.SecondaryHand.MaybeWeapon : base.Owner.Body.EmptyHandWeapon;
                    this.SetEnchantmentInItem(enchantmentInItem2);
                }
            }
        }

        private void SetEnchantmentInItem(ItemEntity item) {
            if (item == null) {
                return;
            }
            if (item.EnchantmentsCollection != null && !item.EnchantmentsCollection.HasFact(this.Enchantment)) {
                base.Data.AddEnchantment(item.AddEnchantment(this.Enchantment, base.Context, null));
            }
        }

        [SerializeField]
        [FormerlySerializedAs("Enchantment")]
        public BlueprintItemEnchantmentReference m_EnchantmentBlueprint;
        public EquipSlotBase.SlotType Slot;
    }
}
