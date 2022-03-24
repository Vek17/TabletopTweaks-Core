using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.NewRules;

namespace TabletopTweaks.Core.NewComponents {
    [ComponentName("Reroll Concealment Checks")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("ef0e2207f0a341c19277f1481b0b8c57")]
    public class RerollFortification : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleFortificationCheck>,
        IRulebookHandler<RuleFortificationCheck>, ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleFortificationCheck evt) {
            if (ConfirmedCriticals && evt.ForCritical) {
                evt.Roll.AddReroll(RerollCount, TakeBest, base.Fact);
                return;
            }
            if (SneakAttacks && evt.ForSneakAttack) {
                evt.Roll.AddReroll(RerollCount, TakeBest, base.Fact);
                return;
            }
            if (PreciseStrike && evt.ForPreciseStrike) {
                evt.Roll.AddReroll(RerollCount, TakeBest, base.Fact);
                return;
            }
            if (AllEvents) {
                evt.Roll.AddReroll(RerollCount, TakeBest, base.Fact);
                return;
            }
        }

        public void OnEventDidTrigger(RuleFortificationCheck evt) {
        }

        public int RerollCount = 1;
        public bool TakeBest = true;
        public bool ConfirmedCriticals = true;
        public bool SneakAttacks = true;
        public bool PreciseStrike = true;
        public bool AllEvents;
    }
}
