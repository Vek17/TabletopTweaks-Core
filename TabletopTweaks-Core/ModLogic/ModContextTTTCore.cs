using TabletopTweaks.Core.Config;
using static TabletopTweaks.Core.Main;
using static UnityModManagerNet.UnityModManager;

namespace TabletopTweaks.Core.ModLogic {
    internal class ModContextTTTCore : ModContextBase {

        public ModContextTTTCore(ModEntry ModEntry) : base(ModEntry) {
            LoadBlueprints("TabletopTweaks.Core.Config", TTTContext);
            LoadAllSettings();
        }
        public override void LoadAllSettings() {
            LoadSettings("Blueprints.json", "TabletopTweaks.Core.Config", ref Blueprints);
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
