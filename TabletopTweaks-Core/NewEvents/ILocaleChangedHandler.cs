using HarmonyLib;
using Kingmaker.Localization;
using Kingmaker.PubSubSystem;

namespace TabletopTweaks.Core.NewEvents {
    public interface ILocaleChangedHandler : IGlobalSubscriber {
        void HandleLocaleChanged();

        [HarmonyPatch(typeof(LocalizationManager), nameof(LocalizationManager.OnLocaleChanged))]
        internal static class LocalizationManager_OnLocaleChanged_Handler {
            static void Postfix() {
                EventBus.RaiseEvent<ILocaleChangedHandler>(h => h.HandleLocaleChanged());
            }
        }
    }
}
