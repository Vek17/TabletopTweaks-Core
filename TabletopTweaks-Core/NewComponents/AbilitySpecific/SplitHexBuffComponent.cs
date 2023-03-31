using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
