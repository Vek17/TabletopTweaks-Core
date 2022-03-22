using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Localization;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UI.Common;
using Kingmaker.UI.MVVM._VM.ServiceWindows.Spellbook.Metamagic;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Text;
using TabletopTweaks.Core.ModLogic;
using TabletopTweaks.Core.NewUnitParts;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using static TabletopTweaks.Core.NewUnitParts.UnitPartCustomMechanicsFeatures;

// This work is largly based on work by https://github.com/Stari0n/MagicTime Copyright (c) 2021 Starion
namespace TabletopTweaks.Core.MechanicsChanges {
    public static class MetamagicExtention {

        [Flags]
        public enum CustomMetamagic {
            // Owlcat stops at 1 << 9
            Intensified = 1 << 12,
            Dazing = 1 << 13,
            //Unused Space
            Rime = 1 << 16,
            Burning = 1 << 17,
            Flaring = 1 << 18,
            Piercing = 1 << 19,
            SolidShadows = 1 << 20,
            Encouraging = 1 << 21,
        }

        public static void RegisterMetamagic(
            ModContextBase context,
            Metamagic metamagic,
            string name,
            Sprite icon,
            int defaultCost,
            CustomMechanicsFeature? favoriteMetamagic,
            ISubscriber metamagicMechanics = null) {
            var metamagicData = new CustomMetamagicData() {
                Name = name == null ? null : Helpers.CreateString(context, $"{name}SpellMetamagic", name),
                Icon = icon,
                DefaultCost = defaultCost,
                FavoriteMetamagic = favoriteMetamagic
            };
            RegisteredMetamagic.Add(metamagic, metamagicData);
            if (metamagicMechanics != null) {
                EventBus.Subscribe(metamagicMechanics);
            }
        }

        public static string GetMetamagicName(Metamagic metamagic) {
            CustomMetamagicData result;
            RegisteredMetamagic.TryGetValue(metamagic, out result);
            return result?.Name ?? string.Empty;
        }

        public static Sprite GetMetamagicIcon(Metamagic metamagic) {
            CustomMetamagicData result;
            RegisteredMetamagic.TryGetValue(metamagic, out result);
            return result?.Icon;
        }

        public static int GetMetamagicDefaultCost(Metamagic metamagic) {
            CustomMetamagicData result;
            RegisteredMetamagic.TryGetValue(metamagic, out result);
            return result?.DefaultCost ?? 0;
        }

        public static bool HasFavoriteMetamagic(UnitDescriptor unit, Metamagic metamagic) {
            CustomMetamagicData result;
            RegisteredMetamagic.TryGetValue(metamagic, out result);
            return result?.FavoriteMetamagic == null ? false : unit.CustomMechanicsFeature(result.FavoriteMetamagic.Value);
        }

        public static bool IsRegisistered(Metamagic metamagic) {
            return RegisteredMetamagic.ContainsKey(metamagic);
        }

        private static Dictionary<Metamagic, CustomMetamagicData> RegisteredMetamagic = new();

        private class CustomMetamagicData {
            public LocalizedString Name;
            public Sprite Icon;
            public int DefaultCost;
            public CustomMechanicsFeature? FavoriteMetamagic;
        }

        public static bool IsNewMetamagic(this Metamagic metamagic) {
            return (int)metamagic >= (int)CustomMetamagic.Intensified;
        }

        [HarmonyPatch(typeof(RuleApplyMetamagic), "OnTrigger")]
        static class RuleApplyMetamagic_OnTrigger_NewMetamagic_Patch {
            static void Postfix(RuleApplyMetamagic __instance) {
                var lv_adjustment = 0;
                foreach (var metamagic in __instance.AppliedMetamagics) {
                    if (MetamagicExtention.HasFavoriteMetamagic(__instance.Initiator, metamagic)) {
                        lv_adjustment++;
                    }
                }
                __instance.Result.SpellLevelCost -= lv_adjustment;
                var CompletlyNormalCorrection = 0;
                if (__instance.AppliedMetamagics.Contains(Metamagic.CompletelyNormal)) {
                    CompletlyNormalCorrection = 1;
                }
                if (__instance.BaseLevel + __instance.Result.SpellLevelCost + CompletlyNormalCorrection < 0) {
                    __instance.Result.SpellLevelCost = -__instance.BaseLevel;
                }
            }
        }

        [HarmonyPatch(typeof(RuleCollectMetamagic), "AddMetamagic")]
        static class RuleCollectMetamagic_AddMetamagic_NewMetamagic_Patch {
            static void Postfix(RuleCollectMetamagic __instance, Feature metamagicFeature) {
                if (!__instance.KnownMetamagics.Contains(metamagicFeature)) { return; }

                AddMetamagicFeat component = metamagicFeature.GetComponent<AddMetamagicFeat>();
                Metamagic metamagic = component.Metamagic;
                if (__instance.m_SpellLevel < 0) {
                    return;
                }
                if (__instance.m_SpellLevel >= 10) {
                    return;
                }
                if (__instance.m_SpellLevel + component.Metamagic.DefaultCost() > 10) {
                    return;
                }
                if (__instance.Spell != null
                    && !__instance.SpellMetamagics.Contains(metamagicFeature)
                    && (__instance.Spell.AvailableMetamagic & metamagic) == metamagic) {
                    __instance.SpellMetamagics.Add(metamagicFeature);
                }
            }
        }

