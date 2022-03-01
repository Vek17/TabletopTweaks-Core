using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System.Linq;
using static TabletopTweaks.Core.Main;
using static TabletopTweaks.Core.MechanicsChanges.AdditionalModifierDescriptors;

namespace TabletopTweaks.Core.Bugfixes.Classes {
    class Paladin {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Paladin");
                PatchBase();
            }
            static void PatchBase() {
                PatchDivineMount();
                PatchSmiteAttackBonus();

                void PatchDivineMount() {
                    if (TTTContext.Fixes.Paladin.Base.IsDisabled("DivineMountTemplate")) { return; }

                    var TemplateCelestial = Resources.GetModBlueprint<BlueprintFeature>(modContext: TTTContext, "TemplateCelestial");
                    var PaladinDivineMount11Feature = Resources.GetBlueprint<BlueprintFeature>("ea31185f4e0f91041bf766d67214182f");
                    var addFeatureToPet = PaladinDivineMount11Feature.Components.OfType<AddFeatureToPet>().FirstOrDefault();
                    if (addFeatureToPet != null) {
                        addFeatureToPet.m_Feature = TemplateCelestial.ToReference<BlueprintFeatureReference>();
                    }
                    TTTContext.Logger.LogPatch("Patched", PaladinDivineMount11Feature);
                }
                void PatchSmiteAttackBonus() {
                    if (TTTContext.Fixes.Paladin.Base.IsDisabled("SmiteAttackBonus")) { return; }

                    var SmiteChaosBuff = Resources.GetBlueprint<BlueprintBuff>("161051816b1530843a8096167be9b8a7");
                    var SmiteEvilBuff = Resources.GetBlueprint<BlueprintBuff>("b6570b8cbb32eaf4ca8255d0ec3310b0");
                    var AuraOfJusticeSmiteEvilBuff = Resources.GetBlueprint<BlueprintBuff>("ac3c66782859eb84692a8782320ffd2c");
                    var CelestialSmiteEvilBuff = Resources.GetBlueprint<BlueprintBuff>("db4abdd3a772eec4c97048c1cf4b7417");
                    var FiendishSmiteGoodBuff = Resources.GetBlueprint<BlueprintBuff>("a9035e49d6d79a64eaec321f2cb629a8");
                    var HalfFiendSmiteGoodBuff = Resources.GetBlueprint<BlueprintBuff>("114af78efc58e5a4c86bb12ee1d907cc");

                    SmiteChaosBuff.GetComponent<AttackBonusAgainstTarget>().Descriptor = (ModifierDescriptor)Untyped.Charisma;
                    SmiteEvilBuff.GetComponent<AttackBonusAgainstTarget>().Descriptor = (ModifierDescriptor)Untyped.Charisma;
                    AuraOfJusticeSmiteEvilBuff.GetComponent<AttackBonusAgainstTarget>().Descriptor = (ModifierDescriptor)Untyped.Charisma;
                    CelestialSmiteEvilBuff.GetComponent<AttackBonusAgainstTarget>().Descriptor = (ModifierDescriptor)Untyped.Charisma;
                    FiendishSmiteGoodBuff.GetComponent<AttackBonusAgainstTarget>().Descriptor = (ModifierDescriptor)Untyped.Charisma;
                    HalfFiendSmiteGoodBuff.GetComponent<AttackBonusAgainstTarget>().Descriptor = (ModifierDescriptor)Untyped.Charisma;

                    TTTContext.Logger.LogPatch("Patched", SmiteChaosBuff);
                    TTTContext.Logger.LogPatch("Patched", SmiteEvilBuff);
                    TTTContext.Logger.LogPatch("Patched", AuraOfJusticeSmiteEvilBuff);
                    TTTContext.Logger.LogPatch("Patched", CelestialSmiteEvilBuff);
                    TTTContext.Logger.LogPatch("Patched", FiendishSmiteGoodBuff);
                    TTTContext.Logger.LogPatch("Patched", HalfFiendSmiteGoodBuff);
                }
            }

            static void PatchArchetypes() {
            }
        }
    }
}
