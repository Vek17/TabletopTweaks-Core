using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System.Linq;

namespace TabletopTweaks.Core.Utilities {
    public class WildShapeTools {
        public static class WildShapeBuffs{
            public static BlueprintBuffReference ShifterDragonFormBlackBuff14 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("8fb6bf56c9174d5e8cf24069e6b0c965");
            public static BlueprintBuffReference ShifterDragonFormBlackBuff9 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("662bdcd3eef541fb91d88b9ee79d0d37");
            public static BlueprintBuffReference ShifterDragonFormBlueBuff14 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("b9c75c14fe6d48e0962e1ce9f42d4c9e");
            public static BlueprintBuffReference ShifterDragonFormBlueBuff9 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("abaa7d56f843410e97c61ff2c87d39c6");
            public static BlueprintBuffReference ShifterDragonFormBrassBuff14 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("445d70781c2848dc9c63d80718a6c26f");
            public static BlueprintBuffReference ShifterDragonFormBrassBuff9 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("205e7bae0d7c428b8f2a451f7934219a");
            public static BlueprintBuffReference ShifterDragonFormBronzeBuff14 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("0ff1819f465140068e02aaf87c17ec2c");
            public static BlueprintBuffReference ShifterDragonFormBronzeBuff9 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("cd72e19154f143269e48caff753eab63");
            public static BlueprintBuffReference ShifterDragonFormCopperBuff14 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("e9736d47de3643009a5514668a48ffe0");
            public static BlueprintBuffReference ShifterDragonFormCopperBuff9 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("3f5625345d0c481abec69c0241d50019");
            public static BlueprintBuffReference ShifterDragonFormGoldBuff14 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("5a679cd137d64c629995c626616dbb17");
            public static BlueprintBuffReference ShifterDragonFormGoldBuff9 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("f5ac253cbee44744a7399f17765160d5");
            public static BlueprintBuffReference ShifterDragonFormGreenBuff14 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("3d887a79a7384149bd38b4d9d97c44b5");
            public static BlueprintBuffReference ShifterDragonFormGreenBuff9 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("ab25d91564a04b3fa0ae84d52b6407d5");
            public static BlueprintBuffReference ShifterDragonFormRedBuff14 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("8242311f5c3c4cad90e67ef79cf5a6c2");
            public static BlueprintBuffReference ShifterDragonFormRedBuff9 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("a1f0de0190ce40e19d97c6967a9693c3");
            public static BlueprintBuffReference ShifterDragonFormSilverBuff14 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("2de04456ce2d4e79804f899498ab31cc");
            public static BlueprintBuffReference ShifterDragonFormSilverBuff9 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("3c4bf82676d345dca2718cac680f5906");
            public static BlueprintBuffReference ShifterDragonFormWhiteBuff14 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("b9b1fbf0ec224ccfac3dc5451d00a26a");
            public static BlueprintBuffReference ShifterDragonFormWhiteBuff9 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("8b82ee0ca203452a952a25c0f867b2fe");
            public static BlueprintBuffReference ShifterWildShapeBearBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("68ca4094f4e7488c8e869af833e153f1");
            public static BlueprintBuffReference ShifterWildShapeBearBuff15 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("5adecdc82eda4271b6dc4a7e6a921c89");
            public static BlueprintBuffReference ShifterWildShapeBearBuff8 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("9cc0eceb9cbc44e4b1ecc5e8b5f97c45");
            public static BlueprintBuffReference ShifterWildShapeBoarBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("c6b493bf01ec4478bbb7247f56d670f8");
            public static BlueprintBuffReference ShifterWildShapeBoarBuff15 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("b8c0ad632442496aa20b96864dde4454");
            public static BlueprintBuffReference ShifterWildShapeBoarBuff8 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("ae71fed93f9f442cbce5b601e4aa1a23");
            public static BlueprintBuffReference ShifterWildShapeDinosaurBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("bdbc24300ebd4fd6810172e4d4b1ab19");
            public static BlueprintBuffReference ShifterWildShapeDinosaurBuff15 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("ab0f757d55a74658b812829518566df4");
            public static BlueprintBuffReference ShifterWildShapeDinosaurBuff8 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("f6d0e7d4d96b4f7897557968a0706f74");
            public static BlueprintBuffReference ShifterWildShapeElephantBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("7e90cc7393624b13b6d319774bf6d812");
            public static BlueprintBuffReference ShifterWildShapeElephantBuff15 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("fdb39ec525c2411aad43f6b12ad4b1c0");
            public static BlueprintBuffReference ShifterWildShapeElephantBuff8 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("38f84f5a0da94b169131b863be81957b");
            public static BlueprintBuffReference ShifterWildShapeFeyBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("477259fe81a647ad9a38b47140e38de6");
            public static BlueprintBuffReference ShifterWildShapeFeyBuff15 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("dee544177d0148edbaa7ca6a0aee03c0");
            public static BlueprintBuffReference ShifterWildShapeFeyBuff8 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("6fc7df0ddb9a466d976a3808a8f1437a");
            public static BlueprintBuffReference ShifterWildShapeGriffonBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("e76d475eb1f1470e9950a5fee99ddb40");
            public static BlueprintBuffReference ShifterWildShapeGriffonBuff14 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("821fa7f586ca44238a0894115824035c");
            public static BlueprintBuffReference ShifterWildShapeGriffonBuff9 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("3a7511f1b8a94b11bbb21245e150c0b6");
            public static BlueprintBuffReference ShifterWildShapeGriffonBuff_Cutscene => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("af4af06317b94a39bd3fd80ebde86070");
            public static BlueprintBuffReference ShifterWildShapeGriffonDemonBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("6236b745b60a4a578c435c861df393f4");
            public static BlueprintBuffReference ShifterWildShapeGriffonDemonBuff14 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("431ca9188d6f401f9f8df8079c526e59");
            public static BlueprintBuffReference ShifterWildShapeGriffonDemonBuff9 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("7d4798e5fe5f4a349b56686340008824");
            public static BlueprintBuffReference ShifterWildShapeGriffonGodBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("4b95ed9a351e4effbb2a83e246ee6334");
            public static BlueprintBuffReference ShifterWildShapeGriffonGodBuff14 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("10c913c645364bafbde759f83d103ce6");
            public static BlueprintBuffReference ShifterWildShapeGriffonGodBuff9 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("d8b979bf19554b85bbed05e6369c0f63");
            public static BlueprintBuffReference ShifterWildShapeGriffonGodBuff_Cutscene => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("78ee750ff66c47dc9cbf82d59013ae8f");
            public static BlueprintBuffReference ShifterWildShapeHorseBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("b3e5540f487d4a2b9aa50aad34c60ec3");
            public static BlueprintBuffReference ShifterWildShapeHorseBuff15 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("f493bc3e006d44fb8815267ffa49ec76");
            public static BlueprintBuffReference ShifterWildShapeHorseBuff8 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("4a5e4c66ca8d4657b281060a24886ad1");
            public static BlueprintBuffReference ShifterWildShapeLizardBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("0b15a8dce67746e1979dfc597f13827f");
            public static BlueprintBuffReference ShifterWildShapeLizardBuff15 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("ba5b0df349c94c91ad7a38309b042537");
            public static BlueprintBuffReference ShifterWildShapeLizardBuff8 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("f246e4e203f84f52bed2dc16e6d36087");
            public static BlueprintBuffReference ShifterWildShapeManticoreBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("91f12a442b374bd7bfdfb05f5ab80f4c");
            public static BlueprintBuffReference ShifterWildShapeManticoreBuff15 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("f5331d1b15ac4c4a833e2928ce3bf18d");
            public static BlueprintBuffReference ShifterWildShapeManticoreBuff8 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("28861899db294aa593ada213a8d1fd36");
            public static BlueprintBuffReference ShifterWildShapeSpiderBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("58e7d688873242f38b6cc66a7ae3d794");
            public static BlueprintBuffReference ShifterWildShapeSpiderBuff15 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("d895c3b8663f4b73a3868e6369bba6d4");
            public static BlueprintBuffReference ShifterWildShapeSpiderBuff8 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("c163b127ce354363a20946bb01967cac");
            public static BlueprintBuffReference ShifterWildShapeTigerBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("7a5da4b80a494bf2b96fa7756d3f89cc");
            public static BlueprintBuffReference ShifterWildShapeTigerBuff15 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("f49109fdb0a24f75a4ab466bf95843af");
            public static BlueprintBuffReference ShifterWildShapeTigerBuff8 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("defa946808514ab0a1023192fa13ede3");
            public static BlueprintBuffReference ShifterWildShapeWolfBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("fe61e45eb3d1441795179eb0bff1ef3b");
            public static BlueprintBuffReference ShifterWildShapeWolfBuff15 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("14bf6a8c9f9f40dba2f6fc41e6235270");
            public static BlueprintBuffReference ShifterWildShapeWolfBuff8 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("a983157daa464ab7a725ed5f53110a32");
            public static BlueprintBuffReference ShifterWildShapeWolverineBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("350ba136f5d04e588f1a6e3ba22233cb");
            public static BlueprintBuffReference ShifterWildShapeWolverineBuff15 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("cd5f29d5f50a43f9a2d5f3debce86477");
            public static BlueprintBuffReference ShifterWildShapeWolverineBuff8 => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("12272456258f4334a11b87610cf33abd");
            public static BlueprintBuffReference WildShapeElementalAirHugeBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("eb52d24d6f60fc742b32fe943b919180");
            public static BlueprintBuffReference WildShapeElementalAirLargeBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("814bc75e74f969641bf110addf076ff9");
            public static BlueprintBuffReference WildShapeElementalAirMediumBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("65fdf187fea97c94b9cf4ff6746901a6");
            public static BlueprintBuffReference WildShapeElementalAirSmallBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("dc1ef6f6d52b9fd49bc0696ab1a4f18b");
            public static BlueprintBuffReference WildShapeElementalEarthHugeBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("f0826c3794c158c4cbbe9ceb4210d6d6");
            public static BlueprintBuffReference WildShapeElementalEarthLargeBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("bf145574939845d43b68e3f4335986b4");
            public static BlueprintBuffReference WildShapeElementalEarthMediumlBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("e76500bc1f1f269499bf027a5aeb1471");
            public static BlueprintBuffReference WildShapeElementalEarthSmallBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("add5378a75feeaf4384766da10ddc40d");
            public static BlueprintBuffReference WildShapeElementalFireHugeBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("e85abd773dbce30498efa8da745d7ca7");
            public static BlueprintBuffReference WildShapeElementalFireLargeBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("7f30b0f7f3c4b6748a2819611fb236f8");
            public static BlueprintBuffReference WildShapeElementalFireMediumBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("3e3f33fb3e581ab4e8923a5eabd15923");
            public static BlueprintBuffReference WildShapeElementalFireSmallBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("9e6b7b058bc74fc45903679adcab8553");
            public static BlueprintBuffReference WildShapeElementalWaterHugeBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("ea2cd08bdf2ca1c4f8a8870804790cd7");
            public static BlueprintBuffReference WildShapeElementalWaterLargeBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("5993b78c793667e45bf0380e9275fab7");
            public static BlueprintBuffReference WildShapeElementalWaterMediumBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("c5925e7b9e7fc2e478526b4cfc8c6427");
            public static BlueprintBuffReference WildShapeElementalWaterSmallBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("9c58cfcad11f7fd4cb85e22187fddac7");
            public static BlueprintBuffReference WildShapeIIISmilodonBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("49a77c5c5266c42429f7afbb038ada60");
            public static BlueprintBuffReference WildShapeIILeopardBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("8abf1c437ebee8048a4a3335efc27eb3");
            public static BlueprintBuffReference WildShapeIVBearBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("d5640d96888dd20489922045fde35059");
            public static BlueprintBuffReference WildShapeIVShamblingMoundBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("0d29c50c956e82d4eae56710987de9f7");
            public static BlueprintBuffReference WildShapeIWolfBuff => BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("470fb1a22e7eb5849999f1101eacc5dc");

