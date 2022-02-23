using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.Extensions;
using TabletopTweaks.Core.Utilities;

namespace TabletopTweaks.Core.NewContent.Features {
    public class FighterTrainingFakeLevel {
        public static void AddFighterTrainingFakeLevel() {
            var FighterTrainingFakeLevel = Helpers.CreateBlueprint<BlueprintFeature>("FighterTrainingFakeLevel", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 40;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.SetName("Fighter Training Fake Level");
            });
        }
    }
}
