using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using TabletopTweaks.Core.NewUnitParts;

namespace TabletopTweaks.Core.NewComponents {
    [TypeId("d1cf71ca58a6480481e0574de51bc771")]
    public class AddSpellResistancePenaltyTTT : UnitFactComponentDelegate {
        public override void OnTurnOn() {
            base.Owner.Ensure<UnitPartSpellResistanceTTT>().AddGlobalSRPenalty(Penalty.Calculate(base.Context), base.Fact);
        }
        public override void OnTurnOff() {
            var part = base.Owner.Get<UnitPartSpellResistanceTTT>();
            if (part == null) { return; }
            part.RemoveGlobalSRPenalty(base.Fact);
        }
        public ContextValue Penalty;
    }
}
