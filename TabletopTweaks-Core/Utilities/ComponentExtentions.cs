using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System;
using System.Data;
using System.Linq;

namespace TabletopTweaks.Core.Utilities {
    public static class ComponentExtentions {
        /// <summary>
        /// Gets the first SelectionEntry that matches the supplied Predicate.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="predicate">
        /// Predicate to determine which SelectionEntry to get.
        /// </param>
        /// <returns>
        /// The first selection entry that matches the supplied Predicate.
        /// </returns>
        public static SelectionEntry GetSelection(this AddClassLevels obj, Predicate<SelectionEntry> predicate) {
            return obj.Selections.Where(c => predicate(c)).FirstOrDefault();
        }
        /// <summary>
        /// Checks if the LevelEntry or any facts that the LevelEntry adds a fact with the supplied Id.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="id">
        /// Id to check for.
        /// </param>
        /// <returns>
        /// true if fact is found that matchs the supplied id.
        /// </returns>
        public static bool HasFeatureWithId(this LevelEntry level, BlueprintGuid id) {
            return level.Features.Any(f => HasFeatureWithId(f, id));
        }
        /// <summary>
        /// Checks if the fact or any facts that the fact adds a fact with the supplied Id.
        /// </summary>
        /// <param name="fact"></param>
        /// <param name="id">
        /// Id to check for.
        /// </param>
        /// <returns>
        /// true if fact is found that matchs the supplied id.
        /// </returns>
        public static bool HasFeatureWithId(this BlueprintUnitFact fact, BlueprintGuid id) {
            if (fact.AssetGuid == id) return true;
            foreach (var c in fact.ComponentsArray) {
                var addFacts = c as AddFacts;
                if (addFacts != null) return addFacts.Facts.Any(f => HasFeatureWithId(f, id));
            }
            return false;
        }
        /// <summary>
        /// Checks if the BlueprintAbility has an area of effect.
        /// </summary>
        /// <param name="spell"></param>
        /// <returns>
        /// true if BlueprintAbility has an area of effect.
        /// </returns>
        public static bool HasAreaEffect(this BlueprintAbility spell) {
            return spell.AoERadius.Meters > 0f || spell.ProjectileType != AbilityProjectileType.Simple;
        }
        /// <summary>
        /// Adds a new action to the end of the actionlist of the AbilityEffectRunAction.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="action">
        /// Action to be added.
        /// </param>
        public static void AddAction(this AbilityEffectRunAction component, GameAction action) {
            if (component.Actions != null) {
                component.Actions = Helpers.CreateActionList(component.Actions.Actions);
                component.Actions.Actions = component.Actions.Actions.AppendToArray(action);
            } else {
                component.Actions = Helpers.CreateActionList(action);
            }
        }
        /// <summary>
        /// Creates a new action and intilizes it with the supplied initializer and adds it to the end of the actionlist of the AbilityEffectRunAction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <param name="init">
        /// Initializer action to setup the newly created action.
        /// </param>
        public static void AddAction<T>(this AbilityEffectRunAction component, Action<T> init = null) where T : GameAction, new() {
            T action = Helpers.Create(init);
            component.AddAction(action);
        }
    }
}
