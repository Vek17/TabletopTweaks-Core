using Kingmaker.PubSubSystem;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using TabletopTweaks.Core.Config;
using TabletopTweaks.Core.Localization;
using TabletopTweaks.Core.NewEvents;
using static UnityModManagerNet.UnityModManager;

namespace TabletopTweaks.Core.ModLogic {
    public abstract class ModContextBase : IBlueprintCacheInitHandler,
        IGlobalSubscriber, ISubscriber {
        public readonly ModEntry ModEntry;
        public readonly ModLogger Logger;
        public string BlueprintsFile = "Blueprints.json";
        public Blueprints Blueprints = new Blueprints();
        public MultiLocalizationPack ModLocalizationPack = new MultiLocalizationPack();
        public virtual string UserConfigFolder => ModEntry.Path + "UserSettings";
        public virtual string LocalizationFolder => ModEntry.Path + "Localization";
        public string LocalizationFile = "LocalizationPack.json";
        public bool Debug;
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
            EventBus.Subscribe(this);
        }

        public abstract void LoadAllSettings();
        public virtual void SaveAllSettings() {
            SaveSettings(BlueprintsFile, Blueprints);
        }
        public virtual void LoadBlueprints(string classPath, ModContextBase context) {
            LoadSettings(BlueprintsFile, classPath, ref Blueprints);
            Blueprints.Context = context;
        }
        public virtual void LoadLocalization(string classPath) {
            JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = $"{classPath}.{LocalizationFile}"; ;
            var localizationPath = $"{LocalizationFolder}{Path.DirectorySeparatorChar}{LocalizationFile}";
            Directory.CreateDirectory(LocalizationFolder);
            if (File.Exists(localizationPath)) {
                using (StreamReader streamReader = File.OpenText(localizationPath))
                using (JsonReader jsonReader = new JsonTextReader(streamReader)) {
                    try {
                        MultiLocalizationPack localization = serializer.Deserialize<MultiLocalizationPack>(jsonReader);
                        ModLocalizationPack = localization;
                    } catch {
                        ModLocalizationPack = new MultiLocalizationPack();
                        Logger.LogError("Failed to localization. Settings will be rebuilt.");
                        try { File.Copy(localizationPath, ModEntry.Path + $"{Path.DirectorySeparatorChar}BROKEN_{LocalizationFile}", true); } catch { Logger.LogError("Failed to archive broken localization."); }
                    }
                }
            } else {
                using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
                using (StreamReader streamReader = new StreamReader(stream))
                using (JsonReader jsonReader = new JsonTextReader(streamReader)) {
                    ModLocalizationPack = serializer.Deserialize<MultiLocalizationPack>(jsonReader);
                }
            }
            ModLocalizationPack.Context = this;
            EventBus.Subscribe(ModLocalizationPack);
        }
        public virtual void SaveLocalization(MultiLocalizationPack localizaiton) {
            localizaiton.Strings.Sort((x, y) => string.Compare(x.SimpleName, y.SimpleName));
            Directory.CreateDirectory(UserConfigFolder);
            var localizationPath = $"{LocalizationFolder}{Path.DirectorySeparatorChar}{LocalizationFile}";

            JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
            using (StreamWriter streamWriter = new StreamWriter(localizationPath))
            using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter)) {
                serializer.Serialize(jsonWriter, localizaiton);
            }
            Logger.Log($"Localization: {ModLocalizationPack.Strings.Count}");
        }
        public virtual void LoadSettings<T>(string fileName, string path, ref T setting) where T : IUpdatableSettings {
            JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
            var assembly = ModEntry.Assembly;
            var resourcePath = $"{path}.{fileName}";
            var userPath = $"{UserConfigFolder}{Path.DirectorySeparatorChar}{fileName}";

            Directory.CreateDirectory(UserConfigFolder);
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
                        Logger.LogError("Failed to load user settings. Settings will be rebuilt.");
                        try { File.Copy(userPath, UserConfigFolder + $"{Path.DirectorySeparatorChar}BROKEN_{fileName}", true); } catch { Logger.LogError("Failed to archive broken settings."); }
                    }
                }
            }
            SaveSettings(fileName, setting);
        }
        public virtual void SaveSettings(string fileName, object setting) {
            Directory.CreateDirectory(UserConfigFolder);
            var userPath = $"{UserConfigFolder}{Path.DirectorySeparatorChar}{fileName}";

            JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
            using (StreamWriter streamWriter = new StreamWriter(userPath))
            using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter)) {
                serializer.Serialize(jsonWriter, setting);
            }
        }

        public virtual void BeforeBlueprintCachePatches() {
        }

        public virtual void BeforeBlueprintCacheInit() {
        }

        public virtual void AfterBlueprintCacheInit() {
        }

        public virtual void AfterBlueprintCachePatches() {
            Blueprints.GenerateUnused();
            SaveSettings(BlueprintsFile, Blueprints);
        }
    }
}
