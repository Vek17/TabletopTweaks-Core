using Kingmaker.EntitySystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;

namespace TabletopTweaks.Core.NewUnitParts {
    public class OutgoingWeaponDamageBonus : UnitPart {

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
                baseDamage.Bonus += additionalDamage.Bonus;
            }
        }

        private BaseDamage baseDamage;
        private RuleCalculateDamage evt;
        private RuleAttackRoll lastAttack;
    }
}