using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;

namespace TabletopTweaks.Core.NewComponents {
    [AllowedOn(typeof(BlueprintFeature), false)]
    [AllowedOn(typeof(BlueprintBuff), false)]
    [AllowMultipleComponents]
    [TypeId("5a17b3c967e746a6a9071a3c397de217")]
    public class AddArmorEnhancementBonusToStat : UnitFactComponentDelegate<AddWeaponEnhancementBonusToStat.ComponentData>,
        IUnitActiveEquipmentSetHandler,
        IGlobalSubscriber, ISubscriber,
        IUnitBuffHandler,
        IUnitEquipmentHandler {
        public override void OnTurnOn() {
            Update();
        }

        public override void OnTurnOff() {
            RemoveStatBonus();
        }

        public override void OnActivate() {
            Update();
        }

        public override void OnDeactivate() {
            RemoveStatBonus();
        }

        public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit) {
            if (unit != base.Owner) {
                return;
            }
            Update();
        }

        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem) {
            if (slot.Owner != base.Owner) {
                return;
            }
            Update();
        }

        public void HandleBuffDidAdded(Buff buff) {
            if (buff.Owner != base.Owner) { return; }
            Update();
        }

        public void HandleBuffDidRemoved(Buff buff) {
            if (buff.Owner != base.Owner) { return; }
            Update();
        }

        private bool ShouldApply() {
            if (Owner.Body.IsPolymorphed) { return false; }
            var Armor = Owner.Body?.Armor?.MaybeArmor;
            return Armor != null;
        }

        private void Update() {
            if (ShouldApply()) {
                AddStatBonus();
            } else {
                RemoveStatBonus();
            }
        }

        private void AddStatBonus() {
            int num = GetMaxBonus(base.Owner.Body.Armor);
            ModifiableValue stat = base.Owner.Stats.GetStat(Stat);
            int num2 = num * Multiplier;
            num2 += Kingmaker.UnitLogic.FactLogic.AddStatBonus.CalculatePowerfulChangeBonus(base.Fact, Stat);
            if (num2 != 0) {
                stat.RemoveModifiersFrom(base.Runtime);
                stat.AddModifier(num2, base.Runtime, Descriptor);
            }
        }

        private void RemoveStatBonus() {
            ModifiableValue stat = base.Owner.Stats.GetStat(Stat);
            if (stat == null) {
                return;
            }
            stat.RemoveModifiersFrom(base.Runtime);
        }

        private int GetMaxBonus(ArmorSlot armorSlot) {
            ItemEntityArmor MaybeArmor = armorSlot.MaybeArmor;
            if (MaybeArmor != null) {
                return GameHelper.GetItemEnhancementBonus(MaybeArmor);
            }
            return 0;
        }

        public ModifierDescriptor Descriptor;
        public StatType Stat;
        public int Multiplier = 1;
    }
}
