using Kingmaker.EntitySystem.Persistence.Versioning;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using TabletopTweaks.Core.Modlogic;

namespace TabletopTweaks.Core.Upgraders {
    internal class TabletopTweaksMigration : IJsonUpgrader {
        public bool NeedsPlayerPriorityLoad => false;
        private readonly ModContextBase Context;

        public TabletopTweaksMigration(ModContextBase context) {
            Context = context;
        }

        public bool WillUpgrade(string jsonName) {
            return true;
        }


        public void Upgrade(JObject root) {
            this.Root = root;
            Upgrade();
        }

        public void Upgrade() {
            Context.Logger.Log("Migrating from TabletopTweaks$ to TabletopTweaks-Core");
            foreach (JToken jtoken in this.Root.SelectTokens("..$type").ToList<JToken>()) {
                JValue jvalue = jtoken as JValue;
                string text = ((jvalue != null) ? jvalue.Value : null) as string;
                if (text != null && Regex.IsMatch(text, "TabletopTweaks$")/*text.Contains("TabletopTweaks")*/) {
                    text = Regex.Replace(text, "TabletopTweaks$", "TabletopTweaks-Core");
                    text = Regex.Replace(text, @"TabletopTweaks\.", "TabletopTweaks.Core.");
                    jvalue.Value = text;
                }
            }
        }

        protected void RemoveTypeMarkers(string type) {
            foreach (JToken jtoken in this.Root.SelectTokens("..$type").ToList<JToken>()) {
                JValue jvalue = jtoken as JValue;
                string text = ((jvalue != null) ? jvalue.Value : null) as string;
                if (text != null && text.Contains(type)) {
                    text = Regex.Replace(text, ", TabletopTweaks", ", TabletopTweaks-Core");
                    text = Regex.Replace(text, @"TabletopTweaks\.", "TabletopTweaks.Core.");
                    jvalue.Value = text;
                }
            }
        }

        private JObject Root;
    }
}
