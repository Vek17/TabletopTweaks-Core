using HarmonyLib;
using Kingmaker.EntitySystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks.Core.NewUnitParts {
    public class UnitPartSpellResistanceTTT : OldStyleUnitPart {
        public void AddGlobalSRPenalty(int penalty, EntityFact source) {
            SpellResistancePenalties.Add(new SpellResistancePenalty(penalty, source));
        }
        public void RemoveGlobalSRPenalty(EntityFact source) {
            SpellResistancePenalties.RemoveAll(entry => entry.Source == source);
            TryRemove();
        }
        private void TryRemove() {
            if (!SpellResistancePenalties.Any()) { this.RemoveSelf(); }
        }
        private int CalculatePenalty() {
            if (!SpellResistancePenalties.Any()) { return 0; }
            return SpellResistancePenalties.Select(entry => entry.Penalty).Sum();
        }

        private readonly List<SpellResistancePenalty> SpellResistancePenalties = new();
        public class SpellResistancePenalty {
            public int Penalty;
            public EntityFactRef Source;
            public SpellResistancePenalty(int penalty, EntityFactRef source) {
                Penalty = penalty;
                Source = source;
            }
        }

        [HarmonyPatch(typeof(UnitPartSpellResistance), "GetValue", new Type[] { typeof(MechanicsContext) })]
        static class UnitPartSpellResistance_GetValue_Penalty {
            static void Postfix(UnitPartSpellResistance __instance, ref int __result) {
                var part = __instance.Owner.Get<UnitPartSpellResistanceTTT>();
                if (part == null) { return; }
                var penalty = part.CalculatePenalty();
                __result = Math.Max(__result - penalty, 0);
            }
        }
    }
}
