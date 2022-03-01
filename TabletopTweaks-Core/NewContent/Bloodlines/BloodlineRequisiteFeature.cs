using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.NewContent.Bloodlines {
    static class BloodlineRequisiteFeature {
        public static void AddBloodlineRequisiteFeature() {
            var BloodlineRequisiteFeature = Helpers.CreateBlueprint<BlueprintFeature>(modContext: TTTContext, "BloodlineRequisiteFeature", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.SetName("Bloodline");
                bp.SetDescription("Bloodline Requisite Feature");
            });
        }
    }
}