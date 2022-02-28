using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using TabletopTweaks.Core.Modlogic;

namespace TabletopTweaks.Core.Utilities {
    public class PostPatchInitializeAttribute : Attribute {
    }
    public class InitializeStaticStringAttribute : Attribute {
    }

    public static class PostPatchInitializer {
        public static void Initialize(ModContextBase context) {
            var assembly = context.ModEntry.Assembly;
            context.Logger.Log($"PostPatchInitializer: {assembly.FullName}");
            var methods = assembly.GetTypes()
                .Where(x => x.IsClass)
                .SelectMany(x => AccessTools.GetDeclaredMethods(x))
                .Where(x => x.GetCustomAttributes(typeof(PostPatchInitializeAttribute), false).FirstOrDefault() != null);

            foreach (var method in methods) {
                context.Logger.Log($"Executing Post Patch: {method.DeclaringType.Name}.{method.Name}");
                method.Invoke(null, null); // invoke the method
            }

            var fields = assembly.GetTypes()
                .Where(x => x.IsClass)
                .SelectMany(x => AccessTools.GetDeclaredFields(x))
                .Where(x => x.IsStatic)
                .Where(x => x.GetCustomAttributes(typeof(InitializeStaticStringAttribute), false).FirstOrDefault() != null);

            foreach (var field in fields) {
                context.Logger.Log($"Loading Static String: {field.DeclaringType.Name}.{field.Name}");
                field.GetValue(null); // load the field
            }
        }
    }
}
