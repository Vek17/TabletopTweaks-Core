using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.MythicReworks.Main;

namespace TabletopTweaks.MythicReworks.NewContent.Classes {
    static class Trickster {
        private static BlueprintGuid TricksterDomainMasterID = TTTContext.Blueprints.GetDerivedMaster("TricksterDomainMasterID");
        private static BlueprintGuid[] TricksterSpellResource = new BlueprintGuid[9] {
            TTTContext.Blueprints.GetDerivedMaster("TricksterSpellResource1"),
            TTTContext.Blueprints.GetDerivedMaster("TricksterSpellResource2"),
            TTTContext.Blueprints.GetDerivedMaster("TricksterSpellResource3"),
            TTTContext.Blueprints.GetDerivedMaster("TricksterSpellResource4"),
            TTTContext.Blueprints.GetDerivedMaster("TricksterSpellResource5"),
            TTTContext.Blueprints.GetDerivedMaster("TricksterSpellResource6"),
            TTTContext.Blueprints.GetDerivedMaster("TricksterSpellResource7"),
            TTTContext.Blueprints.GetDerivedMaster("TricksterSpellResource8"),
            TTTContext.Blueprints.GetDerivedMaster("TricksterSpellResource9")
        };
        public static void AddTricksterDomains() {
            var DomainsSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("48525e5da45c9c243a343fc6545dbdb9");
            DomainsSelection.AllFeatures
                .OfType<BlueprintProgression>()
                .ForEach(domain => GenerateTricksterDomain(TricksterDomainMasterID, domain));
        }
        private static BlueprintProgression GenerateTricksterDomain(BlueprintGuid masterID, BlueprintProgression domain) {
            return domain.CreateCopy(TTTContext, $"TricksterTTT{domain.name}", masterID, bp => {
                var SpellList = bp.GetComponent<LearnSpellList>()?.m_SpellList;
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[0];
                bp.m_Archetypes = new BlueprintProgression.ArchetypeWithLevel[0];
                bp.m_FeaturesRankIncrease = new List<BlueprintFeatureReference>();
                ResourcesLibrary.GetRoot()
                    .Progression
                    .CharacterMythics
                    .ForEach(mythic => bp.AddClass(mythic));
                bp.LevelEntries.ForEach(entry => {
                    if (entry.Level > 1) {
                        entry.Level /= 2;
                    }
                });
                bp.RemoveComponents<LearnSpellList>();
                bp.RemoveComponents<Prerequisite>();
                bp.AddComponent<AddSpellListAsAbilitiesTTT>(c => {
                    c.m_SpellList = SpellList;
                    c.m_ResourcePerSpellLevel = new BlueprintAbilityResourceReference[] {
                        CreateTricksterSpellResource(1, SpellList),
                        CreateTricksterSpellResource(2, SpellList),
                        CreateTricksterSpellResource(3, SpellList),
                        CreateTricksterSpellResource(4, SpellList),
                        CreateTricksterSpellResource(5, SpellList),
                        CreateTricksterSpellResource(6, SpellList),
                        CreateTricksterSpellResource(7, SpellList),
                        CreateTricksterSpellResource(8, SpellList),
                        CreateTricksterSpellResource(9, SpellList),
                    };
                });
                bp.AddComponent<RecalculateOnLevelUp>();
            });
        }
        private static BlueprintAbilityResourceReference CreateTricksterSpellResource(int spellLevel, BlueprintSpellList spellList) {
            return Helpers.CreateDerivedBlueprint<BlueprintAbilityResource>(
                modContext: TTTContext, 
                $"TricksterTTT{spellList.name}Resource{spellLevel}",
                TricksterSpellResource[spellLevel - 1],
                new SimpleBlueprint[] { spellList },
                bp => {
                    bp.m_MaxAmount = new BlueprintAbilityResource.Amount() {
                        m_Class = new BlueprintCharacterClassReference[0],
                        m_ClassDiv = new BlueprintCharacterClassReference[0],
                        m_Archetypes = new BlueprintArchetypeReference[0],
                        m_ArchetypesDiv = new BlueprintArchetypeReference[0],
                        BaseValue = 1,
                        IncreasedByLevel = false,
                        IncreasedByStat = false
                    };
                }
            ).ToReference<BlueprintAbilityResourceReference>();
        }
    }
}
