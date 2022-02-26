using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.Utilities;

namespace TabletopTweaks.Core.NewContent.Bloodlines {
    static class BloodlineRequisiteFeature {
        public static void AddBloodlineRequisiteFeature() {
            var BloodlineRequisiteFeature = Helpers.CreateBlueprint<BlueprintFeature>("BloodlineRequisiteFeature", bp => {
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