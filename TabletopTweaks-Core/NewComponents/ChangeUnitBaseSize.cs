using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using TabletopTweaks.Core.NewUnitParts;
using UnityEngine;


namespace TabletopTweaks.Core.NewComponents {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("bc8e9ee439304cf8a208f32f1f116f3a")]
    public class ChangeUnitBaseSize : UnitFactComponentDelegate {
        [UsedImplicitly]
        private bool IsTypeValue => m_Type == UnitPartBaseSizeAdjustment.ChangeType.Value;

        [UsedImplicitly]
        private bool IsTypeDelta => m_Type == UnitPartBaseSizeAdjustment.ChangeType.Delta;

        public override void OnTurnOn() {
            if (IsTypeValue) {
                base.Owner.Ensure<UnitPartBaseSizeAdjustment>().AddEntry(Size, base.Fact);
            }
            if (IsTypeDelta) {
                base.Owner.Ensure<UnitPartBaseSizeAdjustment>().AddEntry(SizeDelta, base.Fact);
            }
        }

        public override void OnTurnOff() {
            base.Owner.Ensure<UnitPartBaseSizeAdjustment>().RemoveEntry(base.Fact);
        }

        [SerializeField]
        public UnitPartBaseSizeAdjustment.ChangeType m_Type;

        [ShowIf("IsTypeDelta")]
        public int SizeDelta;

        [ShowIf("IsTypeValue")]
        public Size Size;
    }
}
