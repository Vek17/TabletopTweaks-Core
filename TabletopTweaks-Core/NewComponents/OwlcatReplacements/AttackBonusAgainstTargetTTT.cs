using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using Owlcat.QA.Validation;
using UnityEngine;


namespace TabletopTweaks.Core.NewComponents.OwlcatReplacements {
    [AllowedOn(typeof(BlueprintBuff), false)]
    [TypeId("2a55c3471cbf44409a6193a7a8b8655f")]
    public class AttackBonusAgainstTargetTTT : UnitBuffComponentDelegate, 
        ITargetRulebookHandler<RuleCalculateAttackBonus>, 
        IRulebookHandler<RuleCalculateAttackBonus>, 
        ISubscriber, ITargetRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateAttackBonus evt) {
            UnitEntityData maybeCaster = base.Buff.Context.MaybeCaster;
            bool flag = this.CheckCaster && evt.Initiator == maybeCaster;
            bool flag2 = this.CheckCasterFriend && maybeCaster != null && evt.Initiator.GroupId == maybeCaster.GroupId && evt.Initiator != maybeCaster;
            if (!flag && !flag2 && (CheckCaster || CheckCasterFriend)) {
                return;
            }
            BlueprintItemWeapon blueprint = evt.Weapon.Blueprint;
            if (this.CheckRangeType && (!blueprint || !this.RangeType.IsSuitableWeapon(blueprint))) {
                return;
            }
            evt.AddModifier(this.Value.Calculate(base.Buff.Context), base.Fact, this.Descriptor);
        }

        public void OnEventDidTrigger(RuleCalculateAttackBonus evt) {
        }

        public override void ApplyValidation(ValidationContext context, int parentIndex) {
            base.ApplyValidation(context, parentIndex);
        }

        public ContextValue Value;
        public ModifierDescriptor Descriptor = ModifierDescriptor.UntypedStackable;
        public bool CheckCaster;
        public bool CheckCasterFriend;
        public bool CheckRangeType;

        [SerializeField]
        [ShowIf("CheckRangeType")]
        public WeaponRangeType RangeType;
    }
}
