using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using static TabletopTweaks.Core.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TabletopTweaks.Core.NewUnitParts {
    public class UnitPartCustomMechanicsFeatures : OldStyleUnitPart {

        public void AddMechanicsFeature(CustomMechanicsFeature type) {
            CountableFlag MechanicsFeature = GetMechanicsFeature(type);
            MechanicsFeature.Retain();
        }

        public void RemoveMechanicsFeature(CustomMechanicsFeature type) {
            CountableFlag MechanicsFeature = GetMechanicsFeature(type);
            MechanicsFeature.Release();
        }

        public void ClearMechanicsFeature(CustomMechanicsFeature type) {
            CountableFlag MechanicsFeature = GetMechanicsFeature(type);
            MechanicsFeature.ReleaseAll();
        }

        public CountableFlag GetMechanicsFeature(CustomMechanicsFeature type) {
            CountableFlag MechanicsFeature;
            MechanicsFeatures.TryGetValue(type, out MechanicsFeature);
            if (MechanicsFeature == null) {
                MechanicsFeature = new CountableFlag();
                MechanicsFeatures[type] = MechanicsFeature;
            }
            return MechanicsFeature;
        }

        private readonly Dictionary<CustomMechanicsFeature, CountableFlag> MechanicsFeatures = new Dictionary<CustomMechanicsFeature, CountableFlag>();

        //If you want to extend this externally please use something > 1000
        public enum CustomMechanicsFeature : int {
            QuickDraw,
            UseWeaponOneHanded,
            UndersizedMount,
            MountedSkirmisher,
            ManyshotMythic,
            FavoriteMetamagicPersistent,
            FavoriteMetamagicSelective,
            FavoriteMetamagicIntensified,
            FavoriteMetamagicRime,
            FavoriteMetamagicBurning,
            FavoriteMetamagicFlaring,
            FavoriteMetamagicPiercing,
            FavoriteMetamagicSolidShadows,
            FavoriteMetamagicEncouraging,
            IdealizeDiscovery,
            IdealizeDiscoveryUpgrade,
            BypassSneakAttackImmunity,
            BypassCriticalHitImmunity,
            TricksterReworkPersuasion2,
            TricksterReworkPersuasion3
        }
    }
    public static class CustomMechanicsFeaturesExtentions {
        public static CountableFlag CustomMechanicsFeature(this UnitDescriptor unit, CustomMechanicsFeature type) {
            var mechanicsFeatures = unit.Ensure<UnitPartCustomMechanicsFeatures>();
            return mechanicsFeatures.GetMechanicsFeature(type);
        }

        public static CountableFlag CustomMechanicsFeature(this UnitEntityData unit, CustomMechanicsFeature type) {
            return unit.Descriptor.CustomMechanicsFeature(type);
        }
    }

    [HarmonyPatch(typeof(RuleAttackRoll), nameof(RuleAttackRoll.ImmuneToSneakAttack), MethodType.Getter)]
    static class RuleAttackRoll_SneakImmunity_MechanicsFeature {
        static void Postfix(RuleAttackRoll __instance, ref bool __result) {
            if (__instance.Initiator.CustomMechanicsFeature(CustomMechanicsFeature.BypassSneakAttackImmunity)) {
                __result = false;
            }
        }
    }

    [HarmonyPatch(typeof(RuleAttackRoll), nameof(RuleAttackRoll.ImmuneToCriticalHit), MethodType.Getter)]
    static class RuleAttackRoll_Critical_MechanicsFeature {
        static void Postfix(RuleAttackRoll __instance, ref bool __result) {
            if (__instance.Initiator.CustomMechanicsFeature(CustomMechanicsFeature.BypassCriticalHitImmunity)) {
                __result = false;
            }
        }
    }

    [HarmonyPatch(typeof(AddImmunityToCriticalHits), "OnEventAboutToTrigger", new Type[] { typeof(RuleDealStatDamage) })]
    static class AddImmunityToCriticalHits_StatDamage_Fix {
        static bool Prefix(RuleCalculateDamage evt) {
            if (evt.Initiator == null) { return true; }
            if (evt.Initiator.CustomMechanicsFeature(CustomMechanicsFeature.BypassCriticalHitImmunity)) {
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(AddImmunityToPrecisionDamage), nameof(AddImmunityToPrecisionDamage.OnEventAboutToTrigger), new Type[] { typeof(RuleCalculateDamage) })]
    static class AddImmunityToPrecisionDamage_SneakImmunity_Fix {
        static bool Prefix(RuleCalculateDamage evt) {
            if (evt.Initiator == null) { return true; }
            if (evt.Initiator.CustomMechanicsFeature(CustomMechanicsFeature.BypassSneakAttackImmunity)) {
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(GhostCriticalAndPrecisionImmunity), nameof(GhostCriticalAndPrecisionImmunity.OnEventAboutToTrigger))]
    static class GhostCriticalAndPrecisionImmunity_SneakImmunity_Fix {
        static bool Prefix(GhostCriticalAndPrecisionImmunity __instance, RuleCalculateDamage evt) {
            if (evt.DamageBundle.WeaponDamage != null && (evt.DamageBundle.WeaponDamage.Reality & DamageRealityType.Ghost) != (DamageRealityType)0) {
                return false;
            }
            foreach (BaseDamage baseDamage in evt.DamageBundle) {
                EnergyDamage energyDamage = baseDamage as EnergyDamage;
                PhysicalDamage physicalDamage = baseDamage as PhysicalDamage;
                if (baseDamage.Type != DamageType.Force && baseDamage.Type != DamageType.Direct && (energyDamage == null || energyDamage.EnergyType != DamageEnergyType.PositiveEnergy) && (energyDamage == null || energyDamage.EnergyType != DamageEnergyType.Holy) && (energyDamage == null || energyDamage.EnergyType != DamageEnergyType.Unholy) && (energyDamage == null || energyDamage.EnergyType != DamageEnergyType.Divine)) {
                    evt.CritImmunity = true;
                    if ((baseDamage.Precision && !evt.Initiator.CustomMechanicsFeature(CustomMechanicsFeature.BypassSneakAttackImmunity)) || (physicalDamage != null && physicalDamage.EnchantmentTotal < 1)) {
                        baseDamage.AddDecline(new DamageDecline(DamageDeclineType.Total, __instance));
                    } else {
                        baseDamage.AddDecline(new DamageDecline(DamageDeclineType.ByHalf, __instance));
                    }
                }
            }
            return false;
        }
    }
}
