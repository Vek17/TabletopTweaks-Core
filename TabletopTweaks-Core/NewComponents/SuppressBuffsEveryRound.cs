using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers.Units;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.Core.NewComponents {
    [TypeId("44703c29059c48f8bd7d398b874c708e")]
    public class SuppressBuffsEveryRound : UnitFactComponentDelegate, ITickEachRound {

        public ReferenceArrayProxy<BlueprintBuff, BlueprintBuffReference> Buffs {
            get {
                return m_Buffs;
            }
        }

        public void OnNewRound() {
            if (conditionsChecker.Check()) {
                UnitPartBuffSuppress unitPartBuffSuppress = base.Owner.Ensure<UnitPartBuffSuppress>();
                unitPartBuffSuppress.ApplyChanges();
                //foreach (BlueprintBuff buff in m_SuppressedBuffs) {
                //    unitPartBuffSuppress.PrepareForSuppression(buff);
                //}
            }
        }

        public override void OnActivate() {
            UnitPartBuffSuppress unitPartBuffSuppress = base.Owner.Ensure<UnitPartBuffSuppress>();
            List<BlueprintBuff> suppressedBuffs = new List<BlueprintBuff>();

            foreach (Buff buff in Owner.Buffs) {
                if (IsSuppressed(buff)) {
                    unitPartBuffSuppress.PrepareForSuppression(buff.Blueprint);
                    suppressedBuffs.Add(buff.Blueprint);
                }
            }


            if (Descriptor != SpellDescriptor.None) {
                unitPartBuffSuppress.PrepareForSuppression(Descriptor);
            }
            if (Schools != null && Schools.Length > 0) {
                unitPartBuffSuppress.PrepareForSuppression(Schools);
            }
            foreach (BlueprintBuff buff in Buffs) {
                unitPartBuffSuppress.PrepareForSuppression(buff);
            }
            m_SuppressedBuffs = suppressedBuffs.Select(bp => bp.ToReference<BlueprintBuffReference>()).ToArray();
            unitPartBuffSuppress.ApplyChanges();
        }

        public override void OnDeactivate() {
            base.OnTurnOff();
            UnitPartBuffSuppress unitPartBuffSuppress = base.Owner.Get<UnitPartBuffSuppress>();
            if (!unitPartBuffSuppress) {
                return;
            }
            if (Descriptor != SpellDescriptor.None) {
                unitPartBuffSuppress.PrepareForRelease(Descriptor);
            }
            if (Schools != null && Schools.Length > 0) {
                unitPartBuffSuppress.PrepareForRelease(Schools);
            }
            foreach (BlueprintBuff buff in Buffs) {
                unitPartBuffSuppress.PrepareForRelease(buff);
            }
            unitPartBuffSuppress.ApplyChanges();
        }

        private static IEnumerable<SpellDescriptor> GetValues(SpellDescriptor spellDescriptor) {


            return from v in EnumUtils.GetValues<SpellDescriptor>()
                   where v != SpellDescriptor.None && (spellDescriptor & v) > SpellDescriptor.None
                   select v;
        }

        public bool IsSuppressed(Buff buff) {
            return Buffs.Contains(buff.Blueprint) ||
                (Descriptor != SpellDescriptor.None &&
                UnitPartBuffSuppress.GetValues(buff.Context.SpellDescriptor).Any((SpellDescriptor d) => GetValues(Descriptor).Any(c => (c & d) > SpellDescriptor.None))) ||
                (Schools != null && Schools.Length > 0 &&
                Schools.Contains(buff.Context.SpellSchool));
        }
#pragma warning disable IDE0044 // Add readonly modifier
        [SerializeField]
        [FormerlySerializedAs("Buffs")]
        private BlueprintBuffReference[] m_Buffs = new BlueprintBuffReference[0];

        [SerializeField]
        private BlueprintBuffReference[] m_SuppressedBuffs = new BlueprintBuffReference[0];

        public SpellSchool[] Schools = new SpellSchool[0];

        public SpellDescriptorWrapper Descriptor = SpellDescriptor.None;


        public ConditionsChecker conditionsChecker = new ConditionsChecker();
#pragma warning restore IDE0044 // Add readonly modifier
    }
}
