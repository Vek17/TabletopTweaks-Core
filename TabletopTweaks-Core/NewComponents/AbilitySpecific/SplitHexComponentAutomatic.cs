using Kingmaker.Controllers.Optimization;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System.Collections.Generic;
using UnityEngine;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using System.Linq;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("4656e3c83ed94900900313383aaa6980")]
    public class SplitHexComponentAutomatic : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCastSpell>,
        IRulebookHandler<RuleCastSpell> {

        public void OnEventAboutToTrigger(RuleCastSpell evt) {
        }

        

        public void OnEventDidTrigger(RuleCastSpell evt) {
            if (!isValidTrigger(evt)) {
                return;
            }
            AbilityData spell = evt.Spell;
            UnitEntityData newTarget = GetNewTarget(spell, evt.SpellTarget.Unit);
            if (newTarget == null) {
                return;
            }
            Rulebook.Trigger(new RuleCastSpell(spell, newTarget) {
                IsDuplicateSpellApplied = true
            });
        }

        private bool isValidTrigger(RuleCastSpell evt) {
            return evt.Success
                && evt.Spell.Blueprint.SpellDescriptor.HasFlag(SpellDescriptor.Hex)
                && !evt.IsDuplicateSpellApplied
                && !evt.Spell.IsAOE;
        }

        private UnitEntityData GetNewTarget(AbilityData data, UnitEntityData baseTarget) {
            List<UnitEntityData> list = EntityBoundsHelper.FindUnitsInRange(baseTarget.Position, m_FeetsRadius.Feet().Meters);
            list.Remove(baseTarget);
            list.Remove(base.Owner);
            list.RemoveAll((UnitEntityData x) => x.Faction != baseTarget.Faction || !data.CanTarget(x));
            if (list.Count <= 0) {
                return null;
            }
            return list.OrderBy(t => t.DistanceTo(baseTarget)).First();
            //return list.GetRandomElements(1, new System.Random())[0];
        }

        [SerializeField]
        private int m_FeetsRadius = 30;
    }
}
