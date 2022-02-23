using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using TabletopTweaks.Core.Config;

namespace TabletopTweaks.Core.MechanicsChanges {
    static class SelectiveMetamagic {
        [HarmonyPatch(typeof(AreaEffectEntityData), "CheckSelective")]
        class UnitDescriptor_FixSizeModifiers_Patch {
            static void Postfix(AreaEffectEntityData __instance) {
                if (ModSettings.Fixes.BaseFixes.IsDisabled("SelectiveMetamagicNonInstantaneous")) { return; }
                __instance.m_CanAffectAllies = true;
            }
        }
    }
}
