using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.EntitySystem;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Utility;
using HarmonyLib;

namespace TabletopTweaks.Core.NewUnitParts {
    public class UnitPartFakeClassLevels : OldStyleUnitPart {
        public void AddPrerequisiteEntry(EntityFact source, 
            BlueprintCharacterClassReference actualClass, 
            BlueprintCharacterClassReference fakeClass, 
            double modifier,
            int summand,
            FeatureGroup[] checkedGroups
        ) {
            var entry = new FakeClassPrerequisiteEntry(
                source,
                actualClass,
                fakeClass,
                modifier,
                summand,
                checkedGroups
            );
            FakeClassPrerequisites.Add(entry);
        }
        public void RemovePrerequisiteEntry(EntityFact source) {
            FakeClassPrerequisites.RemoveAll((FakeClassPrerequisiteEntry c) => c.Source == source);
            TryRemove();
        }
        private void TryRemove() {
            if (!FakeClassPrerequisites.Any()) { this.RemoveSelf(); }
        }

        private int GetBonusLevels(PrerequisiteClassLevel prerequisite, UnitDescriptor unit) {
            if (!FakeClassPrerequisites.Any()) { return 0; }
            return FakeClassPrerequisites.Sum(entry => entry.GetBonusLevels(prerequisite, unit));
        }

        private int GetBonusLevels(PrerequisiteArchetypeLevel prerequisite, UnitDescriptor unit) {
            if (!FakeClassPrerequisites.Any()) { return 0; }
            return FakeClassPrerequisites.Sum(entry => entry.GetBonusLevels(prerequisite, unit));
        }

        private readonly List<FakeClassPrerequisiteEntry> FakeClassPrerequisites = new();
        public class FakeClassPrerequisiteEntry {
            public BlueprintCharacterClass ActualClass => m_ActualClass?.Get();
            public BlueprintCharacterClass FakeClass => m_FakeClass?.Get();
            public readonly EntityFactRef<EntityFact> Source;

            public  FakeClassPrerequisiteEntry(
                EntityFact source,
                BlueprintCharacterClassReference fakeClass,
                BlueprintCharacterClassReference actualClass,
                double modifier,
                int summand,
                FeatureGroup[] checkedGroups
            ) {
                this.Source = source;
                this.m_ActualClass = actualClass;
                this.m_FakeClass = fakeClass;
                this.Modifier = modifier;
                this.Summand = summand;
                this.CheckedGroups = checkedGroups ?? new FeatureGroup[0];
            }

            public int GetBonusLevels(PrerequisiteClassLevel prerequisite, UnitDescriptor unit) {
                var feature = prerequisite.OwnerBlueprint as BlueprintFeature;

                if (feature == null) { return 0; }
                if (prerequisite.CharacterClass != FakeClass) { return 0; } 
                if (CheckedGroups.Length > 0 && !CheckedGroups.Any(g => feature.HasGroup(g))) { return 0; }

                return (int)(this.Modifier * unit.Progression.GetClassLevel(this.ActualClass)) + this.Summand;
            }
            public int GetBonusLevels(PrerequisiteArchetypeLevel prerequisite, UnitDescriptor unit) {
                var feature = prerequisite.OwnerBlueprint as BlueprintFeature;

                if (feature == null) { return 0; }
                if (prerequisite.CharacterClass != FakeClass) { return 0; }
                if (!unit.Progression.GetClassData(prerequisite.CharacterClass)?.Archetypes.Contains(prerequisite.Archetype) ?? true) { return 0; }
                if (CheckedGroups.Length > 0 && !CheckedGroups.Any(g => feature.HasGroup(g))) { return 0; }

                return (int)(this.Modifier * unit.Progression.GetClassLevel(this.ActualClass)) + this.Summand;
            }

            private readonly BlueprintCharacterClassReference m_FakeClass;
            private readonly BlueprintCharacterClassReference m_ActualClass;
            private readonly double Modifier;
            private readonly int Summand;
            private readonly FeatureGroup[] CheckedGroups = new FeatureGroup[0];
        }

        [HarmonyPatch(typeof(PrerequisiteArchetypeLevel), nameof(PrerequisiteArchetypeLevel.GetArchetypeLevel))]
        static class PrerequisiteArchetypeLevel_GetArchetypeLevel_Patch {
            static void Postfix(PrerequisiteArchetypeLevel __instance, UnitDescriptor unit, ref int? __result) {
                var fakeClassLevelPart = unit.Get<UnitPartFakeClassLevels>();
                if (fakeClassLevelPart == null) { return; }
                var bonus = fakeClassLevelPart.GetBonusLevels(__instance, unit);
                if (__result == null && bonus == 0) { return; }
                if (__result == null) {
                    __result = bonus;
                } else {
                    __result = __result + bonus;
                }
            }
        }
        [HarmonyPatch(typeof(PrerequisiteClassLevel), nameof(PrerequisiteClassLevel.GetClassLevel))]
        static class PrerequisiteClassLevel_GetArchetypeLevel_Patch {
            static void Postfix(PrerequisiteClassLevel __instance, UnitDescriptor unit, ref int? __result) {
                var fakeClassLevelPart = unit.Get<UnitPartFakeClassLevels>();
                if (fakeClassLevelPart == null) { return; }
                var bonus = fakeClassLevelPart.GetBonusLevels(__instance, unit);
                if (__result == null && bonus == 0) { return; }
                if (__result == null) {
                    __result = bonus;
                } else {
                    __result = __result + bonus;
                }
            }
        }
    }
}