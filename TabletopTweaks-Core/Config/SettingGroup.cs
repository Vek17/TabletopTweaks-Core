using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks.Core.Config {
    public class SettingGroup : IDisableableGroup {
        public bool DisableAll = false;
        public virtual bool GroupIsDisabled() => DisableAll;
        public virtual void SetGroupDisabled(bool value) => DisableAll = value;
        //[JsonProperty(ItemConverterType = typeof(SortedDictonaryConverter))]
        public SortedDictionary<string, SettingData> Settings = new SortedDictionary<string, SettingData>(StringComparer.InvariantCulture);
        public virtual bool this[string key] => IsEnabled(key);
        public bool IsExpanded = true;
        private bool hasDumpedKeys = false;

        public void LoadSettingGroup(SettingGroup group, bool frozen) {
            DisableAll = group.DisableAll;
            if (frozen) {
                this.Settings.Keys.ToList().ForEach(key => {
                    Settings[key].Enabled = false;
                });
            }
            this.DisableAll = group.DisableAll;
            group.Settings.ForEach(entry => {
                if (Settings.ContainsKey(entry.Key)) {
                    Settings[entry.Key].Enabled = entry.Value.Enabled;
                }
            });
        }
        public virtual bool IsEnabled(string key) {
            if (!Settings.TryGetValue(key, out SettingData result)) {
                if (!hasDumpedKeys) {
                    hasDumpedKeys = true;
                    Main.TTTContext.Logger.Log($"DUMPING KEYS: {key}");
                    foreach (var entry in Settings) {
                        Main.TTTContext.Logger.Log($"{entry.Key}");
                    }
                }
                Main.TTTContext.Logger.LogError($"COULD NOT FIND SETTING KEY: {key}");
                return false;
            }
            return result.Enabled && !GroupIsDisabled();
        }
        public virtual bool IsDisabled(string key) {
            return !IsEnabled(key);
        }

        public virtual void ChangeSetting(string key, bool value) {
            if (GroupIsDisabled()) {
                return;
            }
            Settings[key].Enabled = value;
        }

        ref bool ICollapseableGroup.IsExpanded() {
            return ref IsExpanded;
        }

        public void SetExpanded(bool value) {
            IsExpanded = value;
        }

        public class SettingData {
            public bool Enabled;
            public bool Homebrew;
            public string Description;

            public static implicit operator SettingData(bool enabled) {
                return new SettingData {
                    Enabled = enabled,
                    Description = string.Empty
                };
            }
        }
    }
}
