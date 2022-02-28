using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Modlogic;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;

namespace TabletopTweaks.Core.Utilities {
    static class QuickFixTools {
        public static void ReplaceSuppression(BlueprintBuff buff, ModContextBase context, bool continuous = false) {
            var suppressBuffComponent = buff.GetComponent<SuppressBuffs>();
            if (suppressBuffComponent == null) { return; }
            buff.RemoveComponents<SuppressBuffs>();
            buff.AddComponent<SuppressBuffsTTT>(c => {
                c.m_Buffs = suppressBuffComponent.m_Buffs;
                c.Descriptor = suppressBuffComponent.Descriptor;
                c.Schools = suppressBuffComponent.Schools;
                c.Continuous = continuous;
            });

            context.Logger.LogPatch("Patched", buff);
        }
    }
}
