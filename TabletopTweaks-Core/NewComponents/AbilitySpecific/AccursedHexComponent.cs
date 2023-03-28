using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.NewUnitParts;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [AllowMultipleComponents]
    [TypeId("06ce0cbace4243da87384dd4afb6f206")]
    public class AccursedHexComponent : UnitFactComponentDelegate {
        public override void OnTurnOn() {
            var UnitPartAccursedHex = Owner.Ensure<UnitPartAccursedHexTTT>();
            UnitPartAccursedHex.SetMythicFeat(m_MythicFeature);
            UnitPartAccursedHex.SetAccursedBuff(m_AccursedBuff);
        }
        public override void OnTurnOff() {
            var UnitPartAccursedHex = Owner.Get<UnitPartAccursedHexTTT>();
            if (UnitPartAccursedHex == null) { return; }
            UnitPartAccursedHex.Remove();
        }

        public BlueprintFeatureReference m_MythicFeature;
        public BlueprintBuffReference m_AccursedBuff;
    }
}
