using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.ModLogic;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;

namespace TabletopTweaks.Core.Utilities {
    public static class QuickFixTools {
        public static void ReplaceSuppression(BlueprintBuff buff, ModContextBase context, bool continuous = false) {
            var oldComponents = buff.GetComponents<SuppressBuffs>().ToArray();
            if (oldComponents == null || oldComponents.Length == 0) { return; }
            buff.RemoveComponents<SuppressBuffs>();
            oldComponents.ForEach(oldComponent => {
                buff.AddComponent<SuppressBuffsTTT>(c => {
                    c.m_Buffs = oldComponent.m_Buffs;
                    c.Descriptor = oldComponent.Descriptor;
                    c.Schools = oldComponent.Schools;
                    c.Continuous = continuous;
                });
            });

            context.Logger.LogPatch(buff);
        }
        public static void ReplaceClassLevelsForPrerequisites(BlueprintFeature feature, ModContextBase context, params FeatureGroup[] groups) {
            var oldComponents = feature.GetComponents<ClassLevelsForPrerequisites>().ToArray();
            if (oldComponents == null || oldComponents.Length == 0) { return; }
            feature.RemoveComponents<ClassLevelsForPrerequisites>();
            oldComponents.ForEach(oldComponent => {
                feature.AddComponent<ClassLevelsForPrerequisitesTTT>(c => {
                    c.m_ActualClass = oldComponent.m_ActualClass;
                    c.m_FakeClass = oldComponent.m_FakeClass;
                    c.Modifier = oldComponent.Modifier;
                    c.Summand = oldComponent.Summand;
                    c.CheckedGroups = groups;
                });
            });
            
            context.Logger.LogPatch(feature);
        }
    }
}
