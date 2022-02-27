using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Localization;

namespace TabletopTweaks.Core.Utilities {
    /// <summary>
    /// Collection of extentions for interacting with localized strings.
    /// </summary>
    public static class LocalizationExtensions {
        /// <summary>
        /// Updates the name of the archetype to the supplied string.
        /// </summary>
        /// <param name="archetype"></param>
        /// <param name="name">
        /// Text to set as the name.
        /// </param>
        public static void SetName(this BlueprintArchetype archetype, string name) {
            archetype.LocalizedName = Helpers.CreateString($"{archetype.name}.Name", name, shouldProcess: false);
        }
        /// <summary>
        /// Updates the description of the archetype to the supplied string.
        /// </summary>
        /// <param name="archetype"></param>
        /// <param name="description">
        /// Text to set as the description.
        /// </param>
        public static void SetDescription(this BlueprintArchetype archetype, string description) {
            archetype.LocalizedDescription = Helpers.CreateString($"{archetype.name}.Description", description, shouldProcess: true);
        }
        /// <summary>
        /// Updates the name of the item to the supplied string.
        /// </summary>
        /// <param name="archetype"></param>
        /// <param name="name">
        /// Text to set as the name.
        /// </param>
        public static void SetName(this BlueprintItem item, string name) {
            item.m_DisplayNameText = Helpers.CreateString($"{item.name}.Name", name, shouldProcess: false);
        }
        /// <summary>
        /// Updates the description of the item to the supplied string.
        /// </summary>
        /// <param name="archetype"></param>
        /// <param name="description">
        /// Text to set as the description.
        /// </param>
        public static void SetDescription(this BlueprintItem item, string description) {
            item.m_DescriptionText = Helpers.CreateString($"{item.name}.Description", description, shouldProcess: true);
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
            enchantment.SetPrefix(prefix);
            enchantment.SetSuffix(suffix);
        }
        /// <summary>
        /// Updates the name of the enchantment to the supplied string.
        /// </summary>
        /// <param name="enchantment"></param>
        /// <param name="name">
        /// Text to set as the name.
        /// </param>
        public static void SetName(this BlueprintItemEnchantment enchantment, string name) {
            enchantment.m_EnchantName = Helpers.CreateString($"{enchantment.name}.Name", name);
        }
        /// <summary>
        /// Updates the description of the enchantment to the supplied string.
        /// </summary>
        /// <param name="enchantment"></param>
        /// <param name="description">
        /// Text to set as the description.
        /// </param>
        public static void SetDescription(this BlueprintItemEnchantment enchantment, string description) {
            enchantment.m_Description = Helpers.CreateString($"{enchantment.name}.Description", description, shouldProcess: true);
        }
        /// <summary>
        /// Updates the prefix of the enchantment to the supplied string.
        /// </summary>
        /// <param name="enchantment"></param>
        /// <param name="prefix">
        /// Text to set as the prefix.
        /// </param>
        public static void SetPrefix(this BlueprintItemEnchantment enchantment, string prefix) {
            enchantment.m_Prefix = Helpers.CreateString($"{enchantment.name}.Prefix", prefix);
        }
        /// <summary>
        /// Updates the suffix of the enchantment to the supplied string.
        /// </summary>
        /// <param name="enchantment"></param>
        /// <param name="suffix">
        /// Text to set as the suffix.
        /// </param>
        public static void SetSuffix(this BlueprintItemEnchantment enchantment, string suffix) {
            enchantment.m_Suffix = Helpers.CreateString($"{enchantment.name}.Suffix", suffix);
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
            feature.SetName(name);
            feature.SetDescription(description);
        }
        /// <summary>
        /// Updates the name and description of the feature to match the supplied fact's.
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="other">
        /// Fact to copy the m_DisplayName and m_Description of.
        /// </param>
        public static void SetNameDescription(this BlueprintUnitFact feature, BlueprintUnitFact other) {
            feature.m_DisplayName = other.m_DisplayName;
            feature.m_Description = other.m_Description;
        }
        /// <summary>
        /// Updates the name of the feature to the supplied localized string.
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="name">
        /// String to use as the name.
        /// </param>
        public static void SetName(this BlueprintUnitFact feature, LocalizedString name) {
            feature.m_DisplayName = name;
        }
        /// <summary>
        /// Updates the name of the feature to the supplied string.
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="name">
        /// Text to use as the name.
        /// </param>
        public static void SetName(this BlueprintUnitFact feature, string name) {
            feature.m_DisplayName = Helpers.CreateString($"{feature.name}.Name", name);
        }
        /// <summary>
        /// Updates the description of the feature to the supplied localized string.
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="description">
        /// String to use as the description.
        /// </param>
        public static void SetDescription(this BlueprintUnitFact feature, LocalizedString description) {
            feature.m_Description = description;
        }
        /// <summary>
        /// Updates the description of the feature to the supplied string.
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="description">
        /// Text to use as the description.
        /// </param>
        public static void SetDescription(this BlueprintUnitFact feature, string description) {
            feature.m_Description = Helpers.CreateString($"{feature.name}.Description", description, shouldProcess: true);
        }
    }
}
