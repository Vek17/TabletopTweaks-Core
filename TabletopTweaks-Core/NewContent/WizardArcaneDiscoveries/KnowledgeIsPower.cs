using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.MechanicsChanges;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.NewContent.WizardArcaneDiscoveries {
    static class KnowledgeIsPower {
        public static void AddKnowledgeIsPower() {
            var KnowledgeIsPower = Helpers.CreateBlueprint<BlueprintFeature>(modContext: TTTContext, $"KnowledgeIsPower", bp => {
                bp.SetName($"Knowledge Is Power");
                bp.SetDescription("Your understanding of physical forces gives you power over them.\n" +
                    "You add your Intelligence modifier on combat maneuver checks and to your CMD.");
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { };
                bp.AddComponent<CMDBonus>(c => {
                    c.Descriptor = (ModifierDescriptor)AdditionalModifierDescriptors.Untyped.Intelligence;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                });
                bp.AddComponent<CMBBonus>(c => {
                    c.Descriptor = (ModifierDescriptor)AdditionalModifierDescriptors.Untyped.Intelligence;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                });
                bp.AddComponent<RecalculateOnStatChange>(c => {
                    c.Stat = StatType.Intelligence;
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.StatBonus;
                    c.m_Stat = StatType.Intelligence;
                });
            });
            if (TTTContext.AddedContent.WizardArcaneDiscoveries.IsDisabled("KnowledgeIsPower")) { return; }
            ArcaneDiscoverySelection.AddToArcaneDiscoverySelection(KnowledgeIsPower);
        }
    }
}
