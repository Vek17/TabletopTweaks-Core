using Kingmaker.Blueprints.Items.Ecnchantments;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.NewContent.WeaponEnchantments {
    class TwoHandedDamageMultiplier {
        public static void AddTwoHandedDamageMultiplierEnchantment() {
            var TwoHandedDamageMultiplierEnchantment = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>(TTTContext, $"TwoHandedDamageMultiplierEnchantment", bp => {
                bp.SetName("Increased Damage Multiplier");
                bp.SetDescription("Attacks are made with a 1.5 damage multipler.");
                bp.SetPrefix("");
                bp.SetSuffix("");
                bp.m_EnchantmentCost = 1;
                bp.AddComponent<WeaponDamageMultiplierReplacement>(c => {
                    c.Multiplier = 1.5f;
                });
            });
        }
    }
}
