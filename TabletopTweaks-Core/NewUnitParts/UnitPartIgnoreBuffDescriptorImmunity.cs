using HarmonyLib;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Owlcat.Runtime.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TabletopTweaks.Core.NewUnitParts {
    public class UnitPartIgnoreBuffDescriptorImmunity : UnitPart {
        public void AddEntry(SpellDescriptor? descriptor, EntityFact source) {
            if (descriptor == null) {
                return;
            }
            ImmunityIgnoreEntry item = new ImmunityIgnoreEntry {
                Descriptor = descriptor.Value,
                Source = source
            };
            Descriptors.Add(item);
        }

        public void RemoveEntry(EntityFact source) {
            Descriptors.RemoveAll(p => p.Source == source);
            TryRemove();
        }

        private void TryRemove() {
            if (!Descriptors.Any()) { this.RemoveSelf(); }
        }

        public bool HasEntry(SpellDescriptor category) {
            return Descriptors.Any(p => p.Descriptor == category);
        }

        public SpellDescriptor Entries() {
            SpellDescriptor result = SpellDescriptor.None;
            foreach (var item in Descriptors) {
                result |= item.Descriptor;
            }
            return result;
        }

        public List<ImmunityIgnoreEntry> Descriptors = new();

        public class ImmunityIgnoreEntry {
            public SpellDescriptor Descriptor;
            public EntityFact Source;
        }


        [HarmonyPatch(typeof(BuffDescriptorImmunity))]
        private static class BuffDescriptorImmunity_Patch {

            /**
             * Replaces this line
             * 
             * bool flag = this.Descriptor.HasAnyFlag(context.SpellDescriptor);
             * 
             * with this
             * 
             * bool flag = BuffDescriptorImmunity_Patch.GetWorkingImmunities(this).HasAnyFlag(context.SpellDescriptor);
             */
            [HarmonyPatch(nameof(BuffDescriptorImmunity.IsImmune)), HarmonyTranspiler]
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                try {
                    var code = new List<CodeInstruction>(instructions);

                    var fDescriptorFind = 0;
                    FieldInfo descriptorField = AccessTools.Field(typeof(BuffDescriptorImmunity), nameof(BuffDescriptorImmunity.Descriptor));
                    for (int i = 0; i < code.Count; i++) {
                        if (code[i].LoadsField(descriptorField, true)) {
                            fDescriptorFind = i;
                            break;
                        }
                    }
                    if (fDescriptorFind == 0) {
                        throw new InvalidOperationException("Unable to find the insertion index for Descriptor field find.");
                    }
                    code[fDescriptorFind] = CodeInstruction.Call(typeof(BuffDescriptorImmunity_Patch), nameof(BuffDescriptorImmunity_Patch.GetWorkingImmunities));

                    var mHasAnyFlagCall = 0;
                    MethodInfo hasAnyFlagCall = AccessTools.Method(typeof(SpellDescriptorWrapper), nameof(SpellDescriptorWrapper.HasAnyFlag));
                    for (int i = fDescriptorFind; i < code.Count; i++) {
                        if (code[i].Calls(hasAnyFlagCall)) {
                            mHasAnyFlagCall = i;
                            break;
                        }
                    }

                    if (mHasAnyFlagCall == 0) {
                        throw new InvalidOperationException("Unable to find the insertion index for HasAnyFlag call.");
                    }
                    code[mHasAnyFlagCall] = CodeInstruction.Call(typeof(SpellDescriptorOperations), nameof(SpellDescriptorOperations.HasAnyFlag));

                    return code;
                } catch (Exception e) {
                    LogChannel.Mods.Error($"BuffDescriptorImmunity.IsImmune Transpiler exception {e.Message}", e);
                    return instructions;
                }

            }
            static SpellDescriptor GetWorkingImmunities(BuffDescriptorImmunity bdi) {
                var ignorePart = bdi.Owner.Get<UnitPartIgnoreBuffDescriptorImmunity>();
                var ignoredDescriptors = ignorePart == null ? SpellDescriptor.None : ignorePart.Entries();
                if (ignoredDescriptors != SpellDescriptor.None) {
                    return bdi.Descriptor.Value & ~ignoredDescriptors;
                } else {
                    return bdi.Descriptor.Value;
                }
            }
        }
    }
}
