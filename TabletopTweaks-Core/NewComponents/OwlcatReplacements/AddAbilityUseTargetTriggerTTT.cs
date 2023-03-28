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
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;

namespace TabletopTweaks.Core.NewComponents.OwlcatReplacements {
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("d9675b68ab7715d42ba3bb44256ddd6b")]
    public class AddAbilityUseTargetTriggerTTT : UnitFactComponentDelegate,
        ITargetRulebookHandler<RuleCastSpell>, IRulebookHandler<RuleCastSpell>,
        ISubscriber, ITargetRulebookSubscriber,
        IApplyAbilityEffectHandler, IGlobalSubscriber {
        public ReferenceArrayProxy<BlueprintSpellbook, BlueprintSpellbookReference> Spellbooks {
            get {
                return this.m_Spellbooks;
            }
        }

        public ReferenceArrayProxy<BlueprintAbility, BlueprintAbilityReference> Spells {
            get {
                return this.m_Spells;
            }
        }

        private void RunAction(AbilityData reasonAbility, UnitEntityData caster) {
            if (reasonAbility == null) {
                return;
            }
            if ((!this.FromSpellbook || (reasonAbility.Spellbook != null && this.Spellbooks.HasReference(reasonAbility.Spellbook.Blueprint))) && (this.DontCheckType || reasonAbility.Blueprint.Type == this.Type) && (!this.SpellList || this.Spells.HasReference(reasonAbility.Blueprint)) && (!this.CheckDescriptor || reasonAbility.Blueprint.SpellDescriptor.HasAnyFlag(this.SpellDescriptor))) {
                if (this.ToCaster) {
                    IFactContextOwner factContextOwner = base.Fact as IFactContextOwner;
                    if (factContextOwner == null) {
                        return;
                    }
                    factContextOwner.RunActionInContext(this.Action, caster);
                    return;
                } else {
                    IFactContextOwner factContextOwner2 = base.Fact as IFactContextOwner;
                    if (factContextOwner2 == null) {
                        return;
                    }
                    factContextOwner2.RunActionInContext(this.Action, base.Owner);
                }
            }
        }

        public void OnEventAboutToTrigger(RuleCastSpell evt) {
            if (!this.AfterCast && !this.TriggerOnEffectApply) {
                this.RunAction(evt.Reason.Ability, evt.Initiator);
            }
        }

        public void OnEventDidTrigger(RuleCastSpell evt) {
            if (this.AfterCast && !this.TriggerOnEffectApply) {
                this.RunAction(evt.Reason.Ability, evt.Initiator);
            }
        }

        public void OnAbilityEffectApplied(AbilityExecutionContext context) {
        }

        public void OnTryToApplyAbilityEffect(AbilityExecutionContext context, TargetWrapper target) {
            if (this.TriggerOnEffectApply && TriggerEvenIfNoEffect && target == base.Owner) {
                UnitEntityData maybeCaster = context.MaybeCaster;
                if (maybeCaster == null && this.ToCaster) {
                    return;
                }
                this.RunAction(context.Ability, maybeCaster);
            }
        }

        public void OnAbilityEffectAppliedToTarget(AbilityExecutionContext context, TargetWrapper target) {
            if (this.TriggerOnEffectApply && !TriggerEvenIfNoEffect && target == base.Owner) {
                UnitEntityData maybeCaster = context.MaybeCaster;
                if (maybeCaster == null && this.ToCaster) {
                    return;
                }
                this.RunAction(context.Ability, maybeCaster);
            }
        }

        public ActionList Action;
        public bool AfterCast;
        public bool FromSpellbook;
        public AbilityType Type;
        public bool DontCheckType;
        public bool ToCaster;
        [InfoBox("Useful option for AoE spells to trigger when Owner is not primary target. Attention: will trigger for every spell effect (on each scorching ray reached Owner)")]
        public bool TriggerOnEffectApply;
        public BlueprintSpellbookReference[] m_Spellbooks = new BlueprintSpellbookReference[0];
        public bool SpellList;
        public BlueprintAbilityReference[] m_Spells = new BlueprintAbilityReference[0];
        public bool CheckDescriptor;
        public SpellDescriptorWrapper SpellDescriptor;
        public bool TriggerEvenIfNoEffect;
    }
}
