using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Localization;
using Kingmaker.Localization.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using static TabletopTweaks.Core.Main;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.NewEvents;
using Kingmaker.PubSubSystem;
using TabletopTweaks.Core.Modlogic;

namespace TabletopTweaks.Core.Localization {
    /// <summary>
    /// Contains and manages localization for all available langauges.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class MultiLocalizationPack : ILocaleChangedHandler,
        IBlueprintCacheInitHandler,
        IGlobalSubscriber, ISubscriber {

        public MultiLocalizationPack() { }
        public MultiLocalizationPack(ModContextBase context) {
            this.Context = context;
        }
        /// <summary>
        /// Dictionary of extisting strings of enGB text and corresponding MultiLocaleStrings.
        /// </summary>
        public SortedDictionary<string, MultiLocaleString> Text {
            get {
                if (text == null) {
                    text = new SortedDictionary<string, MultiLocaleString>();
                    foreach (var entry in Strings) {
                        text[entry.enGB] = entry;
                    }
                }
                return text;
            }
        }
        /// <summary>
        /// Dictionary of extisting LocalizedString Ids and corresponding MultiLocaleStrings.
        /// </summary>
        public SortedDictionary<string, MultiLocaleString> Ids {
            get {
                if (ids == null) {
                    ids = new SortedDictionary<string, MultiLocaleString>();
                    foreach (var entry in Strings) {
                        ids[entry.Key] = entry;
                    }
                }
                return ids;
            }
        }
        /// <summary>
        /// Applies the contents of the MultiLocalizationPack to the current LocalizationPack in the LocalizationManager.
        /// </summary>
        public void ApplyToCurrentPack() {
            LocalizationManager.CurrentPack.AddStrings(GeneratePack());
            applied = true;
        }
        /// <summary>
        /// Clears data in Text and Ids.
        /// </summary>
        public void ResetCache() {
            ids = null;
            text = null;
        }
        /// <summary>
        /// Gets the MultiLocaleString assosiated with the entered text.
        /// </summary>
        /// <param name="text">
        /// Text to search for in the current cache.
        /// </param>
        /// <param name="result">
        /// MultiLocaleString that will contain the result if the text exists.
        /// </param>
        /// <param name="locale"></param>
        /// <returns>
        /// true if the text exists in the current cache, otherwise false.
        /// </returns>
        public bool TryGetText(string text, out MultiLocaleString result, Locale locale = Locale.enGB) {
            return Text.TryGetValue(text, out result);
        }
        /// <summary>
        /// Adds the MultiLocaleString to the MultiLocalizationPack as well as the CurrentPack in the LocalizationManager.
        /// </summary>
        /// <param name="newString">
        /// MultiLocaleString to be added to the pack.
        /// </param>
        public void AddString(MultiLocaleString newString) {
            Ids[newString.Key] = newString;
            Text[newString.StringEntry(applied ? LocalizationManager.CurrentLocale : Locale.enGB).Text] = newString;
            Strings.Add(newString);
            if (applied) {
                LocalizationManager.CurrentPack.m_Strings[newString.Key] = newString.StringEntry(LocalizationManager.CurrentLocale);
            }
        }
        /// <summary>
        /// Generates a new LocalizationPack based on the contents of the MultiLocalizationPack.
        /// </summary>
        /// <returns>
        /// New LocalizationPack created with the contents of the MultiLocalizationPack.
        /// </returns>
        private LocalizationPack GeneratePack() {
            var pack = new LocalizationPack {
                Locale = LocalizationManager.CurrentPack.Locale,
                m_Strings = new Dictionary<string, LocalizationPack.StringEntry>()
            };
            foreach (var entry in Strings) {
                pack.m_Strings[entry.Key] = entry.StringEntry(pack.Locale);
            }
            return pack;
        }

        public void HandleLocaleChanged() {
            this.ResetCache();
            this.ApplyToCurrentPack();
        }

        public void BeforeBlueprintCacheInit() {
            this.ApplyToCurrentPack();
        }

        public void AfterBlueprintCacheInit() {
        }

        public void BeforeBlueprintCachePatches() {
        }

        public void AfterBlueprintCachePatches() {
            Context.SaveLocalization(this);
        }

        /// <summary>
        /// All MultiLocaleStrings in the MultiLocalizationPack.
        /// </summary>
        [NotNull]
        [JsonProperty(PropertyName = "LocalizedStrings")]
        public List<MultiLocaleString> Strings = new List<MultiLocaleString>();
        private SortedDictionary<string, MultiLocaleString> text;
        private SortedDictionary<string, MultiLocaleString> ids;
        private bool applied = false;
        /// <summary>
        /// Context used to load the pack.
        /// </summary>
        public ModContextBase Context;
        /// <summary>
        /// Contains key used for LocalizedString as well as localized text for all supported lanagues.
        /// </summary>
        [JsonObject(MemberSerialization.OptIn)]
        public class MultiLocaleString {
            /// <summary>
            /// Key used for the resulting LocalizedString.
            /// </summary>
            [JsonProperty]
            public string Key;
            /// <summary>
            /// Used for human reference in the localization pack.
            /// </summary>
            [JsonProperty]
            public string SimpleName;
            /// <summary>
            /// Determines if the text will be passed though the tagging system before being added to the current LocalizationPack.
            /// </summary>
            [JsonProperty]
            public bool ProcessTemplates;
#if DEBUG
            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
            [DefaultValue(true)]
            private bool IsUsed = false;
#endif
            /// <summary>
            /// English Text.
            /// </summary>
            [JsonProperty]
            public string enGB = "";
            /// <summary>
            /// Russian Text.
            /// </summary>
            [JsonProperty]
            public string ruRU;
            /// <summary>
            /// German Text.
            /// </summary>
            [JsonProperty]
            public string deDE;
            /// <summary>
            /// French Text.
            /// </summary>
            [JsonProperty]
            public string frFR;
            /// <summary>
            /// Chinese Text.
            /// </summary>
            [JsonProperty]
            public string zhCN;
            /// <summary>
            /// Spanish Text.
            /// </summary>
            [JsonProperty]
            public string esES;
            private LocalizedString m_LocalizedString;
            /// <summary>
            /// The LocalizedString representation of the the MultiLocaleString.
            /// </summary>
            public LocalizedString LocalizedString {
                get {
                    m_LocalizedString ??= new LocalizedString {
                        m_Key = Key,
                        m_ShouldProcess = ProcessTemplates
                    };
#if DEBUG
                    IsUsed = true;
#endif
                    return m_LocalizedString;
                }
            }
            public MultiLocaleString() { }
            /// <summary></summary>
            /// <param name="simpleName">
            /// Used for human reference in the localization pack.
            /// </param>
            /// <param name="text">
            /// Initial text to include in the MultiLocaleString.
            /// </param>
            /// <param name="shouldProcess">
            /// Determines if the resulting LocalizedString will be processed by the tagging system.
            /// </param>
            /// <param name="locale">
            /// Locale to use for the initial text.
            /// </param>
            public MultiLocaleString(string simpleName, string text, bool shouldProcess = false, Locale locale = Locale.enGB) {
                ProcessTemplates = shouldProcess;
                SimpleName = simpleName;
                Key = Guid.NewGuid().ToString("N");
                SetText(locale, text);
            }
            /// <summary>
            /// Sets to text for the specified locale.
            /// </summary>
            /// <param name="locale">
            /// Locale to set the text for.
            /// </param>
            /// <param name="text">
            /// Text to use for the specified locale.
            /// </param>
            public void SetText(Locale locale, string text) {
                switch (locale) {
                    case Locale.enGB:
                        enGB = text;
                        break;
                    case Locale.ruRU:
                        ruRU = text;
                        break;
                    case Locale.deDE:
                        deDE = text;
                        break;
                    case Locale.frFR:
                        frFR = text;
                        break;
                    case Locale.zhCN:
                        zhCN = text;
                        break;
                    case Locale.esES:
                        esES = text;
                        break;
                    default:
                        enGB = text;
                        break;
                }
            }
            /// <summary>
            /// Generates a new StringEntry of the supplied locale.
            /// </summary>
            /// <param name="locale">
            /// Locale to use the text of.
            /// </param>
            /// <returns>
            /// StringEntry containing the text present in specified locale, or Locale.enGB if the text in the specified locale is null.
            /// </returns>
            public LocalizationPack.StringEntry StringEntry(Locale locale = Locale.enGB) {
                string result;
                switch (locale) {
                    case Locale.enGB:
                        result = enGB;
                        break;
                    case Locale.ruRU:
                        result = ruRU;
                        break;
                    case Locale.deDE:
                        result = deDE;
                        break;
                    case Locale.frFR:
                        result = frFR;
                        break;
                    case Locale.zhCN:
                        result = zhCN;
                        break;
                    case Locale.esES:
                        result = esES;
                        break;
                    default:
                        result = enGB;
                        break;
                }
                if (string.IsNullOrEmpty(result)) {
                    result = enGB;
                }
                return new LocalizationPack.StringEntry {
                    Text = ProcessTemplates ? DescriptionTools.TagEncyclopediaEntries(result) : result
                };
            }
            public override string ToString() {
                return this.StringEntry(LocalizationManager.CurrentLocale).Text;
            }
            public override int GetHashCode() {
                return Key.GetHashCode() ^ enGB.GetHashCode();
            }
        }
    }
}
