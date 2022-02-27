using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Persistence.Versioning;
using System;
using TabletopTweaks.Core.ModLogic;
using TabletopTweaks.Core.Upgraders;
using TabletopTweaks.Core.Utilities;
using UnityModManagerNet;

namespace TabletopTweaks.Core {
    internal static class Main {
        private static bool Enabled;
        public static ModContextTTTCore ModContext;
        static bool Load(UnityModManager.ModEntry modEntry) {
            var harmony = new Harmony(modEntry.Info.Id);
            ModContext = new ModContextTTTCore(modEntry);
            ModContext.LoadAllSettings();
            ModContext.ModEntry.OnSaveGUI = OnSaveGUI;
            ModContext.ModEntry.OnGUI = UMMSettingsUI.OnGUI;
            harmony.PatchAll();
            PostPatchInitializer.Initialize();
            RegisterUpgrades();
            ModContext.Logger.Log("This is a test message!");
            return true;
        }
        private static void RegisterUpgrades() {
            JsonUpgradeSystem.Register(-117, "Migrate TabletopTweaks to TabletopTweaks-Core", new TabletopTweaksMigration());
        }
        private static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            ModContext.SaveSettings("Fixes.json", ModContext.Fixes);
            ModContext.SaveSettings("AddedContent.json", ModContext.AddedContent);
            ModContext.SaveSettings("Homebrew.json", ModContext.Homebrew);
        }
        [Obsolete("Needs to be replaced with instance version")]
        public static void Log(string msg) {
            ModContext.ModEntry.Logger.Log(msg);
        }
        [Obsolete("Needs to be replaced with instance version")]
        [System.Diagnostics.Conditional("DEBUG")]
        public static void LogDebug(string msg) {
            Log(msg);
        }
        [Obsolete("Needs to be replaced with instance version")]
        public static void LogPatch([NotNull] IScriptableObjectWithAssetId bp, bool debug = false) {
            LogPatch("Patched", bp, debug);
        }
        [Obsolete("Needs to be replaced with instance version")]
        public static void LogPatch(string action, [NotNull] IScriptableObjectWithAssetId bp, bool debug = false) {
            if (debug) {
                LogDebug($"{action}: {bp.AssetGuid} - {bp.name}");
            } else {
                Log($"{action}: {bp.AssetGuid} - {bp.name}");
            }
        }
        [Obsolete("Needs to be replaced with instance version")]
        public static void LogHeader(string msg) {
            Log($"--{msg.ToUpper()}--");
        }
        [Obsolete("Needs to be replaced with instance version")]
        public static void Error(Exception e, string message) {
            Log(message);
            Log(e.ToString());
            PFLog.Mods.Error(message);
        }
        [Obsolete("Needs to be replaced with instance version")]
        public static void Error(string message) {
            Log(message);
            PFLog.Mods.Error(message);
        }
    }
}