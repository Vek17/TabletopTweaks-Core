using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.NewContent.Feats {
    static class ShingleRunner {
        public static void AddShingleRunner() {
            var ShingleRunner = FeatTools.CreateSkillFeat(modContext: TTTContext, "ShingleRunner", StatType.SkillAthletics, StatType.SkillMobility, bp => {
                bp.SetName("Shingle Runner");
                bp.SetDescription("Many of those who dwell among the rooftops become skillful at making bounding " +
                    "leaps and clambering up steep surfaces, and learn how to land more safely when they fall." +
                    "\nYou get a +2 bonus on Athletics and " +
                    "Mobility skill checks. If you have 10 or more ranks in one of these skills," +
                    " the bonus increases to +4 for that skill.");
            });
            if (TTTContext.AddedContent.Feats.IsDisabled("ShingleRunner")) { return; }
            FeatTools.AddAsFeat(ShingleRunner);
        }
    }
}
