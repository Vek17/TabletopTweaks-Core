using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;

namespace TabletopTweaks.Core.NewComponents {
    [AllowMultipleComponents]
    [TypeId("9ea33bacd9fb466e996d243274f84f9a")]
    public class AddAdditionalWeaponDamage : EntityFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
        IRulebookHandler<RuleCalculateWeaponStats>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
            if (CheckFacts && m_Facts.Any(fact => !evt.Initiator.HasFact(fact))) {
                return;
            }
            if (CheckWeaponRangeType && !RangeType.IsSuitableWeapon(evt.Weapon)) {
                return;
            }
            if (CheckWeaponCatergoy && evt.Weapon.Blueprint.Category != Category) {
                return;
            }
            if (CheckWeaponGroup && !evt.Weapon.Blueprint.FighterGroup.Contains(this.Group)) {
                return;
            }
            DamageDescription Damage = new DamageDescription {
                TypeDescription = DamageType,
                Dice = new DiceFormula(Value.DiceCountValue.Calculate(base.Context), Value.DiceType),
                Bonus = Value.BonusValue.Calculate(base.Context),
                SourceFact = base.Fact
            };
            evt.DamageDescription.Add(Damage);
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
        }

        public DamageTypeDescription DamageType;
        public ContextDiceValue Value;
        public bool CheckFacts;
        public BlueprintUnitFactReference[] m_Facts = new BlueprintUnitFactReference[0];
        public bool CheckWeaponRangeType;
        public WeaponRangeType RangeType;
        public bool CheckWeaponCatergoy;
        public WeaponCategory Category;
        public bool CheckWeaponGroup;
        public WeaponFighterGroup Group;
    }
}