        //This has to be patched after UIUtilityTexts loads normally or everything explodes
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class UIUtilityTexts_NewMetamagic_Patchs {
            private static bool patched = false;
            static void Postfix() {
                if (patched) { return; }
                var GetMetamagicList = AccessTools.Method(typeof(UIUtilityTexts), "GetMetamagicList");
                var GetMetamagicName = AccessTools.Method(typeof(UIUtilityTexts), "GetMetamagicName");
                var GetMetamagicListPostfix = AccessTools.Method(typeof(UIUtilityTexts_NewMetamagic_Patchs), "GetMetamagicList");
                var GetMetamagicNamePostfix = AccessTools.Method(typeof(UIUtilityTexts_NewMetamagic_Patchs), "GetMetamagicName");
                var harmony = new Harmony(Main.TTTContext.ModEntry.Info.Id);
                harmony.Patch(GetMetamagicList, postfix: new HarmonyMethod(GetMetamagicListPostfix));
                harmony.Patch(GetMetamagicName, postfix: new HarmonyMethod(GetMetamagicNamePostfix));
                patched = true;
            }
            static void GetMetamagicList(ref string __result, Metamagic mask) {
                StringBuilder stringBuilder = new StringBuilder(__result);
                var addComma = !string.IsNullOrEmpty(__result);
                foreach (object obj in Enum.GetValues(typeof(CustomMetamagic))) {
                    Metamagic metamagic = (Metamagic)obj;
                    if (mask.HasMetamagic(metamagic)) {
                        if (!MetamagicExtention.IsRegisistered(metamagic)) { continue; }
                        if (addComma) {
                            stringBuilder.Append(", ");
                        }
                        stringBuilder.Append(MetamagicExtention.GetMetamagicName(metamagic));
                        addComma = true;
                    }
                }
                __result = stringBuilder.ToString();
            }
            static void GetMetamagicName(ref string __result, Metamagic metamagic) {
                if (!string.IsNullOrEmpty(__result)) { return; }
                if (!MetamagicExtention.IsRegisistered(metamagic)) { return; }
                __result = MetamagicExtention.GetMetamagicName(metamagic);
            }
        }

        [HarmonyPatch(typeof(MetamagicHelper), "DefaultCost")]
        static class MetamagicHelper_DefaultCost_NewMetamagic_Patch {
            //Prefixed to prevent excessive exceptions from being thrown by vanilla logger
            static bool Prefix(ref int __result, Metamagic metamagic) {
                if (MetamagicExtention.IsRegisistered(metamagic)) {
                    __result = MetamagicExtention.GetMetamagicDefaultCost(metamagic);
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(MetamagicHelper), "SpellIcon")]
        static class MetamagicHelper_SpellIcon_NewMetamagic_Patch {
            private static bool Prefix(ref Sprite __result, Metamagic metamagic) {
                //Prefixed to prevent excessive exceptions from being thrown by vanilla logger
                if (MetamagicExtention.GetMetamagicIcon(metamagic) != null) {
                    __result = MetamagicExtention.GetMetamagicIcon(metamagic);
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(SpellbookMetamagicSelectorVM), "GetCost")]
        static class SpellbookMetamagicSelectorVM_GetCost_NewMetamagic_Patch {
            private static void Postfix(SpellbookMetamagicSelectorVM __instance, ref int __result, Metamagic metamagic) {
                if (MetamagicExtention.HasFavoriteMetamagic(__instance.m_Unit.Value, metamagic)) {
                    __result--;
                }
            }
        }
        // I think these are general patches for base game bugs... Not 100% sure but I am scared to touch things
        [HarmonyPatch(typeof(SpellbookMetamagicSelectorVM), "AddMetamagic")]
        static class SpellbookMetamagicSelectorVM_GetCost_AddMetamagic_Patch {
            private static void Postfix(SpellbookMetamagicSelectorVM __instance) {
                var CompletlyNormalCorrection = 0;
                if (__instance.m_MetamagicBuilder.Value.AppliedMetamagics.Contains(Metamagic.CompletelyNormal)) {
                    CompletlyNormalCorrection = 1;
                }
                if (__instance.CurrentTemporarySpell.Value.SpellLevel < __instance.m_MetamagicBuilder.Value.BaseSpellLevel - CompletlyNormalCorrection) {
                    __instance.CurrentTemporarySpell.Value.SpellLevel = __instance.m_MetamagicBuilder.Value.BaseSpellLevel - CompletlyNormalCorrection;
                    __instance.m_MetamagicBuilder.Value.ResultSpellLevel = __instance.m_MetamagicBuilder.Value.BaseSpellLevel - CompletlyNormalCorrection;
                }
            }
        }
        // I think these are general patches for base game bugs... Not 100% sure but I am scared to touch things
        [HarmonyPatch(typeof(SpellbookMetamagicSelectorVM), "RemoveMetamagic")]
        static class SpellbookMetamagicSelectorVM_GetCost_RemoveMetamagic_Patch {
            private static void Postfix(SpellbookMetamagicSelectorVM __instance) {
                var CompletlyNormalCorrection = 0;
                if (__instance.m_MetamagicBuilder.Value.AppliedMetamagics.Contains(Metamagic.CompletelyNormal)) {
                    CompletlyNormalCorrection = 1;
                }
                if (__instance.CurrentTemporarySpell.Value.SpellLevel < __instance.m_MetamagicBuilder.Value.BaseSpellLevel - CompletlyNormalCorrection) {
                    __instance.CurrentTemporarySpell.Value.SpellLevel = __instance.m_MetamagicBuilder.Value.BaseSpellLevel - CompletlyNormalCorrection;
                    __instance.m_MetamagicBuilder.Value.ResultSpellLevel = __instance.m_MetamagicBuilder.Value.BaseSpellLevel - CompletlyNormalCorrection;
                }
            }
        }
    }
}
