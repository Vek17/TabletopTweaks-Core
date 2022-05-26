using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;

namespace TabletopTweaks.Core.NewComponents.Prerequisites {
    [TypeId("cb189aeff170432c856d0f3f837ddf4f")]
    public class PrerequisiteInPlayerParty : Prerequisite {
        private BlueprintFeature BypassInSelection => m_BypassInSelection?.Get();

        public override bool CheckInternal(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state) {
            var result = unit.Unit.IsMainCharacter
                || unit.Unit.IsCloneOfMainCharacter
                || unit.Unit.IsStoryCompanion()
                || unit.Unit.IsCustomCompanion()
                || (unit.Blueprint?.IsCompanion ?? false);
            var ignore = (unit.Progression.CharacterLevel < IgnoreLevelsBelow)
                || BypassInSelection is not null ? (selectionState?.Selection as BlueprintFeatureSelection)?.AssetGuid == BypassInSelection.AssetGuid : false;
            return ignore || (Not ? !result : result);
        }

        public override string GetUITextInternal(UnitDescriptor unit) {
            return "";
        }

        public bool Not = false;
        public int IgnoreLevelsBelow = 0;
        public BlueprintFeatureSelectionReference m_BypassInSelection;
    }
}
