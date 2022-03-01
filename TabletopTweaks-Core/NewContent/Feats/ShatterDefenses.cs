using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.NewContent.Feats {
    static class ShatterDefenses {
        public static void AddNewShatterDefenseBlueprints() {
            var ShatterDefenses = Resources.GetBlueprint<BlueprintFeature>("61a17ccbbb3d79445b0926347ec07577");

            var ShatterDefensesDisplayBuff = Helpers.CreateBuff(modContext: TTTContext, "ShatterDefensesDisplayBuff", bp => {
                bp.m_Icon = ShatterDefenses.m_Icon;
                bp.Stacking = StackingType.Prolong;
                bp.SetName("Shattered Defenses");
                bp.SetDescription("An opponent you affect with Shatter Defenses is flat-footed to your attacks.");
            });
            var ShatterDefensesBuff = Helpers.CreateBuff(modContext: TTTContext, "ShatterDefensesBuff", bp => {
                bp.m_Icon = ShatterDefenses.m_Icon;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Stack;
                bp.AddComponent<AddFactContextActions>(c => {
                    c.Activated = Helpers.CreateActionList(
                        new ContextActionApplyBuff() {
                            m_Buff = ShatterDefensesDisplayBuff.ToReference<BlueprintBuffReference>(),
                            DurationValue = new ContextDurationValue() {
                                m_IsExtendable = false,
                                Rate = DurationRate.Rounds,
                                DiceCountValue = 0,
                                BonusValue = 2
                            },
                            AsChild = true
                        }
                    );
                    c.NewRound = Helpers.CreateActionList();
                    c.Deactivated = Helpers.CreateActionList();
                });
                bp.AddComponent<ForceFlatFooted>(c => {
                    c.AgainstCaster = true;
                });
                bp.SetName("Shattered Defenses");
                bp.SetDescription("An opponent you affect with Shatter Defenses is flat-footed to your attacks.");
            });
        }
    }
}
