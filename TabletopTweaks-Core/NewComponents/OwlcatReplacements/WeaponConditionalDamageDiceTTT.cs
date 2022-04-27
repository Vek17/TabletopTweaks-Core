using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;

namespace TabletopTweaks.Core.NewComponents.OwlcatReplacements {
    [TypeId("4d34be1ec1c24ed4aeff48da776720ac")]
    public  class WeaponConditionalDamageDiceTTT : WeaponEnchantmentLogic,
        IInitiatorRulebookHandler<RulePrepareDamage>,
        IRulebookHandler<RulePrepareDamage>,
        ISubscriber, IInitiatorRulebookSubscriber, IResourcesHolder {

        public void OnEventAboutToTrigger(RulePrepareDamage evt) {
            if (!CheckCondition(evt)) { return; }
            AddBonusDamage(evt);
        }

        public void OnEventDidTrigger(RulePrepareDamage evt) {   
        }

        private void AddBonusDamage(RulePrepareDamage evt) {
            BaseDamage damage = this.Damage.CreateDamage();
            evt.Add(damage);
        }
        private bool CheckCondition(RulePrepareDamage evt) {
            RuleAttackRoll attackRoll = evt.ParentRule.AttackRoll;
            RuleAttackWithWeapon ruleAttackWithWeapon = attackRoll?.RuleAttackWithWeapon;
            if (ruleAttackWithWeapon != null) {
                return IsSuitable(ruleAttackWithWeapon);
            }
            return false;
        }
        private bool IsSuitable(RuleAttackWithWeapon evt) {
            if (evt.Weapon != base.Owner) {
                return false;
            }
            if (this.OnlyHit && !evt.AttackRoll.IsHit) {
                return false;
            }
            if (this.OnMiss && evt.AttackRoll.IsHit) {
                return false;
            }
            if (this.OnAttackOfOpportunity && !evt.IsAttackOfOpportunity) {
                return false;
            }
            if (this.OnlyFlatFooted && !evt.AttackRoll.IsTargetFlatFooted) {
                return false;
            }
            if (this.OnCharge && !evt.IsCharge) {
                return false;
            }
            if (this.CriticalHit && (!evt.AttackRoll.IsCriticalConfirmed || evt.AttackRoll.FortificationNegatesCriticalHit)) {
                return false;
            }
            if (this.OnlySneakAttack && (!evt.AttackRoll.IsSneakAttack || evt.AttackRoll.FortificationNegatesSneakAttack)) {
                return false;
            }
            return true;
        }

        public bool OnlyHit = true;
        public bool OnMiss;
        public bool CriticalHit;
        public bool OnAttackOfOpportunity;
        public bool OnlySneakAttack;
        public bool OnCharge;
        public bool OnlyFlatFooted;
        public bool ActionsOnWielder;
        public DamageDescription Damage;
    }
}