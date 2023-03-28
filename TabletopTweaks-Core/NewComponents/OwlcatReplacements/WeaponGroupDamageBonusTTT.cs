using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using System.Linq;

namespace TabletopTweaks.Core.NewComponents.OwlcatReplacements {
    [ComponentName("Weapon group damage bonus")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("13606541914542f3a1d22f5a666cddc7")]
    public class WeaponGroupDamageBonusTTT : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
            if (evt.Weapon != null && evt.Weapon.Blueprint.FighterGroup.Contains(this.WeaponGroup)) {
                evt.AddDamageModifier(DamageBonus.Calculate(base.Context) * base.Fact.GetRank(), base.Fact, this.Descriptor);
            }
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
        }
        public WeaponFighterGroup WeaponGroup;
        public ContextValue DamageBonus;
        public ModifierDescriptor Descriptor;
    }
}
