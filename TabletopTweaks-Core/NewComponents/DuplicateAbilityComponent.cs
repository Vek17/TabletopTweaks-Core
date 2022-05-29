using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers.Optimization;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks.Core.NewComponents {
    [TypeId("52e3fe63d19a437bb79e4fc9529b22c6")]
    public class DuplicateAbilityComponent : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCastSpell>,
        IRulebookHandler<RuleCastSpell>,
        ISubscriber,
        IInitiatorRulebookSubscriber {

        private ReferenceArrayProxy<BlueprintAbility, BlueprintAbilityReference> Abilities => m_Abilities;

        void IRulebookHandler<RuleCastSpell>.OnEventAboutToTrigger(RuleCastSpell evt) {
        }

        private bool isValidTrigger(RuleCastSpell evt) {
            return evt.Success
                && Abilities.Contains(evt.Spell.Blueprint)
                && !evt.IsDuplicateSpellApplied;
        }

        void IRulebookHandler<RuleCastSpell>.OnEventDidTrigger(RuleCastSpell evt) {
            if (!isValidTrigger(evt)) {
                return;
            }
            var spell = evt.Spell;
            var newTargets = this.GetNewTargets(spell, evt.SpellTarget.Unit);
            if (newTargets == null) {
                return;
            }
            newTargets.ForEach(target => {
                Rulebook.Trigger<RuleCastSpell>(new RuleCastSpell(spell, target) {
                    IsDuplicateSpellApplied = true
                });
            });
        }

        private IEnumerable<UnitEntityData> GetNewTargets(AbilityData data, UnitEntityData baseTarget) {
            List<UnitEntityData> list = EntityBoundsHelper.FindUnitsInRange(baseTarget.Position, Radius.Meters);
            list.Remove(baseTarget);
            list.Remove(base.Owner);
            list.RemoveAll((UnitEntityData x) => !data.CanTarget(x));
            if (list.Count <= 0) {
                return null;
            }
            return list.GetRandomElements(AdditionalTargets, new System.Random()).ToArray();
        }

        public Feet Radius = 30.Feet();
        public int AdditionalTargets = 2;
        public BlueprintAbilityReference[] m_Abilities = new BlueprintAbilityReference[0];
    }
}
