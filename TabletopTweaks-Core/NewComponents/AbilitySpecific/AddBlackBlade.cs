using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using TabletopTweaks.Core.NewUnitParts;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    /// <summary>
    /// Adds a black blade to the fact owner that is assosiated with thier UnitPartBlackBlade.
    /// </summary>
    [TypeId("446f9e9a24684cbdb77e3b270af7b5dc")]
    public class AddBlackBlade : UnitFactComponentDelegate, IUnitLevelUpHandler {

        public override void OnTurnOn() {

        }

        public void HandleUnitBeforeLevelUp(UnitEntityData unit) {
        }

        public void HandleUnitAfterLevelUp(UnitEntityData unit, LevelUpController controller) {
            //TTTContext.Logger.Log($"Mode: {controller.State.Mode}");
            var part = base.Owner.Ensure<UnitPartBlackBlade>();
            part.AddBlackBlade(BlackBlade, base.Context, base.Fact);
        }

        public BlueprintItemWeaponReference BlackBlade;
    }
}
