using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.ActionBar;
using Kingmaker.UI.MVVM._VM.ActionBar;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.NewUnitParts;

namespace TabletopTweaks.Core.NewUI {
    class ActionBarPatches {

        [HarmonyPatch(typeof(ActionBarSpontaneousConvertedSlot), "Set", new Type[] { typeof(UnitEntityData), typeof(AbilityData) })]
        static class ActionBarSpontaneousConvertedSlot_Set_Patch {
            static bool Prefix(ActionBarSpontaneousConvertedSlot __instance, UnitEntityData selected, AbilityData spell) {
                var pseudoActivatableComponent = spell.Blueprint.GetComponent<PseudoActivatable>();
                if (pseudoActivatableComponent != null) {
                    __instance.Selected = selected;
                    if (selected == null) {
                        return true;
                    }
                    __instance.MechanicSlot = new MechanicActionBarSlotPseudoActivatableAbilityVariant {
                        Spell = spell,
                        Unit = selected,
                        BuffToWatch = pseudoActivatableComponent.Buff
                    };
                    __instance.MechanicSlot.SetSlot(__instance);
                    selected.Ensure<UnitPartPseudoActivatableAbilities>().RegisterPseudoActivatableAbilitySlot(__instance.MechanicSlot);
                    return false;
                } else if (spell is MetaRageComponent.MetaRageAbilityData) {
                    __instance.Selected = selected;
                    if (selected == null) {
                        return true;
                    }
                    __instance.MechanicSlot = new MechanicActionBarSlotMetaRage {
                        Spell = spell,
                        Unit = selected
                    };
                    __instance.MechanicSlot.SetSlot(__instance);
                } else if (spell.Blueprint.GetComponent<QuickStudyComponent>()) {
                    __instance.Selected = selected;
                    if (selected == null) {
                        return true;
                    }
                    __instance.MechanicSlot = new MechanicActionBarSlotQuickStudy {
                        Spell = spell,
                        Unit = selected
                    };
                    __instance.MechanicSlot.SetSlot(__instance);
                    return false;
                } else if (spell is UnitPartSpellKenning.SpellKenningAbilityData) {
                    __instance.Selected = selected;
                    if (selected == null) {
                        return true;
                    }
                    __instance.MechanicSlot = new MechanicActionBarSlotSpellKenning {
                        Spell = spell,
                        Unit = selected
                    };
                    __instance.MechanicSlot.SetSlot(__instance);
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(SlotConversion), "GetMechanicSlots")]
        static class SlotConversion_GetMechanicSlots_Patch {
            static void Postfix(SlotConversion __instance, UnitEntityData unit, ref IEnumerable<MechanicActionBarSlot> __result) {
                var newResult = __result.Select(slot => {
                    switch (slot) {
                        case MechanicActionBarSlotSpontaneusConvertedSpell spontaneusConvertedSpell: {
                                var abilityData = spontaneusConvertedSpell.Spell;
                                var pseudoActivatable = abilityData.Blueprint.GetComponent<PseudoActivatable>();
                                if (pseudoActivatable != null) {
                                    var pseudoActivatableSlot = new MechanicActionBarSlotPseudoActivatableAbilityVariant {
                                        Spell = abilityData,
                                        Unit = unit,
                                        BuffToWatch = pseudoActivatable.Buff
                                    };
                                    unit.Ensure<UnitPartPseudoActivatableAbilities>().RegisterPseudoActivatableAbilitySlot(slot);
                                    return pseudoActivatableSlot;
                                } else if (abilityData is MetaRageComponent.MetaRageAbilityData) {
                                    return new MechanicActionBarSlotMetaRage {
                                        Spell = abilityData,
                                        Unit = unit
                                    };
                                } else if (abilityData.Blueprint.GetComponent<QuickStudyComponent>()) {
                                    return new MechanicActionBarSlotQuickStudy {
                                        Spell = abilityData,
                                        Unit = unit
                                    };
                                } else if (abilityData is UnitPartSpellKenning.SpellKenningAbilityData) {
                                    return new MechanicActionBarSlotSpellKenning {
                                        Spell = abilityData,
                                        Unit = unit
                                    };
                                }
                                return spontaneusConvertedSpell;
                            };
                        case MechanicActionBarSlotActivableAbility activatableSlot: return activatableSlot;
                        default: return slot;
                    }
                }).ToList<MechanicActionBarSlot>();
                __result = newResult;
            }
        }

