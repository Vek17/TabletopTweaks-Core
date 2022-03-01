using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Core.NewComponents.Properties;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.NewContent.Archetypes {
    static class MadDog {
        public static void AddMadDogFeatures() {
            var madDogPetDRProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>(modContext: TTTContext, "MadDogPetDRProperty", bp => {
                bp.AddComponent<MadDogPetDRProperty>();
            });
        }
    }
}
