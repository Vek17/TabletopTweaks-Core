using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.Items;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [TypeId("74a953732ee34497b79fe48728c69188")]
    public class DisjointedEffect : ItemEnchantmentComponentDelegate<ItemEntity, DisjointedEffect.DisjointedEffectData> {

        private ReferenceArrayProxy<BlueprintEquipmentEnchantment, BlueprintEquipmentEnchantmentReference> IgnoreEnchantments => m_IgnoreEnchantments;

        public override void OnActivate() {
            base.OnTurnOn();
        }
        public override void OnTurnOn() {
            if (base.Data.HasDisjointed) {
                UpdateDisjoints();
                return; 
            }

            base.Owner.Enchantments
                .Where(e => e.Blueprint != base.OwnerBlueprint)
                .Where(e => !IgnoreEnchantments.Contains(e.Blueprint))
                .ForEach(e => {
                    e.Deactivate();
                    e.TurnOff();
                    base.Data.Enchantments.Add(e);
                });
            if (base.Owner is ItemEntityArmor) {
                //(base.Owner as ItemEntityArmor).RecalculateStats();
            }
            Data.HasDisjointed = true;
        }
        public void UpdateDisjoints() {
            base.Data.Enchantments.ForEach(e => {
                e.Fact?.Deactivate();
                e.Fact?.TurnOff();
            });
        }
        public override void OnTurnOff() {
            base.Data.Enchantments.ForEach(e => {
                e.Fact?.TurnOn();
                e.Fact?.Activate();
            });
            base.Data.Enchantments.Clear();
        }
        public override void OnDeactivate() {
            base.OnTurnOff();
        }

        public BlueprintEquipmentEnchantmentReference[] m_IgnoreEnchantments = new BlueprintEquipmentEnchantmentReference[0];

        public class DisjointedEffectData {
            public List<EntityFactRef<ItemEnchantment>> Enchantments = new List<EntityFactRef<ItemEnchantment>>();
            public bool HasDisjointed = false;
            public DisjointedEffectData() {}
        }
    }
}
