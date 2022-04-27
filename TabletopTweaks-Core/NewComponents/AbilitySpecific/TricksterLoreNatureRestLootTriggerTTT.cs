using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers.Rest;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;


namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowMultipleComponents]
    [TypeId("f6433147a6d44348b79355a96d581380")]
    public class TricksterLoreNatureRestLootTriggerTTT : UnitFactComponentDelegate,
        IRestFinishedHandler,
        IGlobalSubscriber, ISubscriber {

        private ReferenceArrayProxy<BlueprintItemEquipment, BlueprintItemEquipmentReference> LootList => m_LootList;

        public void HandleRestFinished(RestStatus status) {
            if (status.Result != RestResult.Success) {
                return;
            }
            List<BlueprintItemEquipment> LootResults = new List<BlueprintItemEquipment>();
            int timesRolled = 0;
            do {
                LootResults.Add(LootList
                        .Where(item => item.CR >= (CROffset + Owner.Progression.MythicLevel) && item.CR <= (CROffset + CRRange + Owner.Progression.MythicLevel))
                        .Where(item => item.Cost >= CostFloor)
                        .Random());
                timesRolled++;
            } while (timesRolled < TableRolls);
            var SortedLootResults = LootResults.OrderBy(o => o.Cost).ToStack();
            int addedItems = 0;
            do {
                if (!SortedLootResults.Any()) { return; }
                base.Owner.Inventory.Add(SortedLootResults.Pop());
                addedItems++;
            } while (addedItems < AddBest);
        }

        public BlueprintItemEquipmentReference[] m_LootList = new BlueprintItemEquipmentReference[0];
        public int TableRolls = 1;
        public int AddBest = 1;
        public int CROffset = 5;
        public int CRRange = 15;
        public int CostFloor = 20000;
    }
}
