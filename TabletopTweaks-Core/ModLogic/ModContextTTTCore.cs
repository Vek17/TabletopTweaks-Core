using TabletopTweaks.Core.Modlogic;
using TabletopTweaks.Core.Config;
using static UnityModManagerNet.UnityModManager;

namespace TabletopTweaks.Core.ModLogic {
    internal class ModContextTTTCore : ModContextBase {
        public Fixes Fixes;
        public AddedContent AddedContent;
        public Homebrew Homebrew;

        public ModContextTTTCore(ModEntry ModEntry) : base(ModEntry) {
            LoadAllSettings();
        }
        public override void LoadAllSettings() {
            LoadSettings("Fixes.json", "TabletopTweaks.Core.Config", ref Fixes);
            LoadSettings("AddedContent.json", "TabletopTweaks.Core.Config", ref AddedContent);
            LoadSettings("Homebrew.json", "TabletopTweaks.Core.Config", ref Homebrew);
            LoadSettings("Blueprints.json", "TabletopTweaks.Core.Config", ref Blueprints);
            LoadLocalization("TabletopTweaks.Core.Localization");
        }
    }
}
