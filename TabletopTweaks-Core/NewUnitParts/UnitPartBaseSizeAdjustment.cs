using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks.Core.NewUnitParts {
    public class UnitPartBaseSizeAdjustment : OldStyleUnitPart, IUnitSizeHandler {
        public Size GetAdjustedSize(Size originalSize) {
            var adjustment = Adjustments.LastItem();
            if (adjustment == null) { return originalSize; }
            if (adjustment.Type == ChangeType.Value) {
                return adjustment.Size;
            }
            Size result = originalSize.Shift(adjustment.SizeDelta);
            return result;
        }
        public void AddEntry(int sizeDelta, EntityFact source) {
            Adjustments.Add(new BaseSizeAdjustmentEntry(sizeDelta, source));
            skipEvent = true;
            UpdateSize();
        }
        public void AddEntry(Size size, EntityFact source) {
            Adjustments.Add(new BaseSizeAdjustmentEntry(size, source));
            skipEvent = true;
            UpdateSize();
        }
        public void RemoveEntry(EntityFact source) {
            Adjustments.RemoveAll((BaseSizeAdjustmentEntry c) => c.Source == source);
            skipEvent = true;
            UpdateSize();
            TryRemove();
        }
        private void TryRemove() {
            if (!Adjustments.Any()) { this.RemoveSelf(); }
        }
        private void UpdateSize() {
            base.Owner.State.Size = Owner.OriginalSize;
            var adjustment = this.Adjustments.LastItem();
            var changeSizePart = Owner.Get<UnitPartSizeModifier>();
            if (adjustment == null) {
                if (changeSizePart != null) {
                    changeSizePart.UpdateSize();
                }
                return;
            }
            if (adjustment.Type == ChangeType.Value) {
                base.Owner.State.Size = GetAdjustedSize(Owner.State.Size);
                if (changeSizePart != null) {
                    changeSizePart.UpdateSize();
                }
                return;
            }
            if (changeSizePart != null) {
                changeSizePart.UpdateSize();
            }
            base.Owner.State.Size = GetAdjustedSize(Owner.State.Size);
            skipEvent = false;
        }

        public void HandleUnitSizeChanged(UnitEntityData unit) {
            if (unit == Owner) {
                if (skipEvent) { return; }
                skipEvent = true;
                UpdateSize();
            }
        }
        private bool skipEvent = false;
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
    }
}
