using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Localization;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Text;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.NewComponents.Prerequisites {
    /// <summary>
    /// Alternate version of PrerequisiteNoClassLevel with a visable UI string.
    /// </summary>
    [TypeId("cb76145587814eabbbbaed3d2a9b5d99")]
    public class PrerequisiteNoClassLevelVisible : Prerequisite {
        [InitializeStaticString]
        private static readonly LocalizedString NoLevelsInClass = Helpers.CreateString(modContext: TTTContext, "PrerequisiteNoClassLevelVisible.UI", "Has no levels in the class:");
        public BlueprintCharacterClass CharacterClass {
            get {
                BlueprintCharacterClassReference characterClass = m_CharacterClass;
                if (characterClass == null) {
                    return null;
                }
                return characterClass.Get();
            }
        }
        public override string GetUITextInternal(UnitDescriptor unit) {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(NoLevelsInClass);
            stringBuilder.Append(" ");
            stringBuilder.Append(CharacterClass.LocalizedName);

            return stringBuilder.ToString();
        }

        public override bool CheckInternal([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state) {
            return unit.Progression.GetClassLevel(CharacterClass) < 1;
        }
        /// <summary>
        /// Class that is not allowed.
        /// </summary>
        [NotNull]
        public BlueprintCharacterClassReference m_CharacterClass;
    }
}
