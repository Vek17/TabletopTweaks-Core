using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Localization;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Linq;
using System.Text;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.NewComponents.Prerequisites {
    /// <summary>
    /// Requires a specific spellbook that can cast spells of the required level or higher.
    /// </summary>
    [TypeId("7686e2d0ab864daaaf01150c62741aba")]
    public class PrerequisiteSpellbook : Prerequisite {
        [InitializeStaticString]
        private static readonly LocalizedString HasSpellbook = Helpers.CreateString(TTTContext, "PrerequisiteSpellbook.UI", "Has spellbook:");
        [InitializeStaticString]
        private static readonly LocalizedString CanCastSpells = Helpers.CreateString(TTTContext, "PrerequisiteSpellbook.UI", "Can cast spells of level");
        [InitializeStaticString]
        private static readonly LocalizedString FromSpellbook = Helpers.CreateString(TTTContext, "PrerequisiteSpellbook.UI", "or higher from spellbook:");

        public override bool CheckInternal([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state) {
            return unit.Spellbooks
                .Where(book => book.Blueprint.AssetGuid.Equals(Spellbook.Guid))
                .Any(book => book.MaxSpellLevel >= RequiredSpellLevel);
        }

        public override string GetUITextInternal(UnitDescriptor unit) {
            StringBuilder stringBuilder = new StringBuilder();
            if (RequiredSpellLevel > 0) {
                stringBuilder.Append(CanCastSpells);
                stringBuilder.Append(" ");
                stringBuilder.Append(RequiredSpellLevel);
                stringBuilder.Append(" ");
                stringBuilder.Append(FromSpellbook);
                stringBuilder.Append(" ");
                stringBuilder.Append(Spellbook.Get().Name);
            } else {
                stringBuilder.Append(HasSpellbook);
                stringBuilder.Append(" ");
                stringBuilder.Append(Spellbook.Get().Name);
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// Spellbook that is required.
        /// </summary>
        [NotNull]
        public BlueprintSpellbookReference Spellbook;
        /// <summary>
        /// Required spell level the book can cast. 0 to ignore.
        /// </summary>
        public int RequiredSpellLevel;
    }
}
