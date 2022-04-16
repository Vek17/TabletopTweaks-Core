using HarmonyLib;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;

namespace TabletopTweaks.Core.NewEvents {
    public interface ICriticalRangeCalculatedHandler : IUnitSubscriber {

        void OnAfterCriticalRangeCalculated(RuleCalculateWeaponStats sourceRule, int value, ref int adjustment);

        [HarmonyPatch(typeof(RuleCalculateWeaponStats), nameof(RuleCalculateWeaponStats.CriticalRange), MethodType.Getter)]
        static class RuleCalculateWeaponStats_CriticalRange_Patch {
            static bool runningEvent = false;
            static void Postfix(RuleCalculateWeaponStats __instance, ref int __result) {
                if (runningEvent) { return; }
                if (__instance.Initiator == null) { return; }
                runningEvent = true;
                var CriticalRange = __result;
                int adjustment = 0;
                EventBus.RaiseEvent<ICriticalRangeCalculatedHandler>(__instance.Initiator, h => h.OnAfterCriticalRangeCalculated(__instance, CriticalRange, ref adjustment));
                __result += adjustment;
                runningEvent = false;
            }
        }
    }
}
