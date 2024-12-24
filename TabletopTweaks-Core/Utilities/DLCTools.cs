using Kingmaker.DLC;
using Kingmaker.Stores;

namespace TabletopTweaks.Core.Utilities {
    public static class DLCTools {

        private static BlueprintDlc Dlc1 = BlueprintTools.GetBlueprint<BlueprintDlc>("8576a633c8fe4ce78530b55c1f0d14e5");
        private static BlueprintDlc Dlc2 = BlueprintTools.GetBlueprint<BlueprintDlc>("4f7ae2d1e6e74a0c807b4020e9e99354");
        private static BlueprintDlc Dlc3 = BlueprintTools.GetBlueprint<BlueprintDlc>("962e8c01fd834805b3ddf93134f77d44");
        private static BlueprintDlc Dlc4 = BlueprintTools.GetBlueprint<BlueprintDlc>("35b89606cfe9405085a35b02cf15017f");
        private static BlueprintDlc Dlc5 = BlueprintTools.GetBlueprint<BlueprintDlc>("95a25ca16bd54ce3b3ea56f83538fa0d");
        private static BlueprintDlc Dlc6 = BlueprintTools.GetBlueprint<BlueprintDlc>("c2340df3fdaf403baffe824ae7a0a547");

        public static bool HasDLC(int number) {
            var DLC = number switch {
                1 => Dlc1,
                2 => Dlc2,
                3 => Dlc3,
                4 => Dlc4,
                5 => Dlc5,
                6 => Dlc6,
                _ => null
            };
            if (DLC == null) { return false; }
            StoreManager.RefreshDLCs(new BlueprintDlc[] { DLC });
            return DLC.IsAvailable;
        }
    }
}
