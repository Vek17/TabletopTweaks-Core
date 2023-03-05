using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;

namespace TabletopTweaks.Core.NewComponents {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("fcb8e58de3aa4dbab92026c7789ac790")]
    public class AddExtraSkillPoint : UnitFactComponentDelegate, IUnitCalculateSkillPointsOnLevelupHandler, IUnitSubscriber, ISubscriber {
        public void HandleUnitCalculateSkillPointsOnLevelup(LevelUpState state, ref int extraSkillPoints) {
            extraSkillPoints += Value * base.Fact.GetRank();
        }
        public int Value = 1;
    }
}
