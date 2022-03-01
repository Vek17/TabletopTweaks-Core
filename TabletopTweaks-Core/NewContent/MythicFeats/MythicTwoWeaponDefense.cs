using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.NewContent.MythicFeats {
    class MythicTwoWeaponDefense {
        public static void AddMythicTwoWeaponDefense() {
            var Icon_TwoWeaponDefense = AssetLoader.LoadInternal("Feats", "Icon_TwoWeaponDefense.png");
            var TwoWeaponDefenseFeature = Resources.GetModBlueprint<BlueprintFeature>(modContext: TTTContext, "TwoWeaponDefenseFeature");

            var TwoWeaponDefenseMythicFeature = Helpers.CreateBlueprint<BlueprintFeature>(modContext: TTTContext, "TwoWeaponDefenseMythicFeature", bp => {
                bp.SetName("Two-Weapon Defense (Mythic)");
                bp.SetDescription("Your graceful flow between attack and defense makes you difficult to hit.\n" +
                    "When using Two-Weapon Defense, you apply the highest enhancement " +
                    "bonus from your two weapons to the shield bonus granted by that feat.");
                bp.m_Icon = Icon_TwoWeaponDefense;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicFeat };
                bp.AddPrerequisiteFeature(TwoWeaponDefenseFeature);
            });

            if (TTTContext.AddedContent.MythicFeats.IsDisabled("MythicTwoWeaponDefense")) { return; }
            FeatTools.AddAsMythicFeat(TwoWeaponDefenseMythicFeature);
        }
    }
}
