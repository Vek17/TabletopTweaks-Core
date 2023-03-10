using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Parts;
using static TabletopTweaks.Core.NewComponents.SetPetMinimumStat;

namespace TabletopTweaks.Core.NewComponents {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowMultipleComponents]
    [TypeId("e248fa7c86974b30b0954ce4cee5050b")]
    public class SetPetMinimumStat : UnitFactComponentDelegate<SetPetMinimumStatData>, IPartyHandler, IGlobalSubscriber, ISubscriber {
        public override void OnActivate() {
            this.TryAdd();
        }

        public override void OnDeactivate() {
            this.TryRemove();
        }

        public override void OnTurnOn() {
            base.OnTurnOn();
            this.TryAdd();
        }

        private void TryAdd() {
            foreach (EntityPartRef<UnitEntityData, UnitPartPet> entityPartRef in base.Owner.Pets) {
                UnitEntityData entity = entityPartRef.Entity;
                UnitPartPet unitPartPet = entity?.Get<UnitPartPet>();
                if (unitPartPet != null && unitPartPet.Type == this.PetType) {
                    var petStat = entityPartRef.Entity.Descriptor.Stats.GetStat(Stat);
                    var newValue = Value.Calculate(base.Context);
                    if (petStat.BaseValue < newValue) {
                        base.Data.statAdjustment = newValue - petStat.BaseValue;
                        petStat.BaseValue = newValue;
                    }
                }
            }
        }

        private void TryRemove() {
            foreach (EntityPartRef<UnitEntityData, UnitPartPet> entityPartRef in base.Owner.Pets) {
                UnitEntityData entity = entityPartRef.Entity;
                UnitPartPet unitPartPet = entity?.Get<UnitPartPet>();
                if (unitPartPet != null && unitPartPet.Type == this.PetType) {
                    var petStat = entityPartRef.Entity.Descriptor.Stats.GetStat(Stat);
                    if (base.Data.statAdjustment != 0) {
                        petStat.BaseValue = petStat.BaseValue - base.Data.statAdjustment;
                        base.Data.statAdjustment = 0;
                    }
                }
            }
        }

        public void HandleAddCompanion(UnitEntityData unit) {
            if (unit.IsPet && unit.Master == base.Owner) {
                this.TryAdd();
            }
        }

        public void HandleCompanionActivated(UnitEntityData unit) {
        }

        public void HandleCompanionRemoved(UnitEntityData unit, bool stayInGame) {
        }

        public void HandleCapitalModeChanged() {
        }
        public StatType Stat;
        public ContextValue Value;
        public PetType PetType;

        public class SetPetMinimumStatData{
            public int statAdjustment = 0;
        }
    }
}
