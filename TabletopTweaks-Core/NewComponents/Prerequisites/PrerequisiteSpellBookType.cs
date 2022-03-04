using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.Localization;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Text;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.NewComponents.Prerequisites {
    /// <summary>
    /// Requires the unit to be able to cast prepared or spontaneous spells of the specified level.
    /// </summary>
    [TypeId("1cdf85e299bf474d89e2ce827aa6ecbf")]
    public class PrerequisiteSpellBookType : Prerequisite {
        [InitializeStaticString]
        private static readonly LocalizedString CanCast = Helpers.CreateString(TTTContext, "PrerequisiteSpellBookType.UI", "Can cast");
        [InitializeStaticString]
        private static readonly LocalizedString SpellsOfLevel = Helpers.CreateString(TTTContext, "PrerequisiteSpellBookType.UI", "spells of level");
        [InitializeStaticString]
        private static readonly LocalizedString OrHigher = Helpers.CreateString(TTTContext, "PrerequisiteSpellBookType.UI", "or higher");

        public override bool CheckInternal(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state) {
            int? casterTypeSpellLevel = this.GetCasterTypeSpellLevel(unit);
            return (casterTypeSpellLevel.GetValueOrDefault() >= RequiredSpellLevel) && (casterTypeSpellLevel != null);
        }

        public override string GetUITextInternal(UnitDescriptor unit) {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(CanCast);
            stringBuilder.Append(" ");
            stringBuilder.Append(Type.ToString());
            stringBuilder.Append(" ");
            stringBuilder.Append(SpellsOfLevel);
            stringBuilder.Append(" ");
            stringBuilder.Append(RequiredSpellLevel);
            stringBuilder.Append(" ");
            stringBuilder.Append(OrHigher);
            int? casterTypeSpellLevel = this.GetCasterTypeSpellLevel(unit);
            if (unit != null && casterTypeSpellLevel != null) {
                stringBuilder.Append("\n");
                stringBuilder.Append(string.Format(UIStrings.Instance.Tooltips.CurrentValue, casterTypeSpellLevel));
            }
            return stringBuilder.ToString();
        }

        private int? GetCasterTypeSpellLevel(UnitDescriptor unit) {
            foreach (ClassData classData in unit.Progression.Classes) {
                BlueprintSpellbook spellbook = classData.Spellbook;
                if (spellbook == null) { continue; }
                var correctType = Type switch {
                    SpellbookType.Prepared => !spellbook.Spontaneous || spellbook.IsArcanist,
                    SpellbookType.Spontaneous => spellbook.Spontaneous || !spellbook.IsArcanist,
                    _ => false
                };
                if (!spellbook.IsMythic && !spellbook.IsAlchemist && correctType) {
                    return new int?(unit.DemandSpellbook(classData.CharacterClass).MaxSpellLevel);
                }
            }
            return null;
        }
        public enum SpellbookType : int {
            Prepared,
            Spontaneous
        }
        /// <summary>
        /// Type of spellbook to require.
        /// </summary>
        public SpellbookType Type;
        /// <summary>
        /// Spell level to require to be castable.
        /// </summary>
        public int RequiredSpellLevel;
    }
}