        [HarmonyPatch(typeof(ActionBarSlotVM), nameof(ActionBarSlotVM.UpdateResource))]
        static class ActionBarSlotVM_SetResource_Patch {
            static void Postfix(ActionBarSlotVM __instance) {
                if (!(__instance.MechanicActionBarSlot == null)
                    && !(__instance.MechanicActionBarSlot.IsBad())
                    && __instance.MechanicActionBarSlot is MechanicActionBarSlotPseudoActivatableAbility pseudoActivatable
                    && pseudoActivatable.ShouldUpdateForeIcon) {
                    __instance.ForeIcon.Value = pseudoActivatable.GetForeIcon();
                    pseudoActivatable.ShouldUpdateForeIcon = false;
                }
            }
        }

        [HarmonyPatch(typeof(ActionBarVM), nameof(ActionBarVM.CollectAbilities))]
        static class ActionBarVM_CollectAbilities_Patch {
            static bool Prefix(ActionBarVM __instance, UnitEntityData unit) {
                foreach (Ability ability in unit.Abilities) {
                    if (!ability.Hidden && !ability.Blueprint.IsCantrip) {
                        List<ActionBarSlotVM> groupAbilities = __instance.GroupAbilities;
                        if (ability.GetComponent<PseudoActivatable>() != null) {
                            MechanicActionBarSlotPseudoActivatableAbility actionBarSlotPseudoActivatableAbility = new MechanicActionBarSlotPseudoActivatableAbility {
                                Ability = ability.Data,
                                Unit = unit,
                                BuffToWatch = ability.GetComponent<PseudoActivatable>().Buff
                            };
                            unit.Ensure<UnitPartPseudoActivatableAbilities>().RegisterPseudoActivatableAbilitySlot(actionBarSlotPseudoActivatableAbility);
                            ActionBarSlotVM actionBarSlotVm = new ActionBarSlotVM(actionBarSlotPseudoActivatableAbility);
                            groupAbilities.Add(actionBarSlotVm);
                        } else {
                            MechanicActionBarSlotAbility actionBarSlotAbility = new MechanicActionBarSlotAbility();
                            actionBarSlotAbility.Ability = ability.Data;
                            actionBarSlotAbility.Unit = unit;
                            ActionBarSlotVM actionBarSlotVm = new ActionBarSlotVM(actionBarSlotAbility);
                            groupAbilities.Add(actionBarSlotVm);
                        }
                    }
                }
                foreach (ActivatableAbility activatableAbility in unit.ActivatableAbilities) {
                    List<ActionBarSlotVM> groupAbilities = __instance.GroupAbilities;
                    MechanicActionBarSlotActivableAbility activableAbility = new MechanicActionBarSlotActivableAbility();
                    activableAbility.ActivatableAbility = activatableAbility;
                    activableAbility.Unit = unit;
                    ActionBarSlotVM actionBarSlotVm = new ActionBarSlotVM(activableAbility);
                    groupAbilities.Add(actionBarSlotVm);
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(UnitUISettings), nameof(UnitUISettings.PostLoad))]
        static class UnitUISettings_PostLoad_Patch {
            static void Postfix(UnitUISettings __instance) {
                if (__instance.Owner?.Unit == null || __instance.m_Slots == null)
                    return;

                for (int i = 0; i < __instance.m_Slots.Length; i++) {
                    if (__instance.m_Slots[i] is MechanicActionBarSlotPseudoActivatableAbility pseudoActivatable) {
                        __instance.Owner.Unit.Ensure<UnitPartPseudoActivatableAbilities>().RegisterPseudoActivatableAbilitySlot(pseudoActivatable);
                    }
                    // This probably never happens, but just in case...
                    else if (__instance.m_Slots[i] is MechanicActionBarSlotPseudoActivatableAbilityVariant pseudoActivatableAbilityVariant) {
                        __instance.Owner.Unit.Ensure<UnitPartPseudoActivatableAbilities>().RegisterPseudoActivatableAbilitySlot(pseudoActivatableAbilityVariant);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(UnitUISettings.AbilityWrapper), nameof(UnitUISettings.AbilityWrapper.CreateSlot))]
        static class UnitUISettingsAbilityWrapper_CreateSlot_Patch {
            static bool Prefix(UnitUISettings.AbilityWrapper __instance, UnitEntityData unit, ref MechanicActionBarSlot __result) {
                if (__instance.SpellSlot != null || __instance.SpontaneousSpell != null || __instance.Ability == null)
                    return true;

                var pseudoActivatableComponent = __instance.Ability.GetComponent<PseudoActivatable>();
                if (pseudoActivatableComponent == null)
                    return true;

                var slot = new MechanicActionBarSlotPseudoActivatableAbility {
                    Ability = __instance.Ability.Data,
                    Unit = unit,
                    BuffToWatch = pseudoActivatableComponent.Buff
                };
                unit.Ensure<UnitPartPseudoActivatableAbilities>().RegisterPseudoActivatableAbilitySlot(slot);
                __result = slot;
                return false;
            }
        }
    }
}
