using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Properties;

namespace TabletopTweaks.Core.Bugfixes.General {
    internal class PropertyFixes {
        //[HarmonyPatch(typeof(ClassLevelGetter), nameof(ClassLevelGetter.GetBaseValue))]
        static class AbilityData_RequireFullRoundAction_QuickStudy_Patch {
            static void Postfix(ClassLevelGetter __instance, UnitEntityData unit, ref int __result) {
                if (__instance.Archetype != null) {
                    ClassData classData = unit.Progression.GetClassData(__instance.Class);
                    if (classData == null || !classData.Archetypes.Contains(__instance.Archetype)) {
                        __result = 0;
                        return;
                    }
                }
                __result = unit.Progression.GetClassLevel(__instance.Class);
            }
        }
    }
}
