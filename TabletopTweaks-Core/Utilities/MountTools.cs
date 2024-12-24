using Kingmaker.Blueprints;
using UnityEngine;

namespace TabletopTweaks.Core.Utilities {
    public static class MountTools {
        public static class RaceOptions {
            public static BlueprintRaceReference Aasimar => BlueprintTools.GetBlueprintReference<BlueprintRaceReference>("b7f02ba92b363064fb873963bec275ee");//Normal
            public static BlueprintRaceReference Dhampir => BlueprintTools.GetBlueprintReference<BlueprintRaceReference>("64e8b7d5f1ae91d45bbf1e56a3fdff01");//Normal
            public static BlueprintRaceReference Dwarf => BlueprintTools.GetBlueprintReference<BlueprintRaceReference>("c4faf439f0e70bd40b5e36ee80d06be7");
            public static BlueprintRaceReference Elf => BlueprintTools.GetBlueprintReference<BlueprintRaceReference>("25a5878d125338244896ebd3238226c8");
            public static BlueprintRaceReference Gnome => BlueprintTools.GetBlueprintReference<BlueprintRaceReference>("ef35a22c9a27da345a4528f0d5889157");
            public static BlueprintRaceReference HalfElf => BlueprintTools.GetBlueprintReference<BlueprintRaceReference>("b3646842ffbd01643ab4dac7479b20b0");//Normal
            public static BlueprintRaceReference Halfling => BlueprintTools.GetBlueprintReference<BlueprintRaceReference>("b0c3ef2729c498f47970bb50fa1acd30");
            public static BlueprintRaceReference HalfOrc => BlueprintTools.GetBlueprintReference<BlueprintRaceReference>("1dc20e195581a804890ddc74218bfd8e");
            public static BlueprintRaceReference Human => BlueprintTools.GetBlueprintReference<BlueprintRaceReference>("0a5d473ead98b0646b94495af250fdc4");//Normal
            public static BlueprintRaceReference Kitsune => BlueprintTools.GetBlueprintReference<BlueprintRaceReference>("fd188bb7bb0002e49863aec93bfb9d99");//Normal
            public static BlueprintRaceReference Mongrelman => BlueprintTools.GetBlueprintReference<BlueprintRaceReference>("daca06a3f3355664bba1e87e67f3b5b3");//Normal
            public static BlueprintRaceReference Oread => BlueprintTools.GetBlueprintReference<BlueprintRaceReference>("4d4555326b9b7144f93be1ea61337cd7");
            public static BlueprintRaceReference Tiefling => BlueprintTools.GetBlueprintReference<BlueprintRaceReference>("5c4e42124dc2b4647af6e36cf2590500");//Normal
                                                                                                                                                              //Weird options
            public static BlueprintRaceReference Android => BlueprintTools.GetBlueprintReference<BlueprintRaceReference>("d1d114f539b74468b157ac69c275f266");//Normal
            public static BlueprintRaceReference AscendingSuccubus => BlueprintTools.GetBlueprintReference<BlueprintRaceReference>("5e464d1d5fd0e7a4380b6ce60ef2c83b");//Normal

        }

        public static Transform CreateMountBone(Transform parent, string type, Vector3 posOffset, Vector3? rotOffset = null) {
            var offsetBone = new GameObject($"Saddle_{type}_parent");
            offsetBone.transform.SetParent(parent);
            offsetBone.transform.localPosition = posOffset;
            if (rotOffset.HasValue)
                offsetBone.transform.localEulerAngles = rotOffset.Value;

            var target = new GameObject($"Saddle_{type}");
            target.transform.SetParent(offsetBone.transform);

            return target.transform;
        }
    }
}
