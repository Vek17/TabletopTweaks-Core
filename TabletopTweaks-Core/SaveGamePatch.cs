using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.ModLogic;

namespace TabletopTweaks.Core {
    public static class SaveGameFix {
        static private List<Action<UnitEntityData>> save_game_actions = new List<Action<UnitEntityData>>();

        public static void AddUnitPatch(Action<UnitEntityData> patch) {
            save_game_actions.Add(patch);
        }

        public static void AddRetroactiveClassFeature(ModContextBase context, BlueprintCharacterClass characterClass, int level, BlueprintUnitFact fact) {
            AddUnitPatch((unit) => {
                if (unit.Progression.GetClassLevel(characterClass) >= level) {
                    if (!unit.HasFact(fact)) {
                        if (unit.AddFact(fact) != null) {
                            context.Logger.Log($"SaveFix: Added: {fact.name} To: {unit.CharacterName} At: {characterClass.Name} Level: {level}");
                            return;
                        }
                        context.Logger.Log($"SaveFix: Failed: {fact.name} To: {unit.CharacterName} At: {characterClass.Name} Level: {level}");
                    }
                }
            });
        }

        public static void AddRetroactiveClassFeature(ModContextBase context, BlueprintCharacterClass characterClass, BlueprintArchetype archetype, int level, BlueprintUnitFact fact) {
            AddUnitPatch((unit) => {
                if (unit.Progression.GetClassLevel(characterClass) >= level && !unit.Progression.GetClassData(characterClass).Archetypes
                    .Any(achetype => achetype.AssetGuid == archetype.AssetGuid)) {
                    if (!unit.HasFact(fact)) {
                        if (unit.AddFact(fact) != null) {
                            context.Logger.Log($"SaveFix: Added: {fact.name} To: {unit.CharacterName} At: {characterClass.Name} - {archetype.Name} Level: {level}");
                            return;
                        }
                        context.Logger.Log($"SaveFix: Failed: {fact.name} To: {unit.CharacterName} At: {characterClass.Name} - {archetype.Name} Level: {level}");
                    }
                }
            });
        }

        [HarmonyPatch(typeof(UnitEntityData), "OnAreaDidLoad")]
        internal class UnitDescriptor__PostLoad__Patch {
            static void Postfix(UnitEntityData __instance) {
                foreach (var action in SaveGameFix.save_game_actions) {
                    action(__instance);
                }
            }
        }
    }
}
