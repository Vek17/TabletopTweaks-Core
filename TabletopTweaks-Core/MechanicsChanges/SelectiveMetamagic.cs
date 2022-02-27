using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.MechanicsChanges {
    static class SelectiveMetamagic {
        [HarmonyPatch(typeof(AreaEffectEntityData), "CheckSelective")]
        class UnitDescriptor_FixSizeModifiers_Patch {
            static void Postfix(AreaEffectEntityData __instance) {
                if (ModContext.Fixes.BaseFixes.IsDisabled("SelectiveMetamagicNonInstantaneous")) { return; }
                __instance.m_CanAffectAllies = true;
            }
        }
    }
}
