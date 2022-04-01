using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace TabletopTweaks.Core.NewComponents {
    [TypeId("a1184bbf398944a5a5deb60a614a4134")]
    public class IncreaseCriticalRange : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
        IRulebookHandler<RuleCalculateWeaponStats>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
            evt.CriticalEdgeBonus += CriticalRangeIncrease;
        }

        // Token: 0x0600C5E9 RID: 50665 RVA: 0x00003AE3 File Offset: 0x00001CE3
        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
        }

        public int CriticalRangeIncrease = 1;
    }
}
