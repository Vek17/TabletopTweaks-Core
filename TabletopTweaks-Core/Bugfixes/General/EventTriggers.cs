using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.Visual.CharacterSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaks.Core.Bugfixes.General {
    internal class EventTriggers {
        // Breaks generics
        //[HarmonyPatch]
        static class Rulebook_EventHandler_Ensure {
            static MethodBase TargetMethod() {
                return AccessTools.Method(typeof(Rulebook), "TriggerEventInternal").MakeGenericMethod(new Type[] { typeof(RulebookEvent) });
            }
            static bool Prefix(Rulebook __instance, RulebookEvent evt) {
                Type T = evt.GetType();
                Main.TTTContext.Logger.Log($"TriggerEventInternal<{T.Name}>");
                Type handlerType = typeof(Rulebook.EventHandler<>).MakeGenericType(new Type[] { T });
                Main.TTTContext.Logger.Log($"Handler Type: {handlerType.Name}<{handlerType.GetGenericArguments()[0].Name}>");
                var handlerConstructors = AccessTools.GetDeclaredConstructors(handlerType);
                var handlerConstructor = handlerConstructors.FirstOrDefault();
                try {
                    var newHandler = handlerConstructor.Invoke(null);
                    Main.TTTContext.Logger.Log($"New Handler Type: {newHandler.GetType().Name}<{newHandler.GetType().GetGenericArguments()[0].Name}>");
                    var instanceField = AccessTools.Field(handlerType, "s_Instance");
                    var beforeEvent = AccessTools.Field(handlerType, "m_BeforeEvent");
                    var afterEvent = AccessTools.Field(handlerType, "m_AfterEvent");
                    instanceField.SetValue(null, newHandler);
                    var test = instanceField.GetValue(null);
                    Main.TTTContext.Logger.Log($"HasAfterEvent: {afterEvent.GetValue(newHandler) != null}");
                    Main.TTTContext.Logger.Log($"HasBeforeEvent: {beforeEvent.GetValue(newHandler) != null}");
                    Main.TTTContext.Logger.Log($"Set complete: {newHandler == test}");
                    
                } catch(Exception e) {
                    Main.TTTContext.Logger.Log($"{handlerConstructor?.FullDescription()}");
                    Main.TTTContext.Logger.LogError(e.ToString());
                    Main.TTTContext.Logger.LogError(e.Message);
                }
                return true;
            }
        }
        [HarmonyPatch]
        static class Rulebook_EventHandler_CleanUp {
            static MethodBase TargetMethod() {
                return AccessTools.Method(typeof(Rulebook.EventHandler<>).MakeGenericType(typeof(RulebookEvent)), "CleanUp");
            }
            static bool Prefix() {
                return false;
            }
        }
    }
}
