using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [AllowMultipleComponents]
    [TypeId("d8a317a699f041868c65268a97cc6920")]
    public class ElementalBarrageIncomingTrigger : UnitBuffComponentDelegate,
        ITargetRulebookHandler<RuleDealDamage>,
        IRulebookHandler<RuleDealDamage>,
        ISubscriber, IInitiatorRulebookSubscriber {

        private void RunAction(RuleDealDamage e, UnitEntityData target) {
            if ((!this.IgnoreDamageFromThisFact || e.Reason.Fact != base.Fact) && this.TriggerActions.HasActions) {
                IFactContextOwner factContextOwner = base.Fact as IFactContextOwner;
                if (factContextOwner == null) {
                    return;
                }
                factContextOwner.RunActionInContext(this.TriggerActions, base.Owner);
            }
        }

        public void OnEventAboutToTrigger(RuleDealDamage evt) {
        }

        public void OnEventDidTrigger(RuleDealDamage evt) {
            this.Apply(evt);
        }

        private void Apply(RuleDealDamage evt) {
            if (!CheckSource(evt)) {
                return;
            }
            if (!CheckEnergyDamageType && !AboveDamageThreshold(evt.Result)) {
                return;
            }
            if (!CheckEnergyType(evt)) {
                return;
            }
            this.RunAction(evt, evt.Target);
        }

        private bool CheckSource(RuleDealDamage evt) {
            AbilityData ability = evt.Reason.Ability;
            AbilityType abilityType = ability?.Blueprint?.Type ?? (AbilityType)(-1);
            if (abilityType == AbilityType.Spell && ability.Caster == this.Buff?.Context?.MaybeCaster) {
                return false;
            }
            return true;
        }
        private bool CheckEnergyType(RuleDealDamage evt) {
            if (this.CheckEnergyDamageType) {
                foreach (DamageValue damageValue in evt.ResultList) {
                    if (damageValue.Source.Type == DamageType.Energy && (damageValue.Source as EnergyDamage).EnergyType == this.EnergyType) {
                        if (AboveDamageThreshold(damageValue.FinalValue)) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool AboveDamageThreshold(int damageValue) {
            return !this.CheckDamageDealt || this.CompareType.CheckCondition((float)damageValue, (float)this.TargetValue.Calculate(base.Fact.MaybeContext));
        }

        public bool TriggerOnStatDamageOrEnergyDrain;
        public bool NotZeroDamage;
        public bool CheckDamageDealt;
        public CompareOperation.Type CompareType;
        public ContextValue TargetValue;
        public bool CheckEnergyDamageType;
        public DamageEnergyType EnergyType;
        public bool IgnoreDamageFromThisFact = true;
        public ActionList TriggerActions;
    }
}
