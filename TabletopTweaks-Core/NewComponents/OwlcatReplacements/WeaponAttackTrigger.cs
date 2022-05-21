using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem.Rules;

namespace TabletopTweaks.Core.NewComponents.OwlcatReplacements {
    [TypeId("58b0ca9a50214e418c4c4565df6578c4")]
    public class WeaponAttackTrigger : WeaponEnchantmentLogic,
        IInitiatorRulebookHandler<RuleAttackWithWeapon>,
        IRulebookHandler<RuleAttackWithWeapon>,
        ISubscriber, IInitiatorRulebookSubscriber, IResourcesHolder {

        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt) {
        }

        public void OnEventDidTrigger(RuleAttackWithWeapon evt) {
            if (!IsSuitable(evt)) { return; }
            RunActions(evt);
        }

        private void RunActions(RuleAttackWithWeapon rule) {
            UnitEntityData unit = ActionsOnWielder ? base.Wielder : rule.Target;
            using (base.Context.GetDataScope(unit)) {
                Action.Run();
            }
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
        public ActionList Action;
    }
}
