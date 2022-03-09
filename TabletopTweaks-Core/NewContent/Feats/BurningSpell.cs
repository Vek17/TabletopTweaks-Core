using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Loot;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewContent.MechanicsChanges;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Core.Main;
using static TabletopTweaks.Core.NewContent.MechanicsChanges.MetamagicExtention;
using static TabletopTweaks.Core.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TabletopTweaks.Core.NewContent.Feats {
    static class BurningSpell {
        public static void AddBurningSpell() {
            var CausticEruption = BlueprintTools.GetBlueprint<BlueprintAbility>("8c29e953190cc67429dc9c701b16b7c2");
            var FireStormBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("ef7d021abb6bbfd4cad4f2f2b70bcf28");
            var FirstStage_AcidBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("6afe27c9a2d64eb890673ff3649dacb3");
            var FavoriteMetamagicSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("503fb196aa222b24cb6cfdc9a284e838");
            var Icon_BurningSpellFeat = AssetLoader.LoadInternal(modContext: TTTContext, folder: "Feats", file: "Icon_BurningSpellFeat.png");
            var Icon_BurningSpellMetamagic = AssetLoader.LoadInternal(modContext: TTTContext, folder: "Metamagic", file: "Icon_BurningSpellMetamagic.png", size: 128);
            var Icon_MetamagicRodBurningLesser = AssetLoader.LoadInternal(modContext: TTTContext, folder: "Equipment", file: "Icon_MetamagicRodBurningLesser.png", size: 64);
            var Icon_MetamagicRodBurningNormal = AssetLoader.LoadInternal(modContext: TTTContext, folder: "Equipment", file: "Icon_MetamagicRodBurningNormal.png", size: 64);
            var Icon_MetamagicRodBurningGreater = AssetLoader.LoadInternal(modContext: TTTContext, folder: "Equipment", file: "Icon_MetamagicRodBurningGreater.png", size: 64);

            var BurningSpellFeat = Helpers.CreateBlueprint<BlueprintFeature>(modContext: TTTContext, "BurningSpellFeat", bp => {
                bp.SetName("Metamagic (Burning Spell)");
                bp.SetDescription("You cause creatures to take extra damage when you affect them with a spell that has the acid or fire descriptor.\n" +
                    "Benefit: The acid or fire effects of the affected spell adhere to the creature, causing more " +
                    "damage the next round. When a creature takes acid or fire damage from the affected spell, " +
                    "that creature takes damage equal to 2x the spell’s actual level at the start of its next turn. " +
                    "The damage is acid or fire, as determined by the spell’s descriptor.\n" +
                    "Level Increase: +2 (a burning spell uses up a slot two levels higher than the spell’s actual level.)");
                bp.m_Icon = Icon_BurningSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.WizardFeat };
                bp.AddComponent<AddMetamagicFeat>(c => {
                    c.Metamagic = (Metamagic)CustomMetamagic.Burning;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Magic | FeatureTag.Metamagic;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.Intelligence;
                    c.Value = 3;
                });
                bp.AddComponent<RecommendationRequiresSpellbook>();
            });
            var FavoriteMetamagicBurning = Helpers.CreateBlueprint<BlueprintFeature>(modContext: TTTContext, "FavoriteMetamagicBurning", bp => {
                bp.SetName("Favorite Metamagic — Burning");
                bp.m_Description = FavoriteMetamagicSelection.m_Description;
                //bp.m_Icon = Icon_IntensifiedSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { };
                bp.AddComponent<AddCustomMechanicsFeature>(c => {
                    c.Feature = CustomMechanicsFeature.FavoriteMetamagicBurning;
                });
                bp.AddPrerequisiteFeature(BurningSpellFeat);
            });
            var BurningSpellFireBuff = Helpers.CreateBuff(modContext: TTTContext, "BurningSpellFireBuff", bp => {
                bp.SetName("Burning Spell");
                bp.SetDescription("This target will take fire damage at the start of next round.");
                bp.Stacking = StackingType.Stack;
                bp.m_Icon = FireStormBuff.Icon;
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.FxOnStart = FireStormBuff.FxOnStart;
                bp.ResourceAssetIds = FireStormBuff.ResourceAssetIds;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<AddFactContextActions>(c => {
                    c.NewRound = Helpers.CreateActionList(
                        new ContextActionDealDamage() {
                            DamageType = new DamageTypeDescription() {
                                Type = DamageType.Energy,
                                Common = new DamageTypeDescription.CommomData(),
                                Physical = new DamageTypeDescription.PhysicalData(),
                                Energy = DamageEnergyType.Fire
                            },
                            Duration = new ContextDurationValue() {
                                m_IsExtendable = true,
                                DiceCountValue = new ContextValue(),
                                BonusValue = new ContextValue()
                            },
                            Value = new ContextDiceValue() {
                                DiceCountValue = 0,
                                BonusValue = new ContextValue() {
                                    ValueType = ContextValueType.Rank
                                }
                            },
                            IgnoreCritical = true
                        },
                        new ContextActionRemoveSelf()
                    );
                    c.Activated = Helpers.CreateActionList();
                    c.Deactivated = Helpers.CreateActionList();
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                });
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    c.Descriptor = SpellDescriptor.Fire;
                });
            });
            var BurningSpellAcidBuff = Helpers.CreateBuff(modContext: TTTContext, "BurningSpellAcidBuff", bp => {
                bp.SetName("Burning Spell");
                bp.SetDescription("This target will take acid damage at the start of next round.");
                bp.Stacking = StackingType.Stack;
                bp.m_Icon = CausticEruption.Icon;
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.FxOnStart = FirstStage_AcidBuff.FxOnStart;
                bp.ResourceAssetIds = FirstStage_AcidBuff.ResourceAssetIds;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<AddFactContextActions>(c => {
                    c.NewRound = Helpers.CreateActionList(
                        new ContextActionDealDamage() {
                            DamageType = new DamageTypeDescription() {
                                Type = DamageType.Energy,
                                Common = new DamageTypeDescription.CommomData(),
                                Physical = new DamageTypeDescription.PhysicalData(),
                                Energy = DamageEnergyType.Acid
                            },
                            Duration = new ContextDurationValue() {
                                m_IsExtendable = true,
                                DiceCountValue = new ContextValue(),
                                BonusValue = new ContextValue()
                            },
                            Value = new ContextDiceValue() {
                                DiceCountValue = 0,
                                BonusValue = new ContextValue() {
                                    ValueType = ContextValueType.Rank
                                }
                            },
                            IgnoreCritical = true
                        },
                        new ContextActionRemoveSelf()
                    );
                    c.Activated = Helpers.CreateActionList();
                    c.Deactivated = Helpers.CreateActionList();
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                });
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    c.Descriptor = SpellDescriptor.Acid;
                });
            });
            if (TTTContext.AddedContent.Feats.IsEnabled("MetamagicBurningSpell")) {
                MetamagicExtention.RegisterMetamagic(
                    metamagic: (Metamagic)CustomMetamagic.Burning,
                    name: "Burning",
                    icon: Icon_BurningSpellMetamagic,
                    defaultCost: 2,
                    favoriteMetamagic: CustomMechanicsFeature.FavoriteMetamagicBurning
                );
            }
            var MetamagicRodsRime = ItemTools.CreateAllMetamagicRods(
                modContext: TTTContext, rodName: "Burning Metamagic Rod",
                lesserIcon: Icon_MetamagicRodBurningLesser,
                normalIcon: Icon_MetamagicRodBurningNormal,
                greaterIcon: Icon_MetamagicRodBurningGreater,
                metamagic: (Metamagic)CustomMetamagic.Burning,
                rodDescriptionStart: "This rod grants its wielder the ability to make up to three spells they cast per day burning, " +
                    "as though using the Burning Spell feat.",
                metamagicDescription: "Burning Spell: When a creature takes acid or fire damage from the affected spell, " +
                    "that creature takes damage equal to 2x the spell’s actual level at the start of its next turn. " +
                    "The damage is acid or fire, as determined by the spell’s descriptor."
            );

            if (TTTContext.AddedContent.Feats.IsDisabled("MetamagicBurningSpell")) { return; }

            UpdateSpells();
            AddRodsToVenders();
            FeatTools.AddAsFeat(BurningSpellFeat);
            FeatTools.AddAsMetamagicFeat(BurningSpellFeat);
            FavoriteMetamagicSelection.AddFeatures(FavoriteMetamagicBurning);
        }
        private static void UpdateSpells() {
            var spells = SpellTools.GetAllSpells();
            foreach (var spell in spells) {
                bool isBurningSpell = spell.AbilityAndVariants()
                    .SelectMany(s => s.AbilityAndStickyTouch())
                    .Any(s => s.GetComponent<SpellDescriptorComponent>()?
                        .Descriptor.HasAnyFlag(SpellDescriptor.Fire | SpellDescriptor.Acid) ?? false)
                    || spell.GetComponent<AbilityShadowSpell>();
                if (isBurningSpell) {
                    if (!spell.AvailableMetamagic.HasMetamagic((Metamagic)CustomMetamagic.Burning)) {
                        spell.AvailableMetamagic |= (Metamagic)CustomMetamagic.Burning;
                        TTTContext.Logger.LogPatch("Enabled Burning Metamagic", spell);
                    }
                };
            }
        }
        private static void AddRodsToVenders() {
            var WarCamp_REVendorTableMagic = BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("f02cf582e915ae343aa489f11dba42aa");
            var RE_Chapter3VendorTableMagic = BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("e8e384f0e411fab42a69f16991cac161");
            var KrebusSlaveTraderTable = BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("d43baa8b603f4604f8e36b048072e759");

            WarCamp_REVendorTableMagic.AddComponent<LootItemsPackFixed>(c => {
                c.m_Item = new LootItem() {
                    m_Item = BlueprintTools.GetModBlueprintReference<BlueprintItemReference>(modContext: TTTContext, "MetamagicRodLesserBurning"),
                    m_Loot = new BlueprintUnitLootReference()
                };
                c.m_Count = 1;
            });
            RE_Chapter3VendorTableMagic.AddComponent<LootItemsPackFixed>(c => {
                c.m_Item = new LootItem() {
                    m_Item = BlueprintTools.GetModBlueprintReference<BlueprintItemReference>(modContext: TTTContext, "MetamagicRodNormalBurning"),
                    m_Loot = new BlueprintUnitLootReference()
                };
                c.m_Count = 1;
            });
            KrebusSlaveTraderTable.AddComponent<LootItemsPackFixed>(c => {
                c.m_Item = new LootItem() {
                    m_Item = BlueprintTools.GetModBlueprintReference<BlueprintItemReference>(modContext: TTTContext, "MetamagicRodGreaterBurning"),
                    m_Loot = new BlueprintUnitLootReference()
                };
                c.m_Count = 1;
            });
        }
    }
}
