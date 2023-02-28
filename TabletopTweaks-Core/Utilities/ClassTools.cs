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
            public static BlueprintCharacterClass ShifterClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("a406d6ebea5c46bba3160246be03e96f");
            public static BlueprintCharacterClass SkaldClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("6afa347d804838b48bda16acb0573dc0");
            public static BlueprintCharacterClass SlayerClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("c75e0971973957d4dbad24bc7957e4fb");
            public static BlueprintCharacterClass SorcererClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("b3a505fb61437dc4097f43c3f8f9a4cf");
            public static BlueprintCharacterClass WarpriestClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("30b5e47d47a0e37438cc5a80c96cfb99");
            public static BlueprintCharacterClass WitchClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("1b9873f1e7bfe5449bc84d03e9c8e3cc");
            public static BlueprintCharacterClass WizardClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("ba34257984f4c41408ce1dc2004e342e");
            public static BlueprintCharacterClass ArcaneTricksterClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("9c935a076d4fe4d4999fd48d853e3cf3");
            public static BlueprintCharacterClass AssassinClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("eb284ea8d40a2d045911f525eb96c43d");
            public static BlueprintCharacterClass StalwartDefenderClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("d5917881586ff1d4d96d5b7cebda9464");
            public static BlueprintCharacterClass EldritchKnightClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("de52b73972f0ed74c87f8f6a8e20b542");
            public static BlueprintCharacterClass DragonDiscipleClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("72051275b1dbb2d42ba9118237794f7c");
            public static BlueprintCharacterClass HellknightClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("ed246f1680e667b47b7427d51e651059");
            public static BlueprintCharacterClass HellknightSigniferClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("ee6425d6392101843af35f756ce7fefd");
            public static BlueprintCharacterClass MysticTheurgeClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("0920ea7e4fd7a404282e3d8b0ac41838");
            public static BlueprintCharacterClass DuelistClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("4e0ea99612ae87a499c7fb0588e31828");
            public static BlueprintCharacterClass StudentOfWarClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("46b1fe9759ea01f4d883b23d8f0aecbb");
            public static BlueprintCharacterClass SwordlordClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("90e4d7da3ccd1a8478411e07e91d5750");
            public static BlueprintCharacterClass LoremasterClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("4a7c05adfbaf05446a6bf664d28fb103");
            public static BlueprintCharacterClass WinterWitchClass => BlueprintTools.GetBlueprint<BlueprintCharacterClass>("eb24ca44debf6714aabe1af1fd905a07");

            public static BlueprintCharacterClass[] AllClasses => Game.Instance.BlueprintRoot.Progression.m_CharacterClasses.Select(c => c.Get()).ToArray();
        }
        public static class ClassReferences {
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
            public static BlueprintCharacterClassReference ShifterClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("a406d6ebea5c46bba3160246be03e96f");
            public static BlueprintCharacterClassReference SkaldClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("6afa347d804838b48bda16acb0573dc0");
            public static BlueprintCharacterClassReference SlayerClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("c75e0971973957d4dbad24bc7957e4fb");
            public static BlueprintCharacterClassReference SorcererClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("b3a505fb61437dc4097f43c3f8f9a4cf");
            public static BlueprintCharacterClassReference WarpriestClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("30b5e47d47a0e37438cc5a80c96cfb99");
            public static BlueprintCharacterClassReference WitchClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("1b9873f1e7bfe5449bc84d03e9c8e3cc");
            public static BlueprintCharacterClassReference WizardClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("ba34257984f4c41408ce1dc2004e342e");
            public static BlueprintCharacterClassReference ArcaneTricksterClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("9c935a076d4fe4d4999fd48d853e3cf3");
            public static BlueprintCharacterClassReference AssassinClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("eb284ea8d40a2d045911f525eb96c43d");
            public static BlueprintCharacterClassReference StalwartDefenderClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("d5917881586ff1d4d96d5b7cebda9464");
            public static BlueprintCharacterClassReference EldritchKnightClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("de52b73972f0ed74c87f8f6a8e20b542");
            public static BlueprintCharacterClassReference DragonDiscipleClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("72051275b1dbb2d42ba9118237794f7c");
            public static BlueprintCharacterClassReference HellknightClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("ed246f1680e667b47b7427d51e651059");
            public static BlueprintCharacterClassReference HellknightSigniferClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("ee6425d6392101843af35f756ce7fefd");
            public static BlueprintCharacterClassReference MysticTheurgeClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("0920ea7e4fd7a404282e3d8b0ac41838");
            public static BlueprintCharacterClassReference DuelistClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("4e0ea99612ae87a499c7fb0588e31828");
            public static BlueprintCharacterClassReference StudentOfWarClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("46b1fe9759ea01f4d883b23d8f0aecbb");
            public static BlueprintCharacterClassReference SwordlordClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("90e4d7da3ccd1a8478411e07e91d5750");
            public static BlueprintCharacterClassReference LoremasterClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("4a7c05adfbaf05446a6bf664d28fb103");
            public static BlueprintCharacterClassReference WinterWitchClass => BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("eb24ca44debf6714aabe1af1fd905a07");

            public static BlueprintCharacterClassReference[] AllClasses => Game.Instance.BlueprintRoot.Progression.m_CharacterClasses;
        }
    }
}
