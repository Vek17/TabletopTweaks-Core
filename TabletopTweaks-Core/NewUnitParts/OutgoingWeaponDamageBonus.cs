using Kingmaker.EntitySystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using System;

namespace TabletopTweaks.Core.NewUnitParts {
    [Obsolete("AddOutgoingWeaponDamageBonus no longer requires")]
    public class OutgoingWeaponDamageBonus : UnitPart {
        public override void OnTurnOn() {
            base.RemoveSelf();
        }
        public void AddBonus(RuleCalculateDamage evt, BaseDamage additionalDamage, EntityFact source) {
            if (this.evt != evt) {
                if (lastAttack == evt.ParentRule?.AttackRoll) { return; }
                this.evt = evt;
                lastAttack = evt.ParentRule?.AttackRoll;
                baseDamage = null;
            }
            if (baseDamage == null) {
                baseDamage = additionalDamage;
                evt.ParentRule.m_DamageBundle.m_Chunks.Insert(1, baseDamage);
            } else {
                baseDamage.Dice.Modify(new DiceFormula(baseDamage.Dice.ModifiedValue.Rolls + additionalDamage.Dice.ModifiedValue.Rolls, baseDamage.Dice.ModifiedValue.Dice), source);
                baseDamage.AddModifier(new Modifier(additionalDamage.Bonus, source, Kingmaker.Enums.ModifierDescriptor.UntypedStackable));
            }
        }

        private BaseDamage baseDamage;
        private RuleCalculateDamage evt;
        private RuleAttackRoll lastAttack;
    }
}