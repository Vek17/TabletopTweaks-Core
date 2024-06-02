using Kingmaker;
using Kingmaker.Blueprints.Root;
using Kingmaker.DLC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaks.Core.Utilities {
    public static class DLCTools {

        private static BlueprintDlc Dlc1 = BlueprintTools.GetBlueprint<BlueprintDlc>("8576a633c8fe4ce78530b55c1f0d14e5");
        private static BlueprintDlc Dlc2 = BlueprintTools.GetBlueprint<BlueprintDlc>("4f7ae2d1e6e74a0c807b4020e9e99354");
        private static BlueprintDlc Dlc3 = BlueprintTools.GetBlueprint<BlueprintDlc>("962e8c01fd834805b3ddf93134f77d44");
        private static BlueprintDlc Dlc4 = BlueprintTools.GetBlueprint<BlueprintDlc>("35b89606cfe9405085a35b02cf15017f");
        private static BlueprintDlc Dlc5 = BlueprintTools.GetBlueprint<BlueprintDlc>("95a25ca16bd54ce3b3ea56f83538fa0d");
        private static BlueprintDlc Dlc6 = BlueprintTools.GetBlueprint<BlueprintDlc>("c2340df3fdaf403baffe824ae7a0a547");

        public static bool HasDLC(int number) {
            switch (number) {
                case 1: return Dlc1.IsAvailable;
                case 2: return Dlc2.IsAvailable;
                case 3: return Dlc3.IsAvailable;
                case 4: return Dlc4.IsAvailable;
                case 5: return Dlc5.IsAvailable;
                case 6: return Dlc6.IsAvailable;
                default: return false;
            }
        }
    }
}
