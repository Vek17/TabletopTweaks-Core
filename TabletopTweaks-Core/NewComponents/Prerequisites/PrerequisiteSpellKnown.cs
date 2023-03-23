using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Localization;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Linq;
using System.Text;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.NewComponents.Prerequisites {
    /// <summary>
    /// Requires the unit to know the specified spell in one of thier spellbooks.
    /// </summary>
    [TypeId("e523c74b8da74fec91ae651138ec0ca0")]
    public class PrerequisiteSpellKnown : Prerequisite {
        [InitializeStaticString]
        private static readonly LocalizedString CanCastSpell = Helpers.CreateString(TTTContext, "PrerequisiteSpellKnown.UI", "Can cast spell:");

        private BlueprintAbility Spell {
            get {
                if (m_Spell == null) {
                    return null;
                }
                return m_Spell.Get();
            }
        }
        public override bool CheckInternal([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state) {
            var SpellIsKnown = unit.Spellbooks.Any(book => book.IsKnown(Spell));
            return SpellIsKnown || RequireSpellbook ? false : unit.Abilities.GetAbility(Spell) != null;
        }

        public override string GetUITextInternal(UnitDescriptor unit) {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(CanCastSpell);
            stringBuilder.Append(" ");
            stringBuilder.Append(Spell.Name);
            return stringBuilder.ToString();
        }
        /// <summary>
        /// Required spell.
        /// </summary>
        public BlueprintAbilityReference m_Spell;
        public bool RequireSpellbook = true;
    }
}
