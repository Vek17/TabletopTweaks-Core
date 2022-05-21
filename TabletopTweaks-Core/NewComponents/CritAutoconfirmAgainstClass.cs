using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using System.Linq;


namespace TabletopTweaks.Core.NewComponents {
    [ComponentName("Crits against target with certain alignment are autoconfirmed")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("8b46f45c3ca24c4c877f1b4464f3720f")]
    public class CritAutoconfirmAgainstClass : EntityFactComponentDelegate,
        IInitiatorRulebookHandler<RuleAttackRoll>,
        IRulebookHandler<RuleAttackRoll>, ISubscriber,
        IInitiatorRulebookSubscriber {

        private ReferenceArrayProxy<BlueprintCharacterClass, BlueprintCharacterClassReference> Classes {
            get {
                return this.m_Classes;
            }
        }

        public void OnEventAboutToTrigger(RuleAttackRoll evt) {
            if (!ExceptClasses && evt.Target.Descriptor.Progression.Classes.Any(c => Classes.Contains(c.CharacterClass))) {
                evt.AutoCriticalConfirmation = true;
            } else if (ExceptClasses && !evt.Target.Descriptor.Progression.Classes.Any(c => Classes.Contains(c.CharacterClass))) {
                evt.AutoCriticalConfirmation = true;
            }
        }

        public void OnEventDidTrigger(RuleAttackRoll evt) {
        }

        public BlueprintCharacterClassReference[] m_Classes;
        public bool ExceptClasses;
    }
}
