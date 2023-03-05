using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.Core.NewComponents.OwlcatReplacements {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("8eb0ff68ec104810924118f4963fec9b")]
    public class FavoredEnemyTTT : UnitFactComponentDelegate {
        public ReferenceArrayProxy<BlueprintUnitFact, BlueprintUnitFactReference> CheckedFacts {
            get {
                return this.m_CheckedFacts;
            }
        }

        public override void OnTurnOn() {
            base.Owner.Ensure<UnitPartFavoredEnemy>().AddEntry(this.CheckedFacts.ToArray<BlueprintUnitFact>(), base.Fact, ValuePerRank * base.Fact.GetRank());
        }

        public override void OnTurnOff() {
            base.Owner.Ensure<UnitPartFavoredEnemy>().RemoveEntry(base.Fact);
        }

        [SerializeField]
        [FormerlySerializedAs("CheckedFacts")]
        public BlueprintUnitFactReference[] m_CheckedFacts;
        public int ValuePerRank = 2;
    }
}
