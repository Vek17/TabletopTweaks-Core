using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.NewUnitParts;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [AllowMultipleComponents]
    [TypeId("43f6a6c5a7bf4141a1ebea193cb5b3f7")]
    public class AddAgeNegate : UnitFactComponentDelegate {
        public override void OnTurnOn() {
            var AgePart = Owner.Ensure<UnitPartAgeTTT>();
            AgePart.AddNegate(Age, Type);
        }
        public override void OnTurnOff() {
            var AgePart = Owner.Get<UnitPartAgeTTT>();
            if (AgePart == null) { return; }
            AgePart.RemoveNegate(Age, Type);
        }

        public UnitPartAgeTTT.AgeLevel Age;
        public UnitPartAgeTTT.NegateType Type;
    }
}
