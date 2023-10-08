using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.NewEvents;
using TabletopTweaks.Core.Utilities;

namespace TabletopTweaks.Core.NewComponents {
    [TypeId("b522a7a4b3a44772bc5cfbdd55b1e0f9")]
    public class AddSpecificSpellConversion : UnitFactComponentDelegate, ISpontaneousConversionHandler {

        private BlueprintAbility TargetSpell { get => m_targetSpell?.Get(); }
        private BlueprintAbility ConvertSpell { get => m_convertSpell?.Get(); }

        public void HandleGetConversions(AbilityData ability, ref IEnumerable<AbilityData> conversions) {
            if (ability.Blueprint != TargetSpell) { return; }

            var conversionList = conversions.ToList();
            //RuleCollectMetamagic collectMetamagic = new RuleCollectMetamagic(ability.Spellbook, ability.Blueprint, ability.SpellLevel);
            //Rulebook.Trigger(collectMetamagic);

            AbilityData convertedAbility = new AbilityData(ability, ConvertSpell);
            convertedAbility.TemporaryContext(conversion => {
                conversion.DecorationBorderNumber = ability.DecorationBorderNumber;
                conversion.DecorationColorNumber = ability.DecorationColorNumber;
            });
            conversionList.Add(convertedAbility);

            conversions = conversionList;
        }
        /// <summary>
        /// Spell that will get a new conversion.
        /// </summary>
        public BlueprintAbilityReference m_targetSpell;
        /// <summary>
        /// Conversion that will be added.
        /// </summary>
        public BlueprintAbilityReference m_convertSpell;

    }
}
