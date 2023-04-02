using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.NewUnitParts;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("4d50378d99f943988d64a46f9ed880b5")]
    public class SplitHexBuffComponent : UnitFactComponentDelegate {
        public override void OnTurnOn() {
            var SplitHexPart = Owner.Ensure<UnitPartSplitHex>();
            SplitHexPart.SplitHexEnabled.Retain();
        }
        public override void OnTurnOff() {
            var SplitHexPart = Owner.Get<UnitPartSplitHex>();
            if (SplitHexPart != null) {
                SplitHexPart.SplitHexEnabled.Release();
            }
        }
    }
}
