using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.Wrappers {
    internal static class LocalizationWrapper {
        /// <summary>
        /// Updates the name of the archetype to the supplied string.
        /// </summary>
        /// <param name="archetype"></param>
        /// <param name="name">
        /// Text to set as the name.
        /// </param>
        public static void SetName(this BlueprintArchetype archetype, string name) {
            archetype.SetName(TTTContext, name);
        }
        /// <summary>
        /// Updates the description of the archetype to the supplied string.
        /// </summary>
        /// <param name="archetype"></param>
        /// <param name="description">
        /// Text to set as the description.
        /// </param>
        public static void SetDescription(this BlueprintArchetype archetype, string description) {
            archetype.SetDescription(TTTContext, description);
        }
        /// <summary>
        /// Updates the name of the item to the supplied string.
        /// </summary>
        /// <param name="archetype"></param>
        /// <param name="name">
        /// Text to set as the name.
        /// </param>
        public static void SetName(this BlueprintItem item, string name) {
            item.SetName(TTTContext, name);
        }
        /// <summary>
        /// Updates the description of the item to the supplied string.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="description">
        /// Text to set as the description.
        /// </param>
        public static void SetDescription(this BlueprintItem item, string description) {
            item.SetDescription(TTTContext, description);
        }
        /// <summary>
        /// Updates the prefix and suffix of the enchantment to the supplied strings.
        /// </summary>
        /// <param name="enchantment"></param>
        /// <param name="prefix">
        /// Text to use as the enchantment's prefix.
        /// </param>
        /// <param name="suffix">
        /// Text to use as the enchantment's suffix.
        /// </param>
        public static void UpdatePrefixSuffix(this BlueprintItemEnchantment enchantment, string prefix, string suffix) {
            enchantment.UpdatePrefixSuffix(TTTContext, prefix, suffix);
        }
        /// <summary>
        /// Updates the name of the enchantment to the supplied string.
        /// </summary>
        /// <param name="enchantment"></param>
        /// <param name="name">
        /// Text to set as the name.
        /// </param>
        public static void SetName(this BlueprintItemEnchantment enchantment, string name) {
            enchantment.SetName(TTTContext, name);
        }
        /// <summary>
        /// Updates the description of the enchantment to the supplied string.
        /// </summary>
        /// <param name="enchantment"></param>
        /// <param name="description">
        /// Text to set as the description.
        /// </param>
        public static void SetDescription(this BlueprintItemEnchantment enchantment, string description) {
            enchantment.SetDescription(TTTContext, description);
        }
        /// <summary>
        /// Updates the prefix of the enchantment to the supplied string.
        /// </summary>
        /// <param name="enchantment"></param>
        /// <param name="prefix">
        /// Text to set as the prefix.
        /// </param>
        public static void SetPrefix(this BlueprintItemEnchantment enchantment, string prefix) {
            enchantment.SetPrefix(TTTContext, prefix);
        }
        /// <summary>
        /// Updates the suffix of the enchantment to the supplied string.
        /// </summary>
        /// <param name="enchantment"></param>
        /// <param name="suffix">
        /// Text to set as the suffix.
        /// </param>
        public static void SetSuffix(this BlueprintItemEnchantment enchantment, string suffix) {
            enchantment.SetSuffix(TTTContext, suffix);
        }
        /// <summary>
        /// Updates the name and description of the feature to the supplied strings.
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="name">
        /// Text to set as the name.
        /// </param>
        /// <param name="description">
        /// Text to set as the description.
        /// </param>
        public static void SetNameDescription(this BlueprintUnitFact feature, string name, string description) {
            feature.SetNameDescription(TTTContext, name, description);
        }
        /// <summary>
        /// Updates the name of the feature to the supplied string.
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="name">
        /// Text to use as the name.
        /// </param>
        public static void SetName(this BlueprintUnitFact feature, string name) {
            feature.SetName(TTTContext, name);
        }
        /// <summary>
        /// Updates the description of the feature to the supplied string.
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="description">
        /// Text to use as the description.
        /// </param>
        public static void SetDescription(this BlueprintUnitFact feature, string description) {
            feature.SetDescription(TTTContext, description);
        }
    }
}
