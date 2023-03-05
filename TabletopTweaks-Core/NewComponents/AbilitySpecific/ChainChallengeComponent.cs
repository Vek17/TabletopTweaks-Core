using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using TabletopTweaks.Core.NewUnitParts;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [TypeId("c35b164a27ce4a608a3cb898c23f279d")]
    public class ChainChallengeComponent : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCastSpell>,
        IRulebookHandler<RuleCastSpell>, ISubscriber,
        IInitiatorRulebookSubscriber,
        IGlobalSubscriber {

        private BlueprintAbility CavalierChallengeAbility => m_CavalierChallengeAbility?.Get();
        private BlueprintBuff CavalierChallengeBuff => m_CavalierChallengeBuff?.Get();
        private BlueprintAbility KnightsChallengeAbility => m_KnightsChallengeAbility?.Get();
        private BlueprintBuff KnightsChallengeBuff => m_KnightsChallengeBuff?.Get();

        public void OnEventAboutToTrigger(RuleCastSpell evt) {
            UnitPartChainChallengeTTT part;
            if (evt.Spell.Blueprint == CavalierChallengeAbility && !evt.IsDuplicateSpellApplied) {
                part = base.Owner.Ensure<UnitPartChainChallengeTTT>();
                part.Setup(m_CavalierChallengeAbility, m_CavalierChallengeBuff, TriggerCount.Calculate(base.Context));
            }
            if (evt.Spell.Blueprint == KnightsChallengeAbility && !evt.IsDuplicateSpellApplied) {
                part = base.Owner.Ensure<UnitPartChainChallengeTTT>();
                part.Setup(m_KnightsChallengeAbility, m_KnightsChallengeBuff, TriggerCount.Calculate(base.Context));
            }
        }

        public void OnEventDidTrigger(RuleCastSpell evt) {
        }

        public ContextValue TriggerCount = 0;
        public BlueprintAbilityReference m_CavalierChallengeAbility;
        public BlueprintBuffReference m_CavalierChallengeBuff;
        public BlueprintAbilityReference m_KnightsChallengeAbility;
        public BlueprintBuffReference m_KnightsChallengeBuff;
    }
}
