using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace TabletopTweaks.Core.NewComponents.OwlcatReplacements {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("30b36119dec242d19273366e0baebbb7")]
    public class TricksterParryTTT : UnitFactComponentDelegate,
        ITargetRulebookHandler<RuleAttackRoll>,
        ISubscriber, ITargetRulebookSubscriber {
        public void OnEventAboutToTrigger(RuleAttackRoll evt) {
        }

        public void OnEventDidTrigger(RuleAttackRoll evt) {
            if (!evt.IsHit || evt.IsFake || evt.Initiator.IsAlly(evt.Target)) {
                return;
            }
            int dc = evt.D20 + evt.AttackBonus;
            var ruleSkillCheck = new RuleSkillCheck(base.Owner, StatType.SkillMobility, dc);
            var modifier = base.Owner.Stats.SkillMobility.AddModifier(-10, base.Runtime, ModifierDescriptor.Penalty);
            ruleSkillCheck.AddTemporaryModifier(modifier);
            Rulebook.Trigger<RuleSkillCheck>(ruleSkillCheck);
            if (ruleSkillCheck.Success) {
                evt.AutoMiss = true;
            }
        }

        public int PenaltyValue = -10;
    }
}
