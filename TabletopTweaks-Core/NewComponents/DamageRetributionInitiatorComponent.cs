﻿using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using UnityEngine;

namespace TabletopTweaks.Core.NewComponents {
    [AllowedOn(typeof(BlueprintBuff), false)]
    [AllowMultipleComponents]
    [TypeId("eff875ec05f64613b34f21970041f111")]
    public class DamageRetributionInitiatorComponent : UnitBuffComponentDelegate,
        IInitiatorRulebookHandler<RuleDealDamage>, IRulebookHandler<RuleDealDamage> {

        public void OnEventDidTrigger(RuleDealDamage evt) {
            if (evt.Reason.Fact == base.Fact) { return; }
            var reflectedDamage = (int)(evt.Result * (PercentRedirected / 100f));
            var caster = this.Context.MaybeCaster;
            if (CheckRangeType && evt.AttackRoll?.AttackType != null && !RangeType.IsSuitableAttackType(evt.AttackRoll.AttackType)) {
                return;
            }
            if (reflectedDamage > 0 && caster != null) {
                if (caster == evt.Initiator) { return; }
                Game.Instance.Rulebook.TriggerEvent(
                    new RuleDealDamage(caster, evt.Initiator, new DirectDamage(DiceFormula.Zero, reflectedDamage) {
                        SourceFact = base.Fact
                    }) {
                        SourceAbility = base.Context.SourceAbility,
                        Reason = new RuleReason(base.Fact)
                    }
                );
            }
        }

        public void OnEventAboutToTrigger(RuleDealDamage evt) {
        }


        [SerializeField]
        public int PercentRedirected = 50;
        public bool CheckRangeType;
        public WeaponRangeType RangeType;
    }
}
