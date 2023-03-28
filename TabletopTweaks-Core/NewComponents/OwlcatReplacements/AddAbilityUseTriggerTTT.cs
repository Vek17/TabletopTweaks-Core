using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace TabletopTweaks.Core.NewComponents.OwlcatReplacements {
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("56291ed53e7845f59586a77820c27e3b")]
    public class AddAbilityUseTriggerTTT : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCastSpell>, IRulebookHandler<RuleCastSpell>,
        IInitiatorRulebookHandler<RuleApplySpell>, IRulebookHandler<RuleApplySpell>,
        ISubscriber, IInitiatorRulebookSubscriber, IApplyAbilityEffectHandler, IGlobalSubscriber {

        public ReferenceArrayProxy<BlueprintSpellbook, BlueprintSpellbookReference> Spellbooks {
            get {
                return this.m_Spellbooks;
            }
        }

        public BlueprintAbility Ability {
            get {
                BlueprintAbilityReference ability = this.m_Ability;
                if (ability == null) {
                    return null;
                }
                return ability.Get();
            }
        }

        private void RunAction(AbilityData spell, TargetWrapper target) {
            if ((!this.FromSpellbook || (spell.Spellbook != null && this.Spellbooks.HasReference(spell.Spellbook.Blueprint))) && (!this.ForOneSpell || spell.Blueprint == this.Ability) && (!this.ForMultipleSpells || this.Abilities.HasItem((BlueprintAbilityReference r) => r.Is(spell.Blueprint))) && (!this.CheckAbilityType || spell.Blueprint.Type == this.Type) && (!this.MinSpellLevel || spell.SpellLevel >= this.MinSpellLevelLimit) && (!this.ExactSpellLevel || spell.SpellLevel == this.ExactSpellLevelLimit) && (!this.CheckDescriptor || spell.Blueprint.SpellDescriptor.HasAnyFlag(this.SpellDescriptor)) && (!this.CheckAoE || spell.Blueprint.IsAoEDamage == this.IsAoE) && (!this.CheckSpellSchool || spell.Blueprint.School == this.IsSpellSchool) && (!this.CheckRange || spell.Blueprint.Range == this.Range) && (!this.CheckSourceItemType || SpellSourceTypeHelper.CheckSourceItemType(spell, this.SourceItemType, this.SourceItemTypeExclude))) {
                if (this.ActionsOnTarget || this.ActionsOnAllTargets) {
                    IFactContextOwner factContextOwner = base.Fact as IFactContextOwner;
                    if (factContextOwner == null) {
                        return;
                    }
                    factContextOwner.RunActionInContext(this.Action, target);
                    return;
                } else {
                    IFactContextOwner factContextOwner2 = base.Fact as IFactContextOwner;
                    if (factContextOwner2 == null) {
                        return;
                    }
                    factContextOwner2.RunActionInContext(this.Action, null);
                }
            }
        }

        public void OnEventAboutToTrigger(RuleCastSpell evt) {
            if (this.UseCastRule) {
                return;
            }
            if (!this.AfterCast && !this.ActionsOnAllTargets && this.CanUse(evt.Context, evt.Spell, evt.SpellTarget)) {
                this.RunAction(evt.Spell, evt.SpellTarget);
            }
        }

        public void OnEventDidTrigger(RuleCastSpell evt) {
        }

        public void OnAbilityEffectApplied(AbilityExecutionContext context) {
            if (this.UseCastRule) {
                return;
            }
            if (this.AfterCast && !this.ActionsOnAllTargets) {
                UnitEntityData maybeCaster = context.MaybeCaster;
                if (maybeCaster == null || maybeCaster != base.Owner) {
                    return;
                }
                TargetWrapper target = this.ActionsOnTarget ? context.MainTarget : maybeCaster;
                if (this.CanUse(context, context.Ability, target)) {
                    this.RunAction(context.Ability, target);
                }
            }
        }

        public void OnTryToApplyAbilityEffect(AbilityExecutionContext context, TargetWrapper target) {
        }

        public void OnAbilityEffectAppliedToTarget(AbilityExecutionContext context, TargetWrapper target) {
            if (this.UseCastRule) {
                return;
            }
            if (this.ActionsOnAllTargets) {
                UnitEntityData maybeCaster = context.MaybeCaster;
                if (maybeCaster == null || maybeCaster != base.Owner) {
                    return;
                }
                if (this.CanUse(context, context.Ability, target)) {
                    this.RunAction(context.Ability, target);
                }
            }
        }

        public void OnEventAboutToTrigger(RuleApplySpell evt) {
            if (!this.UseCastRule) {
                return;
            }
            if (evt.IsValid(this.AfterCast, this.ActionsOnAllTargets)) {
                if (evt.Place == RuleApplySpell.PlaceType.After) {
                    MechanicsContext context = evt.Context;
                    UnitEntityData maybeCaster = context.MaybeCaster;
                    if (maybeCaster == null || maybeCaster != base.Owner) {
                        return;
                    }
                    TargetWrapper target = evt.SpellTarget ?? (this.ActionsOnTarget ? context.MainTarget : maybeCaster);
                    if (this.CanUse(context, evt.Spell, target)) {
                        this.RunAction(evt.Spell, target);
                        return;
                    }
                } else if (this.CanUse(evt.Context, evt.Spell, evt.SpellTarget)) {
                    this.RunAction(evt.Spell, evt.SpellTarget);
                }
            }
        }

        public void OnEventDidTrigger(RuleApplySpell evt) {
        }

        private bool CanUse(MechanicsContext mechanicsContext, AbilityData abilityData, TargetWrapper target) {
            if (IgnoreAttackSpells && (abilityData.AbilityDeliverProjectile?.NeedAttackRoll ?? false)) {
                return false;
            }
            if (this.OnlyOnce) {
                if (abilityData.IsChildSpell) {
                    return false;
                }
                if (mechanicsContext != null) {
                    string key = base.OwnerBlueprint.AssetGuidThreadSafe + ((target != null) ? target.ToString() : "");
                    int num;
                    if (mechanicsContext.LinkedCounters.TryGetValue(key, out num)) {
                        num++;
                        mechanicsContext.LinkedCounters[key] = num;
                        return false;
                    }
                    mechanicsContext.LinkedCounters[key] = 1;
                }
            }
            return true;
        }

        public ActionList Action;
        [InfoBox(Text = "Применить действия на всех юнитов, на которых подействовал спел (AoE, projectiles). Только после каста")]
        public bool ActionsOnAllTargets;
        [HideIf("ActionsOnAllTargets")]
        public bool AfterCast;
        [HideIf("ActionsOnAllTargets")]
        public bool ActionsOnTarget;
        public bool FromSpellbook;
        [ShowIf("FromSpellbook")]
        [SerializeField]
        public BlueprintSpellbookReference[] m_Spellbooks = new BlueprintSpellbookReference[0];
        public bool ForOneSpell;
        [ShowIf("ForOneSpell")]
        [SerializeField]
        public BlueprintAbilityReference m_Ability;
        public bool ForMultipleSpells;
        [ShowIf("ForMultipleSpells")]
        [SerializeField]
        public List<BlueprintAbilityReference> Abilities;
        public bool MinSpellLevel;
        [ShowIf("MinSpellLevel")]
        public int MinSpellLevelLimit;
        public bool ExactSpellLevel;
        [ShowIf("ExactSpellLevel")]
        public int ExactSpellLevelLimit;
        public bool CheckAbilityType;
        [ShowIf("CheckAbilityType")]
        public AbilityType Type;
        public bool CheckDescriptor;
        [ShowIf("CheckDescriptor")]
        public SpellDescriptorWrapper SpellDescriptor;
        public bool CheckAoE;
        [ShowIf("CheckAoE")]
        public bool IsAoE;
        public bool CheckSpellSchool;
        [ShowIf("CheckSpellSchool")]
        public SpellSchool IsSpellSchool;
        public bool CheckRange;
        [ShowIf("CheckRange")]
        public AbilityRange Range;
        public bool CheckSourceItemType;
        [ShowIf("CheckSourceItemType")]
        public bool SourceItemTypeExclude;
        [ShowIf("CheckSourceItemType")]
        public SpellSourceTypeFlag SourceItemType;
        public bool UseCastRule;
        public bool OnlyOnce;
        public bool IgnoreAttackSpells;
    }
}
