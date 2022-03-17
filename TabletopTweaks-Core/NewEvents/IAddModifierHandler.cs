using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using System;

namespace TabletopTweaks.Core.NewEvents {
    public interface IAddModifierHandler : IGlobalSubscriber {
        void BeforeAddHandler(ref int value, [NotNull] EntityFact sourceFact, ModifierDescriptor descriptor);

        private class EventTriggers {

            [HarmonyPatch(typeof(ModifiableValue), nameof(ModifiableValue.AddModifier), new Type[] { 
                typeof(int),
                typeof(EntityFactComponent),
                typeof(ModifierDescriptor)
            })]
            static class ModifiableValue_AddModifier_Patch {
                
                static void Prefix(ref int value, [NotNull] EntityFactComponent source, ModifierDescriptor desc) {
                    var temp = value;
                    EventBus.RaiseEvent<IAddModifierHandler>(h => h.BeforeAddHandler(ref temp, source.Fact, desc));
                    value = temp;
                }
            }
            [HarmonyPatch(typeof(ModifiableValue), nameof(ModifiableValue.AddModifier), new Type[] {
                typeof(int),
                typeof(EntityFact),
                typeof(ModifierDescriptor)
            })]
            static class ModifiableValue_AddModifier_Patch2 {

                static void Prefix(ref int value, [NotNull] EntityFact sourceFact, ModifierDescriptor desc) {
                    var temp = value;
                    EventBus.RaiseEvent<IAddModifierHandler>(h => h.BeforeAddHandler(ref temp, sourceFact, desc));
                    value = temp;
                }
            }
            [HarmonyPatch(typeof(RulebookEvent), nameof(RulebookEvent.AddModifier), new Type[] {
                typeof(int),
                typeof(EntityFact),
                typeof(ModifierDescriptor)
            })]
            static class RulebookEvent_AddModifier_Patch {

                static void Prefix(ref int bonus, [NotNull] EntityFact source, ModifierDescriptor descriptor) {
                    var temp = bonus;
                    EventBus.RaiseEvent<IAddModifierHandler>(h => h.BeforeAddHandler(ref temp, source, descriptor));
                    bonus = temp;
                }
            }
        }
    }
}
