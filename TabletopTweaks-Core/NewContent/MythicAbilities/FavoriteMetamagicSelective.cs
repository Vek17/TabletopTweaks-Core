using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Config;
using TabletopTweaks.Core.Extensions;
using TabletopTweaks.Core.Utilities;

namespace TabletopTweaks.Core.NewContent.MythicAbilities {
    static class FavoriteMetamagicSelective {
        public static void AddFavoriteMetamagicSelective() {
            var SelectiveSpellFeat = Resources.GetBlueprint<BlueprintFeature>("85f3340093d144dd944fff9a9adfd2f2");
            var FavoriteMetamagicSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("503fb196aa222b24cb6cfdc9a284e838");

            var FavoriteMetamagicSelective = Helpers.CreateBlueprint<BlueprintFeature>("FavoriteMetamagicSelective", bp => {
                bp.SetName("Favorite Metamagic — Selective");
                bp.m_Description = FavoriteMetamagicSelection.m_Description;
                //bp.m_Icon = Icon_IntensifiedSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { };
                bp.AddComponent<AddMechanicsFeature>(c => {
                    c.m_Feature = AddMechanicsFeature.MechanicsFeatureType.FavoriteMetamagicSelective;
                });
                bp.AddPrerequisiteFeature(SelectiveSpellFeat);
            });

            if (ModSettings.AddedContent.MythicAbilities.IsDisabled("FavoriteMetamagicSelective")) { return; }
            FavoriteMetamagicSelection.AddFeatures(FavoriteMetamagicSelective);
        }
    }
}
