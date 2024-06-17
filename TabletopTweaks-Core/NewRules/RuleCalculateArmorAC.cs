using HarmonyLib;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Kingmaker.EntitySystem.Entities;

namespace TabletopTweaks.Core.NewRules {
    public class RuleCalculateArmorAC : RulebookEvent {

        public int ArmorBonus => ArmorBaseBonus + ArmorEnhancementBonus + ArmorModifier + ArmoredMightBonus;
        public int ArmoredMightBonus {
            get {
                int limit = (base.Initiator.Progression.MythicLevel + 1) / 2;
                int bonus = (ArmorBaseBonus + ArmorEnhancementBonus + ArmorModifier) / 2;
                return m_ArmoredMight ? Math.Min(bonus, limit) : 0; ;
            }
        }
        public int ArmorBaseBonus => ArmorItem.Blueprint.ArmorBonus;
        public int ArmorEnhancementBonus => GameHelper.GetItemEnhancementBonus(ArmorItem);
        public int ArmorModifier => TotalBonusValue;

        public RuleCalculateArmorAC([NotNull] UnitEntityData initiator, [NotNull] ItemEntityArmor armorItem) : base(initiator) {
            ArmorItem = armorItem;
        }

        public override void OnTrigger(RulebookEventContext context) {
        }

        public void EnableArmoredMight() {
            m_ArmoredMight = true;
        }

        public readonly ItemEntityArmor ArmorItem;
        private bool m_ArmoredMight;

        private class CalculateArmor {
            [HarmonyPatch(typeof(ItemEntityArmor), nameof(ItemEntityArmor.RecalculateStats))]
            static class ItemEntityArmor_RecalculateStats_ICalculateArmorStatsHandler_Patch {

                static readonly MethodInfo EventTriggers_AddEvent = AccessTools.Method(
                    typeof(CalculateArmor),
                    nameof(CalculateArmor.CallEvent),
                    new Type[] { typeof(ItemEntityArmor) }
                );
                static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {

                    var codes = new List<CodeInstruction>();
                    //Utilities.ILUtils.LogIL(TTTContext, codes);
                    codes.AddRange(new CodeInstruction[] {
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Call, EventTriggers_AddEvent)
                    });
                    //Utilities.ILUtils.LogIL(TTTContext, codes);
                    return codes.AsEnumerable();
                }
            }

            private static void CallEvent(ItemEntityArmor itemArmor) {
                if (itemArmor.m_RecalculateInProgress) {
                    return;
                }
                if (itemArmor.Wielder == null) {
                    return;
                }
                itemArmor.m_RecalculateInProgress = true;
                List<ModifiableValue.Modifier> modifiers = itemArmor.m_Modifiers;
                if (modifiers != null) {
                    modifiers.ForEach(delegate (ModifiableValue.Modifier m)
                    {
                        ModifiableValue appliedTo = m.AppliedTo;
                        if (((appliedTo != null) ? appliedTo.Owner : null) != null) {
                            m.Remove();
                        }
                    });
                }
                itemArmor.m_Modifiers = null;
                ModifierDescriptor modifierDescriptor = (itemArmor.Shield != null) ? ModifierDescriptor.Shield : ModifierDescriptor.Armor;
                AddArmorACBonuses(itemArmor);
                int ArmorCheckPenalty = Rulebook.Trigger<RuleCalculateArmorCheckPenalty>(new RuleCalculateArmorCheckPenalty(itemArmor.Wielder.Unit, itemArmor)).Result;
                if (ArmorCheckPenalty < 0) {
                    AddArmorPenalties(itemArmor, ArmorCheckPenalty);
                }
                itemArmor.RecalculateMaxDexBonus();
                if (((itemArmor.ArmorType() == ArmorProficiencyGroup.Medium && !itemArmor.Wielder.State.Features.ImmunityToMediumArmorSpeedPenalty) || itemArmor.ArmorType() == ArmorProficiencyGroup.Heavy) && !itemArmor.Wielder.State.Features.ImmuneToArmorSpeedPenalty) {
                    int value = (itemArmor.Wielder.Stats.Speed.Racial < 30) ? -5 : -10;
                    itemArmor.m_Modifiers.Add(itemArmor.Wielder.Stats.Speed.AddItemModifier(value, itemArmor, modifierDescriptor));
                }
                itemArmor.m_RecalculateInProgress = false;
            }
            private static void AddArmorACBonuses(ItemEntityArmor itemArmor) {
                CharacterStats stats = itemArmor.Wielder.Stats;
                ModifierDescriptor modifierDescriptor = (itemArmor.Shield != null) ? ModifierDescriptor.Shield : ModifierDescriptor.Armor;
                itemArmor.m_Modifiers = new List<ModifiableValue.Modifier>();
                var RuleCalculateArmorAC = Rulebook.Trigger<RuleCalculateArmorAC>(new RuleCalculateArmorAC(itemArmor.Wielder, itemArmor));
                itemArmor.AddModifier(stats.AC, RuleCalculateArmorAC.ArmorBonus, modifierDescriptor);
            }
            private static void AddArmorPenalties(ItemEntityArmor itemArmor, int result) {
                CharacterStats stats = itemArmor.Wielder.Stats;
                ModifierDescriptor modifierDescriptor = (itemArmor.Shield != null) ? ModifierDescriptor.Shield : ModifierDescriptor.Armor;
                foreach (StatType type in ItemEntityArmor.PenaltyDependentSkills) {
                    itemArmor.AddModifier(itemArmor.Wielder.Stats.GetStat(type), result, modifierDescriptor);
                }
            }
        }
    }  
}
