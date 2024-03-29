﻿using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using TabletopTweaks.Core.MechanicsChanges;
using TabletopTweaks.Core.ModLogic;
using UnityEngine;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.Utilities {
    public static class ItemTools {

        public enum MetamagicRodType : int {
            Lesser = 3,
            Normal = 6,
            Greater = 9
        }
        public enum PotionColor : int {
            Blue,
            Cyan,
            Green,
            Red,
            Yellow,
        }

        private static readonly string LesserMetamagicRodString = "Lesser rods can be used with spells of 3rd level or lower.";
        private static readonly string NormalMetamagicRodString = "Regular rods can be used with spells of 6th level or lower.";
        private static readonly string GreaterMetamagicRodString = "Greater rods can be used with spells of 9th level or lower.";

        private static Sprite Form01_Blue_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form01_Blue_Simple.png");
        private static Sprite Form03_Blue_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form03_Blue_Simple.png");
        private static Sprite Form04_Blue_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form04_Blue_Simple.png");
        private static Sprite Form05_Blue_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form05_Blue_Simple.png");
        private static Sprite Form06_Blue_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form06_Blue_Simple.png");

        private static Sprite Form01_Cyan_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form01_Cyan_Simple.png");
        private static Sprite Form03_Cyan_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form03_Cyan_Simple.png");
        private static Sprite Form04_Cyan_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form04_Cyan_Simple.png");
        private static Sprite Form05_Cyan_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form05_Cyan_Simple.png");
        private static Sprite Form06_Cyan_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form06_Cyan_Simple.png");

        private static Sprite Form01_Green_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form01_Green_Simple.png");
        private static Sprite Form03_Green_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form03_Green_Simple.png");
        private static Sprite Form04_Green_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form04_Green_Simple.png");
        private static Sprite Form05_Green_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form05_Green_Simple.png");
        private static Sprite Form06_Green_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form06_Green_Simple.png");

        private static Sprite Form01_Red_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form01_Red_Simple.png");
        private static Sprite Form03_Red_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form03_Red_Simple.png");
        private static Sprite Form04_Red_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form04_Red_Simple.png");
        private static Sprite Form05_Red_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form05_Red_Simple.png");
        private static Sprite Form06_Red_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form06_Red_Simple.png");

        private static Sprite Form01_Yellow_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form01_Yellow_Simple.png");
        private static Sprite Form03_Yellow_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form03_Yellow_Simple.png");
        private static Sprite Form04_Yellow_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form04_Yellow_Simple.png");
        private static Sprite Form05_Yellow_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form05_Yellow_Simple.png");
        private static Sprite Form06_Yellow_Simple = AssetLoader.LoadInternal(TTTContext, folder: "Potions", file: "Form06_Yellow_Simple.png");


        private static BlueprintItemEquipmentUsable CreateMetamagicRod(
            ModContextBase modContext,
            string rodName,
            Sprite icon,
            Metamagic metamagic,
            string metamagicName,
            MetamagicRodType type,
            string rodDescriptionStart,
            string metamagicDescription, Action<BlueprintItemEquipmentUsable> init = null
        ) {
            var description = $"{rodDescriptionStart}\n{GetRodString(type)}\n{metamagicDescription}";

            var Buff = Helpers.CreateBlueprint<BlueprintBuff>(modContext, $"MetamagicRod{type}{metamagicName}Buff", bp => {
                bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
                bp.ResourceAssetIds = new string[0];
                bp.SetName(modContext, rodName);
                bp.SetDescription(modContext, description);
                bp.m_DescriptionShort = Helpers.CreateString(modContext, $"{bp.name}.Description_Short", "");
                bp.m_Icon = icon;
            });
            var ActivatableAbility = Helpers.CreateBlueprint<BlueprintActivatableAbility>(modContext, $"MetamagicRod{type}{metamagicName}ToggleAbility", bp => {
                bp.m_Buff = Buff.ToReference<BlueprintBuffReference>();
                bp.m_SelectTargetAbility = new BlueprintAbilityReference();
                bp.Group = ActivatableAbilityGroup.MetamagicRod;
                bp.WeightInGroup = 1;
                bp.DeactivateImmediately = true;
                bp.ResourceAssetIds = new string[0];
                bp.SetName(modContext, rodName);
                bp.SetDescription(modContext, description);
                bp.m_DescriptionShort = Helpers.CreateString(modContext, $"{bp.name}.Description_Short", "");
                bp.m_Icon = icon;
                bp.AddComponent<ActivatableAbilityResourceLogic>(c => {
                    c.m_RequiredResource = new BlueprintAbilityResourceReference();
                    c.m_FreeBlueprint = new BlueprintUnitFactReference();
                    c.Categories = new Kingmaker.Enums.WeaponCategory[0];
                });
            });
            Buff.AddComponent<MetamagicRodMechanics>(c => {
                c.m_RodAbility = ActivatableAbility.ToReference<BlueprintActivatableAbilityReference>();
                c.m_AbilitiesWhiteList = new BlueprintAbilityReference[0];
                c.Metamagic = metamagic;
                c.MaxSpellLevel = (int)type;
            });
            var MetamagicRod = Helpers.CreateBlueprint<BlueprintItemEquipmentUsable>(modContext, $"MetamagicRod{type}{metamagicName}", bp => {
                bp.m_InventoryEquipSound = "WandPut";
                bp.m_BeltItemPrefab = new Kingmaker.ResourceLinks.PrefabLink();
                bp.m_Enchantments = new BlueprintEquipmentEnchantmentReference[0];
                bp.m_Ability = new BlueprintAbilityReference();
                bp.m_ActivatableAbility = ActivatableAbility.ToReference<BlueprintActivatableAbilityReference>();
                bp.m_EquipmentEntity = new KingmakerEquipmentEntityReference();
                bp.m_EquipmentEntityAlternatives = new KingmakerEquipmentEntityReference[0];
                bp.SpendCharges = true;
                bp.Charges = 3;
                bp.RestoreChargesOnRest = true;
                bp.m_DisplayNameText = Helpers.CreateString(modContext, $"{bp.name}.Name", rodName);
                bp.m_DescriptionText = Helpers.CreateString(modContext, $"{bp.name}.Description", description, shouldProcess: true);
                bp.m_FlavorText = Helpers.CreateString(modContext, $"{bp.name}.Flavor", "");
                bp.m_NonIdentifiedNameText = Helpers.CreateString(modContext, $"{bp.name}.Unidentified_Name", "Rod");
                bp.m_NonIdentifiedDescriptionText = Helpers.CreateString(modContext, $"{bp.name}.Unidentified_Description", "");
                bp.m_Icon = icon;
                bp.m_Cost = GetRodCost(metamagic, type);
                bp.CR = GetRodCR(metamagic, type);
                bp.m_Weight = 1;
                bp.m_Destructible = true;
                bp.m_ShardItem = BlueprintTools.GetBlueprintReference<BlueprintItemReference>("e6820e62423d4c81a2ba20d236251b67"); //MetalShardItem
                bp.m_InventoryPutSound = "WandPut";
                bp.m_InventoryTakeSound = "WandTake";
                bp.TrashLootTypes = new Kingmaker.Enums.TrashLootType[0];
                bp.m_Overrides = new List<string>();
            });
            return MetamagicRod;

            string GetRodString(MetamagicRodType type) {
                switch (type) {
                    case MetamagicRodType.Lesser:
                        return LesserMetamagicRodString;
                    case MetamagicRodType.Normal:
                        return NormalMetamagicRodString;
                    case MetamagicRodType.Greater:
                        return GreaterMetamagicRodString;
                    default:
                        return string.Empty;
                }
            }

            int GetRodCost(Metamagic metamagic, MetamagicRodType type) {
                switch (type) {
                    case MetamagicRodType.Lesser:
                        switch (metamagic.DefaultCost()) {
                            case 0:
                                return 3000;
                            case 1:
                                return 3000;
                            case 2:
                                return 9000;
                            case 3:
                                return 14000;
                            case 4:
                                return 35000;
                            default:
                                return 70000;
                        }
                    case MetamagicRodType.Normal:
                        switch (metamagic.DefaultCost()) {
                            case 0:
                                return 11000;
                            case 1:
                                return 11000;
                            case 2:
                                return 32500;
                            case 3:
                                return 54000;
                            case 4:
                                return 75500;
                            default:
                                return 15000;
                        }
                    case MetamagicRodType.Greater:
                        switch (metamagic.DefaultCost()) {
                            case 0:
                                return 24500;
                            case 1:
                                return 24500;
                            case 2:
                                return 73000;
                            case 3:
                                return 121500;
                            case 4:
                                return 170000;
                            default:
                                return 300000;
                        }
                    default:
                        return 0;
                }
            }
            int GetRodCR(Metamagic metamagic, MetamagicRodType type) {
                switch (type) {
                    case MetamagicRodType.Lesser:
                        switch (metamagic.DefaultCost()) {
                            case 0:
                                return 2;
                            case 1:
                                return 5;
                            case 2:
                                return 7;
                            case 3:
                                return 10;
                            case 4:
                                return 11;
                            default:
                                return 12;
                        }
                    case MetamagicRodType.Normal:
                        switch (metamagic.DefaultCost()) {
                            case 0:
                                return 12;
                            case 1:
                                return 13;
                            case 2:
                                return 14;
                            case 3:
                                return 15;
                            case 4:
                                return 16;
                            default:
                                return 17;
                        }
                    case MetamagicRodType.Greater:
                        switch (metamagic.DefaultCost()) {
                            case 0:
                                return 17;
                            case 1:
                                return 18;
                            case 2:
                                return 20;
                            case 3:
                                return 22;
                            case 4:
                                return 24;
                            default:
                                return 25;
                        }
                    default:
                        return 0;
                }
            }
        }
        public static BlueprintItemEquipmentUsable CreateMetamagicRod(
            ModContextBase modContext,
            string rodName,
            Sprite icon,
            Metamagic metamagic,
            MetamagicRodType type,
            string rodDescriptionStart,
            string metamagicDescription, Action<BlueprintItemEquipmentUsable> init = null
        ) {
            if (metamagic.IsNewMetamagic()) {
                return CreateMetamagicRod(
                    modContext, rodName,
                    icon,
                    metamagic,
                    ((MetamagicExtention.CustomMetamagic)metamagic).ToString(),
                    type,
                    rodDescriptionStart,
                    metamagicDescription
                );
            }
            return CreateMetamagicRod(
                modContext, rodName,
                icon,
                metamagic,
                metamagic.ToString(),
                type,
                rodDescriptionStart,
                metamagicDescription
            );
        }
        public static BlueprintItemEquipmentUsable[] CreateAllMetamagicRods(
            ModContextBase modContext,
            string rodName,
            Sprite lesserIcon,
            Sprite normalIcon,
            Sprite greaterIcon,
            Metamagic metamagic,
            string rodDescriptionStart, string metamagicDescription) {

            return new BlueprintItemEquipmentUsable[] {
                CreateMetamagicRod(
                    modContext, $"Lesser {rodName}",
                    lesserIcon,
                    metamagic,
                    type: MetamagicRodType.Lesser,
                    rodDescriptionStart: rodDescriptionStart,
                    metamagicDescription: metamagicDescription
                ),
                CreateMetamagicRod(
                    modContext, rodName,
                    normalIcon,
                    metamagic,
                    type: MetamagicRodType.Normal,
                    rodDescriptionStart: rodDescriptionStart,
                    metamagicDescription: metamagicDescription
                ),
                CreateMetamagicRod(
                    modContext, $"Greater {rodName}",
                    greaterIcon,
                    metamagic,
                    type: MetamagicRodType.Greater,
                    rodDescriptionStart: rodDescriptionStart,
                    metamagicDescription: metamagicDescription
                )
            };
        }
        public static BlueprintItemEquipmentUsable CreateScroll(ModContextBase modContext, string name, Sprite icon, BlueprintAbility spell, int spellLevel, int casterLevel) {
            var scrollItemPrefab = new PrefabLink() {
                AssetId = "d711efe72d029364a9ad378d5f0955c0"
            };
            var Scroll = Helpers.CreateBlueprint<BlueprintItemEquipmentUsable>(modContext, name, bp => {
                bp.m_InventoryEquipSound = "ScrollPut";
                bp.m_BeltItemPrefab = scrollItemPrefab;
                bp.m_Enchantments = new BlueprintEquipmentEnchantmentReference[0];
                bp.Type = UsableItemType.Scroll;
                bp.m_Ability = spell.ToReference<BlueprintAbilityReference>();
                bp.m_ActivatableAbility = new BlueprintActivatableAbilityReference();
                bp.m_EquipmentEntity = new KingmakerEquipmentEntityReference();
                bp.m_EquipmentEntityAlternatives = new KingmakerEquipmentEntityReference[0];
                bp.SpendCharges = true;
                bp.Charges = 1;
                bp.CasterLevel = casterLevel;
                bp.SpellLevel = spellLevel;
                bp.DC = 10 + spellLevel + (spellLevel / 2);
                bp.m_DisplayNameText = Helpers.CreateString(modContext, $"{bp.name}.Name", "");
                bp.m_DescriptionText = Helpers.CreateString(modContext, $"{bp.name}.Description", "");
                bp.m_FlavorText = Helpers.CreateString(modContext, $"{bp.name}.Flavor", "");
                bp.m_NonIdentifiedNameText = Helpers.CreateString(modContext, $"{bp.name}.Unidentified_Name", "");
                bp.m_NonIdentifiedDescriptionText = Helpers.CreateString(modContext, $"{bp.name}.Unidentified_Description", "");
                bp.m_Icon = icon;
                bp.m_Cost = GetScrollCost(spellLevel);
                bp.m_Weight = 0.2f;
                bp.m_Destructible = true;
                bp.m_ShardItem = BlueprintTools.GetBlueprintReference<BlueprintItemReference>("08133117418642fb9d1d2adba9785f43"); //PaperShardItem
                bp.m_InventoryPutSound = "ScrollPut";
                bp.m_InventoryTakeSound = "ScrollTake";
                bp.TrashLootTypes = new TrashLootType[] { TrashLootType.Scrolls_RE };
                bp.m_Overrides = new List<string>();
                bp.AddComponent<CopyScroll>(c => {
                    c.m_CustomSpell = spell.ToReference<BlueprintAbilityReference>();
                });
            });
            AddScrollToCraftRoot(Scroll);
            return Scroll;
        }
        public static BlueprintItemEquipmentUsable CreatePotion(ModContextBase modContext, string name, PotionColor color, BlueprintAbility spell, int spellLevel, int casterLevel) {
            var Potion = Helpers.CreateBlueprint<BlueprintItemEquipmentUsable>(modContext, name, bp => {
                bp.m_InventoryEquipSound = "ScrollPut";
                bp.m_BeltItemPrefab = GetPotionPrefab(color);
                bp.m_Enchantments = new BlueprintEquipmentEnchantmentReference[0];
                bp.Type = UsableItemType.Potion;
                bp.m_Ability = spell.ToReference<BlueprintAbilityReference>();
                bp.m_ActivatableAbility = new BlueprintActivatableAbilityReference();
                bp.m_EquipmentEntity = new KingmakerEquipmentEntityReference();
                bp.m_EquipmentEntityAlternatives = new KingmakerEquipmentEntityReference[0];
                bp.SpendCharges = true;
                bp.Charges = 1;
                bp.CasterLevel = casterLevel;
                bp.SpellLevel = spellLevel;
                bp.DC = 10 + spellLevel + (spellLevel / 2);
                bp.m_DisplayNameText = Helpers.CreateString(modContext, $"{bp.name}.Name", "");
                bp.m_DescriptionText = Helpers.CreateString(modContext, $"{bp.name}.Description", "");
                bp.m_FlavorText = Helpers.CreateString(modContext, $"{bp.name}.Flavor", "");
                bp.m_NonIdentifiedNameText = Helpers.CreateString(modContext, $"{bp.name}.Unidentified_Name", "");
                bp.m_NonIdentifiedDescriptionText = Helpers.CreateString(modContext, $"{bp.name}.Unidentified_Description", "");
                bp.m_Icon = GetPotionIcon(color, spellLevel);
                bp.m_Cost = GetPotionCost(spellLevel);
                bp.m_Weight = 0.5f;
                bp.m_Destructible = true;
                bp.m_ShardItem = BlueprintTools.GetBlueprintReference<BlueprintItemReference>("2b2107f98002425bb1309d31ff531f37"); //GlassShardItem
                bp.m_InventoryPutSound = "BottlePut";
                bp.m_InventoryTakeSound = "BottleTake";
                bp.TrashLootTypes = new TrashLootType[] { TrashLootType.Potions };
                bp.m_Overrides = new List<string>();
            });
            AddPotionToCraftRoot(Potion);
            return Potion;
        }
        private static int GetScrollCost(int spellLevel) {
            return spellLevel switch {
                0 => 13,
                1 => 25,
                2 => 150,
                3 => 375,
                4 => 700,
                5 => 1125,
                6 => 1650,
                7 => 2275,
                8 => 3000,
                9 => 3825,
                10 => 5000,
                _ => 0
            };
        }
        private static void AddScrollToCraftRoot(BlueprintItemEquipmentUsable scroll) {
            if (scroll.Type != UsableItemType.Scroll) { return; }

            Game.Instance.BlueprintRoot.CraftRoot.m_ScrollsItems.Add(scroll.ToReference<BlueprintItemEquipmentUsableReference>());
        }
        private static PrefabLink GetPotionPrefab(PotionColor color) {
            var AssetID = color switch {
                PotionColor.Blue => "7b2a2ed1f3284224c804038a713c391f",
                PotionColor.Cyan => "e805c0e867b583b4f8c24b2b045b5be3",
                PotionColor.Green => "51097fd1d322c0d41b33dac27da51bf4",
                PotionColor.Red => "8de60d0edae1a1a47ba9fee1e1d97e32",
                PotionColor.Yellow => "9b57d6e56c83fc14d9580c6f766fbe20",
                _ => "9b57d6e56c83fc14d9580c6f766fbe20"
            };
            return new PrefabLink() {
                AssetId = AssetID
            };
        }
        private static Sprite GetPotionIcon(PotionColor color, int spellLevel) {
            return color switch {
                PotionColor.Blue => spellLevel switch {
                    1 => Form01_Blue_Simple,
                    2 => Form03_Blue_Simple,
                    3 => Form04_Blue_Simple,
                    4 => Form05_Blue_Simple,
                    5 => Form06_Blue_Simple,
                    _ => Form06_Blue_Simple
                },
                PotionColor.Cyan => spellLevel switch {
                    1 => Form01_Cyan_Simple,
                    2 => Form03_Cyan_Simple,
                    3 => Form04_Cyan_Simple,
                    4 => Form05_Cyan_Simple,
                    5 => Form06_Cyan_Simple,
                    _ => Form06_Cyan_Simple
                },
                PotionColor.Green => spellLevel switch {
                    1 => Form01_Green_Simple,
                    2 => Form03_Green_Simple,
                    3 => Form04_Green_Simple,
                    4 => Form05_Green_Simple,
                    5 => Form06_Green_Simple,
                    _ => Form06_Green_Simple
                },
                PotionColor.Red => spellLevel switch {
                    1 => Form01_Red_Simple,
                    2 => Form03_Red_Simple,
                    3 => Form04_Red_Simple,
                    4 => Form05_Red_Simple,
                    5 => Form06_Red_Simple,
                    _ => Form06_Red_Simple
                },
                PotionColor.Yellow => spellLevel switch {
                    1 => Form01_Yellow_Simple,
                    2 => Form03_Yellow_Simple,
                    3 => Form04_Yellow_Simple,
                    4 => Form05_Yellow_Simple,
                    5 => Form06_Yellow_Simple,
                    _ => Form06_Yellow_Simple
                },
                _ => Form01_Blue_Simple
            };
        }
        private static int GetPotionCost(int spellLevel) {
            return spellLevel switch {
                0 => 25,
                1 => 50,
                2 => 300,
                3 => 750,
                4 => 1400,
                5 => 2250,
                6 => 3300,
                _ => 25
            };
        }
        private static void AddPotionToCraftRoot(BlueprintItemEquipmentUsable potion) {
            if (potion.Type != UsableItemType.Potion) { return; }

            Game.Instance.BlueprintRoot.CraftRoot.m_PotionsItems.Add(potion.ToReference<BlueprintItemEquipmentUsableReference>());
        }
    }
}
