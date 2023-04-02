using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers.Optimization;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TabletopTweaks.Core.NewComponents.OwlcatReplacements {
    [TypeId("39daf11ada364bbab00b4ff8a92dba1d")]
    public class AzataZippyMagicTTT : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCastSpell>,
        IRulebookHandler<RuleCastSpell>,
        ISubscriber,
        IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCastSpell evt) {
        }

        private bool isValidTrigger(RuleCastSpell evt) {
            return evt.Success
                && isValidAbility(evt.Spell.Blueprint)
                && !evt.Spell.IsAOE
                && !evt.Spell.Blueprint.GetComponents<AbilityEffectStickyTouch>().Any()
                && !evt.Spell.Blueprint.GetComponents<BlockSpellDuplicationComponent>().Any();
        }

        private bool isValidAbility(BlueprintAbility ability) {
            if (CheckAbilityType) {
                AbilityType? abilityType = ability?.Type;
                if (!Types.Any(t => t == abilityType)) {
                    SpellDescriptor? abilityDescriptors = ability?.SpellDescriptor;
                    if (!AllowDescriptorOverride || ((abilityDescriptors & Descriptors) == 0)) {
                        return false;
                    }
                }
            }
            return true;
        }

        public void OnEventDidTrigger(RuleCastSpell evt) {
            if (!isValidTrigger(evt)) { return; }
            TriggerZippyDamage(evt.SpellTarget.Unit);
            if (evt.IsDuplicateSpellApplied) { return; }
            AbilityData spell = evt.Spell;
            UnitEntityData newTarget = this.GetNewTarget(spell, evt.SpellTarget.Unit);
            if (newTarget == null) {
                return;
            }
            Rulebook.Trigger(new RuleCastSpell(spell, newTarget) {
                IsDuplicateSpellApplied = true
            });
        }

        private void TriggerZippyDamage(UnitEntityData target) {
            if (target == null) { return; }
            if (!target.IsEnemy(base.Owner)) { return; }
            var rule = new RuleDealDamage(
                base.Owner, target,
                new EnergyDamage(ZippyDamageDice, ZippyDamageBonus.Calculate(base.Context), DamageEnergyType.Divine) {
                    SourceFact = base.Fact
                }) {
                Reason = new RuleReason(base.Fact)
            };
            Rulebook.Trigger(rule);
        }

        private UnitEntityData GetNewTarget(AbilityData data, UnitEntityData baseTarget) {
            List<UnitEntityData> list = EntityBoundsHelper.FindUnitsInRange(baseTarget.Position, m_FeetsRadius.Feet().Meters);
            list.Remove(baseTarget);
            list.Remove(base.Owner);
            list.RemoveAll((UnitEntityData x) => x.Faction != baseTarget.Faction || !data.CanTarget(x));
            if (list.Count <= 0) {
                return null;
            }
            return list.GetRandomElements(1, new System.Random())[0];
        }

        [SerializeField]
        private int m_FeetsRadius = 30;
        public bool CheckAbilityType;
        public AbilityType[] Types = new AbilityType[0];
        public bool AllowDescriptorOverride;
        public SpellDescriptorWrapper Descriptors;
        public DiceFormula ZippyDamageDice = DiceFormula.Zero;
        public ContextValue ZippyDamageBonus = 0;
    }
}
