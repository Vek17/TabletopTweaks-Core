using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;

namespace TabletopTweaks.Core.NewComponents.Prerequisites {
    [TypeId("cb189aeff170432c856d0f3f837ddf4f")]
    public class PrerequisiteInPlayerParty : Prerequisite {
        public override bool CheckInternal(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state) {
            var result = unit.Unit.IsMainCharacter
                || unit.Unit.IsCloneOfMainCharacter
                || unit.Unit.IsStoryCompanion()
                || unit.Unit.IsCustomCompanion()
                || (unit.Blueprint?.IsCompanion ?? false);
            return Not ? !result : result;
        }

        public override string GetUITextInternal(UnitDescriptor unit) {
            return "";
        }

        public bool Not = false;
    }
}
