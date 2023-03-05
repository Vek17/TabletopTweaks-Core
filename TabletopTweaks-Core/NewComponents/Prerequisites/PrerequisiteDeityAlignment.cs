using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.UI.Common;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Text;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.NewComponents.Prerequisites {
    [TypeId("57d33f58eb4a460fbf27964e54caa2a8")]
    public class PrerequisiteDeityAlignment : Prerequisite {
        [InitializeStaticString]
        private static readonly LocalizedString DeityAlignment = Helpers.CreateString(modContext: TTTContext, "PrerequisiteCasterLevel.UI", "Deity has Alignment of:\n");

        public override bool CheckInternal([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state) {
            var DeityAlignment = UIUtilityUnit.GetDeity(unit).GetComponent<PrerequisiteAlignment>()?.Alignment ?? AlignmentMaskType.None;
            return (DeityAlignment & Alignment) > 0;
        }

        public override string GetUITextInternal(UnitDescriptor unit) {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(DeityAlignment);
            stringBuilder.Append(UIUtility.GetAlignmentText(this.Alignment, true));
            return stringBuilder.ToString();
        }
        /// <summary>
        /// Must have caster level of this or higher to qualify.
        /// </summary>
        public AlignmentMaskType Alignment;
    }
}
