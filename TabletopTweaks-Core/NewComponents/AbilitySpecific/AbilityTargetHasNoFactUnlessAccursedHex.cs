using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System.Linq;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Utility;
using Kingmaker.UI.Models.Log;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.NewUnitParts;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [AllowedOn(typeof(BlueprintAbility), false)]
    [AllowMultipleComponents]
    [TypeId("5c27ac1d0dd84cca82563d8b07f06765")]
    public class AbilityTargetHasNoFactUnlessAccursedHex : BlueprintComponent, IAbilityTargetRestriction {
        public ReferenceArrayProxy<BlueprintUnitFact, BlueprintUnitFactReference> CheckedFacts {
            get {
                return this.m_CheckedFacts;
            }
        }

        public bool IsTargetRestrictionPassed(UnitEntityData caster, TargetWrapper target) {
            UnitEntityData unit = target.Unit;
            if (unit == null) {
                return false;
            }
            bool hasCheckedFact = false;
            foreach (BlueprintUnitFact blueprint in this.CheckedFacts) {
                hasCheckedFact = unit.Descriptor.HasFact(blueprint);
                if (hasCheckedFact) {
                    break;
                }
            }
            var UnitPartAccursedHex = caster.Get<UnitPartAccursedHexTTT>();
            var OwnerBlueprint = this.OwnerBlueprint?.AssetGuid ?? BlueprintGuid.Empty;
            bool isAccursed = UnitPartAccursedHex?.HasActiveEntry(OwnerBlueprint, target.Unit) ?? false;
            return !hasCheckedFact || isAccursed;
        }

        public string GetAbilityTargetRestrictionUIText(UnitEntityData caster, TargetWrapper target) {
            string noFacts = string.Join(", ", from f in this.CheckedFacts
                                               select f.Name);
            return (BlueprintRoot.Instance.LocalizedTexts.Reasons.TargetHasFact).ToString(delegate () {
                GameLogContext.Text = noFacts;
            });
        }

        public BlueprintUnitFactReference[] m_CheckedFacts;
    }
}
