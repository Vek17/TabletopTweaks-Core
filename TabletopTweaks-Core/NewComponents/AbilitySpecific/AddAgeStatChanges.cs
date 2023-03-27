using Epic.OnlineServices.Stats;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using Owlcat.QA.Validation;
using TabletopTweaks.Core.NewEvents;
using TabletopTweaks.Core.NewUnitParts;
using UnityEngine;
using static TabletopTweaks.Core.MechanicsChanges.AdditionalModifierDescriptors;
using static TabletopTweaks.Core.NewUnitParts.UnitPartAgeTTT;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [AllowMultipleComponents]
    [TypeId("bd85e64331034ea89821382babf4a521")]
    public class AddAgeStatChanges : UnitFactComponentDelegate, IAgeNegateHandler, IUnitSubscriber, ISubscriber {

        private static readonly int[] AgePhysicalPenalty = new int[]{
            0,
            -1,
            -2,
            -3
        };
        private static readonly int[] AgeMentalBonus = new int[]{
            0,
            1,
            1,
            1
        };
        private static readonly StatType[] PhysicalStats = new StatType[]{
            StatType.Strength,
            StatType.Dexterity,
            StatType.Constitution
        };
        private static readonly StatType[] MentalStats = new StatType[]{
            StatType.Intelligence,
            StatType.Wisdom,
            StatType.Charisma
        };

        public override void OnTurnOn() {
            this.Update();
        }

        public override void OnTurnOff() {
            this.Cancel();
        }

        private bool ShouldApplyBonus(StatType stat, AgeLevel age) {
            var AgePart = Owner.Ensure<UnitPartAgeTTT>();
            switch (stat) {
                case StatType.Strength:
                case StatType.Dexterity:
                case StatType.Constitution:
                    switch (age) {
                        case AgeLevel.MiddleAge:
                            return !AgePart.MiddleAgePhysicalNegate;
                        case AgeLevel.OldAge:
                            return !AgePart.OldAgePhysicalNegate;
                        case AgeLevel.Venerable:
                            return !AgePart.VenerableAgePhysicalNegate;
                        default: return false;
                    }
                case StatType.Intelligence:
                case StatType.Wisdom:
                case StatType.Charisma:
                    switch (age) {
                        case AgeLevel.MiddleAge:
                            return !AgePart.MiddleAgeMentalNegate;
                        case AgeLevel.OldAge:
                            return !AgePart.OldAgeMentalNegate;
                        case AgeLevel.Venerable:
                            return !AgePart.VenerableAgeMentalNegate;
                        default: return false;
                    }
                default:
                    return false;
            }
        }

        private void Update() {
            Cancel();
            PhysicalStats.ForEach(s => {
                ModifiableValue stat = base.Owner.Stats.GetStat(s);
                if (stat == null) {
                    return;
                }
                int value = 0;
                if (Age >= AgeLevel.MiddleAge && ShouldApplyBonus(s, AgeLevel.MiddleAge)) {
                    value += AgePhysicalPenalty[(int)AgeLevel.MiddleAge];
                }
                if (Age >= AgeLevel.OldAge && ShouldApplyBonus(s, AgeLevel.OldAge)) {
                    value += AgePhysicalPenalty[(int)AgeLevel.OldAge];
                }
                if (Age >= AgeLevel.Venerable && ShouldApplyBonus(s, AgeLevel.Venerable)) {
                    value += AgePhysicalPenalty[(int)AgeLevel.Venerable];
                }
                if (value == 0) { return; }
                stat.AddModifierUnique(value, base.Runtime, this.Descriptor);
            });
            MentalStats.ForEach(s => {
                ModifiableValue stat = base.Owner.Stats.GetStat(s);
                if (stat == null) {
                    return;
                }
                int value = 0;
                if (Age >= AgeLevel.MiddleAge && ShouldApplyBonus(s, AgeLevel.MiddleAge)) {
                    value += AgeMentalBonus[(int)AgeLevel.MiddleAge];
                }
                if (Age >= AgeLevel.OldAge && ShouldApplyBonus(s, AgeLevel.OldAge)) {
                    value += AgeMentalBonus[(int)AgeLevel.OldAge];
                }
                if (Age >= AgeLevel.Venerable && ShouldApplyBonus(s, AgeLevel.Venerable)) {
                    value += AgeMentalBonus[(int)AgeLevel.Venerable];
                }
                if (value == 0) { return; }
                stat.AddModifierUnique(value, base.Runtime, this.Descriptor);
            });
        }

        private void Cancel() {
            PhysicalStats.ForEach(s => {
                ModifiableValue stat = base.Owner.Stats.GetStat(s);
                if (stat == null) {
                    return;
                }
                stat.RemoveModifiersFrom(base.Runtime);
            });
            MentalStats.ForEach(s => {
                ModifiableValue stat = base.Owner.Stats.GetStat(s);
                if (stat == null) {
                    return;
                }
                stat.RemoveModifiersFrom(base.Runtime);
            });
        }

        public void OnAgeNegateChanged() {
            this.Update();
        }

        private ModifierDescriptor Descriptor = (ModifierDescriptor)Untyped.Age;
        public AgeLevel Age;
    }
}
