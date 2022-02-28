using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static Kingmaker.UI.GenericSlot.EquipSlotBase;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.Bugfixes.Units {
    static class Enemies {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Bosses");
                PatchBalors();
            }
        }
        static void PatchBalors() {
            if (TTTContext.Fixes.Units.Enemies.IsDisabled("Balors")) { return; }

            var BalorVorpalStrikeFeature = Resources.GetBlueprint<BlueprintFeature>("acc4a16c4088f2546b4237dcbb774f14");
            var BalorVorpalStrikeBuff = Resources.GetBlueprint<BlueprintBuff>("5220bc4386bf3e147b1beb93b0b8b5e7");
            var Vorpal = Resources.GetBlueprintReference<BlueprintItemEnchantmentReference>("2f60bfcba52e48a479e4a69868e24ebc");

            BalorVorpalStrikeBuff.SetComponents();
            BalorVorpalStrikeBuff.AddComponent<BuffEnchantWornItem>(c => {
                c.m_EnchantmentBlueprint = Vorpal;
                c.Slot = SlotType.PrimaryHand;
            });
            BalorVorpalStrikeBuff.AddComponent<BuffEnchantWornItem>(c => {
                c.m_EnchantmentBlueprint = Vorpal;
                c.Slot = SlotType.SecondaryHand;
            });
            BalorVorpalStrikeFeature.AddComponent<RecalculateOnEquipmentChange>();

            TTTContext.Logger.LogPatch(BalorVorpalStrikeFeature);
            TTTContext.Logger.LogPatch(BalorVorpalStrikeBuff);
        }
    }
}
