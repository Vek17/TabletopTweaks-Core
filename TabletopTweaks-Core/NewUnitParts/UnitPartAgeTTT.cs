using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using TabletopTweaks.Core.NewEvents;

namespace TabletopTweaks.Core.NewUnitParts {
    public class UnitPartAgeTTT : OldStyleUnitPart {

        public UnitPartAgeTTT() {
        }

        public void AddNegate(AgeLevel age, NegateType type) {
            switch (type) {
                case NegateType.Physical:
                    switch (age) {
                        case AgeLevel.MiddleAge:
                            m_MiddleAgePhysicalNegate.Retain();
                            break;
                        case AgeLevel.OldAge:
                            m_OldAgePhysicalNegate.Retain();
                            break;
                        case AgeLevel.Venerable:
                            m_VenerableAgePhysicalNegate.Retain();
                            break;
                        default:
                            break;
                    }
                    break;
                case NegateType.Mental:
                    switch (age) {
                        case AgeLevel.MiddleAge:
                            m_MiddleAgeMentalNegate.Retain();
                            break;
                        case AgeLevel.OldAge:
                            m_OldAgeMentalNegate.Retain();
                            break;
                        case AgeLevel.Venerable:
                            m_VenerableAgeMentalNegate.Retain();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            EventBus.RaiseEvent<IAgeNegateHandler>(base.Owner, h => h.OnAgeNegateChanged());
        }
        public void RemoveNegate(AgeLevel age, NegateType type) {
            switch (type) {
                case NegateType.Physical:
                    switch (age) {
                        case AgeLevel.MiddleAge:
                            m_MiddleAgePhysicalNegate.Release();
                            break;
                        case AgeLevel.OldAge:
                            m_OldAgePhysicalNegate.Release();
                            break;
                        case AgeLevel.Venerable:
                            m_VenerableAgePhysicalNegate.Release();
                            break;
                        default:
                            break;
                    }
                    break;
                case NegateType.Mental:
                    switch (age) {
                        case AgeLevel.MiddleAge:
                            m_MiddleAgeMentalNegate.Release();
                            break;
                        case AgeLevel.OldAge:
                            m_OldAgeMentalNegate.Release();
                            break;
                        case AgeLevel.Venerable:
                            m_VenerableAgeMentalNegate.Release();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            EventBus.RaiseEvent<IAgeNegateHandler>(base.Owner, h => h.OnAgeNegateChanged());
            TryRemoveSelf();
        }

        private void TryRemoveSelf() {
            if (
                !m_MiddleAgePhysicalNegate
                && !m_MiddleAgeMentalNegate
                && !m_OldAgePhysicalNegate
                && !m_OldAgeMentalNegate
                && !m_VenerableAgePhysicalNegate
                && !m_VenerableAgeMentalNegate
            ) {
                base.RemoveSelf();
            }
        }

        public bool MiddleAgePhysicalNegate => m_MiddleAgePhysicalNegate;
        public bool MiddleAgeMentalNegate => m_MiddleAgeMentalNegate;
        public bool OldAgePhysicalNegate => m_OldAgePhysicalNegate;
        public bool OldAgeMentalNegate => m_OldAgeMentalNegate;
        public bool VenerableAgePhysicalNegate => m_VenerableAgePhysicalNegate;
        public bool VenerableAgeMentalNegate => m_VenerableAgeMentalNegate;

        private readonly CountableFlag m_MiddleAgePhysicalNegate = new CountableFlag();
        private readonly CountableFlag m_MiddleAgeMentalNegate = new CountableFlag();
        private readonly CountableFlag m_OldAgePhysicalNegate = new CountableFlag();
        private readonly CountableFlag m_OldAgeMentalNegate = new CountableFlag();
        private readonly CountableFlag m_VenerableAgePhysicalNegate = new CountableFlag();
        private readonly CountableFlag m_VenerableAgeMentalNegate = new CountableFlag();

        public enum AgeLevel : int {
            Adult = 0,
            MiddleAge = 1,
            OldAge = 2,
            Venerable = 3
        }
        public enum NegateType : int {
            Physical = 0,
            Mental = 1
        }
    }
}
