using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.Loot;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks.Core.Utilities {
    public static class VenderTools {

        public static void AddScrollToLeveledVenders(BlueprintItemEquipmentUsable scroll, int count) {
            if (scroll == null) { return; }
            if (scroll.Type != UsableItemType.Scroll) { return; }

            SharedVenderLists.ScrollVenderTables
                .Where(bp => bp.GetComponents<LootItemsPackFixed>().Any(c => {
                    var maybeScroll = c.Item.Item as BlueprintItemEquipmentUsable;
                    if (maybeScroll == null || maybeScroll.Type != UsableItemType.Scroll) { return false; }
                    return maybeScroll.SpellLevel == scroll.SpellLevel;
                }))
                .ForEach(table => {
                    table.AddComponent<LootItemsPackFixed>(c => {
                        c.m_Item = CreateLootItem(scroll);
                        c.m_Count = count;
                    });
                    Main.TTTContext.Logger.Log($"Added {count} {scroll.name} to Vender {table.name}");
                });
        }

        public static void AddScrollToLeveledVenders(BlueprintItemEquipmentUsable scroll) {
            if (scroll == null) { return; }
            if (scroll.Type != UsableItemType.Scroll) { return; }

            var filteredList = SharedVenderLists.ScrollVenderTables
                .Where(bp => bp.GetComponents<LootItemsPackFixed>().Any(c => {
                    var maybeScroll = c.Item.Item as BlueprintItemEquipmentUsable;
                    if (maybeScroll == null || maybeScroll.Type != UsableItemType.Scroll) { return false; }
                    return maybeScroll.SpellLevel == scroll.SpellLevel;
                })).ToArray();
            filteredList.ForEach(table => {
                var scrolls = 0;
                var count = table.GetComponents<LootItemsPackFixed>().Count(c => {
                    var maybeScroll = c.Item.Item as BlueprintItemEquipmentUsable;
                    if (maybeScroll == null || maybeScroll.Type != UsableItemType.Scroll || maybeScroll.SpellLevel != scroll.SpellLevel) { return false; }
                    scrolls += c.Count;
                    return true;
                });
                var quantity = scrolls / count;
                table.AddComponent<LootItemsPackFixed>(c => {
                    c.m_Item = CreateLootItem(scroll);
                    c.m_Count = quantity;
                });
                Main.TTTContext.Logger.Log($"Added {quantity} {scroll.name} to Vender {table.name}");
            });
        }

        public static void AddPotionToLeveledVenders(BlueprintItemEquipmentUsable potion, int count) {
            if (potion == null) { return; }
            if (potion.Type != UsableItemType.Potion) { return; }

            SharedVenderLists.PotionVenderTables
                .Where(bp => bp.GetComponents<LootItemsPackFixed>().Any(c => {
                    var maybePotion = c.Item.Item as BlueprintItemEquipmentUsable;
                    if (maybePotion == null || maybePotion.Type != UsableItemType.Potion) { return false; }
                    return maybePotion.SpellLevel == potion.SpellLevel;
                }))
                .ForEach(table => {
                    table.AddComponent<LootItemsPackFixed>(c => {
                        c.m_Item = CreateLootItem(potion);
                        c.m_Count = count;
                    });
                    Main.TTTContext.Logger.Log($"Added {count} {potion.name} to Vender {table.name}");
                });
        }

        public static void AddPotionToLeveledVenders(BlueprintItemEquipmentUsable potion) {
            if (potion == null) { return; }
            if (potion.Type != UsableItemType.Potion) { return; }

            var filteredList = SharedVenderLists.PotionVenderTables
                .Where(bp => bp.GetComponents<LootItemsPackFixed>().Any(c => {
                    var maybePotion = c.Item.Item as BlueprintItemEquipmentUsable;
                    if (maybePotion == null || maybePotion.Type != UsableItemType.Potion) { return false; }
                    return maybePotion.SpellLevel == potion.SpellLevel;
                })).ToArray();
            filteredList.ForEach(table => {
                var scrolls = 0;
                var count = table.GetComponents<LootItemsPackFixed>().Count(c => {
                    var maybeScroll = c.Item.Item as BlueprintItemEquipmentUsable;
                    if (maybeScroll == null || maybeScroll.Type != UsableItemType.Scroll || maybeScroll.SpellLevel != potion.SpellLevel) { return false; }
                    scrolls += c.Count;
                    return true;
                });
                if (count == 0) { return; }
                var quantity = scrolls / count;
                table.AddComponent<LootItemsPackFixed>(c => {
                    c.m_Item = CreateLootItem(potion);
                    c.m_Count = quantity;
                });
                Main.TTTContext.Logger.Log($"Added {quantity} {potion.name} to Vender {table.name}");
            });
        }

        public static LootItem CreateLootItem([NotNull]BlueprintItem item) {
            return new LootItem() {
                m_Item = item.ToReference<BlueprintItemReference>(),
                m_Loot = new BlueprintUnitLootReference()
            };
        }

        public static class SharedVenderLists {
            public static BlueprintSharedVendorTable Aminas_Chapter5VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("8b2dae3f8da96f640995594f6bcf7a29");
            public static BlueprintSharedVendorTable Anoriel_MVP_VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("7575b096c6ef17243b9b396a7babe6aa");
            public static BlueprintSharedVendorTable ApprenticeWeapon_DLC1VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("c773973cd73d4cd7aa4ccf3868dfeba9");
            public static BlueprintSharedVendorTable AzataTwins_Chapter5VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("4f3a4aa27393c1d43b9d47d1b60c8346");
            public static BlueprintSharedVendorTable Azata_Chapter3VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("869c56944d1da074886b3d04e75c5e01");
            public static BlueprintSharedVendorTable Barbarian_Chapter3VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("dfcb80aa271b82b49849ed1190db2bbb");
            public static BlueprintSharedVendorTable BlacksmithWeapon_DLC1VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("7d73798a7e624afa8340916c7461cf12");
            public static BlueprintSharedVendorTable Blacksmith_Chapter3VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("1317f39fdc425e94f85c331a79f603d3");
            public static BlueprintSharedVendorTable Blacksmith_Chapter5VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("8ad7f4c00b7142f0a1a68f3ba9e14638");
            public static BlueprintSharedVendorTable Demon_Chapter3VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("9d1f55ec933dd7641adf9fc1cd4c65d2");
            public static BlueprintSharedVendorTable DLC2_QuartermasterBaseTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("8035c1313902fae4796d36065e769297");
            public static BlueprintSharedVendorTable DLC2_QuartermasterImprovedTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("7c68519dca4334c408227bb0140ac50f");
            public static BlueprintSharedVendorTable DLC2_TavernEquipment_Vendor => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("8552125c29c244d8b3a1898381c35585");
            public static BlueprintSharedVendorTable DLC2_TavernScrolls_Vendor => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("696b282b2389454692400fb001b5361f");
            public static BlueprintSharedVendorTable DLC2_TavernWeapon_Vendor => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("89fd944a9439451d807467d993bcd035");
            public static BlueprintSharedVendorTable DLC3_6levelMinibossTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("8c1572ff871840aa95414b68654428d6");
            public static BlueprintSharedVendorTable DLC3_AmuletOfCombatAwarenessItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("33b1449c564941adb65e2fcc5d151d35");
            public static BlueprintSharedVendorTable DLC3_BeautybreakerGlaiveWeaponItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("9bab99b828e84fd4be3b9e66b66858f8");
            public static BlueprintSharedVendorTable DLC3_BesmaraHolySymbolUsableItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("0638e81d9833454886fb9c0f89cf8a2b");
            public static BlueprintSharedVendorTable DLC3_BonesnatcherScytheWeaponItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("5a13b5989e0b4f08b442a42e975f3af9");
            public static BlueprintSharedVendorTable DLC3_BootsOfUndersumpItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("9a6c9d2e5ecf4042866a5848e8547ad0");
            public static BlueprintSharedVendorTable DLC3_BracersOfBraveryItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("c824d0dc897047c48d0feec7c49fe07d");
            public static BlueprintSharedVendorTable DLC3_BrokenTricksterGlassesArtifactItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("7ddfe645a36649a18fd647974723b303");
            public static BlueprintSharedVendorTable DLC3_CloakOfRadianceItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("e32ec350e4c54b868893d6c6064daa04");
            public static BlueprintSharedVendorTable DLC3_CloakOfSweetSpeakSetItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("10fbf7195ad64649a04737fed9a798da");
            public static BlueprintSharedVendorTable DLC3_DefacerWarhammerWeaponItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("44bdfa7b0c3d444386addb5503834f30");
            public static BlueprintSharedVendorTable DLC3_DemonbaneLongswordWeaponItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("ac2a1102af014b46b304b151fdd9414a");
            public static BlueprintSharedVendorTable DLC3_DemonhideLeatherArmorItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("8d8952f8179a4d4da13accf6302c1b2f");
            public static BlueprintSharedVendorTable DLC3_EvercoldHeavyCrossbowWeaponItem_VendorList_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("4015429c3de34f748e529cecba922124");
            public static BlueprintSharedVendorTable DLC3_FoxVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("b03c6dce97104f5f8becb29de4c36063");
            public static BlueprintSharedVendorTable DLC3_FoxVendorTable_Astrolabe => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("828bca143f484ac6a206ab28863fe0f5");
            public static BlueprintSharedVendorTable DLC3_FoxVendorTable_RuneStone => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("80897b86c04e49fbb1b0837180b8678a");
            public static BlueprintSharedVendorTable DLC3_FoxVendorTable_SecondRuneStone => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("eed54184b4c7487ebda837e6f460b048");
            public static BlueprintSharedVendorTable DLC3_FoxVendorTable_ThirdRuneStone => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("eef231d9f8864fc19001015df5812b13");
            public static BlueprintSharedVendorTable DLC3_FoxVendorTable_ThirdRuneStone_ForVendor => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("899004d7e13a44f792bc0fe7d2acd45e");
            public static BlueprintSharedVendorTable DLC3_GlassesOfundeniableTruthItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("65da6f7f9dc84df28c2738c6743d01c3");
            public static BlueprintSharedVendorTable DLC3_GlovesOfSurgicalExtractionItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("784b355c38a14bb4b18943ff0205f67b");
            public static BlueprintSharedVendorTable DLC3_GuardianBattleaxeWeaponItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("074197f3f33140e3ac5a72a3d8abb1ad");
            public static BlueprintSharedVendorTable DLC3_GuardianHeavyShieldSetItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("6c433d05bced48cfa4734b71003bc757");
            public static BlueprintSharedVendorTable DLC3_HelmOfBitterEndItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("377226de8c3241ac9847b02259a97c0a");
            public static BlueprintSharedVendorTable DLC3_Integration_Tier1ShipVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("40d8d24f775d4a2da41e1e25992d7813");
            public static BlueprintSharedVendorTable DLC3_Integration_Tier2ShipVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("1f803f22868146f7b8a4c8a436fa211c");
            public static BlueprintSharedVendorTable DLC3_Integration_Tier3ShipVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("4120046f92cb43468778a76a204ad341");
            public static BlueprintSharedVendorTable DLC3_IntergationTier1VendorTableMagicShields => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("febb251a51954d4cb22198d6655a57b4");
            public static BlueprintSharedVendorTable DLC3_IntergationTier1VendorTable_Equipment => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("e8a1ad13302744339d0896dfc1eabb9b");
            public static BlueprintSharedVendorTable DLC3_IntergationTier2VendorTableMagicShields => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("e64addc32fcf4156bb58d139a0fb4f11");
            public static BlueprintSharedVendorTable DLC3_IntergationTier2VendorTable_Equipment => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("81ecdb3eafaa40049849539872b7ff4b");
            public static BlueprintSharedVendorTable DLC3_JabstabberPunchdaggerWeaponItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("74b8255f9e2445e19b680f54be04cf0d");
            public static BlueprintSharedVendorTable DLC3_KapaoNunchakuWeaponItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("71f32775a6764285b16e35aadfefa262");
            public static BlueprintSharedVendorTable DLC3_ManualOfBodilyHealthPlus2_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("db4d2049c9ec4081a17792dacda7fa3a");
            public static BlueprintSharedVendorTable DLC3_ManualOfGainfulExercisePlus2_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("7afd60ec2e524a428a6ac4d8dd6cf9c7");
            public static BlueprintSharedVendorTable DLC3_ManualOfQuicknessOfActionPlus2_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("892019bfd5da40c49870e74cc762ccea");
            public static BlueprintSharedVendorTable DLC3_NahyndrianHolySymbolUsableItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("7a7e74845dad43189e72b871572a3e0e");
            public static BlueprintSharedVendorTable DLC3_NahyndrianVorpalBladeWeaponItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("919ef0a0e4be427db664cc2a5f8689d1");
            public static BlueprintSharedVendorTable DLC3_RewardsTier1_VendorTableArmor => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("dbe46d7723b8434292fed93951a2c5d8");
            public static BlueprintSharedVendorTable DLC3_RewardsTier1_VendorTableExotic => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("22f258fc8acd4080b8856c4bf380cbe3");
            public static BlueprintSharedVendorTable DLC3_RewardsTier1_VendorTableMagic => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("33793d883054413589f21a29373f3af5");
            public static BlueprintSharedVendorTable DLC3_RewardsTier1_VendorTableNonMagicItems => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("cf34290c0b6d44ef9d21b9eb82e2e7fe");
            public static BlueprintSharedVendorTable DLC3_RewardsTier1_VendorTableRanged => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("f94a6cb4f0d4432e8139401beacdd772");
            public static BlueprintSharedVendorTable DLC3_RingOfInstantTriumphItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("9f5b7984b7c2496da16790cea0dd13c1");
            public static BlueprintSharedVendorTable DLC3_RobeOfTheSinmageItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("43d41df5e81241378648dc3a538e02fc");
            public static BlueprintSharedVendorTable DLC3_RobeOfUnspeakableTruthItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("2b24c722da844207a473918ceade4699");
            public static BlueprintSharedVendorTable DLC3_RodOfMagicalAffinityItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("6975d8c557ad4657809c9fd1475119d1");
            public static BlueprintSharedVendorTable DLC3_ShipVendor_UniqueItemsVendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("2e0cfca2acc34b7f8303cb58aa9edfd5");
            public static BlueprintSharedVendorTable DLC3_SplintershredGreataxeWeaponItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("1efa5de345634f8bb144f388c212d7a1");
            public static BlueprintSharedVendorTable DLC3_StaffOfTheMightySummonsWeaponItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("7dcbfaa296be4eceb26c0ce58573d626");
            public static BlueprintSharedVendorTable DLC3_StandhardFullplateArmorItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("7e9c6d54761143e7b3665d2d9edecd60");
            public static BlueprintSharedVendorTable DLC3_ThePromissingMirrorUsableSetItem_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("de55795def8b46fe861c5c413b324e8a");
            public static BlueprintSharedVendorTable DLC3_Tier1ArcaneScrollsVendorTableI => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("2cfb6b43ea544590a0343456b1853c2d");
            public static BlueprintSharedVendorTable DLC3_Tier1ArcaneScrollsVendorTableII => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("bf06ceb9a6024183baae0f5d5e3fcaac");
            public static BlueprintSharedVendorTable DLC3_Tier1ArcaneScrollsVendorTableIII => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("c052d0b2fa7b4a88ba6b7f9f03abd7f6");
            public static BlueprintSharedVendorTable DLC3_Tier1VendorTableArmorHeavy => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("103363f4a8db4f659044bf6bffe73414");
            public static BlueprintSharedVendorTable DLC3_Tier1VendorTableArmorLight => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("e8412a166689411aaa2fefe2e08e20f1");
            public static BlueprintSharedVendorTable DLC3_Tier1VendorTableArmorMedium => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("2819da0e38de436a84fd06017ab6ce93");
            public static BlueprintSharedVendorTable DLC3_Tier1VendorTableBelts_Headgear_Cloaks => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("7edc7d5b763d4ee98c48e374b9e02773");
            public static BlueprintSharedVendorTable DLC3_Tier1VendorTableCooking => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("3417f959f93045698da165a10c442b22");
            public static BlueprintSharedVendorTable DLC3_Tier1VendorTableExotic => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("488d68bfb3a9495fb1ce2265d63d985d");
            public static BlueprintSharedVendorTable DLC3_Tier1VendorTableMagic => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("fac2be1b911745a588b5afcc19c0e184");
            public static BlueprintSharedVendorTable DLC3_Tier1VendorTablePotions => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("804b38fd19dd4a25b9ed42d87740d882");
            public static BlueprintSharedVendorTable DLC3_Tier1VendorTableRanged => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("a956adf655f048e2a9806e00e047351a");
            public static BlueprintSharedVendorTable DLC3_Tier1VendorTableWeaponExotic => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("3b7157f3304a44c7890d42dd9a98b679");
            public static BlueprintSharedVendorTable DLC3_Tier1VendorTableWeaponMartial => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("5b5e0569795748908496619194843baf");
            public static BlueprintSharedVendorTable DLC3_Tier1VendorTableWeaponsArmor => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("f1cc23ef2a00461989444fefea92375c");
            public static BlueprintSharedVendorTable DLC3_Tier1VendorTableWeaponsArmor1 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("a6b02f445ac945a28e67bcfe3b609bb6");
            public static BlueprintSharedVendorTable DLC3_Tier1VendorTableWeaponSimple => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("ae25c59ff2ee45fc8a57ce9b1602837a");
            public static BlueprintSharedVendorTable DLC3_Tier2ArcaneScrollsVendorTableIV => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("513e9bdc6b3a4778ab8be31147c36cd6");
            public static BlueprintSharedVendorTable DLC3_Tier2ArcaneScrollsVendorTableIX => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("5bd5a253447e4ce3bba5c61eb2fdea2c");
            public static BlueprintSharedVendorTable DLC3_Tier2ArcaneScrollsVendorTableV => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("0ad4faf2da3d4fcfae615383c029a2c3");
            public static BlueprintSharedVendorTable DLC3_Tier2ArcaneScrollsVendorTableVI => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("796157d6093146bdb7b1fa6c97dc16d2");
            public static BlueprintSharedVendorTable DLC3_Tier2ArcaneScrollsVendorTableVII => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("f95d5d11b0f942c38a747b96c6d1ff43");
            public static BlueprintSharedVendorTable DLC3_Tier2VendorTableArmorHeavy => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("448c3991b97242568a07ad5537c5b914");
            public static BlueprintSharedVendorTable DLC3_Tier2VendorTableArmorLight => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("e88384cb2c5d48a38a731a52519bdcbc");
            public static BlueprintSharedVendorTable DLC3_Tier2VendorTableArmorMedium => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("6d1ab4a6ca5a4e75bf336731731a29b4");
            public static BlueprintSharedVendorTable DLC3_Tier2VendorTableBelts_Headgear_Cloaks => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("3a01a544818647439150ccf267a7a11d");
            public static BlueprintSharedVendorTable DLC3_Tier2VendorTableExotic => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("f6b5df0eb8df4ed3b1ee3d5125ba82ee");
            public static BlueprintSharedVendorTable DLC3_Tier2VendorTableMagic => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("6176737df85b41fea25e13b67fe557b8");
            public static BlueprintSharedVendorTable DLC3_Tier2VendorTablePotions => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("0d3f964412b74828b94d99feb090a30a");
            public static BlueprintSharedVendorTable DLC3_Tier2VendorTableWeaponExotic => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("38680379d8a548eea36a5f2b662ccf36");
            public static BlueprintSharedVendorTable DLC3_Tier2VendorTableWeaponMartial => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("8f405efc82e54db596a84eff335e582a");
            public static BlueprintSharedVendorTable DLC3_Tier2VendorTableWeaponsArmor => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("d4fb460ab34f48d685f459df8f89229b");
            public static BlueprintSharedVendorTable DLC3_Tier2VendorTableWeaponSimple => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("69e045434ac54604ab2dd1bda979bf08");
            public static BlueprintSharedVendorTable DLC3_Tier3ArcaneScrollsVendorTableVIII => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("e9924274f90049559204ef2fab9c8804");
            public static BlueprintSharedVendorTable DLC3_Tier3ArcaneScrollsVendorTableX => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("b13ee7750a474e4abbf166b056a1bebe");
            public static BlueprintSharedVendorTable DLC3_Tier3ArcaneScrollsVendorTableXI => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("8d48a30f0421477cab663b9282cc2f82");
            public static BlueprintSharedVendorTable DLC3_Tier3ArmorVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("0ec8d07f99eb4e3682e91d0412f53d70");
            public static BlueprintSharedVendorTable DLC3_Tier3JewelerVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("c8fd0ac54b0a42c2ad565f7101edb140");
            public static BlueprintSharedVendorTable DLC3_Tier3ScrollVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("a7c4fef127464eb6a2ea622412cf191c");
            public static BlueprintSharedVendorTable DLC3_Tier3VendorTableArmor_All => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("3b5fdbe6926a4c938b0574d2069ee6c8");
            public static BlueprintSharedVendorTable DLC3_Tier3VendorTableWeapons_All => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("95c08baab4b24d8db727bab12cf2d5db");
            public static BlueprintSharedVendorTable DLC3_Tier3WeaponUniqueVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("f3d160ababeb4734a99ee5d367961402");
            public static BlueprintSharedVendorTable DLC3_Tier3WeaponVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("b9b63fa2b7234216a06defa1478975e7");
            public static BlueprintSharedVendorTable DLC3_TomeOfClearThoughtPlus2_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("e59d5bebe24e4ab3967864268c383783");
            public static BlueprintSharedVendorTable DLC3_TomeOfLeadershipAndInfluencePlus2_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("bfb53cef45364177a33841ace6b7fae4");
            public static BlueprintSharedVendorTable DLC3_TomeOfUnderstandingPlus2_VendorList => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("4b7f0392376948c59d42dce0dda33615");
            public static BlueprintSharedVendorTable DLC3_UniqueVendorTable_Books => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("700c83902f9d4444b476312535ac6703");
            public static BlueprintSharedVendorTable DLC3_VendorTableArmorCommon => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("28c7975b2b9241d9bcccac20cf656976");
            public static BlueprintSharedVendorTable DLC3_VendorTableArmorHeavyPlus1 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("bc6717d405274767ace767b93f49d0dc");
            public static BlueprintSharedVendorTable DLC3_VendorTableArmorHeavyPlus2 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("15a3321565de418fa17105a5d46ba2d6");
            public static BlueprintSharedVendorTable DLC3_VendorTableArmorHeavyPlus3 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("49ee0059375b49838436b11646976b54");
            public static BlueprintSharedVendorTable DLC3_VendorTableArmorHeavyPlus4 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("c865e67aec6e4785af14a09b19d3f759");
            public static BlueprintSharedVendorTable DLC3_VendorTableArmorHeavyPlus5 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("1fda012baf5e49e586785b956025a3da");
            public static BlueprintSharedVendorTable DLC3_VendorTableArmorLightPlus1 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("4978f84722f4450297647cdd7676c487");
            public static BlueprintSharedVendorTable DLC3_VendorTableArmorLightPlus2 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("d1fbfb0e02784bf1841373149e506ec1");
            public static BlueprintSharedVendorTable DLC3_VendorTableArmorLightPlus3 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("853c937653ec4882a223ba608d901902");
            public static BlueprintSharedVendorTable DLC3_VendorTableArmorLightPlus4 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("5c31119d8658412cac90741610c033a5");
            public static BlueprintSharedVendorTable DLC3_VendorTableArmorLightPlus5 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("8c4863d13e344e95b0730382b9d16bfe");
            public static BlueprintSharedVendorTable DLC3_VendorTableArmorMediumPlus1 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("d85f41016efc40a8b35059aa2c6e1779");
            public static BlueprintSharedVendorTable DLC3_VendorTableArmorMediumPlus2 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("96796f89c1964163b8c19fe50f92e4da");
            public static BlueprintSharedVendorTable DLC3_VendorTableArmorMediumPlus3 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("486305b9af7d44269c83929b4285d7ce");
            public static BlueprintSharedVendorTable DLC3_VendorTableArmorMediumPlus4 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("e090fccea207422e8c6254ee688755d5");
            public static BlueprintSharedVendorTable DLC3_VendorTableArmorMediumPlus5 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("0dd526ca355f4e0f867f8965068d4983");
            public static BlueprintSharedVendorTable DLC3_VendorTableFluffyBoots => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("08f0c2e1c54846aab5c801e8edfdfa17");
            public static BlueprintSharedVendorTable DLC3_VendorTableLockpicks => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("ef7edc1aea8a4f47af6aecf1613baac7");
            public static BlueprintSharedVendorTable DLC3_VendorTableMagicShields => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("695e348b2719415f883de90d932aeb66");
            public static BlueprintSharedVendorTable DLC3_VendorTableReagentsCraft => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("9bdd817420914ecb9eadbe89b238e80f");
            public static BlueprintSharedVendorTable DLC3_VendorTableShields => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("011c094dd4194391bac4150c38ae3c90");
            public static BlueprintSharedVendorTable DLC3_VendorTableShields2 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("f0164e4ab9ef4a4da8d86a0c0e0bc45f");
            public static BlueprintSharedVendorTable DLC3_VendorTableWeaponColdIron => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("2464a9da82c04d57b8ea226d65776112");
            public static BlueprintSharedVendorTable DLC3_VendorTableWeaponCommon => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("d009c1615ffa47ffbba9ff155ba82221");
            public static BlueprintSharedVendorTable DLC3_VendorTableWeaponExoticPlus1 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("dce1b78abaf0468ead45da3b26d56261");
            public static BlueprintSharedVendorTable DLC3_VendorTableWeaponExoticPlus2 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("c3f3cb3188d247e88d3415acefc5f1e1");
            public static BlueprintSharedVendorTable DLC3_VendorTableWeaponExoticPlus3 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("5af5a94a1e524351862a3aeb26f01811");
            public static BlueprintSharedVendorTable DLC3_VendorTableWeaponExoticPlus4 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("f8149f9a9fad47f6a040aac2ab13a974");
            public static BlueprintSharedVendorTable DLC3_VendorTableWeaponExoticPlus5 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("1d096839cb7441d1bf7d766940c55a5c");
            public static BlueprintSharedVendorTable DLC3_VendorTableWeaponMartialPlus1 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("cf32b863413d4b7b8e34fb89108c0314");
            public static BlueprintSharedVendorTable DLC3_VendorTableWeaponMartialPlus2 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("e7cbb635f30449e2b8dec802b6f8a31d");
            public static BlueprintSharedVendorTable DLC3_VendorTableWeaponMartialPlus3 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("271c07bf1019484f8ec10ad878786238");
            public static BlueprintSharedVendorTable DLC3_VendorTableWeaponMartialPlus4 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("59d7df2a32c342e2845eb1439068ffa8");
            public static BlueprintSharedVendorTable DLC3_VendorTableWeaponMartialPlus5 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("9cd570fcd5534d3c90d5303ac9e94747");
            public static BlueprintSharedVendorTable DLC3_VendorTableWeaponMasterwork => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("3e821635143040bd8bc1548248717b48");
            public static BlueprintSharedVendorTable DLC3_VendorTableWeaponSimplePlus1 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("0d35745e7d6d4b18b23b0074742d79ce");
            public static BlueprintSharedVendorTable DLC3_VendorTableWeaponSimplePlus2 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("68720847b6284d3ca5f48ca3c6471e5f");
            public static BlueprintSharedVendorTable DLC3_VendorTableWeaponSimplePlus3 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("2d11d82aeff44c75b049b94d2c6c448f");
            public static BlueprintSharedVendorTable DLC3_VendorTableWeaponSimplePlus4 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("f945e0a27bf248009a0507d2122ad5f2");
            public static BlueprintSharedVendorTable DLC3_VendorTableWeaponSimplePlus5 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("96e09defdc76493884772c767f577e48");
            public static BlueprintSharedVendorTable DLC3_VendorTable_Equipment => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("195579adaa20483ca3aad66bb2b06f8f");
            public static BlueprintSharedVendorTable Equipment_DefendersHeartVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("a7948df9d37efc34e841284cf883370e");
            public static BlueprintSharedVendorTable Equipment_Vendor_DLC2 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("e5cdfb113e884ab6967650f5eacbcb42");
            public static BlueprintSharedVendorTable Exotic_Chapter3VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("9c597a1f92dde2f4f8adb27ee5730188");
            public static BlueprintSharedVendorTable Exotic_Chapter5VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("73895d43f46b45079e19d1afcb96efdd");
            public static BlueprintSharedVendorTable GesmerhaScroll_DLC1VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("6bd529a52e1b4dbbac223f36b0a91a35");
            public static BlueprintSharedVendorTable GolemGold_VendorJewelryVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("89567a21927845a0822d8bdaa724b8ce");
            public static BlueprintSharedVendorTable HellKnight_Chapter3VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("4dc0d56a0fd337141a9b9ce03ba8be3b");
            public static BlueprintSharedVendorTable HerraxaVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("d3dabf45a0d64bc4aa7b205a6889a05f");
            public static BlueprintSharedVendorTable HilorArmor_DLC1VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("84fb4303222b413eb42daeb8d98a30dd");
            public static BlueprintSharedVendorTable InnkeeperVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("7b80ddeaf18d5a740bc12e7325044f29");
            public static BlueprintSharedVendorTable InquisitorEquipment_Vendor_DLC2 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("93d29ef9b5744b4da02629776c62c618");
            public static BlueprintSharedVendorTable Jeweler_Chapter3VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("9f959fcff8d929042b1be6311d209580");
            public static BlueprintSharedVendorTable Jeweler_Chapter5VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("fad4f5653f1d4943ad21a2dd85ee746b");
            public static BlueprintSharedVendorTable Jeweler_DLC1VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("3f08f2fcaed14d989ff1230fb214f1fb");
            public static BlueprintSharedVendorTable JorunVane_Chapter3VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("c63be82dd0232be46afad32511459bdb");
            public static BlueprintSharedVendorTable JorunVane_Chapter5VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("c4bcbc0073854f9797d92a273eb762de");
            public static BlueprintSharedVendorTable KrebusSlaveTraderTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("d43baa8b603f4604f8e36b048072e759");
            public static BlueprintSharedVendorTable LannMother_Chapter3VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("a7a198b1c3dc3e24692a6a8c465f3b08");
            public static BlueprintSharedVendorTable Lich_Chapter3VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("3b35f6a55ee83104ea6fab0a8cdcfbd3");
            public static BlueprintSharedVendorTable Lich_Chapter5VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("b15c8997231e48b98087704ac635a660");
            public static BlueprintSharedVendorTable MarauderEquipment_Vendor_DLC2 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("602daa81c1724ead9399fe630d0af6b3");
            public static BlueprintSharedVendorTable Potions_DefendersHeartVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("fc312b3b4e355a842815b5c519924ef7");
            public static BlueprintSharedVendorTable PrologueVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("dec7a716db665ec498134fae15721325");
            public static BlueprintSharedVendorTable Pulura_Chapter3VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("bec69fab59dbf2f47986414216a88453");
            public static BlueprintSharedVendorTable Quartermaster_Chapter3VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("5a425708ce352da4f83e6159bdb73c10");
            public static BlueprintSharedVendorTable Quartermaster_Chapter5VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("5aae5d25fc8e485fbee34a89ab1a2278");
            public static BlueprintSharedVendorTable Ramley_Chapter3VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("a5ceb32ce6083e04782ec267abec0a19");
            public static BlueprintSharedVendorTable RE_Chapter3VendorTableArmor => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("14e9cc6d7368c494fa8c3381b4f8fd7b");
            public static BlueprintSharedVendorTable RE_Chapter3VendorTableExotic => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("5f722616e64f41b4f960cd00c0b4896c");
            public static BlueprintSharedVendorTable RE_Chapter3VendorTableMagic => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("e8e384f0e411fab42a69f16991cac161");
            public static BlueprintSharedVendorTable RE_Chapter3VendorTableRanged => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("ac6e0ceea75b1cc45984598fa71c62cb");
            public static BlueprintSharedVendorTable RE_Chapter5VendorTableArmor => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("3f2fc41a7b7b323418a04e2ac7175c86");
            public static BlueprintSharedVendorTable RE_Chapter5VendorTableArmor1 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("4b4aeecbf98e4bfb81670572f580beb6");
            public static BlueprintSharedVendorTable RE_Chapter5VendorTableExotic => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("341cb6ed505e9f7419ffe00deed81eb2");
            public static BlueprintSharedVendorTable RE_Chapter5VendorTableExotic1 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("8cbf21a7df7d4a709edb0e00d0bd2701");
            public static BlueprintSharedVendorTable RE_Chapter5VendorTableMagic => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("e1d21a0e6c9177d42a1b0fac1d6f8b21");
            public static BlueprintSharedVendorTable RE_Chapter5VendorTableMagic1 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("b43c355d825a40a5ab7c41fccf0e0665");
            public static BlueprintSharedVendorTable RE_Chapter5VendorTableRanged => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("94d47d1ca289c3e4baccf7d59923e13b");
            public static BlueprintSharedVendorTable RE_Chapter5VendorTableRanged1 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("a0b01a2067404f868e1c5b70562445c8");
            public static BlueprintSharedVendorTable RvanyVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("8edcc85ae155a3f45947d09876634131");
            public static BlueprintSharedVendorTable SarzaksisVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("39ca56e5b62726345aabef29deb26f66");
            public static BlueprintSharedVendorTable Scrolls_DefendersHeartVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("79b995e5fc910f34ab9dfec3c6b16c8f");
            public static BlueprintSharedVendorTable Scroll_Chapter3VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("d33d4c7396fc1d74c9569bc38e887e86");
            public static BlueprintSharedVendorTable Scroll_Chapter3VendorTable1 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("37fd0632991e42d0bdf5f6c1985116dd");
            public static BlueprintSharedVendorTable Scroll_Chapter5VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("5b73c93dccd743668734070160dfb82f");
            public static BlueprintSharedVendorTable Scroll_Chapter5VendorTable1 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("1238046a6e0a433aaa17fec01cb4d49c");
            public static BlueprintSharedVendorTable ShaxahVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("2c4120dfb728c4841a5fe356eaaf0e25");
            public static BlueprintSharedVendorTable StorytellerVendorTable_Ch4 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("9edaf8d72271d2e45a5f9a4c7362cc1a");
            public static BlueprintSharedVendorTable StorytellerVendorTable_Ch5 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("dcf70acca2dc4becaec035316584b3aa");
            public static BlueprintSharedVendorTable StreetRat_Ch4 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("8c56dd07853487c4da7bf2945961d455");
            public static BlueprintSharedVendorTable StreetRat_Chapter3VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("3c098c8feea9cc44eadad74b5352ab95");
            public static BlueprintSharedVendorTable StreetRat_Chapter3VendorTable1 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("cdd06bae3fbe4d17aae56f23a535a40e");
            public static BlueprintSharedVendorTable StreetRat_Chapter5VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("74e8b905840c400bbd9092c83c28efb6");
            public static BlueprintSharedVendorTable StreetRat_Chapter5VendorTable1 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("ff8a53707cb9445a9ba07f698cea565d");
            public static BlueprintSharedVendorTable StreetRat_DefendersHeartVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("a8c181f20ae888b4aa7227c2903207e4");
            public static BlueprintSharedVendorTable Tailor_Chapter3VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("9b80d696f04aaef4cb4e13eadcc66731");
            public static BlueprintSharedVendorTable Tailor_Chapter3VendorTable1 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("c0d74edb41944e4383db2d41e9647819");
            public static BlueprintSharedVendorTable Tailor_Chapter5VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("761ab52879cf48c3ba811b28e79a7476");
            public static BlueprintSharedVendorTable Tailor_Chapter5VendorTable1 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("1e28cb02e25a4e308b41f49ba5631774");
            public static BlueprintSharedVendorTable Tailor_DLC1VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("76560f48b5d24c70bb228db6f3f1c099");
            public static BlueprintSharedVendorTable Tailor_DLC1VendorTable1 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("301b28b1b3924e55989c41288048e4d2");
            public static BlueprintSharedVendorTable Tavern_Chapter3VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("f8c18fa37dd0fab4cad5a9d8b427ed18");
            public static BlueprintSharedVendorTable Tavern_Chapter3VendorTable1 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("677b8f618d764017bf72e4829435f162");
            public static BlueprintSharedVendorTable Tavern_Chapter5VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("7d83227c93a74abba4f415cddb70a256");
            public static BlueprintSharedVendorTable Tavern_Chapter5VendorTable1 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("6cf078abc95844149e674ae30dd5db7c");
            public static BlueprintSharedVendorTable Test_Bebilith_Blueprint_Shared_Vendor_Table => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("8c7015b8338257244bd6df3e4c4f968b");
            public static BlueprintSharedVendorTable TownEquipment_Vendor_DLC2 => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("4dd4a19fbb7d48fc9fee74d67f6f7888");
            public static BlueprintSharedVendorTable Unused_vendortable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("20cd2bcf12b142ae9dfaec07b68c4866");
            public static BlueprintSharedVendorTable VendorTiefling_Chapter3VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("d0a54c0af02eed44488c15108fed0433");
            public static BlueprintSharedVendorTable VendorTiefling_Chapter5VendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("8c7626740d3b46328424b7728377111e");
            public static BlueprintSharedVendorTable VirlongVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("a6ba12cd0d91c5244a8105628b42a5a4");
            public static BlueprintSharedVendorTable WarCamp_BlacksmithVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("7aaf7d11ce8541b69b3ce0064dd45d2a");
            public static BlueprintSharedVendorTable WarCamp_QuartermasterVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("5753b6f35e7db234aa44085a358c27af");
            public static BlueprintSharedVendorTable WarCamp_REVendorTableArmor => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("53e984b930aa4fd4fb5783b30689b9fe");
            public static BlueprintSharedVendorTable WarCamp_REVendorTableExotic => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("536e0ffdc574a9647beaa66745c7b5dc");
            public static BlueprintSharedVendorTable WarCamp_REVendorTableMagic => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("f02cf582e915ae343aa489f11dba42aa");
            public static BlueprintSharedVendorTable WarCamp_REVendorTableRanged => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("d8b6c5c0356a8da49a619117bf15d199");
            public static BlueprintSharedVendorTable WarCamp_ScrollVendorClericTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("cdd7aa16e900b9146bc6963ca53b8e71");
            public static BlueprintSharedVendorTable WarCamp_StreetRatVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("24282bde41338884d840a06987c1b3bf");
            public static BlueprintSharedVendorTable Weapon_DefendersHeartVendorTable => BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("5f17d3b47752fb94abe8c98534af8920");

            private static List<BlueprintSharedVendorTable> m_ScrollVenderTables = new List<BlueprintSharedVendorTable>() {
                Anoriel_MVP_VendorTable,
                AzataTwins_Chapter5VendorTable,
                Azata_Chapter3VendorTable,
                Barbarian_Chapter3VendorTable,
                Demon_Chapter3VendorTable,
                DLC2_QuartermasterBaseTable,
                DLC2_QuartermasterImprovedTable,
                DLC2_TavernScrolls_Vendor,
                DLC3_RewardsTier1_VendorTableArmor,
                DLC3_RewardsTier1_VendorTableExotic,
                DLC3_RewardsTier1_VendorTableMagic,
                DLC3_RewardsTier1_VendorTableRanged,
                DLC3_Tier1ArcaneScrollsVendorTableI,
                DLC3_Tier1ArcaneScrollsVendorTableII,
                DLC3_Tier1ArcaneScrollsVendorTableIII,
                DLC3_Tier1VendorTableMagic,
                DLC3_Tier2ArcaneScrollsVendorTableIV,
                DLC3_Tier2ArcaneScrollsVendorTableIX,
                DLC3_Tier2ArcaneScrollsVendorTableV,
                DLC3_Tier2ArcaneScrollsVendorTableVI,
                DLC3_Tier2ArcaneScrollsVendorTableVII,
                DLC3_Tier2VendorTableMagic,
                DLC3_Tier2VendorTablePotions,
                DLC3_Tier3ArcaneScrollsVendorTableVIII,
                DLC3_Tier3ArcaneScrollsVendorTableX,
                DLC3_Tier3ArcaneScrollsVendorTableXI,
                DLC3_Tier3ScrollVendorTable,
                GesmerhaScroll_DLC1VendorTable,
                InquisitorEquipment_Vendor_DLC2,
                Jeweler_Chapter3VendorTable,
                KrebusSlaveTraderTable,
                Lich_Chapter3VendorTable,
                Lich_Chapter5VendorTable,
                MarauderEquipment_Vendor_DLC2,
                PrologueVendorTable,
                Pulura_Chapter3VendorTable,
                Ramley_Chapter3VendorTable,
                RE_Chapter3VendorTableArmor,
                RE_Chapter3VendorTableExotic,
                RE_Chapter3VendorTableMagic,
                RE_Chapter3VendorTableRanged,
                RE_Chapter5VendorTableArmor,
                RE_Chapter5VendorTableArmor1,
                RE_Chapter5VendorTableExotic,
                RE_Chapter5VendorTableExotic1,
                RE_Chapter5VendorTableMagic,
                RE_Chapter5VendorTableMagic1,
                RE_Chapter5VendorTableRanged,
                RE_Chapter5VendorTableRanged1,
                Scrolls_DefendersHeartVendorTable,
                Scroll_Chapter3VendorTable,
                Scroll_Chapter3VendorTable1,
                Scroll_Chapter5VendorTable,
                Scroll_Chapter5VendorTable1,
                StorytellerVendorTable_Ch4,
                StorytellerVendorTable_Ch5,
                StreetRat_Chapter3VendorTable,
                StreetRat_Chapter3VendorTable1,
                StreetRat_Chapter5VendorTable,
                StreetRat_Chapter5VendorTable1,
                StreetRat_DefendersHeartVendorTable,
                TownEquipment_Vendor_DLC2,
                VendorTiefling_Chapter3VendorTable,
                VendorTiefling_Chapter5VendorTable,
                WarCamp_REVendorTableArmor,
                WarCamp_REVendorTableExotic,
                WarCamp_REVendorTableMagic,
                WarCamp_REVendorTableRanged,
                WarCamp_ScrollVendorClericTable,
                WarCamp_StreetRatVendorTable,
            };
            public static BlueprintSharedVendorTable[] ScrollVenderTables => m_ScrollVenderTables.ToArray();
            private static List<BlueprintSharedVendorTable> m_PotionVenderTables = new List<BlueprintSharedVendorTable>() {
                AzataTwins_Chapter5VendorTable,
                Azata_Chapter3VendorTable,
                DLC3_RewardsTier1_VendorTableArmor,
                DLC3_RewardsTier1_VendorTableExotic,
                DLC3_RewardsTier1_VendorTableMagic,
                DLC3_RewardsTier1_VendorTableRanged,
                DLC3_Tier1VendorTableMagic,
                DLC3_Tier1VendorTablePotions,
                DLC3_Tier2VendorTableMagic,
                DLC3_Tier2VendorTablePotions,
                DLC3_Tier3ScrollVendorTable,
                Equipment_Vendor_DLC2,
                GesmerhaScroll_DLC1VendorTable,
                InnkeeperVendorTable,
                InquisitorEquipment_Vendor_DLC2,
                KrebusSlaveTraderTable,
                LannMother_Chapter3VendorTable,
                Lich_Chapter3VendorTable,
                Lich_Chapter5VendorTable,
                MarauderEquipment_Vendor_DLC2,
                Potions_DefendersHeartVendorTable,
                PrologueVendorTable,
                Pulura_Chapter3VendorTable,
                Ramley_Chapter3VendorTable,
                RE_Chapter3VendorTableArmor,
                RE_Chapter3VendorTableExotic,
                RE_Chapter3VendorTableMagic,
                RE_Chapter3VendorTableRanged,
                RE_Chapter5VendorTableArmor,
                RE_Chapter5VendorTableArmor1,
                RE_Chapter5VendorTableExotic,
                RE_Chapter5VendorTableExotic1,
                RE_Chapter5VendorTableMagic,
                RE_Chapter5VendorTableMagic1,
                RE_Chapter5VendorTableRanged,
                RE_Chapter5VendorTableRanged1,
                StorytellerVendorTable_Ch4,
                StorytellerVendorTable_Ch5,
                Tavern_Chapter3VendorTable,
                Tavern_Chapter3VendorTable1,
                Tavern_Chapter5VendorTable,
                Tavern_Chapter5VendorTable1,
                TownEquipment_Vendor_DLC2,
                Unused_vendortable,
                WarCamp_REVendorTableArmor,
                WarCamp_REVendorTableExotic,
                WarCamp_REVendorTableMagic,
                WarCamp_REVendorTableRanged,
                WarCamp_ScrollVendorClericTable,
                WarCamp_StreetRatVendorTable,
            };
            public static BlueprintSharedVendorTable[] PotionVenderTables => m_PotionVenderTables.ToArray();
        }
    }
}
