using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Controllers.Units;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks.Core.NewUnitParts {
    public class UnitPartAccursedHexTTT : OldStyleUnitPart, 
        IInitiatorRulebookHandler<RuleCastSpell>,
        IRulebookHandler<RuleCastSpell>,
        IUnitNewCombatRoundHandler,
        ISubscriber,
        IInitiatorRulebookSubscriber {

        public EntityFact MythicFact => Owner.GetFact(m_MythicFact);
        private BlueprintBuff AccursedBuff => m_Buff?.Get();

        public void SetMythicFeat(BlueprintFeatureReference mythicFeat) {
            m_MythicFact = mythicFeat;
        }

        public void SetAccursedBuff(BlueprintBuffReference buff) {
            m_Buff = buff;
        }

        public void Remove() {
            base.RemoveSelf();
        }

        public void RemoveTriggering(BlueprintGuid guid, UnitEntityData unit) {
            TrackedHexes.Remove(entry => entry.Matches(unit, guid) && entry.Triggering);
        }

        public void SetPassedSave(BlueprintGuid guid, UnitEntityData unit) {
            TrackedHexes
                .Where(entry => entry.Matches(unit, guid))
                .ForEach(entry => entry.PassedSave = true);
            if (TrackedHexes.Any(entry => entry.Matches(unit, guid))) {
                Main.TTTContext.Logger.Log($"UnitPartAccursedHexTTT: Set: {guid} - Unit {unit.UniqueId} - Passed");
            } else {
                Main.TTTContext.Logger.Log($"UnitPartAccursedHexTTT: Did not Set: {guid} - Unit {unit.UniqueId} - Passed");
            }
        }

        public bool HasActiveEntry(BlueprintGuid guid, UnitEntityData unit) {
            return TrackedHexes.Any(entry => entry.Matches(unit, guid) && entry.PassedSave);
        }

        public bool IgnoreRestictions(BlueprintGuid guid, UnitEntityData unit) {
            return TrackedHexes.Any(entry => entry.Matches(unit, guid) && entry.PassedSave);
        }

        public bool ShouldTriggerReroll(BlueprintGuid guid, UnitEntityData unit) {
            return TrackedHexes.Any(entry => entry.Matches(unit, guid) && entry.Triggering) && MythicFact != null;
        }

        public void OnEventAboutToTrigger(RuleCastSpell evt) {
            var HexGUID = evt.Spell?.Blueprint?.AssetGuid ?? BlueprintGuid.Empty;
            if (HexGUID == BlueprintGuid.Empty) { return; }
            if (TrackedHexes.Any(entry => entry.Matches(evt.SpellTarget.Unit, HexGUID))) {
                TrackedHexes
                    .Where(entry => entry.Matches(evt.SpellTarget.Unit, HexGUID) && entry.PassedSave)
                    .ForEach(entry => entry.Triggering = true);
            } else {
                TrackedHexes.Add(new HexData {
                    Unit = evt.SpellTarget.Unit,
                    Guid = HexGUID,
                    RoundsRemaining = 1,
                    Triggering = false,
                    PassedSave = false
                });
                var buff = evt.SpellTarget.Unit.Descriptor.AddBuff(AccursedBuff, this.Owner, 2.Rounds().Seconds);
            }
        }

        public void OnEventDidTrigger(RuleCastSpell evt) {
            if (evt.Success == false) { return; }
            if ((evt.Spell?.Blueprint?.SpellDescriptor & SpellDescriptor.Hex) == 0) { return; }
            var HexGUID = evt.Spell?.Blueprint?.AssetGuid ?? BlueprintGuid.Empty;
            if (HexGUID == BlueprintGuid.Empty) { return; }
            if (!Owner.HasFact(MythicFact)) {
                RemoveTriggering(HexGUID, evt.SpellTarget.Unit);
            }
        }

        public void HandleNewCombatRound(UnitEntityData unit) {
            if (unit != this.Owner) { return; }
            var ToRemove = new List<HexData>();
            foreach (var entry in TrackedHexes) {
                entry.RoundsRemaining -= 1;
                if (entry.RoundsRemaining < 0) {
                    ToRemove.Add(entry);
                }
            }
            foreach (var key in ToRemove) {
                TrackedHexes.Remove(key);
            }
        }

        private BlueprintFeatureReference m_MythicFact;
        private BlueprintBuffReference m_Buff;
        private List<HexData> TrackedHexes = new List<HexData>();

        public class HexData {
            public EntityRef<UnitEntityData> Unit;
            public BlueprintGuid Guid; 
            public int RoundsRemaining;
            public bool Triggering;
            public bool PassedSave;

            public bool Matches(UnitEntityData Unit, BlueprintGuid HexId) {
                return Unit.Equals(Unit) && HexId.Equals(Guid);
            }

            public override bool Equals(object obj) {
                return obj is HexData data &&
                       this.Unit.Equals(data.Unit) &&
                       this.Guid.Equals(data.Guid);
            }

            public override int GetHashCode() {
                int hashCode = 1572805227;
                hashCode = hashCode * -1521134295 + Unit.GetHashCode();
                hashCode = hashCode * -1521134295 + Guid.GetHashCode();
                return hashCode;
            }

            public static bool operator ==(HexData a, HexData b) {
                return a.Unit.Equals(b.Unit) && a.Guid.Equals(b.Guid);
            }

            public static bool operator !=(HexData a, HexData b) {
                return !a.Unit.Equals(b.Unit) || !a.Guid.Equals(b.Guid);
            }
        } 
    }
}
