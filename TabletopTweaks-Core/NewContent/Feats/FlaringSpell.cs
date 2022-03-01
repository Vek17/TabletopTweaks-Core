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
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using System.Linq;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewContent.MechanicsChanges;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Core.Main;
using static TabletopTweaks.Core.NewContent.MechanicsChanges.MetamagicExtention;
using static TabletopTweaks.Core.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TabletopTweaks.Core.NewContent.Feats {
    static class FlaringSpell {
        public static void AddFlaringSpell() {
            var DazzledBuff = Resources.GetBlueprint<BlueprintBuff>("df6d1025da07524429afbae248845ecc");
            var FavoriteMetamagicSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("503fb196aa222b24cb6cfdc9a284e838");
            var Icon_FlaringSpellFeat = AssetLoader.LoadInternal(modContext: TTTContext, folder: "Feats", file: "Icon_FlaringSpellFeat.png");
            var Icon_FlaringSpellMetamagic = AssetLoader.LoadInternal(modContext: TTTContext, folder: "Metamagic", file: "Icon_FlaringSpellMetamagic.png", size: 128);
            var Icon_MetamagicRodFlaringLesser = AssetLoader.LoadInternal(modContext: TTTContext, folder: "Equipment", file: "Icon_MetamagicRodFlaringLesser.png", size: 64);
            var Icon_MetamagicRodFlaringNormal = AssetLoader.LoadInternal(modContext: TTTContext, folder: "Equipment", file: "Icon_MetamagicRodFlaringNormal.png", size: 64);
            var Icon_MetamagicRodFlaringGreater = AssetLoader.LoadInternal(modContext: TTTContext, folder: "Equipment", file: "Icon_MetamagicRodFlaringGreater.png", size: 64);

            var FlaringSpellFeat = Helpers.CreateBlueprint<BlueprintFeature>(modContext: TTTContext, "FlaringSpellFeat", bp => {
                bp.SetName("Metamagic (Flaring Spell)");
                bp.SetDescription("You dazzle creatures when you affect them with a spell that has the fire, light, or electricity descriptor.\n" +
                    "Benefit: The electricity, fire, or light effects of the affected spell create a flaring that " +
                    "dazzles creatures that take damage from the spell. A flaring spell causes a creature that " +
                    "takes fire or electricity damage from the affected spell to become dazzled for a number of " +
                    "rounds equal to the actual level of the spell.\n" +
                    "A flaring spell only affects spells with a fire, light, or electricity descriptor.\n" +
                    "Level Increase: +1 (a flaring spell uses up a spell slot one level higher than the spell’s actual level.)");
                bp.m_Icon = Icon_FlaringSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.WizardFeat };
                bp.AddComponent<AddMetamagicFeat>(c => {
                    c.Metamagic = (Metamagic)CustomMetamagic.Flaring;
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
            var FavoriteMetamagicFlaring = Helpers.CreateBlueprint<BlueprintFeature>(modContext: TTTContext, "FavoriteMetamagicFlaring", bp => {
                bp.SetName("Favorite Metamagic — Flaring");
                bp.m_Description = FavoriteMetamagicSelection.m_Description;
                //bp.m_Icon = Icon_IntensifiedSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { };
                bp.AddComponent<AddCustomMechanicsFeature>(c => {
                    c.Feature = CustomMechanicsFeature.FavoriteMetamagicFlaring;
                });
                bp.AddPrerequisiteFeature(FlaringSpellFeat);
            });
            var FlaringDazzledBuff = Helpers.CreateBuff(modContext: TTTContext, "FlaringDazzledBuff", bp => {
                bp.m_DisplayName = DazzledBuff.m_DisplayName;
                bp.m_Description = DazzledBuff.m_Description;
                bp.m_Icon = DazzledBuff.Icon;
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.ResourceAssetIds = new string[] { };
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    c.Descriptor = SpellDescriptor.Blindness | SpellDescriptor.SightBased;
                });
                bp.AddComponent<AddCondition>(c => {
                    c.Condition = UnitCondition.Dazzled;
                });
                bp.AddComponent<RemoveWhenCombatEnded>();
            });

            if (TTTContext.AddedContent.Feats.IsEnabled("MetamagicFlaringSpell")) {
                MetamagicExtention.RegisterMetamagic(
                    metamagic: (Metamagic)CustomMetamagic.Flaring,
                    name: "Flaring",
                    icon: Icon_FlaringSpellMetamagic,
                    defaultCost: 1,
                    favoriteMetamagic: CustomMechanicsFeature.FavoriteMetamagicFlaring
                );
            }
            var MetamagicRodsFlaring = ItemTools.CreateAllMetamagicRods(
                modContext: TTTContext, rodName: "Flaring Metamagic Rod",
                lesserIcon: Icon_MetamagicRodFlaringLesser,
                normalIcon: Icon_MetamagicRodFlaringNormal,
                greaterIcon: Icon_MetamagicRodFlaringGreater,
                metamagic: (Metamagic)CustomMetamagic.Flaring,
                rodDescriptionStart: "This rod grants its wielder the ability to make up to three spells they cast per day flaring, " +
                    "as though using the Flaring Spell feat.",
                metamagicDescription: "Flaring Spell: A flaring spell causes a creature that " +
                    "takes fire or electricity damage from the affected spell to become dazzled for a number of " +
                    "rounds equal to the actual level of the spell.\n" +
                    "A flaring spell only affects spells with a fire, light, or electricity descriptor."
            );

            if (TTTContext.AddedContent.Feats.IsDisabled("MetamagicFlaringSpell")) { return; }

            UpdateSpells();
            AddRodsToVenders();
            FeatTools.AddAsFeat(FlaringSpellFeat);
            FeatTools.AddAsMetamagicFeat(FlaringSpellFeat);
            FavoriteMetamagicSelection.AddFeatures(FavoriteMetamagicFlaring);
        }
        private static void UpdateSpells() {
            var spells = SpellTools.GetAllSpells();
            foreach (var spell in spells) {

                bool isFlaringSpell = spell.AbilityAndVariants()
                    .SelectMany(s => s.AbilityAndStickyTouch())
                    .Any(s => s.GetComponent<SpellDescriptorComponent>()?
                        .Descriptor.HasAnyFlag(SpellDescriptor.Fire | SpellDescriptor.Electricity) ?? false)
                    || spell.GetComponent<AbilityShadowSpell>();
                if (isFlaringSpell) {
                    if (!spell.AvailableMetamagic.HasMetamagic((Metamagic)CustomMetamagic.Flaring)) {
                        spell.AvailableMetamagic |= (Metamagic)CustomMetamagic.Flaring;
                        TTTContext.Logger.LogPatch("Enabled Flaring Metamagic", spell);
                    }
                };
            }
        }
        private static void AddRodsToVenders() {
            var Scroll_Chapter3VendorTable = Resources.GetBlueprint<BlueprintSharedVendorTable>("d33d4c7396fc1d74c9569bc38e887e86");
            var Scroll_Chapter5VendorTable = Resources.GetBlueprint<BlueprintSharedVendorTable>("5b73c93dccd743668734070160dfb82f");

            Scroll_Chapter3VendorTable.AddComponent<LootItemsPackFixed>(c => {
                c.m_Item = new LootItem() {
                    m_Item = Resources.GetModBlueprintReference<BlueprintItemReference>(modContext: TTTContext, "MetamagicRodNormalFlaring"),
                    m_Loot = new BlueprintUnitLootReference()
                };
                c.m_Count = 1;
            });
            Scroll_Chapter5VendorTable.AddComponent<LootItemsPackFixed>(c => {
                c.m_Item = new LootItem() {
                    m_Item = Resources.GetModBlueprintReference<BlueprintItemReference>(modContext: TTTContext, "MetamagicRodGreaterFlaring"),
                    m_Loot = new BlueprintUnitLootReference()
                };
                c.m_Count = 1;
            });
        }
    }
}
