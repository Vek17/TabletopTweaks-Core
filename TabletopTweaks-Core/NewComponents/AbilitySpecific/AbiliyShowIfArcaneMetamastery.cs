using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using TabletopTweaks.Core.MechanicsChanges;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [TypeId("9708f7b7c8694421bb6b9f60a6917a1a")]
    public class AbiliyShowIfArcaneMetamastery : BlueprintComponent, IAbilityVisibilityProvider {


        public BlueprintUnitFact ArcaneMetamasteryGreater => m_ArcaneMetamasteryGreater?.Get();
        public bool IsAbilityVisible(AbilityData ability) {
            if (ability.Caster.Progression.Features.HasFact(this.ArcaneMetamasteryGreater)) {
                return AdjustedCost(m_Metamagic, ability.Caster) <= 4;
            } else {
                return AdjustedCost(m_Metamagic, ability.Caster) <= 2;
            }
        }

        public Metamagic m_Metamagic;
        public BlueprintUnitFactReference m_ArcaneMetamasteryGreater;
        public bool Not;

        private static int AdjustedCost(Metamagic metamagic, UnitDescriptor unit) {
            UnitMechanicFeatures features = unit.State.Features;
            switch (metamagic) {
                case Metamagic.Empower:
                    if (features.FavoriteMetamagicEmpower) { return metamagic.DefaultCost() - 1; }
                    break;
                case Metamagic.Extend:
                    if (features.FavoriteMetamagicExtend) { return metamagic.DefaultCost() - 1; }
                    break;
                case Metamagic.Bolstered:
                    if (features.FavoriteMetamagicBolstered) { return metamagic.DefaultCost() - 1; }
                    break;
                case Metamagic.Maximize:
                    if (features.FavoriteMetamagicMaximize) { return metamagic.DefaultCost() - 1; }
                    break;
                case Metamagic.Quicken:
                    if (features.FavoriteMetamagicQuicken) { return metamagic.DefaultCost() - 1; }
                    break;
                case Metamagic.Reach:
                    if (features.FavoriteMetamagicReach) { return metamagic.DefaultCost() - 1; }
                    break;
                case Metamagic.Selective:
                    if (features.FavoriteMetamagicSelective) { return metamagic.DefaultCost() - 1; }
                    break;
            }
            if (MetamagicExtention.HasFavoriteMetamagic(unit, metamagic)) {
                return metamagic.DefaultCost() - MetamagicExtention.GetFavoriteMetamagicAdjustment(unit, metamagic); ;
            }
            return metamagic.DefaultCost();
        }
    }
}
