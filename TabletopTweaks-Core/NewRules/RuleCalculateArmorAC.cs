using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.RuleSystem;
using Kingmaker.UI.Common;
using Kingmaker.UI.MVVM._VM.Tooltip.Bricks;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.UI.Tooltip;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using Owlcat.Runtime.UI.Tooltips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace TabletopTweaks.Core.NewRules {
    public class RuleCalculateArmorAC : RulebookEvent {

        public int ArmorBonus => ArmorBaseBonus + ArmorEnhancementBonus + ArmorModifier + MythicMediumArmorEnduranceBonus + ArmoredMightBonus;
        public int ArmoredMightBonus {
            get {
                int limit = (base.Initiator.Progression.MythicLevel + 1) / 2;
                int bonus = (ArmorBaseBonus + ArmorEnhancementBonus + ArmorModifier + MythicMediumArmorEnduranceBonus) / 2;
                return m_ArmoredMight != null && !IsShield ? Math.Min(bonus, limit) : 0; ;
            }
        }
        public int MythicMediumArmorEnduranceBonus {
            get {
                int limit = (base.Initiator.Progression.MythicLevel + 1) / 2;
                int bonus = (ArmorBaseBonus + ArmorEnhancementBonus + ArmorModifier) / 2;
                return m_MythicMediumArmorEndurance != null && !IsShield ? bonus : 0; ;
            }
        }
        public int ArmorBaseBonus => ArmorItem.Blueprint.ArmorBonus;
        public int ArmorEnhancementBonus => GameHelper.GetItemEnhancementBonus(ArmorItem);
        public int ArmorModifier => TotalBonusValue;
        public bool IsShield => ArmorItem.Shield != null;
        public UnitFact ArmoredMight => m_ArmoredMight;
        public UnitFact MythicMediumArmorEndurance => m_MythicMediumArmorEndurance;

        public RuleCalculateArmorAC([NotNull] UnitEntityData initiator, [NotNull] ItemEntityArmor armorItem) : base(initiator) {
            ArmorItem = armorItem;
        }

        public override void OnTrigger(RulebookEventContext context) {
        }

        public void EnableArmoredMight(UnitFact fact) {
            m_ArmoredMight = fact;
        }

        public void EnableMythicMediumArmorEndurance(UnitFact fact) {
            m_MythicMediumArmorEndurance = fact;
        }

        public readonly ItemEntityArmor ArmorItem;
        private UnitFact m_ArmoredMight;
        public UnitFact m_MythicMediumArmorEndurance;

        private class CalculateArmor {
            [HarmonyPatch(typeof(ItemEntityArmor), nameof(ItemEntityArmor.RecalculateStats))]
            static class ItemEntityArmor_RecalculateStats_RuleCalculateArmorAC_Patch {

                static readonly MethodInfo CalculateArmor_AddArmorACBonuses = AccessTools.Method(
                    typeof(CalculateArmor.ItemEntityArmor_RecalculateStats_RuleCalculateArmorAC_Patch),
                    nameof(CalculateArmor.ItemEntityArmor_RecalculateStats_RuleCalculateArmorAC_Patch.AddArmorACBonuses),
                    new Type[] { typeof(ItemEntityArmor) }
                );
                static readonly MethodInfo ItemEntityArmor_AddModifier = AccessTools.Method(
                    typeof(ItemEntityArmor),
                    nameof(ItemEntityArmor.AddModifier),
                    new Type[] { 
                        typeof(ModifiableValue),
                        typeof(int),
                        typeof(ModifierDescriptor),
                    }
                );
                static readonly MethodInfo GameHelper_GetItemEnhancementBonus = AccessTools.Method(
                    typeof(GameHelper),
                    nameof(GameHelper.GetItemEnhancementBonus),
                    new Type[] {
                        typeof(ItemEntity)
                    }
                );
                static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {

                    var codes = new List<CodeInstruction>(instructions);
                    var target = FindInsertionTarget(codes);

                    //Utilities.ILUtils.LogIL(TTTContext, codes);
                    for (int i = target.Index; i <= target.End; i++) {
                        codes[i] = new CodeInstruction(OpCodes.Nop);
                    }
                    codes[target.Index] = new CodeInstruction(OpCodes.Call, CalculateArmor_AddArmorACBonuses);
                    //Utilities.ILUtils.LogIL(TTTContext, codes);
                    return codes.AsEnumerable();
                }

                private static TargetInfo FindInsertionTarget(List<CodeInstruction> codes) {
                    var info = new TargetInfo(0, -1);
                    for (int i = 0; i < codes.Count; i++) {
                        if (codes[i].opcode == OpCodes.Call && codes[i].Calls(GameHelper_GetItemEnhancementBonus)) {
                            info.Index = i;
                            break;
                        }
                    }
                    var firstModifier = false;
                    for (int i = info.Index; i < codes.Count; i++) {
                        if (codes[i].opcode == OpCodes.Call && codes[i].Calls(ItemEntityArmor_AddModifier)) {
                            if (!firstModifier) { 
                                firstModifier = true; 
                                continue; }
                            info.End = i;
                            break;
                        }
                    }
                    if (info.End == -1 || info.Index == 0) {
                        Main.TTTContext.Logger.Log("RuleCalculateArmorAC: COULD NOT FIND TARGET");
                    }
                    return info;
                }
                private struct TargetInfo {
                    public int Index;
                    public int End;

                    public TargetInfo(int index, int end) {
                        this.Index = index;
                        this.End = end;
                    }
                }
                private static void AddArmorACBonuses(ItemEntityArmor itemArmor) {
                    CharacterStats stats = itemArmor.Wielder.Stats;
                    ModifierDescriptor modifierDescriptor = (itemArmor.Shield != null) ? ModifierDescriptor.Shield : ModifierDescriptor.Armor;
                    itemArmor.m_Modifiers = new List<ModifiableValue.Modifier>();
                    var RuleCalculateArmorAC = Rulebook.Trigger<RuleCalculateArmorAC>(new RuleCalculateArmorAC(itemArmor.Wielder, itemArmor));
                    itemArmor.AddModifier(stats.AC, RuleCalculateArmorAC.ArmorBonus, modifierDescriptor);
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
                    if (GameHelper.GetItemEnhancementBonus(rule.ArmorItem) == 0 || string.IsNullOrEmpty(text)) {
                        return;
                    }
                    string enhancementTextSymbol = UIUtilityTexts.EnhancementTextSymbol;
                    string glossaryEntryName = UIUtility.GetGlossaryEntryName(TooltipElement.Enhancement.ToString());
                    bricks.Add(new TooltipBrickValueStatFormula(text, enhancementTextSymbol, glossaryEntryName, TooltipBrickElementType.Small));
                }

                static private void AddModifiers(TooltipTemplateItem __instance, RuleCalculateArmorAC rule, List<ITooltipBrick> bricks) {
                    string directTextSymbol = UIUtilityTexts.DirectTextSymbol;
                    rule.AllBonuses.ForEach(modifier => {
                        string glossaryEntryName = (modifier.Fact?.Blueprint as BlueprintUnitFact)?.Name ?? Game.Instance.BlueprintRoot.LocalizedTexts.AbilityModifiers.GetName(modifier.Descriptor);
                        string text = UIUtility.AddSign(modifier.Value);
                        bricks.Add(new TooltipBrickValueStatFormula(text, directTextSymbol, glossaryEntryName, TooltipBrickElementType.Small));
                    });
                    if (rule.MythicMediumArmorEnduranceBonus > 0) {
                        string text = UIUtility.AddSign(rule.MythicMediumArmorEnduranceBonus);
                        bricks.Add(new TooltipBrickValueStatFormula(text, directTextSymbol, rule.MythicMediumArmorEndurance?.Blueprint?.Name, TooltipBrickElementType.Small));
                    }
                    if (rule.ArmoredMightBonus > 0) {
                        string text = UIUtility.AddSign(rule.ArmoredMightBonus);
                        bricks.Add(new TooltipBrickValueStatFormula(text, directTextSymbol, rule.ArmoredMight?.Blueprint?.Name, TooltipBrickElementType.Small));
                    }
                }
            }
        } 
    }
}
