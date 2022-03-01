// Copyright (c) 2019 Jennifer Messerly
// This code is licensed under MIT license (see LICENSE for details)

using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.Localization.Shared;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TabletopTweaks.Core.Localization;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements.DamageResistance;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.Utilities {
    /// <summary>
    /// Collection of miscellaneous utilities
    /// </summary>
    public static class Helpers {
        /// <summary>
        /// Executes an action on the called object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="run">
        /// Action that is run on the object.
        /// </param>
        public static void TemporaryContext<T>(this T obj, Action<T> run) {
            run?.Invoke(obj);
        }
        /// <summary>
        /// Creates a new object and initializes it with the supplied action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="init">
        /// Action to initialize the new object with.
        /// </param>
        /// <returns>
        /// New object after it has been initialized.
        /// </returns>
        public static T Create<T>(Action<T> init = null) where T : new() {
            var result = new T();
            init?.Invoke(result);
            return result;
        }
        /// <summary>
        /// Creates a new Blueprint with the supplied unity name and initializes it with the supplied action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">
        /// Unity name to give the created blueprint. This also determines the BlueprintGuid used.
        /// </param>
        /// <param name="init">
        /// Action to initialize the new blueprint with.
        /// </param>
        /// <returns>
        /// New blueprint after it has been initialized.
        /// </returns>
        public static T CreateBlueprint<T>([NotNull] string name, Action<T> init = null) where T : SimpleBlueprint, new() {
            var result = new T {
                name = name,
                AssetGuid = Main.TTTContext.Blueprints.GetGUID(name)
            };
            Resources.AddBlueprint(result);
            init?.Invoke(result);
            return result;
        }
        /// <summary>
        /// Creates a new Blueprint with the supplied unity name and initializes it with the supplied action.
        /// This Blueprint will have a BlueprintGuid based on the supplied masterId and component blueprints.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">
        /// Unity name to give the created blueprint. This also determines the BlueprintGuid used.
        /// </param>
        /// <param name="masterId">
        /// Master Id used when generating a derived BlueprintGuid.
        /// </param>
        /// <param name="componentBlueprints">
        /// Additional blueprints used when generating a derived BlueprintGuid.
        /// </param>
        /// <param name="init">
        /// Action to initialize the new blueprint with.
        /// </param>
        /// <returns>
        /// New blueprint after it has been initialized.
        /// </returns>
        public static T CreateDerivedBlueprint<T>([NotNull] string name, BlueprintGuid masterId, [NotNull] IEnumerable<SimpleBlueprint> componentBlueprints, Action<T> init = null) where T : SimpleBlueprint, new() {
            var result = new T {
                name = name,
                AssetGuid = Main.TTTContext.Blueprints.GetDerivedGUID(name, masterId, componentBlueprints.Select(bp => bp.AssetGuid).ToArray())
            };
            Resources.AddBlueprint(result);
            init?.Invoke(result);
            return result;
        }
        /// <summary>
        /// Creates a new BlueprintBuff with the supplied unity name and initializes it with the supplied action.
        /// This sets up some required values to prevent runtime exceptions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">
        /// Unity name to give the created BlueprintBuff. This also determines the BlueprintGuid used.
        /// </param>
        /// <param name="init">
        /// Action to initialize the new BlueprintBuff with.
        /// </param>
        /// <returns>
        /// New BlueprintBuff after it has been initialized.
        /// </returns>
        public static BlueprintBuff CreateBuff(string name, Action<BlueprintBuff> init = null) {
            var result = Helpers.CreateBlueprint<BlueprintBuff>(name, bp => {
                bp.FxOnStart = new PrefabLink();
                bp.FxOnRemove = new PrefabLink();
            });
            init?.Invoke(result);
            return result;
        }
        /// <summary>
        /// Creates a deep copy of the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <param name="init">
        /// Action to initialize the new object with.
        /// </param>
        /// <returns>
        /// New object after it has been initialized.
        /// </returns>
        public static T CreateCopy<T>(T original, Action<T> init = null) {
            var result = (T)ObjectDeepCopier.Clone(original);
            init?.Invoke(result);
            return result;
        }
        /// <summary>
        /// Creates a new LevelEntry with the supplied level and features.
        /// </summary>
        /// <param name="level">
        /// Level to use for the level entry.
        /// </param>
        /// <param name="features">
        /// Features to add to the level entry.
        /// </param>
        /// <returns>
        /// New Level entry setup with supplied arguments.
        /// </returns>
        public static LevelEntry CreateLevelEntry(int level, params BlueprintFeatureBase[] features) {
            return CreateLevelEntry(level, features.Select(f => f.ToReference<BlueprintFeatureBaseReference>()).ToArray());
        }
        /// <summary>
        /// Creates a new LevelEntry with the supplied level and features.
        /// </summary>
        /// <param name="level">
        /// Level to use for the level entry.
        /// </param>
        /// <param name="features">
        /// Features to add to the level entry.
        /// </param>
        /// <returns>
        /// New Level entry setup with supplied arguments.
        /// </returns>
        public static LevelEntry CreateLevelEntry(int level, params BlueprintFeatureBaseReference[] features) {
            LevelEntry levelEntry = new LevelEntry() {
                Level = level,
                m_Features = features.ToList()
            };
            return levelEntry;
        }
        /// <summary>
        /// Creates a new UI group including the supplied features.
        /// </summary>
        /// <param name="features">
        /// Features to include in the UI group.
        /// </param>
        /// <returns>
        /// New UI group built with the supplied features.
        /// </returns>
        public static UIGroup CreateUIGroup(params BlueprintFeatureBase[] features) {
            UIGroup uiGroup = new UIGroup();
            features.ForEach(f => uiGroup.Features.Add(f));
            return uiGroup;
        }
        /// <summary>
        /// Creates a new action list populated with the supplied actions.
        /// </summary>
        /// <param name="actions">
        /// Actions to add to the action list.
        /// </param>
        /// <returns>
        /// New action list filled with the supplied actions.
        /// </returns>
        public static ActionList CreateActionList(params GameAction[] actions) {
            if (actions == null || actions.Length == 1 && actions[0] == null) actions = Array.Empty<GameAction>();
            return new ActionList() { Actions = actions };
        }
        /// <summary>
        /// Creates a new localized string.
        /// </summary>
        /// <param name="simpleName">
        /// name used to refer to the string in the MultiLocalization pack.
        /// </param>
        /// <param name="text">
        /// string that the new localized string will contain.
        /// </param>
        /// <param name="locale">
        /// locale that the text will be created for.
        /// </param>
        /// <param name="shouldProcess">
        /// Determines if the localized string should have tags added to it.
        /// </param>
        /// <returns>
        /// New Localized String built with supplied arguments.
        /// </returns>
        public static LocalizedString CreateString(string simpleName, string text, Locale locale = Locale.enGB, bool shouldProcess = false) {
            // See if we used the text previously.
            // (It's common for many features to use the same localized text.
            // In that case, we reuse the old entry instead of making a new one.)
            string strippedText = text.StripHTML().StripEncyclopediaTags();
            MultiLocalizationPack.MultiLocaleString multiLocalized;
            if (Main.TTTContext.ModLocalizationPack.TryGetText(strippedText, out multiLocalized)) {
                return multiLocalized.LocalizedString;
            }
            multiLocalized = new MultiLocalizationPack.MultiLocaleString(simpleName, strippedText, shouldProcess, locale);
            TTTContext.Logger.LogVerbose($"WARNING: Generated New Localizaed String: {multiLocalized.Key}:{multiLocalized.SimpleName}");
            Main.TTTContext.ModLocalizationPack.AddString(multiLocalized);
            return multiLocalized.LocalizedString;
        }
        /// <summary>
        /// Creates a new ContextRankConfig based on the supplied arguments.
        /// </summary>
        /// <param name="baseValueType">
        /// Base Value Type for the ContextRankConfig.
        /// </param>
        /// <param name="progression">
        /// Progression type for the ContextRankConfig.
        /// </param>
        /// <param name="AbilityRankType">
        /// Ability Rank Type for the ContextRankConfig.
        /// </param>
        /// <param name="min">
        /// Minimum value to use use for the ContextRankConfig. If supplied UseMin is set to true. 
        /// </param>
        /// <param name="max">
        /// Maximum value to use use for the ContextRankConfig. If supplied UseMax is set to true.
        /// </param>
        /// <param name="startLevel">
        /// Start Value for the ContextRankConfig.
        /// </param>
        /// <param name="stepLevel">
        /// Step Level for the ContextRankConfig.
        /// </param>
        /// <param name="exceptClasses">
        /// Classes to exclude for the ContextRankConfig.
        /// </param>
        /// <param name="stat">
        /// Stat to use for the ContextRankConfig.
        /// </param>
        /// <param name="customProperty">
        /// Custom Property to use for the ContextRankConfig.
        /// </param>
        /// <param name="classes">
        /// Classes to use for the ContextRankConfig.
        /// </param>
        /// <param name="archetypes">
        /// Archetypes to use for the ContextRankConfig.
        /// </param>
        /// <param name="archetype">
        /// Archetype to use for the ContextRankConfig.
        /// </param>
        /// <param name="feature">
        /// Feature to use for the ContextRankConfig.
        /// </param>
        /// <param name="featureList">
        /// Features to use for the ContextRankConfig.
        /// </param>
        /// <returns>
        /// New ContextRankConfig configured with supplied arguments.
        /// </returns>
        public static ContextRankConfig CreateContextRankConfig(
            ContextRankBaseValueType baseValueType = ContextRankBaseValueType.CasterLevel,
            ContextRankProgression progression = ContextRankProgression.AsIs,
            AbilityRankType AbilityRankType = AbilityRankType.Default,
            int? min = null, int? max = null, int startLevel = 0, int stepLevel = 0,
            bool exceptClasses = false, StatType stat = StatType.Unknown,
            BlueprintUnitProperty customProperty = null,
            BlueprintCharacterClass[] classes = null, BlueprintArchetype[] archetypes = null, BlueprintArchetype archetype = null,
            BlueprintFeature feature = null, BlueprintFeature[] featureList = null) {
            var config = new ContextRankConfig() {
                m_Type = AbilityRankType,
                m_BaseValueType = baseValueType,
                m_Progression = progression,
                m_UseMin = min.HasValue,
                m_Min = min.GetValueOrDefault(),
                m_UseMax = max.HasValue,
                m_Max = max.GetValueOrDefault(),
                m_StartLevel = startLevel,
                m_StepLevel = stepLevel,
                m_Feature = feature.ToReference<BlueprintFeatureReference>(),
                m_ExceptClasses = exceptClasses,
                m_CustomProperty = customProperty.ToReference<BlueprintUnitPropertyReference>(),
                m_Stat = stat,
                m_Class = classes == null ? Array.Empty<BlueprintCharacterClassReference>() : classes.Select(c => c.ToReference<BlueprintCharacterClassReference>()).ToArray(),
                Archetype = archetype.ToReference<BlueprintArchetypeReference>(),
                m_AdditionalArchetypes = archetypes == null ? Array.Empty<BlueprintArchetypeReference>() : archetypes.Select(c => c.ToReference<BlueprintArchetypeReference>()).ToArray(),
                m_FeatureList = featureList == null ? Array.Empty<BlueprintFeatureReference>() : featureList.Select(c => c.ToReference<BlueprintFeatureReference>()).ToArray()
            };
            return config;
        }
        /// <summary>
        /// Creates a generic ContextRankConfig and initializes with the supplied action.
        /// </summary>
        /// <param name="init">
        /// Action to initialize created ContextRankConfig.
        /// </param>
        /// <returns>
        /// ContextRankConfig initialized by the supplied Action.
        /// </returns>
        public static ContextRankConfig CreateContextRankConfig(Action<ContextRankConfig> init) {
            var config = CreateContextRankConfig();
            init?.Invoke(config);
            return config;
        }
        /// <summary>
        /// Replaces Vanilla Damage Resistance compoents with the TTT rework versions.
        /// </summary>
        /// <typeparam name="V">
        /// Type of the original Damage Resistances.
        /// </typeparam>
        /// <typeparam name="N">
        /// Type of the replacement TTT Damage Resistances.
        /// </typeparam>
        /// <param name="blueprint"></param>
        /// <param name="additionalDamageResistanceConfiguration">
        /// Action that configures additional details of the replacement TTT Damage Resistance.
        /// </param>
        public static void ConvertVanillaDamageResistanceToRework<V, N>(
            this BlueprintScriptableObject blueprint,
            Action<N> additionalDamageResistanceConfiguration = null)
            where V : AddDamageResistanceBase
            where N : TTAddDamageResistanceBase, new() {
            bool foundComponent = false;
            for (int i = 0; i < blueprint.ComponentsArray.Length; i++) {
                if (blueprint.ComponentsArray[i] is V oldResistance) {
                    foundComponent = true;
                    N newResistance = Helpers.Create<N>(newRes => newRes.InitFromVanillaDamageResistance(oldResistance));
                    if (additionalDamageResistanceConfiguration != null)
                        additionalDamageResistanceConfiguration(newResistance);
                    blueprint.ComponentsArray[i] = newResistance;
                    TTTContext.Logger.LogVerbose($"Replaced component: {typeof(V).Name} -> {typeof(N).Name} on {blueprint.AssetGuid} - {blueprint.NameSafe()}");
                } else if (blueprint.ComponentsArray[i] is N newResistance) {
                    foundComponent = true;
                    if (additionalDamageResistanceConfiguration != null) {
                        additionalDamageResistanceConfiguration(newResistance);
                        TTTContext.Logger.LogVerbose($"Configured component: {typeof(N).Name} on {blueprint.AssetGuid} - {blueprint.NameSafe()}");
                    } else {
                        TTTContext.Logger.LogVerbose($"Skipped component: {typeof(N).Name} on {blueprint.AssetGuid} - {blueprint.NameSafe()} (no additional configration specified)");
                    }
                }
            }
            if (!foundComponent) {
                TTTContext.Logger.LogVerbose($"COMPONENT NOT FOUND: {typeof(V).Name} or {typeof(N).Name} on {blueprint.AssetGuid} - {blueprint.NameSafe()}");
            } else {
                TTTContext.Logger.LogVerbose($"Rebuilt DR for: {blueprint.AssetGuid} - {blueprint.NameSafe()}");
            }
        }

        internal class ObjectDeepCopier {
            internal class ArrayTraverse {
                public int[] Position;
                private int[] maxLengths;

                public ArrayTraverse(Array array) {
                    maxLengths = new int[array.Rank];
                    for (int i = 0; i < array.Rank; ++i) {
                        maxLengths[i] = array.GetLength(i) - 1;
                    }
                    Position = new int[array.Rank];
                }

                internal bool Step() {
                    for (int i = 0; i < Position.Length; ++i) {
                        if (Position[i] < maxLengths[i]) {
                            Position[i]++;
                            for (int j = 0; j < i; j++) {
                                Position[j] = 0;
                            }
                            return true;
                        }
                    }
                    return false;
                }
            }
            internal class ReferenceEqualityComparer : EqualityComparer<Object> {
                public override bool Equals(object x, object y) {
                    return ReferenceEquals(x, y);
                }
                public override int GetHashCode(object obj) {
                    if (obj == null) return 0;
                    if (obj is WeakResourceLink wrl) {
                        if (wrl.AssetId == null) {
                            return "WeakResourceLink".GetHashCode();
                        } else {
                            return wrl.GetHashCode();
                        }
                    }
                    return obj.GetHashCode();
                }
            }
            private static readonly MethodInfo CloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);

            internal static bool IsPrimitive(Type type) {
                if (type == typeof(string)) return true;
                return (type.IsValueType & type.IsPrimitive);
            }
            internal static object Clone(object originalObject) {
                return InternalCopy(originalObject, new Dictionary<object, object>(new ReferenceEqualityComparer()));
            }
            private static object InternalCopy(object originalObject, IDictionary<object, object> visited) {
                if (originalObject == null) return null;
                var typeToReflect = originalObject.GetType();
                if (IsPrimitive(typeToReflect)) return originalObject;
                if (originalObject is BlueprintReferenceBase) return originalObject;
                if (visited.ContainsKey(originalObject)) return visited[originalObject];
                if (typeof(Delegate).IsAssignableFrom(typeToReflect)) return null;
                var cloneObject = CloneMethod.Invoke(originalObject, null);
                if (typeToReflect.IsArray) {
                    var arrayType = typeToReflect.GetElementType();
                    if (IsPrimitive(arrayType) == false) {
                        Array clonedArray = (Array)cloneObject;
                        ForEach(clonedArray, (array, indices) => array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices));
                    }

                }
                visited.Add(originalObject, cloneObject);
                CopyFields(originalObject, visited, cloneObject, typeToReflect);
                RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect);
                return cloneObject;

                void ForEach(Array array, Action<Array, int[]> action) {
                    if (array.LongLength == 0) return;
                    ArrayTraverse walker = new ArrayTraverse(array);
                    do action(array, walker.Position);
                    while (walker.Step());
                }
            }
            private static void RecursiveCopyBaseTypePrivateFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect) {
                if (typeToReflect.BaseType != null) {
                    RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
                    CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
                }
            }
            private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null) {
                foreach (FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags)) {
                    if (filter != null && filter(fieldInfo) == false) continue;
                    if (IsPrimitive(fieldInfo.FieldType)) continue;
                    var originalFieldValue = fieldInfo.GetValue(originalObject);
                    var clonedFieldValue = InternalCopy(originalFieldValue, visited);
                    fieldInfo.SetValue(cloneObject, clonedFieldValue);
                }
            }
        }
    }
}