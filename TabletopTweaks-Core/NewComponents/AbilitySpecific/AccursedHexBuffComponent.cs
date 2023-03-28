using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using System;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using TabletopTweaks.Core.NewUnitParts;
using Kingmaker.Blueprints.Classes.Spells;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [AllowMultipleComponents]
    [TypeId("be94df924525444390b9d96fe2edf2ee")]
    public class AccursedHexBuffComponent : UnitBuffComponentDelegate,
        IInitiatorRulebookHandler<RuleRollD20>,
        IRulebookHandler<RuleRollD20>,
        IInitiatorRulebookHandler<RuleSavingThrow>,
        IRulebookHandler<RuleSavingThrow> {

        public void OnEventAboutToTrigger(RuleRollD20 evt) {
            var UnitPartAccursedHex = this.Context.MaybeCaster ?.Get<UnitPartAccursedHexTTT>();
            if (UnitPartAccursedHex == null) { return; }

            var previousEvent = Rulebook.CurrentContext.PreviousEvent as RuleSavingThrow;
            var spellGUID = previousEvent?.Reason?.Ability?.Blueprint?.AssetGuid ?? BlueprintGuid.Empty;
            if (previousEvent == null) { return; }
            if (spellGUID == BlueprintGuid.Empty) { return; }
            if (UnitPartAccursedHex.ShouldTriggerReroll(spellGUID, evt.Initiator)) {
                evt.AddReroll(1, false, UnitPartAccursedHex.MythicFact);
                UnitPartAccursedHex.RemoveTriggering(spellGUID, evt.Initiator);
            }
        }

        public void OnEventDidTrigger(RuleRollD20 evt) {
        }

        public void OnEventAboutToTrigger(RuleSavingThrow evt) {
        }

        public void OnEventDidTrigger(RuleSavingThrow evt) {
            Main.TTTContext.Logger.Log($"AccursedHexBuffComponent: OnEventDidTrigger(RuleSavingThrow evt)");
            var UnitPartAccursedHex = this.Context.MaybeCaster?.Get<UnitPartAccursedHexTTT>();
            if (UnitPartAccursedHex == null) { return; }
            Main.TTTContext.Logger.Log($"AccursedHexBuffComponent: UnitPartAccursedHex Found");
            var spellBlueprint = evt?.Reason?.Ability?.Blueprint;
            if (spellBlueprint == null) { return; }
            Main.TTTContext.Logger.Log($"AccursedHexBuffComponent: Blueprint is {spellBlueprint.name}");
            if ((spellBlueprint.SpellDescriptor & SpellDescriptor.Hex) == 0) { return; }
            Main.TTTContext.Logger.Log($"AccursedHexBuffComponent: Blueprint is Hex");
            if (evt.Success) {
                Main.TTTContext.Logger.Log($"AccursedHexBuffComponent: TrySet: {spellBlueprint.AssetGuid} - Unit {evt.Initiator.UniqueId} - Passed");
                UnitPartAccursedHex.SetPassedSave(spellBlueprint.AssetGuid, evt.Initiator);
            }
        }
    }
}
