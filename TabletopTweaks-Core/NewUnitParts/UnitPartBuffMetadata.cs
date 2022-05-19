using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using static TabletopTweaks.Core.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TabletopTweaks.Core.NewUnitParts {
    public class UnitPartBuffMetadata : UnitPart, IUnitBuffHandler {

        public void AddBuffEntry(Buff source, params CustomMechanicsFeature[] features) {
            if (AffectedBuffs.Any(b => b.Source == source)) {
                AffectedBuffs.First(b => b.Source == source).Features.UnionWith(features);
            } else {
                AffectedBuffs.Add(new BuffMetadata(source, features));
            }
        }

        public bool HasBuffWithFeature(Buff buff, CustomMechanicsFeature feature) {
            return AffectedBuffs.Any(b => b.Source == buff && b.HasFeature(feature));
        }

        public bool HasBuff(Buff buff) {
            return AffectedBuffs.Any(b => b.Source == buff);
        }

        public void HandleBuffDidAdded(Buff buff) {
        }

        public void HandleBuffDidRemoved(Buff buff) {
            AffectedBuffs.RemoveAll(data => data.Source == buff);
            TryRemove();
        }

        private void TryRemove() {
            if (!AffectedBuffs.Any()) { this.RemoveSelf(); }
        }
        [JsonProperty]
        private readonly List<BuffMetadata> AffectedBuffs = new();
        private class BuffMetadata {
            [JsonProperty]
            public readonly EntityFactRef<Buff> Source;
            [JsonProperty]
            public readonly HashSet<CustomMechanicsFeature> Features = new();

            public BuffMetadata() { }
            public BuffMetadata(Buff source, params CustomMechanicsFeature[] features) {
                Source = source;
                Features.UnionWith(features);
            }

            public bool HasFeature(CustomMechanicsFeature feature) {
                return Features.Any(f => f == feature);
            }
        }
    }
}
