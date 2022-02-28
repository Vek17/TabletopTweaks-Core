using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using TabletopTweaks.Core.Modlogic;

namespace TabletopTweaks.Core.Utilities {
    public static class ILUtils {
        public static void LogIL(List<CodeInstruction> codes, ModContextBase context) {
            context.Logger.LogVerbose("");
            for (int i = 0; i < codes.Count; i++) {
                object operand = codes[i].operand;
                if (operand is Label) {
                    context.Logger.LogVerbose($"{i} - {codes[i].labels.Aggregate("", (s, label) => $"{s}[{label.GetHashCode()}]")} - {codes[i].opcode} - {operand.GetHashCode()}");
                } else {
                    context.Logger.LogVerbose($"{i} - {codes[i].labels.Aggregate("", (s, label) => $"{s}[{label.GetHashCode()}]")} - {codes[i].opcode} - {codes[i].operand}");
                }
            }
        }
    }
}
