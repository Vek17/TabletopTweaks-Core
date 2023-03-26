using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Core.ModLogic;

namespace TabletopTweaks.Core.Utilities {
    /// <summary>
    /// Collection of extentions for interacting with localized strings.
    /// </summary>
    public static class LocalizationExtensions {
        /// <summary>
        /// Updates the name of the archetype to the supplied string.
        /// </summary>
        /// <param name="archetype"></param>
        /// <param name="modContext">
        /// Mod to create the localized string in.
        /// </param>
        /// <param name="name">
        /// Text to set as the name.
        /// </param>
        public static void SetName(this BlueprintArchetype archetype, ModContextBase modContext, string name) {
            archetype.LocalizedName = Helpers.CreateString(modContext, $"{archetype.name}.Name", name, shouldProcess: false);
        }
        /// <summary>
        /// Updates the name of the archetype to the supplied string.
        /// </summary>
        /// <param name="archetype"></param>
        /// <param name="name">
        /// Text to set as the name.
        /// </param>
        public static void SetName(this BlueprintArchetype archetype, LocalizedString name) {
            archetype.LocalizedName = name;
        }
        /// <summary>
        /// Updates the description of the archetype to the supplied string.
        /// </summary>
        /// <param name="archetype"></param>
        /// <param name="description">
        /// Text to set as the description.
        /// </param>
        public static void SetDescription(this BlueprintArchetype archetype, LocalizedString description) {
            archetype.LocalizedDescription = description;
        }
        /// <summary>
        /// Updates the description of the archetype to the supplied string.
        /// </summary>
        /// <param name="archetype"></param>
        /// <param name="modContext">
        /// Mod to create the localized string in.
        /// </param>
        /// <param name="description">
        /// Text to set as the description.
        /// </param>
        public static void SetDescription(this BlueprintArchetype archetype, ModContextBase modContext, string description) {
            archetype.LocalizedDescription = Helpers.CreateString(modContext, $"{archetype.name}.Description", description, shouldProcess: true);
        }
        /// <summary>
        /// Updates the name of the item to the supplied string.
        /// </summary>
        /// <param name="archetype"></param>
        /// <param name="name">
        /// Text to set as the name.
        /// </param>
        public static void SetName(this BlueprintItem item, ModContextBase modContext, string name) {
            item.m_DisplayNameText = Helpers.CreateString(modContext, $"{item.name}.Name", name, shouldProcess: false);
        }
        /// <summary>
        /// Updates the description of the item to the supplied string.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="modContext">
        /// Mod to create the localized string in.
        /// </param> 
        /// <param name="description">
        /// Text to set as the description.
        /// </param>
        public static void SetDescription(this BlueprintItem item, ModContextBase modContext, string description) {
            item.m_DescriptionText = Helpers.CreateString(modContext, $"{item.name}.Description", description, shouldProcess: true);
        }
        /// <summary>
        /// Updates the prefix and suffix of the enchantment to the supplied strings.
        /// </summary>
        /// <param name="enchantment"></param>
        /// <param name="modContext">
        /// Mod to create the localized string in.
        /// </param>
        /// <param name="prefix">
        /// Text to use as the enchantment's prefix.
        /// </param>
        /// <param name="suffix">
        /// Text to use as the enchantment's suffix.
        /// </param>
        public static void UpdatePrefixSuffix(this BlueprintItemEnchantment enchantment, ModContextBase modContext, string prefix, string suffix) {
            enchantment.SetPrefix(modContext, prefix);
            enchantment.SetSuffix(modContext, suffix);
        }
        /// <summary>
        /// Updates the name of the enchantment to the supplied string.
        /// </summary>
        /// <param name="enchantment"></param>
        /// <param name="modContext">
        /// Mod to create the localized string in.
        /// </param>
        /// <param name="name">
        /// Text to set as the name.
        /// </param>
        public static void SetName(this BlueprintItemEnchantment enchantment, ModContextBase modContext, string name) {
            enchantment.m_EnchantName = Helpers.CreateString(modContext, $"{enchantment.name}.Name", name);
        }
        /// <summary>
        /// Updates the description of the enchantment to the supplied string.
        /// </summary>
        /// <param name="enchantment"></param>
        /// <param name="modContext">
        /// Mod to create the localized string in.
        /// </param>
        /// <param name="description">
        /// Text to set as the description.
        /// </param>
        public static void SetDescription(this BlueprintItemEnchantment enchantment, ModContextBase modContext, string description) {
            enchantment.m_Description = Helpers.CreateString(modContext, $"{enchantment.name}.Description", description, shouldProcess: true);
        }
        /// <summary>
        /// Updates the prefix of the enchantment to the supplied string.
        /// </summary>
        /// <param name="enchantment"></param>
        /// <param name="modContext">
        /// Mod to create the localized string in.
        /// </param>
        /// <param name="prefix">
        /// Text to set as the prefix.
        /// </param>
        public static void SetPrefix(this BlueprintItemEnchantment enchantment, ModContextBase modContext, string prefix) {
            enchantment.m_Prefix = Helpers.CreateString(modContext, $"{enchantment.name}.Prefix", prefix);
        }
        /// <summary>
        /// Updates the suffix of the enchantment to the supplied string.
        /// </summary>
        /// <param name="enchantment"></param>
        /// <param name="suffix">
        /// Text to set as the suffix.
        /// </param>
        public static void SetSuffix(this BlueprintItemEnchantment enchantment, ModContextBase modContext, string suffix) {
            enchantment.m_Suffix = Helpers.CreateString(modContext, $"{enchantment.name}.Suffix", suffix);
        }
        /// <summary>
        /// Updates the name and description of the feature to the supplied strings.
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="modContext">
        /// Mod to create the localized string in.
        /// </param>
        /// <param name="name">
        /// Text to set as the name.
        /// </param>
        /// <param name="description">
        /// Text to set as the description.
        /// </param>
        public static void SetNameDescription(this BlueprintUnitFact feature, ModContextBase modContext, string name, string description) {
            feature.SetName(modContext, name);
            feature.SetDescription(modContext, description);
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
        /// <param name="modContext">
        /// Mod to create the localized string in.
        /// </param>
        /// <param name="name">
        /// Text to use as the name.
        /// </param>
        public static void SetName(this BlueprintUnitFact feature, ModContextBase modContext, string name) {
            feature.m_DisplayName = Helpers.CreateString(modContext, $"{feature.name}.Name", name);
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
        /// <param name="modContext">
        /// Mod to create the localized string in.
        /// </param>
        /// <param name="description">
        /// Text to use as the description.
        /// </param>
        public static void SetDescription(this BlueprintUnitFact feature, ModContextBase modContext, string description) {
            feature.m_Description = Helpers.CreateString(modContext, $"{feature.name}.Description", description, shouldProcess: true);
        }
        /// <summary>
        /// Updates the description of the feature to the supplied string.
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="modContext">
        /// Mod to create the localized string in.
        /// </param>
        /// <param name="duration">
        /// Text to use as the duration.
        /// </param>
        public static void SetLocalizedDuration(this BlueprintAbility ability, ModContextBase modContext, string duration) {
            ability.LocalizedDuration = Helpers.CreateString(modContext, $"{ability.name}.Duration", duration, shouldProcess: false);
        }
        /// <summary>
        /// Updates the description of the feature to the supplied localized string.
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="duration">
        /// String to use as the duration.
        /// </param>
        public static void SetLocalizedDuration(this BlueprintAbility ability, LocalizedString duration) {
            ability.LocalizedDuration = duration;
        }
        /// <summary>
        /// Updates the description of the feature to the supplied string.
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="modContext">
        /// Mod to create the localized string in.
        /// </param>
        /// <param name="savingThrow">
        /// Text to use as the saving throw.
        /// </param>
        public static void SetLocalizedSavingThrow(this BlueprintAbility ability, ModContextBase modContext, string savingThrow) {
            ability.LocalizedSavingThrow = Helpers.CreateString(modContext, $"{ability.name}.SavingThrow", savingThrow, shouldProcess: false);
        }
        /// <summary>
        /// Updates the description of the feature to the supplied localized string.
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="savingThrow">
        /// String to use as the saving throw.
        /// </param>
        public static void SetLocalizedSavingThrow(this BlueprintAbility ability, LocalizedString savingThrow) {
            ability.LocalizedSavingThrow = savingThrow;
        }
    }
}
