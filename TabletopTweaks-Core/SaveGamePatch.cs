using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using System;
using System.Collections.Generic;

namespace TabletopTweaks.Core {
    public static class SaveGameFix {
        static private List<Action<UnitEntityData>> save_game_actions = new List<Action<UnitEntityData>>();

        public static void AddUnitPatch(Action<UnitEntityData> patch) {
            save_game_actions.Add(patch);
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
