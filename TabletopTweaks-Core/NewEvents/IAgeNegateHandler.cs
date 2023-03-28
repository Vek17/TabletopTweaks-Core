using Kingmaker.PubSubSystem;

namespace TabletopTweaks.Core.NewEvents {
    public interface IAgeNegateHandler : IUnitSubscriber {
        void OnAgeNegateChanged();
    }
}
