using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System.Linq;

namespace TabletopTweaks.Core.NewActions {
    [TypeId("74a953732ee34497b79fe48728c69188")]
    public class ContextActionDisjointEnchantments : ContextAction {
        private BlueprintEquipmentEnchantment DisjointEnchantment => m_DisjointEnchantment?.Get();

        public override string GetCaption() {
            return string.Format("Disjoints equiped items");
        }

        public override void RunAction() {
            if (base.Target == null) { return; }

            Target.Unit.Descriptor.Body.AllSlots

                .Where(slot => slot.HasItem)
                .Select(slot => slot.Item)
                .Append(Target.Unit.Body.PrimaryHand?.MaybeWeapon?.Second)
                .Where(item => item != null)
                .Where(item => item.Blueprint is not BlueprintItemEquipmentUsable)
                .Where(item => item != null)
                .Distinct()
                .ForEach(Item => {
                    using (Context.GetDataScope(base.Target)) {
                        int dc = Context?.Params?.DC ?? 10;
                        RuleSavingThrow ruleSavingThrow = base.Context.TriggerRule(new RuleSavingThrow(base.Target.Unit, SavingThrowType.Will, dc));
                        if (!ruleSavingThrow.IsPassed) {
                            Item.AddEnchantment(DisjointEnchantment, base.Context, Duration.Calculate(base.Context));
                        }
                    }
                });
        }

        public BlueprintEquipmentEnchantmentReference m_DisjointEnchantment;
        public ContextDurationValue Duration;
    }
}
