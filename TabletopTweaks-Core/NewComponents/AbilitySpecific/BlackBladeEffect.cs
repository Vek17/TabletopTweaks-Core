using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Newtonsoft.Json;
using TabletopTweaks.Core.NewUnitParts;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    /// <summary>
    /// Applies the supplied Enchantment to the owner's black blade.
    /// </summary>
    [TypeId("aac8d3adce2c4251ba6531b47d099186")]
    public class BlackBladeEffect : UnitFactComponentDelegate<BlackBladeEffect.AppliedEnchantData> {
        public override void OnTurnOn() {
            var part = base.Owner.Get<UnitPartBlackBlade>();
            if (part == null) { return; }
            Data.EnchantID = part.ApplyEnchantment(Enchantment, base.Context).UniqueId;
        }
        public override void OnTurnOff() {
            var part = base.Owner.Get<UnitPartBlackBlade>();
            if (part == null) { return; }
            if (!string.IsNullOrEmpty(Data.EnchantID)) {
                part.RemoveEnchantment(Data.EnchantID);
            }
            Data.EnchantID = null;
        }
        /// <summary>
        /// Enchantment to be applied to the owner's black blade.
        /// </summary>
        public BlueprintWeaponEnchantmentReference Enchantment;

        public class AppliedEnchantData {
            /// <summary>
            /// Id assosiated with the applied enchantment on the black blade.
            /// </summary>
            [JsonProperty]
            public string EnchantID;
        }
    }
}
