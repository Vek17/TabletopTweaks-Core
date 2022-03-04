using JetBrains.Annotations;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Linq;
using System.Text;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.NewComponents.Prerequisites {
    /// <summary>
    /// Requires a unit to have a stat bonus of any amount of the specified descriptor to the specified stat.
    /// </summary>
    [TypeId("282fa36ad9784f639bbdec2e281e7bed")]
    public class PrerequisiteStatBonus : Prerequisite {
        [InitializeStaticString]
        private static readonly LocalizedString Has = Helpers.CreateString(TTTContext, "PrerequisiteStatBonus.UI", "Has");
        [InitializeStaticString]
        private static readonly LocalizedString Bonus = Helpers.CreateString(TTTContext, "PrerequisiteStatBonus.UI", "bonus");

        public override bool CheckInternal([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state) {
            return unit.Stats.GetStat(Stat)?.Modifiers.Any(m => m.ModDescriptor == Descriptor && m.ModValue > 0) ?? false;
        }

        public override string GetUITextInternal(UnitDescriptor unit) {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(Has);
            stringBuilder.Append(" ");
            stringBuilder.Append(LocalizedTexts.Instance.Stats.GetText(Stat));
            stringBuilder.Append(" ");
            stringBuilder.Append(LocalizedTexts.Instance.AbilityModifiers.GetName(Descriptor));
            stringBuilder.Append(" ");
            stringBuilder.Append(Bonus);
            return stringBuilder.ToString();
        }
        /// <summary>
        /// Stat to check.
        /// </summary>
        public StatType Stat;
        /// <summary>
        /// Descriptor to check bonus for.
        /// </summary>
        public ModifierDescriptor Descriptor;
    }
}
