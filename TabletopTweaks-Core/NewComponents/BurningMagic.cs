﻿using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System.Linq;

namespace TabletopTweaks.Core.NewComponents {
    [AllowMultipleComponents]
    [TypeId("d9558dff3102481dbd918c2abdd0c95b")]
    public class BurningMagic : EntityFactComponentDelegate,
        IInitiatorRulebookHandler<RuleDealDamage>,
        IRulebookHandler<RuleDealDamage>,
        ISubscriber, IInitiatorRulebookSubscriber {

        private BlueprintBuff Buff => m_Buff?.Get();

        public void OnEventAboutToTrigger(RuleDealDamage evt) {

        }

        public void OnEventDidTrigger(RuleDealDamage evt) {
            var context = evt?.Reason?.Context;

            if (evt.SourceAbility?.Type != AbilityType.Spell) { return; }
            if (context?.SavingThrow == null) { return; }
            if (context.SavingThrow.Success) { return; }
            if (evt.Reason?.Fact?.Blueprint == Buff) { return; }
            if (!evt.DamageBundle
                .OfType<EnergyDamage>()
                .Where(damage => damage.EnergyType == EnergyType)
                .Any(damage => !damage.Immune)) { return; }
            var fakeContext = new MechanicsContext(
                caster: evt.Initiator,
                owner: evt.Target,
                blueprint: context.SourceAbility
            );
            var spellLevel = context.Params?.SpellLevel ?? context?.SpellLevel;
            fakeContext.RecalculateAbilityParams();
            fakeContext.SetParams(Context.Params);
            fakeContext.Params.CasterLevel = spellLevel ?? 1;
            fakeContext.Params.Metamagic = 0;
            var appliedBuff = evt.Target?.Descriptor?.AddBuff(Buff, fakeContext, Duration.Calculate(fakeContext).Seconds);
            if (appliedBuff != null) {
                appliedBuff.IsFromSpell = true;
                appliedBuff.IsNotDispelable = true;
            } else {
                //TTTContext.Logger.Log("Buff was null?");
            }
        }

        public DamageEnergyType EnergyType = DamageEnergyType.Fire;
        public BlueprintBuffReference m_Buff;
        public ContextDurationValue Duration;
    }
}
