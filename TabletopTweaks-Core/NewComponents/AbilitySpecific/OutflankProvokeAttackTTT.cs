using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowMultipleComponents]
    [TypeId("af21183db47748f5b3227f293afff0fb")]
    public class OutflankProvokeAttackTTT : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, IInitiatorRulebookSubscriber {
        public BlueprintUnitFact OutflankFact => m_OutflankFact?.Get();

        public void OnEventAboutToTrigger(RuleAttackRoll evt) {
        }

        public void OnEventDidTrigger(RuleAttackRoll evt) {
            if (evt.IsFake 
                || !evt.IsHit 
                || !evt.IsCriticalConfirmed  
                || evt.FortificationNegatesCriticalHit 
                || (!evt.Target.CombatState.IsFlanked && !evt.Weapon.Blueprint.IsMelee)) 
            {
                return;
            }
            foreach (UnitEntityData unitEntityData in evt.Target.CombatState.EngagedBy) {
                if ((unitEntityData.Descriptor.HasFact(this.OutflankFact) || base.Owner.State.Features.SoloTactics) && unitEntityData != base.Owner) {
                    Game.Instance.CombatEngagementController.ForceAttackOfOpportunity(unitEntityData, evt.Target, false);
                }
            }
        }

        public BlueprintUnitFactReference m_OutflankFact;
    }
}
