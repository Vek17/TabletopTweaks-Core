using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Core.Main;

namespace TabletopTweaks.Core.NewContent.Features {
    class LongspearChargeBuff {
        public static void AddLongspearChargeBuff() {
            var LongspearChargeBuff = Helpers.CreateBuff(modContext: TTTContext, "LongspearChargeBuff", bp => {
                bp.SetName("Longspear Charge");
                bp.SetDescription("");
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent(Helpers.Create<AddOutgoingWeaponDamageBonus>(c => {
                    c.BonusDamageMultiplier = 1;
                }));
                bp.AddComponent(Helpers.Create<RemoveBuffOnAttack>());
            });
        }
    }
}
