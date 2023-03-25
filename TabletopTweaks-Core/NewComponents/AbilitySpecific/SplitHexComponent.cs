using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using TabletopTweaks.Core.NewEvents;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Controllers.Units;
using System.Linq;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("00d2d2c9def641d3844a98a5a95f238e")]
    public class SplitHexComponent : UnitFactComponentDelegate<SplitHexComponent.SplitHexData>,
        IInitiatorRulebookHandler<RuleCastSpell>,
        IRulebookHandler<RuleCastSpell>,
        IAbilityGetCommandTypeHandler,
        ITickEachRound {

        private BlueprintAbility[] m_MajorHexes;
        private BlueprintAbility[] MajorHexes {
            get {
                if (m_MajorHexes == null) { 
                    m_MajorHexes = this.m_MajorHex?.Get()?.IsPrerequisiteFor
                        .Select(f => f.Get())
                        .SelectMany(c => c.GetComponents<AddFacts>())
                        .Where(c => c is not null)
                        .SelectMany(c => c.Facts)
                        .OfType<BlueprintAbility>()
                        .SelectMany(hex => hex.AbilityAndVariants())
                        .SelectMany(hex => hex.AbilityAndStickyTouch())
                        .Distinct()
                        .ToArray();
                }
                return m_MajorHexes;
            }
        }
        private BlueprintAbility[] m_GrandHexes;
        private BlueprintAbility[] GrandHexes {
            get {
                if (m_GrandHexes == null) {
                    m_GrandHexes = this.m_GrandHex?.Get()?.IsPrerequisiteFor
                        .Select(f => f.Get())
                        .SelectMany(c => c.GetComponents<AddFacts>())
                        .Where(c => c is not null)
                        .SelectMany(c => c.Facts)
                        .OfType<BlueprintAbility>()
                        .SelectMany(hex => hex.AbilityAndVariants())
                        .SelectMany(hex => hex.AbilityAndStickyTouch())
                        .Distinct()
                        .ToArray();
                }
                return m_GrandHexes;
            }
        }
        private BlueprintFeature SplitMajorHex => m_SplitMajorHex?.Get();

        public ReferenceArrayProxy<BlueprintFeature, BlueprintFeatureReference> Features {
            get {
                return this.m_MajorHex?.Get()?.IsPrerequisiteFor.ToArray();
            }
        }

        public void OnEventAboutToTrigger(RuleCastSpell evt) {
        }

        public void OnEventDidTrigger(RuleCastSpell evt) {
            if (!isValidTrigger(evt)) { return; }
            if (Data.HasStoredHex) {
                if (Data.StoredHex == evt.Spell.Blueprint) {
                    Data.Clear();
                }
            } else if(evt.Spell.Blueprint.SpellDescriptor.HasFlag(SpellDescriptor.Hex) && !evt.Spell.IsAOE)  {
                Data.Store(evt.Spell.Blueprint);
            }
        }

        public void HandleGetCommandType(AbilityData ability, ref UnitCommand.CommandType commandType) {
            if (Data.HasStoredHex && ability.Blueprint.AssetGuid == Data.StoredHex.AssetGuid) {

                commandType = UnitCommand.CommandType.Free;
            }
        }

        public void OnNewRound() {
            Data.Clear();
        }

        private bool isValidTrigger(RuleCastSpell evt) {
            return evt.Success
                && evt.Spell.Blueprint.SpellDescriptor.HasFlag(SpellDescriptor.Hex)
                && !evt.IsDuplicateSpellApplied
                && !evt.Spell.IsAOE
                && !GrandHexes.Any(hex => hex.AssetGuid == evt.Spell.Blueprint.AssetGuid)
                && (evt.Initiator.HasFact(SplitMajorHex) || !MajorHexes.Any(hex => hex.AssetGuid == evt.Spell.Blueprint.AssetGuid));
        }

        public BlueprintFeatureReference m_MajorHex;
        public BlueprintFeatureReference m_GrandHex;
        public BlueprintFeatureReference m_SplitMajorHex;

        public class SplitHexData {
            private BlueprintAbilityReference m_StoredHex;

            public bool HasStoredHex => !m_StoredHex?.IsEmpty() ?? false;
            public BlueprintAbility StoredHex => m_StoredHex?.Get();


            public void Store(BlueprintAbility hex) {
                m_StoredHex = hex.ToReference<BlueprintAbilityReference>();
            }

            public void Clear() {
                m_StoredHex = null;
            }
        }
    }
}
