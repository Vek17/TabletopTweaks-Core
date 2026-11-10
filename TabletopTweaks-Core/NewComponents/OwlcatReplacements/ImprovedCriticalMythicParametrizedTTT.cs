using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;

namespace TabletopTweaks.Core.NewComponents.OwlcatReplacements {
    [TypeId("5f0b94b3bdaf464bbefc9869e39804c2")]
    public class ImprovedCriticalMythicParametrizedTTT : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RulePrepareDamage>,
        IRulebookHandler<RulePrepareDamage>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RulePrepareDamage evt) {
        }

        public void OnEventDidTrigger(RulePrepareDamage evt) {
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
            if (evt.ParentRule.m_DamageBundle.m_Chunks.Count >= 1) {
                evt.ParentRule.m_DamageBundle.m_Chunks.Insert(1, newDamage);
            } else {
                evt.ParentRule.m_DamageBundle.Add(newDamage);
            }
        }

        public ContextValue DiceCount = 1;
        public DiceType Dice = DiceType.D10;
    }
}
