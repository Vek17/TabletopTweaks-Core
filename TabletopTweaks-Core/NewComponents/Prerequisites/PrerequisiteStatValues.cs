using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Localization;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Linq;
using System.Text;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.NewComponents.Prerequisites {
    /// <summary>
    /// Requires the unit to have a number of stats from specified stats that meet a minimum value.
    /// </summary>
    [TypeId("8dccf39766ed482eb9b7f2ae31e50428")]
    public class PrerequisiteStatValues : Prerequisite {
        [InitializeStaticString]
        private static readonly LocalizedString Has = Helpers.CreateString(TTTContext, "PrerequisiteSpellKnown.UI", "Has");
        [InitializeStaticString]
        private static readonly LocalizedString Rank = Helpers.CreateString(TTTContext, "PrerequisiteSpellKnown.UI", "rank");
        [InitializeStaticString]
        private static readonly LocalizedString Ranks = Helpers.CreateString(TTTContext, "PrerequisiteSpellKnown.UI", "ranks");
        [InitializeStaticString]
        private static readonly LocalizedString In = Helpers.CreateString(TTTContext, "PrerequisiteSpellKnown.UI", "in");
        [InitializeStaticString]
        private static readonly LocalizedString OfTheFollowingSkills = Helpers.CreateString(TTTContext, "PrerequisiteSpellKnown.UI", "of the following skills:");
        [InitializeStaticString]
        private static readonly LocalizedString One = Helpers.CreateString(TTTContext, "PrerequisiteSpellKnown.UI", "one");
        [InitializeStaticString]
        private static readonly LocalizedString Two = Helpers.CreateString(TTTContext, "PrerequisiteSpellKnown.UI", "two");
        [InitializeStaticString]
        private static readonly LocalizedString Three = Helpers.CreateString(TTTContext, "PrerequisiteSpellKnown.UI", "three");
        [InitializeStaticString]
        private static readonly LocalizedString Four = Helpers.CreateString(TTTContext, "PrerequisiteSpellKnown.UI", "four");
        [InitializeStaticString]
        private static readonly LocalizedString Five = Helpers.CreateString(TTTContext, "PrerequisiteSpellKnown.UI", "five");
        [InitializeStaticString]
        private static readonly LocalizedString Six = Helpers.CreateString(TTTContext, "PrerequisiteSpellKnown.UI", "six");
        [InitializeStaticString]
        private static readonly LocalizedString Seven = Helpers.CreateString(TTTContext, "PrerequisiteSpellKnown.UI", "seven");
        [InitializeStaticString]
        private static readonly LocalizedString Eight = Helpers.CreateString(TTTContext, "PrerequisiteSpellKnown.UI", "eight");
        [InitializeStaticString]
        private static readonly LocalizedString Nine = Helpers.CreateString(TTTContext, "PrerequisiteSpellKnown.UI", "nine");

        public override bool CheckInternal(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state) {
            return CheckUnit(unit);
        }

        public bool CheckUnit(UnitDescriptor unit) {
            return Stats.Count(stat => GetStatValue(unit, stat) >= Value) >= Amount;
        }

        public override string GetUITextInternal(UnitDescriptor unit) {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(Has);
            stringBuilder.Append(" ");
            stringBuilder.Append(Value);
            stringBuilder.Append(" ");
            stringBuilder.Append(Value > 1 ? Ranks : Rank);
            stringBuilder.Append(" ");
            stringBuilder.Append(In);
            stringBuilder.Append(" ");
            stringBuilder.Append(GetWord(Amount));
            stringBuilder.Append(" ");
            stringBuilder.Append(OfTheFollowingSkills);
            stringBuilder.Append("\n");
            foreach (var stat in Stats) {
                stringBuilder.Append(GetStatString(unit, stat));
            }
            return stringBuilder.ToString();
        }

        private int GetStatValue(UnitDescriptor unit, StatType stat) {
            int num = stat.IsSkill() ? unit.Stats.GetStat(stat).BaseValue : unit.Stats.GetStat(stat).PermanentValue;
            foreach (ReplaceStatForPrerequisites replaceStatForPrerequisites in unit.Progression.Features.SelectFactComponents<ReplaceStatForPrerequisites>()) {
                if (replaceStatForPrerequisites.OldStat == stat) {
                    num = ReplaceStatForPrerequisites.ResultStat(replaceStatForPrerequisites, num, unit, false);
                }
            }
            return num;
        }

        private string GetStatString(UnitDescriptor unit, StatType stat) {
            StringBuilder stringBuilder = new StringBuilder();
            string statString = LocalizedTexts.Instance.Stats.GetText(stat);
            stringBuilder.Append(statString);
            if (unit != null) {
                stringBuilder.Append(": ");
                stringBuilder.Append(string.Format(UIStrings.Instance.Tooltips.CurrentValue, GetStatValue(unit, stat)));
                stringBuilder.Append("\n");
            }
            if (GetStatValue(unit, stat) >= Value) {
                return $"<color=#323545>{stringBuilder}</color>";
            }
            return stringBuilder.ToString();
        }

        private string GetWord(int i) {
            return i switch {
                1 => One,
                2 => Two,
                3 => Three,
                4 => Four,
                5 => Five,
                6 => Six,
                7 => Seven,
                8 => Eight,
                9 => Nine,
                _ => i.ToString(),
            };
        }
        /// <summary>
        /// Stats to be checked.
        /// </summary>
        public StatType[] Stats;
        /// <summary>
        /// Required value of the stat.
        /// </summary>
        public int Value;
        /// <summary>
        /// Required amount of stats that must meet requirments.
        /// </summary>
        public int Amount;
    }
}
