using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Core.NewComponents.Properties;
using TabletopTweaks.Core.Utilities;

namespace TabletopTweaks.Core.NewContent.Archetypes {
    static class MadDog {
        public static void AddMadDogFeatures() {
            var madDogPetDRProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>("MadDogPetDRProperty", bp => {
                bp.AddComponent<MadDogPetDRProperty>();
            });
        }
    }
}
