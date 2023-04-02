using HarmonyLib;
using Kingmaker.EntitySystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks.Core.NewUnitParts {
    public class UnitPartBaseSizeAdjustment : OldStyleUnitPart {
        public int GetSizeDelta(Size originalSize) {
            var adjustment = Adjustments.LastItem();
            if (adjustment == null) { return 0; }
            if (adjustment.Type == ChangeType.Value) {
                return adjustment.Size - originalSize;
            }
            Size result = originalSize.Shift(adjustment.SizeDelta);
            return result - originalSize;
        }
        public void AddEntry(int sizeDelta, EntityFact source) {
            Adjustments.Add(new BaseSizeAdjustmentEntry(sizeDelta, source));
            UpdateSize();
        }
        public void AddEntry(Size size, EntityFact source) {
            Adjustments.Add(new BaseSizeAdjustmentEntry(size, source));
            UpdateSize();
        }
        public void RemoveEntry(EntityFact source) {
            Adjustments.RemoveAll((BaseSizeAdjustmentEntry c) => c.Source == source);
            UpdateSize();
            TryRemove();
        }
        private void TryRemove() {
            if (!Adjustments.Any()) { this.RemoveSelf(); }
        }
        private void UpdateSize() {
            currentSizeDelta = 0;
            var adjustment = this.Adjustments.LastItem();
            if (adjustment == null) {
                return;
            }
            currentSizeDelta = GetSizeDelta(Owner.OriginalSize);
            this.Owner.UpdateSizeModifiers();
            EventBus.RaiseEvent<IUnitSizeHandler>(delegate (IUnitSizeHandler h) {
                h.HandleUnitSizeChanged(this.Owner.Unit);
            }, true);
        }
        public int currentSizeDelta;
        private readonly List<BaseSizeAdjustmentEntry> Adjustments = new();
        public class BaseSizeAdjustmentEntry {
            public EntityFactRef Source;
            public int SizeDelta;
            public Size Size;
            public ChangeType Type;
            public BaseSizeAdjustmentEntry(int sizeDelta, EntityFact source) {
                Source = new EntityFactRef(source);
                SizeDelta = sizeDelta;
                Type = ChangeType.Delta;
            }
            public BaseSizeAdjustmentEntry(Size size, EntityFact source) {
                Source = new EntityFactRef(source);
                Size = size;
                Type = ChangeType.Value;
            }
        }
        public enum ChangeType {
            Delta,
            Value
        }

        [HarmonyPatch(typeof(UnitState), nameof(UnitState.Size), MethodType.Getter)]
        class UnitState_Size_Patch {
            static void Postfix(UnitState __instance, ref Size __result) {
                //if (TTTContext.Fixes.BaseFixes.IsDisabled("FixMythicSpellbookSlotsUI")) { return; }
                var SizePart = __instance.Owner.Get<UnitPartBaseSizeAdjustment>();
                if (SizePart == null) { return; }
                __result += SizePart.currentSizeDelta;
            }
        }
    }
}
