using TabletopTweaks.Core.Config;
using static UnityModManagerNet.UnityModManager;

namespace TabletopTweaks.Core.ModLogic {
    internal class ModContextTTTCore : ModContextBase {

        public ModContextTTTCore(ModEntry ModEntry) : base(ModEntry) {
            LoadAllSettings();
        }
        public override void LoadAllSettings() {
            LoadSettings("Blueprints.json", "TabletopTweaks.Core.Config", ref Blueprints);
            LoadLocalization("TabletopTweaks.Core.Localization");
        }
    }
}
