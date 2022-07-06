using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using static Kingmaker.RuleSystem.Rules.RuleDispelMagic;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    /// <summary>
    /// On a succsessful dispel check triggers a saving throw DC 10 + CL/2 + Highest Stat Bonus.
    /// Runs actions based on the result.
    /// </summary>
    [AllowedOn(typeof(BlueprintFeature), false)]
    [TypeId("ea475e4be98f4eabb361ed8ce58870ad")]
    public class DestructiveDispelComponent : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleDispelMagic>,
        IRulebookHandler<RuleDispelMagic>,
        ISubscriber, IInitiatorRulebookSubscriber {
        public void OnEventAboutToTrigger(RuleDispelMagic evt) {
        }

        public void OnEventDidTrigger(RuleDispelMagic evt) {
            if (evt.Initiator.IsAlly(evt.Target)) { return; }
            MechanicsContext maybeContext = base.Fact.MaybeContext;
            if (maybeContext != null && evt.Success) {
                var abilityParams = base.Context.TriggerRule(new RuleCalculateAbilityParams(evt.Initiator, base.OwnerBlueprint, null));
                using (maybeContext.GetDataScope(evt.Target)) {
                    int dc = evt.Check switch {
                        CheckType.None => dc = evt?.Context?.Params?.DC ?? 10,
                        _ => 10 + ((evt.CasterLevel + evt.Bonus) / 2) + abilityParams.m_BonusDC + getHighestStatBonus(evt.Initiator, StatType.Intelligence, StatType.Wisdom, StatType.Charisma)
                    };
                    RuleSavingThrow ruleSavingThrow = base.Context.TriggerRule(new RuleSavingThrow(evt.Target, SavingThrowType.Fortitude, dc));
                    if (ruleSavingThrow.IsPassed) {
                        SaveSuccees.Run();
                    } else {
                        SaveFailed.Run();
                    }
                }
            }
        }

        static private int getHighestStatBonus(UnitEntityData unit, params StatType[] stats) {
            StatType highestStat = StatType.Unknown;
            int highestValue = -1;
            foreach (StatType stat in stats) {
                var value = unit.Stats.GetStat(stat).ModifiedValue;
                if (value > highestValue) {
                    highestStat = stat;
                    highestValue = value;
                }
            }
            return unit.Stats.GetStat<ModifiableValueAttributeStat>(highestStat).Bonus;
        }
        /// <summary>
        /// Actions to run if the save was failed.
        /// </summary>
        public ActionList SaveFailed;
        /// <summary>
        /// Actions to run if the save was passed.
        /// </summary>
        public ActionList SaveSuccees;
    }
}
