using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics;

namespace TabletopTweaks.Core.NewComponents.OwlcatReplacements {
    [TypeId("5f0b94b3bdaf464bbefc9869e39804c2")]
    public class ImprovedCriticalMythicParametrizedTTT : UnitFactComponentDelegate, 
        IInitiatorRulebookHandler<RuleCalculateDamage>, 
        IRulebookHandler<RuleCalculateDamage>, 
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateDamage evt) {
            var attackRoll = evt.ParentRule?.AttackRoll;
            if (attackRoll == null) { return; }
            if (!attackRoll.IsCriticalConfirmed && !attackRoll.FortificationNegatesCriticalHit) { return; }
            if (attackRoll.Weapon?.Blueprint?.Category != base.Param) { return; }

            var critMultiplier = evt.DamageBundle.WeaponDamage.CriticalModifier;
            if (critMultiplier < 2) { return; }
            var newDamage = evt.DamageBundle.WeaponDamage.CreateTypeDescription().CreateDamage(
                new DiceFormula((critMultiplier.Value - 1) * DiceCount.Calculate(base.Context), Dice),
                0
            );
            newDamage.SourceFact = base.Fact;
            evt.ParentRule.m_DamageBundle.Add(
                newDamage
            );
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt) {
        }

        public ContextValue DiceCount = 1;
        public DiceType Dice = DiceType.D10;
    }
}
