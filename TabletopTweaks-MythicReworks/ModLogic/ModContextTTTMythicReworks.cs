using TabletopTweaks.Core.Config;
using TabletopTweaks.Core.ModLogic;
using static UnityModManagerNet.UnityModManager;

namespace TabletopTweaks.MythicReworks.ModLogic {
    internal class ModContextTTTMythicReworks : ModContextBase {
        public Homebrew Homebrew;

        public ModContextTTTMythicReworks(ModEntry ModEntry) : base(ModEntry) {
            LoadAllSettings();
        }
        public override void LoadAllSettings() {
            LoadSettings("Homebrew.json", "TabletopTweaks.MythicReworks.Config", ref Homebrew);
            LoadBlueprints("TabletopTweaks.MythicReworks.Config");
            LoadLocalization("TabletopTweaks.MythicReworks.Localization");
        }
    }
}
