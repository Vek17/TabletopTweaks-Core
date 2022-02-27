using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using TabletopTweaks.Core.Config;
using TabletopTweaks.Core.Localization;
using TabletopTweaks.Core.ModLogic;
using static UnityModManagerNet.UnityModManager;

namespace TabletopTweaks.Core.Modlogic {
    public abstract class ModContextBase {
        public readonly ModEntry ModEntry;
        public readonly ModLogger Logger;
        public Blueprints Blueprints;
        public MultiLocalizationPack ModLocalizationPack = new MultiLocalizationPack();
        public virtual string userConfigFolder => ModEntry.Path + "UserSettings";
        public virtual string localizationFolder => ModEntry.Path + "Localization";
        private static JsonSerializerSettings cachedSettings;
        private static JsonSerializerSettings SerializerSettings {
            get {
                if (cachedSettings == null) {
                    cachedSettings = new JsonSerializerSettings {
                        CheckAdditionalContent = false,
                        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                        DefaultValueHandling = DefaultValueHandling.Include,
                        FloatParseHandling = FloatParseHandling.Double,
                        Formatting = Formatting.Indented,
                        MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        NullValueHandling = NullValueHandling.Include,
                        ObjectCreationHandling = ObjectCreationHandling.Replace,
                        StringEscapeHandling = StringEscapeHandling.Default,
                    };
                }
                return cachedSettings;
            }
        }

        public ModContextBase(ModEntry modEntry) {
            Blueprints = new Blueprints();
            ModEntry = modEntry;
            Logger = new ModLogger(ModEntry);       
        }

        public abstract void LoadAllSettings();
        public virtual void LoadLocalization(string fileName, string path) {
            JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = $"{path}.{fileName}"; ;
            var localizationPath = $"{localizationFolder}{Path.DirectorySeparatorChar}{fileName}";
            Directory.CreateDirectory(localizationFolder);
            if (File.Exists(localizationPath)) {
                using (StreamReader streamReader = File.OpenText(localizationPath))
                using (JsonReader jsonReader = new JsonTextReader(streamReader)) {
                    try {
                        MultiLocalizationPack localization = serializer.Deserialize<MultiLocalizationPack>(jsonReader);
                        ModLocalizationPack = localization;
                    } catch {
                        ModLocalizationPack = new MultiLocalizationPack();
                        Main.Error("Failed to localization. Settings will be rebuilt.");
                        try { File.Copy(localizationPath, ModEntry.Path + $"{Path.DirectorySeparatorChar}BROKEN_{fileName}", true); } catch { Main.Error("Failed to archive broken localization."); }
                    }
                }
            } else {
                using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
                using (StreamReader streamReader = new StreamReader(stream))
                using (JsonReader jsonReader = new JsonTextReader(streamReader)) {
                    ModLocalizationPack = serializer.Deserialize<MultiLocalizationPack>(jsonReader);
                }
            }
        }
        public virtual void SaveLocalization(string fileName, MultiLocalizationPack localizaiton) {
            localizaiton.Strings.Sort((x, y) => string.Compare(x.SimpleName, y.SimpleName));
            Directory.CreateDirectory(userConfigFolder);
            var localizationPath = $"{localizationFolder}{Path.DirectorySeparatorChar}{fileName}";

            JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
            using (StreamWriter streamWriter = new StreamWriter(localizationPath))
            using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter)) {
                serializer.Serialize(jsonWriter, localizaiton);
            }
            Main.Log($"Localization: {ModLocalizationPack.Strings.Count}");
        }
        public virtual void LoadSettings<T>(string fileName, string path, ref T setting) where T : IUpdatableSettings {
            JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = $"{path}.{fileName}";
            var userPath = $"{userConfigFolder}{Path.DirectorySeparatorChar}{fileName}";

            Directory.CreateDirectory(userConfigFolder);
            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader streamReader = new StreamReader(stream))
            using (JsonReader jsonReader = new JsonTextReader(streamReader)) {
                setting = serializer.Deserialize<T>(jsonReader);
                setting.Init();
            }
            if (File.Exists(userPath)) {
                using (StreamReader streamReader = File.OpenText(userPath))
                using (JsonReader jsonReader = new JsonTextReader(streamReader)) {
                    try {
                        T userSettings = serializer.Deserialize<T>(jsonReader);
                        setting.OverrideSettings(userSettings);
                    } catch {
                        Main.Error("Failed to load user settings. Settings will be rebuilt.");
                        try { File.Copy(userPath, userConfigFolder + $"{Path.DirectorySeparatorChar}BROKEN_{fileName}", true); } catch { Main.Error("Failed to archive broken settings."); }
                    }
                }
            }
            SaveSettings(fileName, setting);
        }

        public virtual void SaveSettings(string fileName, object setting) {
            Directory.CreateDirectory(userConfigFolder);
            var userPath = $"{userConfigFolder}{Path.DirectorySeparatorChar}{fileName}";

            JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
            using (StreamWriter streamWriter = new StreamWriter(userPath))
            using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter)) {
                serializer.Serialize(jsonWriter, setting);
            }
        }
    }
}
