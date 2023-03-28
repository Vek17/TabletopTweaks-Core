using Kingmaker.Blueprints;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;

namespace TabletopTweaks.Core.NewUnitParts {
    internal class UnitPartSplitHex : OldStyleUnitPart {

        public bool ValidTarget(BlueprintAbility ability, UnitEntityData target) {
            return !Data.HasStoredHex || !(Data.StoredHex.AssetGuid == ability.AssetGuid && Data.Unit.Equals(target));
        }

        public SplitHexData Data = new SplitHexData();

        public class SplitHexData : OldStyleUnitPart {
            private BlueprintAbilityReference m_StoredHex;
            public EntityRef<UnitEntityData> Unit;
            public bool HasStoredHex => !m_StoredHex?.IsEmpty() ?? false;
            public BlueprintAbility StoredHex => m_StoredHex?.Get();

            public void Store(BlueprintAbility hex, UnitEntityData unit) {
                m_StoredHex = hex.ToReference<BlueprintAbilityReference>();
                Unit = unit;
            }

            public void Clear() {
                m_StoredHex = null;
                Unit = new EntityRef<UnitEntityData>();
            }
        }
    }
}
