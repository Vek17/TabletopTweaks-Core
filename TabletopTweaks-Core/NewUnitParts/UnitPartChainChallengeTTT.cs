using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Controllers.Optimization;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TabletopTweaks.Core.NewUnitParts {
    public class UnitPartChainChallengeTTT : OldStyleUnitPart, 
        IUnitCombatHandler, IUnitBuffHandler {

        private BlueprintAbility TriggeredAbility => m_TriggeredAbility?.Get();
        private BlueprintBuff CheckedBuff => m_CheckedBuff?.Get();

        private bool CanTrigger() {
            remainingTriggers--;
            return remainingTriggers >= 0;
        }

        public void Setup(BlueprintAbilityReference triggerAbility, BlueprintBuffReference checkedBuff, int triggers) {
            m_TriggeredAbility = triggerAbility;
            m_CheckedBuff = checkedBuff;
            remainingTriggers = triggers;
        }

        public void HandleUnitJoinCombat(UnitEntityData unit) {
        }

        public void HandleUnitLeaveCombat(UnitEntityData unit) {
            if (unit == base.Owner) {
                base.RemoveSelf();
            }
        }

        public void HandleBuffDidAdded(Buff buff) {
        }

        public void HandleBuffDidRemoved(Buff buff) {
            if (buff.Context.MaybeCaster == base.Owner && buff.Blueprint == CheckedBuff) {
                TryTriggerChain(buff.Owner);
            }
        }

        private void TryTriggerChain(UnitDescriptor oldTarget) {
            if (!CanTrigger()) { return; }
            var spellData = new AbilityData(this.TriggeredAbility, Owner);
            var newTarget = GetNewTarget(spellData, oldTarget);
            if (newTarget == null) { return; }
            spellData.OverridenResourceLogic = new AbilityResourceIgnore() {
                m_RequiredResource = spellData.ResourceLogic.RequiredResource.ToReference<BlueprintAbilityResourceReference>()
            };
            if (!spellData.CanTarget(newTarget)) { return; }
            Rulebook.Trigger<RuleCastSpell>(new RuleCastSpell(spellData, newTarget) {
                IsDuplicateSpellApplied = true
            });
        }

        private UnitEntityData GetNewTarget(AbilityData data, UnitEntityData baseTarget) {
            List<UnitEntityData> validTargets = EntityBoundsHelper.FindUnitsInRange(base.Owner.Unit.Position, Radius.Meters);
            validTargets.Remove(baseTarget);
            validTargets.Remove(base.Owner.Unit);
            validTargets.RemoveAll((UnitEntityData x) => !data.CanTarget(x));
            if (validTargets.Count <= 0) {
                return null;
            }
            validTargets.Sort((UnitEntityData u1, UnitEntityData u2) => u1.DistanceTo(baseTarget).CompareTo(u2.DistanceTo(baseTarget)));
            return validTargets.FirstOrDefault();
        }

        private int remainingTriggers;
        private Feet Radius = 30.Feet();
        private BlueprintAbilityReference m_TriggeredAbility;
        private BlueprintBuffReference m_CheckedBuff;

        private class AbilityResourceIgnore : IAbilityResourceLogic {
            public AbilityResourceIgnore() : base() { }

            public BlueprintAbilityResource RequiredResource => m_RequiredResource.Get();

            public bool IsSpendResource => true;

            public int CalculateCost(AbilityData ability) {
                return 0;
            }

            public void Spend(AbilityData ability) {
            }

            [JsonProperty]
            public BlueprintAbilityResourceReference m_RequiredResource;
        }
    }
}
