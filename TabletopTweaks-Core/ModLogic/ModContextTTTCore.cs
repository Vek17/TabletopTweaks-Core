using static TabletopTweaks.Core.Main;
using static UnityModManagerNet.UnityModManager;

namespace TabletopTweaks.Core.ModLogic {
    internal class ModContextTTTCore : ModContextBase {

        public ModContextTTTCore(ModEntry ModEntry) : base(ModEntry) {
            LoadAllSettings();
        }
        public override void LoadAllSettings() {
            LoadBlueprints("TabletopTweaks.Core.Config", TTTContext);
            LoadLocalization("TabletopTweaks.Core.Localization");
        }
        public override void AfterBlueprintCachePatches() {
            base.AfterBlueprintCachePatches();
            if (Debug) {
                //Blueprints.RemoveUnused();
                //SaveSettings(BlueprintsFile, Blueprints);
                ModLocalizationPack.RemoveUnused();
                SaveLocalization(ModLocalizationPack);
            }
        }
    }
}
