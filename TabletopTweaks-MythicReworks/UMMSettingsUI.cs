using ModKit;
using TabletopTweaks.Core.Config;
using UnityModManagerNet;
using TabletopTweaks.Core;

namespace TabletopTweaks.MythicReworks {
    internal static class UMMSettingsUI {
        private static int selectedTab;
        public static void OnGUI(UnityModManager.ModEntry modEntry) {
            UI.AutoWidth();
            UI.TabBar(ref selectedTab,
                    () => UI.Label("SETTINGS WILL NOT BE UPDATED UNTIL YOU RESTART YOUR GAME.".yellow().bold()),
                    new NamedAction("Homebrew", () => SettingsTabs.Homebrew())
            );
        }
    }

    static class SettingsTabs {
        public static void Homebrew() {
            var TabLevel = SetttingUI.TabLevel.Zero;
            var Homebrew = Main.TTTContext.Homebrew;
            UI.Div(0, 15);
            using (UI.VerticalScope()) {
                UI.Toggle("New Settings Off By Default".bold(), ref Homebrew.NewSettingsOffByDefault);
                UI.Space(25);

                SetttingUI.SettingGroup("Feats", TabLevel, Homebrew.Feats);
                SetttingUI.SettingGroup("Mythic Abiltiies", TabLevel, Homebrew.MythicAbilities);
                SetttingUI.SettingGroup("Mythic Feats", TabLevel, Homebrew.MythicFeats);
                SetttingUI.NestedSettingGroup("Mythic Reworks", TabLevel, Homebrew.MythicReworks,
                    ("Aeon", Homebrew.MythicReworks.Aeon),
                    ("Azata", Homebrew.MythicReworks.Azata)
                );
            }
        }
    }
}
