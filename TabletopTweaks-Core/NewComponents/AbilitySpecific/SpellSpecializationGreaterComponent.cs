﻿using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.NewUnitParts;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    /// <summary>
    /// Enables spell conversions for spells that have Spell Specialization.
    /// </summary>
    [TypeId("8390a2bda2e74bfb83cb0483adfcf5f7")]
    public class SpellSpecializationGreaterComponent : UnitFactComponentDelegate {

        public override void OnTurnOn() {
            var part = base.Owner.Ensure<UnitPartSpellSpecialization>();
            part.EnableGreater(base.Fact);
        }

        public override void OnTurnOff() {
            var part = base.Owner.Get<UnitPartSpellSpecialization>();
            if (part != null) {
                part.DisableGreater(base.Fact);
            }
        }

        public override void OnActivate() {
            OnTurnOn();
        }

        public override void OnDeactivate() {
            OnTurnOff();
        }
    }
}
