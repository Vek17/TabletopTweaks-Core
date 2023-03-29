using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem;

namespace TabletopTweaks.Core.NewActions {
    [TypeId("c4530225242448a3a56a27276c2e46ed")]
    public class ContextDuplicateCastSpellOnNewTarget : ContextAction {

        public BlueprintUnitFact FilterNoFact => m_FilterNoFact?.Get();

        public override string GetCaption() {
            return "Recast spell on nearby target";
        }

        public override void RunAction() {
            if (base.Context.MaybeCaster == null) {
                return;
            }
            var Ability = base.AbilityContext?.Ability;
            if (Ability == null) {
                return;
            }
            List<UnitEntityData> list = GameHelper.GetTargetsAround(base.Target.Point, this.Radius.Value, true, false).ToList();
            if (this.SameFaction) {
                list.RemoveAll((UnitEntityData p) => p.Faction.AssetGuid != base.Target.Unit.Faction.AssetGuid);
            }
            if (list.Empty<UnitEntityData>()) {
                return;
            }
            if (this.FilterNoFact != null) {
                list.RemoveAll((UnitEntityData p) => p.HasFact(this.FilterNoFact));
            }
            int remainingTargets = Math.Min(list.Count, this.NumberOfTargets);
            while (remainingTargets > 0) {
                var currentTarget = list.Random();
                Rulebook.Trigger<RuleCastSpell>(new RuleCastSpell(Ability, currentTarget) {
                    IsDuplicateSpellApplied = true
                });
                list.Remove(currentTarget);
                if (list.Empty()) {
                    remainingTargets = 0;
                } else {
                    remainingTargets--;
                }
            }
        }

        public bool SameFaction = true;
        public int NumberOfTargets;
        public Feet Radius;
        public BlueprintUnitFactReference m_FilterNoFact;
    }
}
