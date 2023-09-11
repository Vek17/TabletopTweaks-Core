using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System.Linq;
using TabletopTweaks.Core.MechanicsChanges;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowMultipleComponents]
    [TypeId("af21183db47748f5b3227f293afff0fb")]
    public class OutflankProvokeAttackTTT : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackRoll> {
        public BlueprintUnitFact OutflankFact => m_OutflankFact?.Get();

        public void OnEventAboutToTrigger(RuleAttackRoll evt) {
        }

        public void OnEventDidTrigger(RuleAttackRoll evt) {
            if (!evt.Target.CombatState.IsEngage(base.Owner)) { return; }
            if (!evt.Target.IsFlankedBy(base.Owner)) { return; }
            if (evt.IsFake
                || !evt.IsHit
                || !evt.IsCriticalConfirmed
                || evt.FortificationNegatesCriticalHit
                || (!evt.Target.IsFlankedBy(evt.Initiator) && !evt.Weapon.Blueprint.IsMelee)) {
                return;
            }
            foreach (UnitEntityData unitEntityData in evt.Target.CombatState.EngagedBy.Where(initator => evt.Target.IsFlankedBy(initator))) {
                if (unitEntityData != base.Owner) { continue; }
                if (base.Owner.State.Features.SoloTactics || unitEntityData.Descriptor.HasFact(OutflankFact)) {
                    Game.Instance.CombatEngagementController.ForceAttackOfOpportunity(base.Owner, evt.Target, false);
                }
            }
        }

        public BlueprintUnitFactReference m_OutflankFact;
    }
}
