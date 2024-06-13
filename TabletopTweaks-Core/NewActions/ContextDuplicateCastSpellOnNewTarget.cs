using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.Utilities;

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
            //Special Handling for Magic Deciever Spells
            if (Ability.MagicHackData != null) {
                var SpellBlueprint = Ability.MagicHackData.Spell1.FlattenAllActions().OfType<ContextDuplicateCastSpellOnNewTarget>().Any() ?
                    Ability.MagicHackData.Spell1 : Ability.MagicHackData.Spell2;
                if (Ability.Spellbook == null) { return; }
                Ability = Ability.Spellbook.GetKnownSpell(SpellBlueprint);
            }
            //List<UnitEntityData> list = EntityBoundsHelper.FindUnitsInRange(base.Target.Point, this.Radius.Meters);
            List<UnitEntityData> list = GameHelper.GetTargetsAround(base.Target.Point, this.Radius.Meters, checkLOS: true, includeDead: false).ToList();

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
