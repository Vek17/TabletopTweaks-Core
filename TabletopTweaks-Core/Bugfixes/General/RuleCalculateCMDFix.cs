using HarmonyLib;
using Kingmaker.RuleSystem.Rules;

namespace TabletopTweaks.Core.Bugfixes.General {
    class RuleCalculateCMDFix {
        [HarmonyPatch(typeof(RuleCalculateCMD), nameof(RuleCalculateCMD.Result), MethodType.Getter)]
        static class RuleCalculateCMD_Result_Fix {
            static void Postfix(RuleCalculateCMD __instance, ref int __result) {
                __result = __instance.Base.Result + __instance.TotalBonusValue;
            }
        }
    }
}
