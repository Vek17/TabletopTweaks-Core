using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.ElementsSystem;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TabletopTweaks.Core.Config;
using static Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite;

namespace TabletopTweaks.Core.Utilities {
    /// <summary>
    /// Collection of extentions for interacting with blueprints.
    /// </summary>
    public static class BlueprintExtentions {
        /// <summary>
        /// Creates a deep copy of the blueprint and then runs the supplied action against it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <param name="name">
        /// New unitiy name to give the copy. This also will define the GUID.
        /// </param>
        /// <param name="init">
        /// Action to run against the created blueprint.
        /// </param>
        /// <returns></returns>
        public static T CreateCopy<T>(this T original, string name, Action<T> init = null) where T : SimpleBlueprint {
            var result = (T)Helpers.ObjectDeepCopier.Clone(original);
            result.TemporaryContext(bp => {
                bp.name = name;
                bp.AssetGuid = ModSettings.Blueprints.GetGUID(name);
            });
            Resources.AddBlueprint(result);
            init?.Invoke(result);
            return result;
        }
        /// <summary>
        /// Creates a deep copy of the blueprint and then runs the supplied action against it. Generates 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <param name="name">
        /// New unitiy name to give the copy. This also will define the GUID.
        /// </param>
        /// <param name="masterId"></param>
        /// <param name="init">
        /// Action to run against the created blueprint.
        /// </param>
        /// <returns></returns>
        public static T CreateCopy<T>(this T original, string name, BlueprintGuid masterId, Action<T> init = null) where T : SimpleBlueprint {
            var result = (T)Helpers.ObjectDeepCopier.Clone(original);
            result.TemporaryContext(bp => {
                bp.name = name;
                bp.AssetGuid = ModSettings.Blueprints.GetDerivedGUID(name, masterId, original.AssetGuid);
            });
            Resources.AddBlueprint(result);
            init?.Invoke(result);
            return result;
        }
        /// <summary>
        /// Seaches the objects component array and flattens all actions referenced into one sequence.
        /// </summary>
        /// <param name="blueprint">
        /// Blueprint to be searched.
        /// </param>
        /// <returns>
        /// An IEnumerable that contains all actions in the component array of the object.
        /// </returns>
        public static IEnumerable<GameAction> FlattenAllActions(this BlueprintScriptableObject blueprint) {
            List<GameAction> actions = new List<GameAction>();
            foreach (var component in blueprint.ComponentsArray) {
                Type type = component.GetType();
                var foundActions = AccessTools.GetDeclaredFields(type)
                    .Where(f => f.FieldType == typeof(ActionList))
                    .SelectMany(field => ((ActionList)field.GetValue(component)).Actions);
                actions.AddRange(FlattenAllActions(foundActions));
            }
            return actions;
        }
        /// <summary>
        /// Seaches the supplied actions for more nested action lists and flattens the results into one sequence. 
        /// This is done recursivly until all action lists are found.
        /// </summary>
        /// <param name="actions">
        /// IEnumerable of GameActions to seach through.
        /// </param>
        /// <returns>
        /// An IEnumerable that contains all actions including actions nested in other actions.
        /// </returns>
        private static IEnumerable<GameAction> FlattenAllActions(this IEnumerable<GameAction> actions) {
            List<GameAction> newActions = new List<GameAction>();
            foreach (var action in actions) {
                Type type = action?.GetType();
                var foundActions = AccessTools.GetDeclaredFields(type)?
                    .Where(f => f?.FieldType == typeof(ActionList))
                    .SelectMany(field => ((ActionList)field.GetValue(action)).Actions);
                if (foundActions != null) { newActions.AddRange(foundActions); }
            }
            if (newActions.Count > 0) {
                return actions.Concat(FlattenAllActions(newActions));
            }
            return actions;
        }
        /// <summary>
        /// Checks a BlueprintAbility for any variants and returns both the original and all variants.
        /// </summary>
        /// <param name="ability">
        /// BlueprintAbility to be checked for variants.
        /// </param>
        /// <returns>
        /// An IEnumerable containing the original BlueprintAbility and any variants it has.
        /// </returns>
        public static IEnumerable<BlueprintAbility> AbilityAndVariants(this BlueprintAbility ability) {
            var List = new List<BlueprintAbility>() { ability };
            var varriants = ability.GetComponent<AbilityVariants>();
            if (varriants != null) {
                List.AddRange(varriants.Variants);
            }
            return List;
        }
        /// <summary>
        /// Checks a BlueprintAbility for any sticky touch abilities and returns both the original and the sticky touch ability if found.
        /// </summary>
        /// <param name="ability">
        /// BlueprintAbility to be checked for sticky touch..
        /// </param>
        /// <returns>
        /// An IEnumerable containing the original BlueprintAbility and any sticky touch BlueprintAbilities it has.
        /// </returns>
        public static IEnumerable<BlueprintAbility> AbilityAndStickyTouch(this BlueprintAbility ability) {
            var List = new List<BlueprintAbility>() { ability };
            var stickyTouch = ability.GetComponent<AbilityEffectStickyTouch>();
            if (stickyTouch != null) {
                List.Add(stickyTouch.m_TouchDeliveryAbility);
            }
            return List;
        }
        /// <summary>
        /// Adds class to the progression as a normal progression class.
        /// </summary>
        /// <param name="progression"></param>
        /// <param name="characterClass">
        /// Class to add to progression.
        /// </param>
        public static void AddClass(this BlueprintProgression progression, BlueprintCharacterClass characterClass) {
            progression.AddClass(characterClass.ToReference<BlueprintCharacterClassReference>());
        }
        /// <summary>
        /// Adds class to the progression as a normal progression class.
        /// </summary>
        /// <param name="progression"></param>
        /// <param name="characterClass">
        /// Class to add to progression.
        /// </param>
        public static void AddClass(this BlueprintProgression progression, BlueprintCharacterClassReference characterClass) {
            if (progression.m_Classes.Any(a => a.m_Class == characterClass)) { return; }
            progression.m_Classes = progression.m_Classes.AppendToArray(
                new BlueprintProgression.ClassWithLevel() {
                    m_Class = characterClass,
                });
        }
        /// <summary>
        /// Adds archetype to the progression as a normal progression archetype.
        /// </summary>
        /// <param name="progression"></param>
        /// <param name="archetype">
        /// Archetype to add to progression.
        /// </param>
        public static void AddArchetype(this BlueprintProgression progression, BlueprintArchetype archetype) {
            progression.AddArchetype(archetype.ToReference<BlueprintArchetypeReference>());
        }
        /// <summary>
        /// Adds archetype to the progression as a normal progression archetype.
        /// </summary>
        /// <param name="progression"></param>
        /// <param name="archetype">
        /// Archetype to add to progression.
        /// </param>
        public static void AddArchetype(this BlueprintProgression progression, BlueprintArchetypeReference archetype) {
            if (progression.m_Archetypes.Any(a => a.m_Archetype == archetype)) { return; }
            progression.m_Archetypes = progression.m_Archetypes.AppendToArray(
                new BlueprintProgression.ArchetypeWithLevel() {
                    m_Archetype = archetype,
                });
        }
        /// <summary>
        /// Sets the features of the BlueprintFeatureSelection to equal the specified features.
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="features">
        /// Features to set as the features of the BlueprintFeatureSelection.
        /// </param>
        public static void SetFeatures(this BlueprintFeatureSelection selection, IEnumerable<BlueprintFeature> features) {
            selection.SetFeatures(features.ToArray());
        }
        /// <summary>
        /// Sets the features of the BlueprintFeatureSelection to equal the specified features.
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="features">
        /// Features to set as the features of the BlueprintFeatureSelection.
        /// </param>
        public static void SetFeatures(this BlueprintFeatureSelection selection, params BlueprintFeature[] features) {
            selection.SetFeatures(features.Select(f => f.ToReference<BlueprintFeatureReference>()).ToArray());
        }
        /// <summary>
        /// Sets the features of the BlueprintFeatureSelection to equal the specified features.
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="features">
        /// Features to set as the features of the BlueprintFeatureSelection.
        /// </param>
        public static void SetFeatures(this BlueprintFeatureSelection selection, IEnumerable<BlueprintFeatureReference> features) {
            selection.SetFeatures(features.ToArray());
        }
        /// <summary>
        /// Sets the features of the BlueprintFeatureSelection to equal the specified features.
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="features">
        /// Features to set as the features of the BlueprintFeatureSelection.
        /// </param>
        public static void SetFeatures(this BlueprintFeatureSelection selection, params BlueprintFeatureReference[] features) {
            selection.m_AllFeatures = selection.m_Features = features.Select(bp => bp).ToArray();
        }
        /// <summary>
        /// Removes the specified features from the BlueprintFeatureSelection.
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="features">
        /// Features to remove.
        /// </param>
        public static void RemoveFeatures(this BlueprintFeatureSelection selection, params BlueprintFeature[] features) {
            selection.RemoveFeatures(features.Select(f => f.ToReference<BlueprintFeatureReference>()).ToArray());
        }
        /// <summary>
        /// Removes the specified features from the BlueprintFeatureSelection.
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="features">
        /// Features to remove.
        /// </param>
        public static void RemoveFeatures(this BlueprintFeatureSelection selection, params BlueprintFeatureReference[] features) {
            foreach (var feature in features) {
                if (selection.m_AllFeatures.Contains(feature)) {
                    selection.m_AllFeatures = selection.m_AllFeatures.Where(f => !f.Equals(feature)).ToArray();
                }
                if (selection.m_Features.Contains(feature)) {
                    selection.m_Features = selection.m_Features.Where(f => !f.Equals(feature)).ToArray();
                }
            }
            selection.m_AllFeatures = selection.m_AllFeatures.OrderBy(feature => feature.Get().Name).ToArray();
            selection.m_Features = selection.m_Features.OrderBy(feature => feature.Get().Name).ToArray();
        }
        /// <summary>
        /// Adds the specified features to the BlueprintFeatureSelection.
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="features">
        /// Features to add.
        /// </param>
        public static void AddFeatures(this BlueprintFeatureSelection selection, params BlueprintFeature[] features) {
            selection.AddFeatures(features.Select(f => f.ToReference<BlueprintFeatureReference>()).ToArray());
        }
        /// <summary>
        /// Adds the specified features to the BlueprintFeatureSelection.
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="features">
        /// Features to add.
        /// </param>
        public static void AddFeatures(this BlueprintFeatureSelection selection, params BlueprintFeatureReference[] features) {
            foreach (var feature in features) {
                if (!selection.m_AllFeatures.Contains(feature)) {
                    selection.m_AllFeatures = selection.m_AllFeatures.AppendToArray(feature);
                }
                if (!selection.m_Features.Contains(feature)) {
                    selection.m_Features = selection.m_Features.AppendToArray(feature);
                }
            }
            selection.m_AllFeatures = selection.m_AllFeatures.OrderBy(feature => feature.Get().Name).ToArray();
            selection.m_Features = selection.m_Features.OrderBy(feature => feature.Get().Name).ToArray();
        }
        /// <summary>
        /// Adds feature as a PrerequisiteFeature and updates that feature's IsPrerequisiteFor list to correctly reflect that.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="feature">
        /// Feature to be added as a prerequisite.
        /// </param>
        /// <param name="group">
        /// Group to put the Prerequisite in. Defaults to All.
        /// </param>
        public static void AddPrerequisiteFeature(this BlueprintFeature obj, BlueprintFeature feature, GroupType group = GroupType.All) {
            obj.AddComponent(Helpers.Create<PrerequisiteFeature>(c => {
                c.m_Feature = feature.ToReference<BlueprintFeatureReference>();
                c.Group = group;
            }));
            if (feature.IsPrerequisiteFor == null) { feature.IsPrerequisiteFor = new List<BlueprintFeatureReference>(); }
            if (!feature.IsPrerequisiteFor.Contains(obj.ToReference<BlueprintFeatureReference>())) {
                feature.IsPrerequisiteFor.Add(obj.ToReference<BlueprintFeatureReference>());
            }
        }
        /// <summary>
        /// Adds features in a single PrerequisiteFeaturesFromList and updates those feature's IsPrerequisiteFor list to correctly reflect that. 
        /// PrerequisiteFeaturesFromList group is All.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="features">
        /// Features to be added to the PrerequisiteFeaturesFromList.
        /// </param>
        /// </param>
        public static void AddPrerequisiteFeaturesFromList(this BlueprintFeature obj, int amount, params BlueprintFeature[] features) {
            obj.AddPrerequisiteFeaturesFromList(amount, GroupType.All, features);
        }
        /// <summary>
        /// Adds features in a single PrerequisiteFeaturesFromList and updates those feature's IsPrerequisiteFor list to correctly reflect that.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="features">
        /// Features to be added to the PrerequisiteFeaturesFromList.
        /// </param>
        /// <param name="group">
        /// Group to put the Prerequisite in. Defaults to All.
        /// </param>
        public static void AddPrerequisiteFeaturesFromList(this BlueprintFeature obj, int amount, GroupType group = GroupType.All, params BlueprintFeature[] features) {
            obj.AddComponent(Helpers.Create<PrerequisiteFeaturesFromList>(c => {
                c.m_Features = features.Select(f => f.ToReference<BlueprintFeatureReference>()).ToArray();
                c.Amount = amount;
                c.Group = group;
            }));
            features.ForEach(feature => {
                if (feature.IsPrerequisiteFor == null) { feature.IsPrerequisiteFor = new List<BlueprintFeatureReference>(); }
                if (!feature.IsPrerequisiteFor.Contains(obj.ToReference<BlueprintFeatureReference>())) {
                    feature.IsPrerequisiteFor.Add(obj.ToReference<BlueprintFeatureReference>());
                }
            });
        }
        /// <summary>
        /// Adds a new Prerequisite to the feature and runs any applicable special case logic for its type. 
        /// </summary>
        /// <typeparam name="T">
        /// Type of prerequisite to add.
        /// </typeparam>
        /// <param name="obj"></param>
        /// <param name="init">
        /// Action to initialize the created prerequisite with.
        /// </param>
        public static void AddPrerequisite<T>(this BlueprintFeature obj, Action<T> init = null) where T : Prerequisite, new() {
            obj.AddPrerequisites(Helpers.Create(init));
        }
        /// <summary>
        /// Adds new Prerequisites to the feature and runs any applicable special case logic for thier types. 
        /// </summary>
        /// <typeparam name="T">
        /// Type of prerequisite to add.
        /// </typeparam>
        /// <param name="obj"></param>
        /// <param name="prerequisites">
        /// Prerequisites to add to the feature.
        /// </param>
        public static void AddPrerequisites<T>(this BlueprintFeature obj, params T[] prerequisites) where T : Prerequisite {
            foreach (var prerequisite in prerequisites) {
                obj.AddComponent(prerequisite);
                switch (prerequisite) {
                    case PrerequisiteFeature p:
                        var feature = p.Feature;
                        if (feature.IsPrerequisiteFor == null) { feature.IsPrerequisiteFor = new List<BlueprintFeatureReference>(); }
                        if (!feature.IsPrerequisiteFor.Contains(obj.ToReference<BlueprintFeatureReference>())) {
                            feature.IsPrerequisiteFor.Add(obj.ToReference<BlueprintFeatureReference>());
                        }
                        break;
                    case PrerequisiteFeaturesFromList p:
                        var features = p.Features;
                        features.ForEach(f => {
                            if (f.IsPrerequisiteFor == null) { f.IsPrerequisiteFor = new List<BlueprintFeatureReference>(); }
                            if (!f.IsPrerequisiteFor.Contains(obj.ToReference<BlueprintFeatureReference>())) {
                                f.IsPrerequisiteFor.Add(obj.ToReference<BlueprintFeatureReference>());
                            }
                        });
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// Removes Prerequisites from the feature and runs any applicable special case logic for thier types. 
        /// </summary>
        /// <typeparam name="T">
        /// Type of prerequisite to remove.
        /// </typeparam>
        /// <param name="obj"></param>
        /// <param name="prerequisites">
        /// Prerequisites to remove from the feature.
        /// </param>
        public static void RemovePrerequisites<T>(this BlueprintFeature obj, params T[] prerequisites) where T : Prerequisite {
            foreach (var prerequisite in prerequisites) {
                obj.RemoveComponent(prerequisite);
                switch (prerequisite) {
                    case PrerequisiteFeature p:
                        var feature = p.Feature;
                        if (feature.IsPrerequisiteFor == null) { feature.IsPrerequisiteFor = new List<BlueprintFeatureReference>(); }
                        break;
                    case PrerequisiteFeaturesFromList p:
                        var features = p.Features;
                        features.ForEach(f => {
                            if (f.IsPrerequisiteFor == null) { f.IsPrerequisiteFor = new List<BlueprintFeatureReference>(); }
                            f.IsPrerequisiteFor.RemoveAll(v => v.Guid == obj.AssetGuid);
                        });
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// Removes Prerequisites from the feature and runs any applicable special case logic for thier types. 
        /// </summary>
        /// <typeparam name="T">
        /// Type of prerequisite to remove.
        /// </typeparam>
        /// <param name="obj"></param>
        /// <param name="predicate">
        /// Predicate that matches the Prerequisites to remove.
        /// </param>
        public static void RemovePrerequisites<T>(this BlueprintFeature obj, Predicate<T> predicate) where T : Prerequisite {
            foreach (var prerequisite in obj.GetComponents<T>()) {
                if (predicate(prerequisite)) {
                    obj.RemovePrerequisites(prerequisite);
                }
            }
        }
        /// <summary>
        /// Removes all Prerequisites from the feature and runs any applicable special case logic for thier types. 
        /// </summary>
        /// <typeparam name="T">
        /// Type of prerequisite to remove.
        /// </typeparam>
        /// <param name="obj"></param>
        public static void RemovePrerequisites<T>(this BlueprintFeature obj) where T : Prerequisite {
            foreach (var prerequisite in obj.GetComponents<T>()) {
                obj.RemovePrerequisites(prerequisite);
            }
        }
        /// <summary>
        /// Adds component at specified index in this object's ComponentsArray.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="index">
        /// Index to place component.
        /// </param>
        /// <param name="component">
        /// Component to insert.
        /// </param>
        public static void InsertComponent(this BlueprintScriptableObject obj, int index, BlueprintComponent component) {
            var components = obj.ComponentsArray.ToList();
            components.Insert(index, component);
            obj.SetComponents(components.ToArray());
        }
        /// <summary>
        /// Adds new ContextRankConfig to the object and initalizes it with the supplied action.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="init">
        /// Action to initialize new ContextRankConfig.
        /// </param>
        public static void AddContextRankConfig(this BlueprintScriptableObject obj, Action<ContextRankConfig> init = null) {
            obj.AddComponent(Helpers.CreateContextRankConfig(init));
        }
        /// <summary>
        /// Adds new component to the object's ComponentsArray and initalizes it with the supplied action.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="component">
        /// Components to add.
        /// </param>
        public static void AddComponent(this BlueprintScriptableObject obj, BlueprintComponent component) {
            obj.SetComponents(obj.ComponentsArray.AppendToArray(component));
        }
        /// <summary>
        /// Adds new component to the object's ComponentsArray and initalizes it with the supplied action.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="init">
        /// Action to initialize new Component.
        /// </param>
        public static void AddComponent<T>(this BlueprintScriptableObject obj, Action<T> init = null) where T : BlueprintComponent, new() {
            obj.SetComponents(obj.ComponentsArray.AppendToArray(Helpers.Create(init)));
        }
        /// <summary>
        /// Adds new components to the object's ComponentsArray.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="components">
        /// Components to add.
        /// </param>
        public static void AddComponents(this BlueprintScriptableObject obj, IEnumerable<BlueprintComponent> components) => AddComponents(obj, components.ToArray());
        /// <summary>
        /// Adds new components to the object's ComponentsArray.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="components">
        /// Components to add.
        /// </param>
        public static void AddComponents(this BlueprintScriptableObject obj, params BlueprintComponent[] components) {
            var c = obj.ComponentsArray.ToList();
            c.AddRange(components);
            obj.SetComponents(c.ToArray());
        }
        /// <summary>
        /// Removes specified BlueprintComponent from the object's ComponentsArray.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="component">
        /// BlueprintComponent to remove.
        /// </param>
        public static void RemoveComponent(this BlueprintScriptableObject obj, BlueprintComponent component) {
            obj.SetComponents(obj.ComponentsArray.RemoveFromArray(component));
        }
        /// <summary>
        /// Removes BlueprintComponents of the specified type from the object's ComponentsArray.
        /// </summary>
        /// <typeparam name="T">
        /// Type of BlueprintComponent to remove.
        /// </typeparam>
        /// <param name="obj"></param>
        public static void RemoveComponents<T>(this BlueprintScriptableObject obj) where T : BlueprintComponent {
            var compnents_to_remove = obj.GetComponents<T>().ToArray();
            foreach (var c in compnents_to_remove) {
                obj.SetComponents(obj.ComponentsArray.RemoveFromArray(c));
            }
        }
        /// <summary>
        /// Removes BlueprintComponents that match the supplied Predicate from the object's ComponentsArray.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="predicate">
        /// Predicate to determine which components to remove.
        /// </param>
        public static void RemoveComponents<T>(this BlueprintScriptableObject obj, Predicate<T> predicate) where T : BlueprintComponent {
            var compnents_to_remove = obj.GetComponents<T>().ToArray();
            foreach (var c in compnents_to_remove) {
                if (predicate(c)) {
                    obj.SetComponents(obj.ComponentsArray.RemoveFromArray(c));
                }
            }
        }
        /// <summary>
        /// Gets the first component that matches the supplied Predicate from the object's ComponentsArray.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="predicate">
        /// Predicate to determine which component to get.
        /// </param>
        /// <returns>
        /// First BlueprintComponent that matches the supplied predicate.
        /// </returns>
        public static T GetComponent<T>(this BlueprintScriptableObject obj, Predicate<T> predicate) where T : BlueprintComponent {
            return obj.GetComponents<T>().Where(c => predicate(c)).FirstOrDefault();
        }
        /// <summary>
        /// Gets all components that match the supplied Predicate from the object's ComponentsArray.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="predicate">
        /// Predicate to determine which components to get.
        /// </param>
        /// <returns>
        /// IEnumerable that contains all components that match the supplied predicate.
        /// </returns>
        public static IEnumerable<T> GetComponents<T>(this BlueprintScriptableObject obj, Predicate<T> predicate) where T : BlueprintComponent {
            return obj.GetComponents<T>().Where(c => predicate(c)).ToArray();
        }
        /// <summary>
        /// Removes all components from the object's ComponentsArray that match the supplied predicate, 
        /// and if a component is removed in this way add a new component to the object's ComponentsArray.
        /// </summary>
        /// <param name="blueprint"></param>
        /// <param name="predicate">
        /// Predicate to determine which components to remove.
        /// </param>
        /// <param name="newComponent">
        /// Component to add if a component is removed.
        /// </param>
        public static void ReplaceComponents(this BlueprintScriptableObject blueprint, Predicate<BlueprintComponent> predicate, BlueprintComponent newComponent) {
            bool found = false;
            foreach (var component in blueprint.ComponentsArray) {
                if (predicate(component)) {
                    blueprint.RemoveComponent(component);
                    found = true;
                }
            }
            if (found) {
                blueprint.AddComponent(newComponent);
            }
        }
        /// <summary>
        /// Removes all components from the object's ComponentsArray that match the supplied type, 
        /// and if a component is removed in this way add a new component to the object's ComponentsArray.
        /// </summary>
        /// <typeparam name="T">
        /// Type of BlueprintComponent to remove from the object's ComponentsArray.
        /// </typeparam>
        /// <param name="blueprint"></param>
        /// <param name="newComponent">
        /// Component to add if a component is removed.
        /// </param>
        public static void ReplaceComponents<T>(this BlueprintScriptableObject blueprint, BlueprintComponent newComponent) where T : BlueprintComponent {
            blueprint.ReplaceComponents<T>(c => true, newComponent);
        }
        /// <summary>
        /// Removes all components from the object's ComponentsArray that match the supplied type and Predicate, 
        /// and if a component is removed in this way add a new component to the object's ComponentsArray.
        /// </summary>
        /// <typeparam name="T">
        /// Type of BlueprintComponent to remove from the object's ComponentsArray.
        /// </typeparam>
        /// <param name="blueprint"></param>
        /// /// <param name="predicate">
        /// Predicate to determine which components to remove.
        /// </param>
        /// <param name="newComponent">
        /// Component to add if a component is removed.
        /// </param>
        public static void ReplaceComponents<T>(this BlueprintScriptableObject blueprint, Predicate<T> predicate, BlueprintComponent newComponent) where T : BlueprintComponent {
            var components = blueprint.GetComponents<T>().ToArray();
            bool found = false;
            foreach (var component in components) {
                if (predicate(component)) {
                    blueprint.RemoveComponent(component);
                    found = true;
                }
            }
            if (found) {
                blueprint.AddComponent(newComponent);
            }
        }
        /// <summary>
        /// Overrides the object's existing ComponentsArray with a new ComponentsArray filled with the supplied components.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="components">
        /// Components to set as the new ComponentsArray.
        /// </param>
        public static void SetComponents(this BlueprintScriptableObject obj, params BlueprintComponent[] components) {
            // Fix names of components. Generally this doesn't matter, but if they have serialization state,
            // then their name needs to be unique.
            var names = new HashSet<string>();
            foreach (var c in components) {
                if (string.IsNullOrEmpty(c.name)) {
                    c.name = $"${c.GetType().Name}";
                }
                if (!names.Add(c.name)) {
                    String name;
                    for (int i = 0; !names.Add(name = $"{c.name}${i}"); i++) ;
                    c.name = name;
                }
            }
            obj.ComponentsArray = components;
            obj.OnEnable(); // To make sure components are fully initialized
        }
        /// <summary>
        /// Overrides the object's existing ComponentsArray with a new ComponentsArray filled with the supplied components.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="components">
        /// Components to set as the new ComponentsArray.
        /// </param>
        public static void SetComponents(this BlueprintScriptableObject obj, IEnumerable<BlueprintComponent> components) {
            SetComponents(obj, components.ToArray());
        }
    }
}
