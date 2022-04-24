using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;

namespace TabletopTweaks.Core.NewComponents {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("d7a54fb55c154e618a3aff5d40cb7e6f")]
    public class SavingThrowBonusAgainstFavoredEnemy : UnitFactComponentDelegate, 
        IInitiatorRulebookHandler<RuleSavingThrow>, 
        IRulebookHandler<RuleSavingThrow>, 
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleSavingThrow evt) {
            var part = base.Owner.Get<UnitPartFavoredEnemy>();
            var caster = evt.Reason.Caster;
            if (part == null) { return; }
            if (caster == null) { return; }

            int saveBonus = 0;
            EntityFact sourceFact = null;
            foreach (var favoredEntry in part.Entries) {
                if (favoredEntry.CheckedFeatures.Any(feature => caster.HasFact(feature)) && favoredEntry.Bonus > saveBonus) {
                    saveBonus = favoredEntry.Bonus;
                    sourceFact = favoredEntry.Source;
                }
            }
            if (sourceFact != null) {
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveWill.AddModifier(saveBonus, base.Fact, Descriptor));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveReflex.AddModifier(saveBonus, base.Fact, Descriptor));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveFortitude.AddModifier(saveBonus, base.Fact, Descriptor));
            }
        }

        public void OnEventDidTrigger(RuleSavingThrow evt) {
        }
        public ModifierDescriptor Descriptor = ModifierDescriptor.FavoredEnemy;
    }
}