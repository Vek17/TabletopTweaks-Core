using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System;
using System.Linq;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [AllowMultipleComponents]
    [TypeId("98cbc7a6c41240199426a87ac018686c")]
    public class ElementalBarrageOutgoingTrigger : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleDealDamage>,
        IRulebookHandler<RuleDealDamage>,
        ISubscriber, IInitiatorRulebookSubscriber {

        private BlueprintBuff ElementalBarrageAcidBuff => m_ElementalBarrageAcidBuff?.Get();
        private BlueprintBuff ElementalBarrageColdBuff => m_ElementalBarrageColdBuff?.Get();
        private BlueprintBuff ElementalBarrageElectricityBuff => m_ElementalBarrageElectricityBuff?.Get();
        private BlueprintBuff ElementalBarrageFireBuff => m_ElementalBarrageFireBuff?.Get();
        private BlueprintBuff ElementalBarrageSonicBuff => m_ElementalBarrageSonicBuff?.Get();

        private void RunAction(RuleDealDamage e, UnitEntityData target) {
            if (!this.IgnoreDamageFromThisFact || e.Reason.Fact != base.Fact) {
                foreach (var damageValue in e.ResultList.Where(r => { var energyDamage = r.Source as EnergyDamage; return energyDamage?.EnergyType == DamageEnergyType.Acid; })) {
                    if (!AboveDamageThreshold(damageValue.FinalValue)) { continue; }
                    ApplyBuff(ElementalBarrageAcidBuff);
                    RemoveBuffs(ElementalBarrageColdBuff, ElementalBarrageElectricityBuff, ElementalBarrageFireBuff, ElementalBarrageSonicBuff);
                }
                foreach (var damageValue in e.ResultList.Where(r => { var energyDamage = r.Source as EnergyDamage; return energyDamage?.EnergyType == DamageEnergyType.Cold; })) {
                    if (!AboveDamageThreshold(damageValue.FinalValue)) { continue; }
                    ApplyBuff(ElementalBarrageColdBuff);
                    RemoveBuffs(ElementalBarrageAcidBuff, ElementalBarrageElectricityBuff, ElementalBarrageFireBuff, ElementalBarrageSonicBuff);
                }
                foreach (var damageValue in e.ResultList.Where(r => { var energyDamage = r.Source as EnergyDamage; return energyDamage?.EnergyType == DamageEnergyType.Electricity; })) {
                    if (!AboveDamageThreshold(damageValue.FinalValue)) { continue; }
                    ApplyBuff(ElementalBarrageElectricityBuff);
                    RemoveBuffs(ElementalBarrageAcidBuff, ElementalBarrageColdBuff, ElementalBarrageFireBuff, ElementalBarrageSonicBuff);
                }
                foreach (var damageValue in e.ResultList.Where(r => { var energyDamage = r.Source as EnergyDamage; return energyDamage?.EnergyType == DamageEnergyType.Fire; })) {
                    if (!AboveDamageThreshold(damageValue.FinalValue)) { continue; }
                    ApplyBuff(ElementalBarrageFireBuff);
                    RemoveBuffs(ElementalBarrageAcidBuff, ElementalBarrageColdBuff, ElementalBarrageElectricityBuff, ElementalBarrageSonicBuff);
                }
                foreach (var damageValue in e.ResultList.Where(r => { var energyDamage = r.Source as EnergyDamage; return energyDamage?.EnergyType == DamageEnergyType.Sonic; })) {
                    if (!AboveDamageThreshold(damageValue.FinalValue)) { continue; }
                    ApplyBuff(ElementalBarrageSonicBuff);
                    RemoveBuffs(ElementalBarrageAcidBuff, ElementalBarrageColdBuff, ElementalBarrageElectricityBuff, ElementalBarrageFireBuff);
                }
            }

            void ApplyBuff(BlueprintBuff markBuff) {
                TimeSpan? duration = MarkDuration.Calculate(this.Context).Seconds;
                Buff buff = target.Descriptor.AddBuff(markBuff, Fact.MaybeContext, duration);
                buff.IsFromSpell = false;
                buff.IsNotDispelable = true;
            }
            void RemoveBuffs(params BlueprintBuff[] buffs) {
                Buff[] array = target.Buffs.Enumerable.ToArray();
                foreach (Buff buff in array) {
                    if (buffs.Any(b => b == buff.Blueprint)) {
                        buff.RunActionInContext(this.TriggerActions, target);
                        buff.Remove();
                    }
                }
            }
        }

        public void OnEventAboutToTrigger(RuleDealDamage evt) {
        }

        public void OnEventDidTrigger(RuleDealDamage evt) {
            this.Apply(evt);
        }

        private void Apply(RuleDealDamage evt) {
            if (this.CheckAbilityType) {
                var ability = evt.Reason.Ability?.Blueprint ?? evt.Reason.Context.SourceAbility;
                AbilityType? abilityType = (ability != null) ? new AbilityType?(ability.Type) : null;
                if (!(abilityType.GetValueOrDefault() == m_AbilityType & abilityType != null)) {
                    return;
                }
            }
            if (!this.ApplyToAreaEffectDamage && evt.SourceArea) {
                return;
            }
            this.RunAction(evt, evt.Target);
        }

        private bool AboveDamageThreshold(int damageValue) {
            return !this.CheckDamageDealt || this.CompareType.CheckCondition((float)damageValue, (float)this.TargetValue.Calculate(base.Fact.MaybeContext));
        }

        public bool TriggerOnStatDamageOrEnergyDrain;
        public bool CheckAbilityType;
        public AbilityType m_AbilityType;
        public bool NotZeroDamage;
        public bool CheckDamageDealt;
        public CompareOperation.Type CompareType;
        public ContextValue TargetValue;
        public bool ApplyToAreaEffectDamage;
        public bool IgnoreDamageFromThisFact = true;
        public BlueprintBuffReference m_ElementalBarrageAcidBuff;
        public BlueprintBuffReference m_ElementalBarrageColdBuff;
        public BlueprintBuffReference m_ElementalBarrageElectricityBuff;
        public BlueprintBuffReference m_ElementalBarrageFireBuff;
        public BlueprintBuffReference m_ElementalBarrageSonicBuff;
        public ContextDurationValue MarkDuration;
        public ActionList TriggerActions;
    }
}
