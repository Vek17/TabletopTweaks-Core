using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using System.Linq;

namespace TabletopTweaks.Core.Utilities {
    public static class ClassTools {
        public static class Classes {
            public static BlueprintCharacterClass AlchemistClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("0937bec61c0dabc468428f496580c721");
            public static BlueprintCharacterClass ArcanistClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("52dbfd8505e22f84fad8d702611f60b7");
            public static BlueprintCharacterClass BarbarianClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("f7d7eb166b3dd594fb330d085df41853");
            public static BlueprintCharacterClass BardClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("772c83a25e2268e448e841dcd548235f");
            public static BlueprintCharacterClass BloodragerClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("d77e67a814d686842802c9cfd8ef8499");
            public static BlueprintCharacterClass CavalierClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("3adc3439f98cb534ba98df59838f02c7");
            public static BlueprintCharacterClass ClericClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("67819271767a9dd4fbfd4ae700befea0");
            public static BlueprintCharacterClass DruidClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("610d836f3a3a9ed42a4349b62f002e96");
            public static BlueprintCharacterClass FighterClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
            public static BlueprintCharacterClass HunterClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("34ecd1b5e1b90b9498795791b0855239");
            public static BlueprintCharacterClass InquisitorClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("f1a70d9e1b0b41e49874e1fa9052a1ce");
            public static BlueprintCharacterClass KineticistClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("42a455d9ec1ad924d889272429eb8391");
            public static BlueprintCharacterClass MagusClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("45a4607686d96a1498891b3286121780");
            public static BlueprintCharacterClass MonkClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("e8f21e5b58e0569468e420ebea456124");
            public static BlueprintCharacterClass OracleClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("20ce9bf8af32bee4c8557a045ab499b1");
            public static BlueprintCharacterClass PaladinClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("bfa11238e7ae3544bbeb4d0b92e897ec");
            public static BlueprintCharacterClass RangerClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("cda0615668a6df14eb36ba19ee881af6");
            public static BlueprintCharacterClass RogueClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("299aa766dee3cbf4790da4efb8c72484");
            public static BlueprintCharacterClass ShamanClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("145f1d3d360a7ad48bd95d392c81b38e");
            public static BlueprintCharacterClass SkaldClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("6afa347d804838b48bda16acb0573dc0");
            public static BlueprintCharacterClass SlayerClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("c75e0971973957d4dbad24bc7957e4fb");
            public static BlueprintCharacterClass SorcererClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("b3a505fb61437dc4097f43c3f8f9a4cf");
            public static BlueprintCharacterClass WarpriestClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("30b5e47d47a0e37438cc5a80c96cfb99");
            public static BlueprintCharacterClass WitchClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("1b9873f1e7bfe5449bc84d03e9c8e3cc");
            public static BlueprintCharacterClass WizardClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("ba34257984f4c41408ce1dc2004e342e");

            public static BlueprintCharacterClass[] AllClasses => Game.Instance.BlueprintRoot.Progression.m_CharacterClasses.Select(c => c.Get()).ToArray();
        }
        public static class ClassReferences{
            public static BlueprintCharacterClassReference AlchemistClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("0937bec61c0dabc468428f496580c721");
            public static BlueprintCharacterClassReference ArcanistClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("52dbfd8505e22f84fad8d702611f60b7");
            public static BlueprintCharacterClassReference BarbarianClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("f7d7eb166b3dd594fb330d085df41853");
            public static BlueprintCharacterClassReference BardClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("772c83a25e2268e448e841dcd548235f");
            public static BlueprintCharacterClassReference BloodragerClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("d77e67a814d686842802c9cfd8ef8499");
            public static BlueprintCharacterClassReference CavalierClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("3adc3439f98cb534ba98df59838f02c7");
            public static BlueprintCharacterClassReference ClericClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("67819271767a9dd4fbfd4ae700befea0");
            public static BlueprintCharacterClassReference DruidClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("610d836f3a3a9ed42a4349b62f002e96");
            public static BlueprintCharacterClassReference FighterClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("48ac8db94d5de7645906c7d0ad3bcfbd");
            public static BlueprintCharacterClassReference HunterClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("34ecd1b5e1b90b9498795791b0855239");
            public static BlueprintCharacterClassReference InquisitorClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("f1a70d9e1b0b41e49874e1fa9052a1ce");
            public static BlueprintCharacterClassReference KineticistClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("42a455d9ec1ad924d889272429eb8391");
            public static BlueprintCharacterClassReference MagusClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("45a4607686d96a1498891b3286121780");
            public static BlueprintCharacterClassReference MonkClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("e8f21e5b58e0569468e420ebea456124");
            public static BlueprintCharacterClassReference OracleClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("20ce9bf8af32bee4c8557a045ab499b1");
            public static BlueprintCharacterClassReference PaladinClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("bfa11238e7ae3544bbeb4d0b92e897ec");
            public static BlueprintCharacterClassReference RangerClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("cda0615668a6df14eb36ba19ee881af6");
            public static BlueprintCharacterClassReference RogueClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("299aa766dee3cbf4790da4efb8c72484");
            public static BlueprintCharacterClassReference ShamanClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("145f1d3d360a7ad48bd95d392c81b38e");
            public static BlueprintCharacterClassReference SkaldClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("6afa347d804838b48bda16acb0573dc0");
            public static BlueprintCharacterClassReference SlayerClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("c75e0971973957d4dbad24bc7957e4fb");
            public static BlueprintCharacterClassReference SorcererClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("b3a505fb61437dc4097f43c3f8f9a4cf");
            public static BlueprintCharacterClassReference WarpriestClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("30b5e47d47a0e37438cc5a80c96cfb99");
            public static BlueprintCharacterClassReference WitchClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("1b9873f1e7bfe5449bc84d03e9c8e3cc");
            public static BlueprintCharacterClassReference WizardClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("ba34257984f4c41408ce1dc2004e342e");

            public static BlueprintCharacterClassReference[] AllClasses => Game.Instance.BlueprintRoot.Progression.m_CharacterClasses;
        }
    }
}
