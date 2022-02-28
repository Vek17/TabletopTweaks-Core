using JetBrains.Annotations;
using Kingmaker.Blueprints.JsonSystem;
using Owlcat.Runtime.Core.Logging;
using System;
using static UnityModManagerNet.UnityModManager;

namespace TabletopTweaks.Core.ModLogic {
    public class ModLogger {

        private readonly ModEntry ModEntry;
        private readonly LogChannel ModChannel;

        public ModLogger(ModEntry ModEntry) {
            this.ModEntry = ModEntry;
            ModChannel = LogChannelFactory.GetOrCreate(ModEntry.Info.Id);
        }

        public void Log(string message) {
            ModChannel.Log(message);
            //ModEntry.Logger.Log(message);
        }

        public void LogVerbose(string message) {
            ModChannel.Verbose(message);
        }

        public void LogWarning(string message) {
            ModChannel.Warning(message);
            //ModEntry.Logger.Log($"WARNING: {message}");
        }

        public void LogPatch([NotNull] IScriptableObjectWithAssetId bp) {
            LogPatch("Patched", bp);
        }

        public void LogPatch(string action, [NotNull] IScriptableObjectWithAssetId bp) {
            Log($"{action}: {bp.AssetGuid} - {bp.name}");
        }

        public void LogHeader(string message) {
            ModChannel.Log(message);
            //ModEntry.Logger.Log($"--{message.ToUpper()}--");
        }

        public void LogError(Exception e, string message) {

            ModChannel.Error(message);
            //ModEntry.Logger.Log($"ERROR: {message}");
            //ModEntry.Logger.Log(e.ToString());
        }

        public void LogError(string message) {
            ModChannel.Error(message);
            //ModEntry.Logger.Log(message);
        }
    }
}
