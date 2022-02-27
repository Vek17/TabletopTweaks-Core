using Kingmaker.Blueprints.Classes.Selection;
using static TabletopTweaks.Core.Main;
using TabletopTweaks.Core.Utilities;

namespace TabletopTweaks.Core.NewContent.Feats {
    static class ExtraRogueTalent {
        public static void AddExtraRogueTalent() {
            var RogueTalentSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("c074a5d615200494b8f2a9c845799d93");
            var SylvanTricksterTalentSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("290bbcc3c3bb92144b853fd8fb8ff452");

            var ExtraRogueTalent = FeatTools.CreateExtraSelectionFeat("ExtraRogueTalent", RogueTalentSelection, bp => {
                bp.SetName("Extra Rogue Talent");
                bp.SetDescription("You gain one additional rogue talent. You must meet all of the prerequisites for this rogue talent." +
                    "\nYou can gain Extra Rogue Talent multiple times.");
            });
            var ExtraRogueTalentSylvan = FeatTools.CreateExtraSelectionFeat("ExtraRogueTalentSylvan", SylvanTricksterTalentSelection, bp => {
                bp.SetName("Extra Rogue Talent (Sylvan Trickster)");
                bp.SetDescription("You gain one additional rogue talent. You must meet all of the prerequisites for this rogue talent." +
                    "\nYou can gain Extra Rogue Talent multiple times.");
            });
            if (ModContext.AddedContent.Feats.IsDisabled("ExtraRogueTalent")) { return; }
            FeatTools.AddAsFeat(ExtraRogueTalent);
            FeatTools.AddAsFeat(ExtraRogueTalentSylvan);
        }
    }
}
