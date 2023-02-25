using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using System.Linq;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic;

namespace TabletopTweaks.Core.NewComponents.OwlcatReplacements {
    [TypeId("d1841cc836ad46399f3ad16f1deba7c3")]
    public class DamageBonusOrderOfCockatriceTTT : UnitFactComponentDelegate, 
        IInitiatorRulebookHandler<RuleAttackWithWeapon>, 
        IRulebookHandler<RuleAttackWithWeapon>, 
        ISubscriber, IInitiatorRulebookSubscriber {

        private BlueprintUnitFact CheckedFact => m_CheckedFact?.Get();

        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt) {
            
            if (evt.Weapon != null 
                && evt.Weapon.Blueprint.IsMelee 
                && evt.Target.Descriptor.HasFact(CheckedFact)) 
            {
                var mount = base.Owner.GetSaddledUnit();
                if ((evt.Target.CombatState.EngagedUnits.Contains(base.Owner)
                        && evt.Target.CombatState.EngagedUnits.Count == 1)
                    || (evt.Target.CombatState.EngagedUnits.Contains(base.Owner)
                        && mount != null
                        && evt.Target.CombatState.EngagedUnits.Contains(mount)
                        && evt.Target.CombatState.EngagedUnits.Count == 2)) 
                {
                    evt.AddTemporaryModifier(evt.Initiator.Stats.AdditionalDamage.AddModifier(this.Bonus.Calculate(base.Context), base.Runtime, this.Descriptor));
                }
                
            }
        }

        public void OnEventDidTrigger(RuleAttackWithWeapon evt) {
        }

        public BlueprintUnitFactReference m_CheckedFact;
        public ContextValue Bonus;
        public ModifierDescriptor Descriptor;
    }
}