            public static BlueprintBuffReference[] AllReferences => new BlueprintBuffReference[] {
                ShifterDragonFormBlackBuff14,
                ShifterDragonFormBlackBuff9,
                ShifterDragonFormBlueBuff14,
                ShifterDragonFormBlueBuff9,
                ShifterDragonFormBrassBuff14,
                ShifterDragonFormBrassBuff9,
                ShifterDragonFormBronzeBuff14,
                ShifterDragonFormBronzeBuff9,
                ShifterDragonFormCopperBuff14,
                ShifterDragonFormCopperBuff9,
                ShifterDragonFormGoldBuff14,
                ShifterDragonFormGoldBuff9,
                ShifterDragonFormGreenBuff14,
                ShifterDragonFormGreenBuff9,
                ShifterDragonFormRedBuff14,
                ShifterDragonFormRedBuff9,
                ShifterDragonFormSilverBuff14,
                ShifterDragonFormSilverBuff9,
                ShifterDragonFormWhiteBuff14,
                ShifterDragonFormWhiteBuff9,
                ShifterWildShapeBearBuff,
                ShifterWildShapeBearBuff15,
                ShifterWildShapeBearBuff8,
                ShifterWildShapeBoarBuff,
                ShifterWildShapeBoarBuff15,
                ShifterWildShapeBoarBuff8,
                ShifterWildShapeDinosaurBuff,
                ShifterWildShapeDinosaurBuff15,
                ShifterWildShapeDinosaurBuff8,
                ShifterWildShapeElephantBuff,
                ShifterWildShapeElephantBuff15,
                ShifterWildShapeElephantBuff8,
                ShifterWildShapeFeyBuff,
                ShifterWildShapeFeyBuff15,
                ShifterWildShapeFeyBuff8,
                ShifterWildShapeGriffonBuff,
                ShifterWildShapeGriffonBuff14,
                ShifterWildShapeGriffonBuff9,
                ShifterWildShapeGriffonBuff_Cutscene,
                ShifterWildShapeGriffonDemonBuff,
                ShifterWildShapeGriffonDemonBuff14,
                ShifterWildShapeGriffonDemonBuff9,
                ShifterWildShapeGriffonGodBuff,
                ShifterWildShapeGriffonGodBuff14,
                ShifterWildShapeGriffonGodBuff9,
                ShifterWildShapeGriffonGodBuff_Cutscene,
                ShifterWildShapeHorseBuff,
                ShifterWildShapeHorseBuff15,
                ShifterWildShapeHorseBuff8,
                ShifterWildShapeLizardBuff,
                ShifterWildShapeLizardBuff15,
                ShifterWildShapeLizardBuff8,
                ShifterWildShapeManticoreBuff,
                ShifterWildShapeManticoreBuff15,
                ShifterWildShapeManticoreBuff8,
                ShifterWildShapeSpiderBuff,
                ShifterWildShapeSpiderBuff15,
                ShifterWildShapeSpiderBuff8,
                ShifterWildShapeTigerBuff,
                ShifterWildShapeTigerBuff15,
                ShifterWildShapeTigerBuff8,
                ShifterWildShapeWolfBuff,
                ShifterWildShapeWolfBuff15,
                ShifterWildShapeWolfBuff8,
                ShifterWildShapeWolverineBuff,
                ShifterWildShapeWolverineBuff15,
                ShifterWildShapeWolverineBuff8,
                WildShapeElementalAirHugeBuff,
                WildShapeElementalAirLargeBuff,
                WildShapeElementalAirMediumBuff,
                WildShapeElementalAirSmallBuff,
                WildShapeElementalEarthHugeBuff,
                WildShapeElementalEarthLargeBuff,
                WildShapeElementalEarthMediumlBuff,
                WildShapeElementalEarthSmallBuff,
                WildShapeElementalFireHugeBuff,
                WildShapeElementalFireLargeBuff,
                WildShapeElementalFireMediumBuff,
                WildShapeElementalFireSmallBuff,
                WildShapeElementalWaterHugeBuff,
                WildShapeElementalWaterLargeBuff,
                WildShapeElementalWaterMediumBuff,
                WildShapeElementalWaterSmallBuff,
                WildShapeIIISmilodonBuff,
                WildShapeIILeopardBuff,
                WildShapeIVBearBuff,
                WildShapeIVShamblingMoundBuff,
                WildShapeIWolfBuff
            };
            public static BlueprintBuff[] AllBuffs => AllReferences.Select(b => b.Get()).ToArray();
        }
    }
}
