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
using Kingmaker.UnitLogic;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Root;
using Kingmaker.UI.Common;
using Kingmaker.UI.MVVM._VM.Tooltip.Bricks;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.UI.Tooltip;
using Kingmaker;
using Owlcat.Runtime.UI.Tooltips;
using UnityEngine;
using Kingmaker.Utility;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;

namespace TabletopTweaks.Core.NewRules {
    public class RuleCalculateArmorAC : RulebookEvent {

        public int ArmorBonus => ArmorBaseBonus + ArmorEnhancementBonus + ArmorModifier + ArmoredMightBonus;
        public int ArmoredMightBonus {
            get {
                int limit = (base.Initiator.Progression.MythicLevel + 1) / 2;
                int bonus = (ArmorBaseBonus + ArmorEnhancementBonus + ArmorModifier) / 2;
                return m_ArmoredMight != null && !IsShield ? Math.Min(bonus, limit) : 0; ;
            }
        }
        public int ArmorBaseBonus => ArmorItem.Blueprint.ArmorBonus;
        public int ArmorEnhancementBonus => GameHelper.GetItemEnhancementBonus(ArmorItem);
        public int ArmorModifier => TotalBonusValue;
        public bool IsShield => ArmorItem.Shield != null;
        public UnitFact ArmoredMight => m_ArmoredMight;

        public RuleCalculateArmorAC([NotNull] UnitEntityData initiator, [NotNull] ItemEntityArmor armorItem) : base(initiator) {
            ArmorItem = armorItem;
        }

        public override void OnTrigger(RulebookEventContext context) {
        }

        public void EnableArmoredMight(UnitFact fact) {
            m_ArmoredMight = fact;
        }

        public readonly ItemEntityArmor ArmorItem;
        private UnitFact m_ArmoredMight;

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

        [HarmonyPatch(typeof(TooltipTemplateItem), nameof(TooltipTemplateItem.AddArmorClass), new Type[] { typeof(List<ITooltipBrick>) })]
        class TooltipTemplateItem_AddArmorClass_Patch {
            static bool Prefix(TooltipTemplateItem __instance, List<ITooltipBrick> bricks) {
                var ArmorItem = __instance.m_Item as ItemEntityArmor;
                var Wielder = Game.Instance?.SelectionCharacter?.CurrentSelectedCharacter;
                if (Wielder == null) { return true; }
                if (ArmorItem == null) {
                    ArmorItem = (__instance.m_Item as ItemEntityShield)?.ArmorComponent;
                }
                if (ArmorItem == null) { return true; }
                var CalculateArmorAC = Rulebook.Trigger<RuleCalculateArmorAC>(new RuleCalculateArmorAC(Wielder, ArmorItem));
                if (TryAddArmorClass(__instance, CalculateArmorAC, bricks)) {
                    __instance.AddArmorClassBase(bricks);
                    AddEnhancement(__instance, CalculateArmorAC, bricks);
                    AddModifiers(__instance, CalculateArmorAC, bricks);
                    __instance.AddEnergy(bricks);
                    __instance.AddEnergyResist(bricks);
                    bricks.Add(new TooltipBrickSeparator(TooltipBrickElementType.Small));
                }
                return false;
            }

            static private bool TryAddArmorClass(TooltipTemplateItem __instance, RuleCalculateArmorAC rule, List<ITooltipBrick> bricks) {
                string text = __instance.m_ItemTooltipData.GetText(TooltipElement.FullArmorClass);
                if (string.IsNullOrEmpty(text)) {
                    return false;
                }
                Sprite armorClass = BlueprintRoot.Instance.UIRoot.UIIcons.ArmorClass;
                string value = rule.ArmorBonus.ToString();
                string glossaryEntryName = UIUtility.GetGlossaryEntryName(TooltipElement.ArmorClass.ToString());
                return __instance.TryAddIconValueStat(bricks, value, glossaryEntryName, armorClass, TooltipIconValueStatType.Normal, null);
            }

            static private void AddEnhancement(TooltipTemplateItem __instance, RuleCalculateArmorAC rule, List<ITooltipBrick> bricks) {
                string text = UIUtility.AddSign(GameHelper.GetItemEnhancementBonus(rule.ArmorItem));
                if (string.IsNullOrEmpty(text)) {
                    return;
                }
                string enhancementTextSymbol = UIUtilityTexts.EnhancementTextSymbol;
                string glossaryEntryName = UIUtility.GetGlossaryEntryName(TooltipElement.Enhancement.ToString());
                bricks.Add(new TooltipBrickValueStatFormula(text, enhancementTextSymbol, glossaryEntryName, TooltipBrickElementType.Small));
            }

            static private void AddModifiers(TooltipTemplateItem __instance, RuleCalculateArmorAC rule, List<ITooltipBrick> bricks) {
                string enhancementTextSymbol = UIUtilityTexts.EnhancementTextSymbol;
                rule.AllBonuses.ForEach(modifier => {
                    string glossaryEntryName = (modifier.Fact?.Blueprint as BlueprintUnitFact)?.Name ?? Game.Instance.BlueprintRoot.LocalizedTexts.AbilityModifiers.GetName(modifier.Descriptor);
                    string text = UIUtility.AddSign(modifier.Value);
                    bricks.Add(new TooltipBrickValueStatFormula(text, enhancementTextSymbol, glossaryEntryName, TooltipBrickElementType.Small));
                });
                if (rule.ArmoredMightBonus > 0) {
                    string text = UIUtility.AddSign(rule.ArmoredMightBonus);
                    bricks.Add(new TooltipBrickValueStatFormula(text, enhancementTextSymbol, rule.ArmoredMight?.Blueprint?.Name, TooltipBrickElementType.Small));
                }
            }
        }
    }  
}
