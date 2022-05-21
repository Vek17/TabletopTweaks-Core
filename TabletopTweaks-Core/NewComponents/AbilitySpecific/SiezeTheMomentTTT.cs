using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System.Linq;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowMultipleComponents]
    [TypeId("43ffe8145cf14cf0aa7148e1e2da8e33")]
    public class SiezeTheMomentTTT : UnitFactComponentDelegate, IGlobalRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, IGlobalRulebookSubscriber {
        public BlueprintUnitFact SiezeTheMomentFact => m_SiezeTheMomentFact?.Get();

        public void OnEventAboutToTrigger(RuleAttackRoll evt) {
        }

        public void OnEventDidTrigger(RuleAttackRoll evt) {
            if (evt.IsFake
                || !evt.IsHit
                || !evt.IsCriticalConfirmed
                || evt.FortificationNegatesCriticalHit
                || evt.Initiator.IsPlayersEnemy
                || !evt.Target.CombatState.EngagedBy.Contains(base.Owner)
                || evt.Initiator == base.Owner) {
                return;
            }
            if (evt.Initiator.Descriptor.HasFact(this.SiezeTheMomentFact) || base.Owner.State.Features.SoloTactics) {
                Game.Instance.CombatEngagementController.ForceAttackOfOpportunity(base.Owner, evt.Target, false);
            }
        }

        public BlueprintUnitFactReference m_SiezeTheMomentFact;
    }
}
