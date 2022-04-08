using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;

namespace TabletopTweaks.Core.NewComponents.OwlcatReplacements {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("e55852cfb50c47d6a8eace820445a165")]
    public class EnduringSpellsTTT : UnitFactComponentDelegate, IUnitBuffHandler, IGlobalSubscriber, ISubscriber {
        public BlueprintUnitFact Greater => this.m_Greater?.Get();

        public void HandleBuffDidAdded(Buff buff) {
            var abilityData = buff.Context?.SourceAbilityContext?.Ability;
            if (abilityData == null || abilityData.Spellbook == null || abilityData.SourceItem != null) {
                return;
            }
            var caster = buff.MaybeContext?.MaybeCaster;
            if (caster == base.Owner 
                && (buff.TimeLeft >= EnduringTime 
                    || (buff.TimeLeft >= GreaterTime && base.Owner.HasFact(this.Greater))) 
                && buff.TimeLeft <= 24.Hours()) {
                buff.SetEndTime(24.Hours() + buff.AttachTime);
            }
        }

        public void HandleBuffDidRemoved(Buff buff) {
        }

        public BlueprintUnitFactReference m_Greater;
        public TimeSpan EnduringTime = 60.Minutes();
        public TimeSpan GreaterTime = 5.Minutes();
    }
}
