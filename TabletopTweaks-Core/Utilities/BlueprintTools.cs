using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using System;
using System.Collections.Generic;
using System.IO;
using TabletopTweaks.Core.ModLogic;

namespace TabletopTweaks.Core.Utilities {
    public static class BlueprintTools {
        public static readonly Dictionary<BlueprintGuid, SimpleBlueprint> ModBlueprints = new Dictionary<BlueprintGuid, SimpleBlueprint>();

#if false
        public static IEnumerable<T> GetBlueprints<T>() where T : BlueprintScriptableObject {
            if (blueprints == null) {
                var bundle = ResourcesLibrary.s_BlueprintsBundle;
                blueprints = bundle.LoadAllAssets<BlueprintScriptableObject>();
                //blueprints = Kingmaker.Cheats.Utilities.GetScriptableObjects<BlueprintScriptableObject>();
            }
            return blueprints.Concat(ResourcesLibrary.s_LoadedBlueprints.Values).OfType<T>().Distinct();
        }
#endif
        public static T GetModBlueprintReference<T>(ModContextBase modContext, string name) where T : BlueprintReferenceBase {
            BlueprintGuid? assetId = modContext.Blueprints.GetGUID(name);
            var reference = Activator.CreateInstance<T>();
            if (assetId == null) { Main.TTTContext.Logger.LogError($"COULD NOT LOAD MOD BLUEPRINT REFERENCE: {name} - {typeof(T)}"); }
            reference.deserializedGuid = assetId ?? BlueprintGuid.Empty;
            return reference;
        }
        public static T GetModBlueprint<T>(ModContextBase modContext, string name) where T : SimpleBlueprint {
            var assetId = modContext.Blueprints.GetGUID(name);
            ModBlueprints.TryGetValue(assetId, out var value);
            if (value == null) { Main.TTTContext.Logger.LogError($"COULD NOT LOAD MOD BLUEPRINT: {name} - {typeof(T)}"); }
            return value as T;
        }
        public static T GetBlueprintReference<T>(string id) where T : BlueprintReferenceBase {
            var assetId = BlueprintGuid.Parse(id);
            var reference = Activator.CreateInstance<T>();
            reference.deserializedGuid = assetId;
            return reference;
        }
        public static T GetBlueprintReference<T>(BlueprintGuid id) where T : BlueprintReferenceBase {
            var reference = Activator.CreateInstance<T>();
            reference.deserializedGuid = id;
            return reference;
        }
        public static T GetBlueprint<T>(string id) where T : SimpleBlueprint {
            var assetId = BlueprintGuid.Parse(id);
            return GetBlueprint<T>(assetId);
        }
        public static T GetBlueprint<T>(BlueprintGuid id) where T : SimpleBlueprint {
            SimpleBlueprint asset = ResourcesLibrary.TryGetBlueprint(id);
            T value = asset as T;
            if (value == null) { Main.TTTContext.Logger.LogError($"COULD NOT LOAD BLUEPRINT: {id} - {typeof(T)}"); }
            return value;
        }
        public static void AddBlueprint(ModContextBase modContext, [NotNull] SimpleBlueprint blueprint) {
            AddBlueprint(modContext, blueprint, blueprint.AssetGuid);
        }
        public static void AddBlueprint(ModContextBase modContext, [NotNull] SimpleBlueprint blueprint, string assetId) {
            var Id = BlueprintGuid.Parse(assetId);
            AddBlueprint(modContext, blueprint, Id);
        }
        public static void AddBlueprint(ModContextBase modContext, [NotNull] SimpleBlueprint blueprint, BlueprintGuid assetId) {
            var loadedBlueprint = ResourcesLibrary.TryGetBlueprint(assetId);
            if (loadedBlueprint == null) {
                ModBlueprints[assetId] = blueprint;
                ResourcesLibrary.BlueprintsCache.AddCachedBlueprint(assetId, blueprint);
                blueprint.OnEnable();
                modContext.Logger.LogPatch("Added", blueprint);
            } else {
                modContext.Logger.Log($"Failed to Add: {blueprint.name}");
                modContext.Logger.Log($"Asset ID: {assetId} already in use by: {loadedBlueprint.name}");
            }
        }
        public static void TryExportBlueprint(SimpleBlueprint bp, string path) {
            var Export = new BlueprintJsonWrapper(bp);
            try {
                //Export.Save($"{path}{Path.DirectorySeparatorChar}{bp.name}.jbp");
                Export.Save($"{path}{Path.DirectorySeparatorChar}{bp.name}.{bp.AssetGuid.m_Guid.ToString("N")}.jbp");
            } catch {
                Main.TTTContext.Logger.LogError($"Failed to export {bp.AssetGuid}-{bp.name}");
            }
        }
    }
}