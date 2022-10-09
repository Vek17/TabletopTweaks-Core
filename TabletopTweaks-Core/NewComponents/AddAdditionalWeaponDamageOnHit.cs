using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using UnityEngine;

namespace TabletopTweaks.Core.NewComponents {
    [AllowMultipleComponents]
    [TypeId("8ff5982c28664957b9b29e63075ef3cd")]
    public class AddAdditionalWeaponDamageOnHit : EntityFactComponentDelegate<AddAdditionalWeaponDamageOnHit.ComponentData>,
        IInitiatorRulebookHandler<RulePrepareDamage>,
        IRulebookHandler<RulePrepareDamage>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public BlueprintWeaponType WeaponType => m_WeaponType?.Get();

        public void OnEventAboutToTrigger(RulePrepareDamage evt) {
            if (!IsSuitable(evt?.ParentRule?.AttackRoll?.RuleAttackWithWeapon)) { return; }
            var damage = this.DamageType?.CreateDamage(
                dice: new DiceFormula(Value.DiceCountValue.Calculate(base.Context), Value.DiceType),
                bonus: Value.BonusValue.Calculate(base.Context)
            );
            if (damage == null) { return; }
            damage.SourceFact = this.Fact;
            evt.Add(damage);
        }

        public void OnEventDidTrigger(RulePrepareDamage evt) {
            throw new System.NotImplementedException();
        }

        private bool IsSuitable(RuleAttackWithWeapon evt) {
            if (evt == null) { return false; }
            if (this.OnlyNatural20 && evt.AttackRoll.D20.Result != 20) {
                return false;
            }
            ItemEnchantment itemEnchantment = base.Fact as ItemEnchantment;
            ItemEntity itemEntity = (itemEnchantment != null) ? itemEnchantment.Owner : null;
            if (itemEntity != null && itemEntity != evt.Weapon) {
                return false;
            }
            if (Conditions != null) {
                var context = evt.Reason?.Context;
                if (context != null) {
                    using (context.GetDataScope(evt.Target)) {
                        if (!Conditions.Check()) { return false; }
                    }
                }
            }
            if (this.IgnoreAutoHit && evt.AttackRoll.AutoHit) {
                return false;
            }
            if (this.OnMiss && evt.AttackRoll.IsHit) {
                return false;
            }
            if (this.WeaponType && this.WeaponType != evt.Weapon.Blueprint.Type) {
                return false;
            }
            if (this.CheckWeaponCategory && this.Category != evt.Weapon.Blueprint.Category) {
                return false;
            }
            if (this.OnlyCriticalHit && (!evt.AttackRoll.IsCriticalConfirmed || evt.AttackRoll.FortificationNegatesCriticalHit)) {
                return false;
            }
            if (this.NotCriticalHit && evt.AttackRoll.IsCriticalConfirmed && !evt.AttackRoll.FortificationNegatesCriticalHit) {
                return false;
            }
            if (this.OnlyOnFullAttack && !evt.IsFullAttack) {
                return false;
            }
            if (this.OnlyOnFirstAttack && !evt.IsFirstAttack) {
                return false;
            }
            if (this.OnlyOnFirstHit && !evt.AttackRoll.IsHit) {
                return false;
            }
            if (this.OnlyOnFirstHit && !evt.IsFirstAttack && base.Data.HadHit) {
                return false;
            }
            if (this.CheckWeaponRangeType && !this.RangeType.IsSuitableWeapon(evt.Weapon)) {
                return false;
            }
            if (this.CheckWeaponGroup && !evt.Weapon.Blueprint.FighterGroup.Contains(this.Group)) {
                return false;
            }
            if (this.AllNaturalAndUnarmed && !evt.Weapon.Blueprint.Type.IsNatural && !evt.Weapon.Blueprint.Type.IsUnarmed) {
                return false;
            }
            if (this.CheckDistance && evt.Target.DistanceTo(evt.Initiator) > this.DistanceLessEqual.Meters) {
                return false;
            }
            if (this.OnlyOnFirstHit && evt.IsFirstAttack) {
                base.Data.HadHit = false;
            }
            if (this.OnlyOnFirstHit && evt.AttackRoll.IsHit) {
                base.Data.HadHit = true;
            }
            if (this.OnlySneakAttack && (!evt.AttackRoll.IsSneakAttack || evt.AttackRoll.FortificationNegatesSneakAttack)) {
                return false;
            }
            if (this.NotSneakAttack && evt.AttackRoll.IsSneakAttack && !evt.AttackRoll.FortificationNegatesSneakAttack) {
                return false;
            }
            bool flag = evt.Weapon.Blueprint.Category.HasSubCategory(WeaponSubCategory.Light) || evt.Weapon.Blueprint.Category.HasSubCategory(WeaponSubCategory.OneHandedPiercing) || (evt.Initiator.Descriptor.State.Features.DuelingMastery && evt.Weapon.Blueprint.Category == WeaponCategory.DuelingSword) || evt.Initiator.Descriptor.Ensure<UnitPartDamageGrace>().HasEntry(evt.Weapon.Blueprint.Category);
            return (!this.DuelistWeapon || flag) && (!this.NotExtraAttack || !evt.ExtraAttack) && (!this.OnCharge || evt.IsCharge) && (!this.OnAttackOfOpportunity || evt.IsAttackOfOpportunity);
        }
        public ConditionsChecker Conditions = new ConditionsChecker();
        public DamageTypeDescription DamageType;
        public ContextDiceValue Value;
        [HideInInspector]
        public bool WaitForAttackResolve;
        [HideIf("CriticalHit")]
        public bool OnMiss;
        public bool OnlyOnFullAttack;
        public bool OnlyOnFirstAttack;
        public bool OnlyOnFirstHit;
        public bool OnlyCriticalHit;
        public bool OnlyNatural20;
        public bool OnAttackOfOpportunity;
        [HideIf("CriticalHit")]
        public bool NotCriticalHit;
        public bool OnlySneakAttack;
        public bool NotSneakAttack;
        [SerializeField]
        private BlueprintWeaponTypeReference m_WeaponType;
        public bool CheckWeaponCategory;
        [ShowIf("CheckWeaponCategory")]
        public WeaponCategory Category;
        public bool CheckWeaponGroup;
        [ShowIf("CheckWeaponGroup")]
        public WeaponFighterGroup Group;
        public bool CheckWeaponRangeType;
        [ShowIf("CheckWeaponRangeType")]
        public WeaponRangeType RangeType;
        public bool ReduceHPToZero;
        public bool DamageMoreTargetMaxHP;
        public bool CheckDistance;
        [ShowIf("CheckDistance")]
        public Feet DistanceLessEqual;
        public bool AllNaturalAndUnarmed;
        public bool DuelistWeapon;
        public bool NotExtraAttack;
        public bool OnCharge;
        public bool IgnoreAutoHit;
        public ActionList Action;
        public class ComponentData {
            public bool HadHit;
        }
    }
}
