using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.NewContent.Feats {
    static class StreetSmarts {
        public static void AddStreetSmarts() {
            var StreetSmarts = FeatTools.CreateSkillFeat("StreetSmarts", StatType.SkillKnowledgeWorld, StatType.SkillPerception, bp => {
                bp.SetName("Street Smarts");
                bp.SetDescription("You are able to navigate the streets and personalities of whatever locale you run across." +
                    "\nYou get a +2 bonus on Knowledge (World) and " +
                    "Perception skill checks. If you have 10 or more ranks in one of these skills," +
                    " the bonus increases to +4 for that skill.");
            });
            if (TTTContext.AddedContent.Feats.IsDisabled("StreetSmarts")) { return; }
            FeatTools.AddAsFeat(StreetSmarts);
        }
    }
}
