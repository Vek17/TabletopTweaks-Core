using Kingmaker.EntitySystem.Entities;
using TabletopTweaks.Core.ModLogic;

namespace TabletopTweaks.Core.MechanicsChanges {
    public static class FlankingMechanics {

        public static IFlankingProvider FlankingProvider => m_FlankingProvider;
        public static bool CustomProviderLoaded => m_CustomProviderLoaded;

        private static IFlankingProvider m_FlankingProvider = new DefaultFlankingProvider();
        private static bool m_CustomProviderLoaded = false;

        public static void SetFlankingProvider(ModContextBase context, IFlankingProvider provider) {
            m_FlankingProvider = provider;
            m_CustomProviderLoaded = true;
            context.Logger.Log($"TabletopTweaks-Core FlankingProvider set by {context.ModEntry.Info.Id}");
        }
        public static bool IsFlankedBy(this UnitEntityData target, UnitEntityData initiator) {
            var result = FlankingProvider.CheckFlankedBy(target, initiator);
            //Main.TTTContext.Logger.Log($"Flanking check {initiator.CharacterName} -> {target.CharacterName}: {result}");
            return result;
        }

        public interface IFlankingProvider {

            public float FlankingAngle { get; }
            public float FlankingAngleImproved { get; }
            public bool ApplyToEnemies { get; }

            public bool CheckFlankedBy(UnitEntityData target, UnitEntityData initiator);
        }
        private class DefaultFlankingProvider : IFlankingProvider {

            public float FlankingAngle => 0;
            public float FlankingAngleImproved => 0;
            public bool ApplyToEnemies => true;

            public bool CheckFlankedBy(UnitEntityData target, UnitEntityData initiator) {
                if (target == null || initiator == null) { return false; }
                if (!target.CombatState.IsEngage(initiator)) { return false; }
                return target.CombatState.EngagedBy.Count > 1;
            }
        }
    }
}
