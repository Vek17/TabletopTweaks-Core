using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers.Optimization;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("4656e3c83ed94900900313383aaa6980")]
    public class SplitHexComponentAutomatic : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCastSpell>,
        IRulebookHandler<RuleCastSpell> {

        private BlueprintAbility[] m_MajorHexes;
        private BlueprintAbility[] MajorHexes {
            get {
                if (m_MajorHexes == null) {
                    m_MajorHexes = this.m_MajorHex?.Get()?.IsPrerequisiteFor
                        .Select(f => f.Get())
                        .SelectMany(c => c.GetComponents<AddFacts>())
                        .Where(c => c is not null)
                        .SelectMany(c => c.Facts)
                        .OfType<BlueprintAbility>()
                        .SelectMany(hex => hex.AbilityAndVariants())
                        .SelectMany(hex => hex.AbilityAndStickyTouch())
                        .Distinct()
                        .ToArray();
                }
                return m_MajorHexes;
            }
        }
        private BlueprintAbility[] m_GrandHexes;
        private BlueprintAbility[] GrandHexes {
            get {
                if (m_GrandHexes == null) {
                    m_GrandHexes = this.m_GrandHex?.Get()?.IsPrerequisiteFor
                        .Select(f => f.Get())
                        .SelectMany(c => c.GetComponents<AddFacts>())
                        .Where(c => c is not null)
                        .SelectMany(c => c.Facts)
                        .OfType<BlueprintAbility>()
                        .SelectMany(hex => hex.AbilityAndVariants())
                        .SelectMany(hex => hex.AbilityAndStickyTouch())
                        .Distinct()
                        .ToArray();
                }
                return m_GrandHexes;
            }
        }
        private BlueprintFeature SplitMajorHex => m_SplitMajorHex?.Get();

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
                && !evt.Spell.IsAOE
                && !GrandHexes.Contains(evt.Spell.Blueprint)
                && (evt.Initiator.HasFact(SplitMajorHex) || !MajorHexes.Contains(evt.Spell.Blueprint)); ;
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
        public BlueprintFeatureReference m_MajorHex;
        public BlueprintFeatureReference m_GrandHex;
        public BlueprintFeatureReference m_SplitMajorHex;
    }
}
