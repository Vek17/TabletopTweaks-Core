using Kingmaker.UI.Common;
using Kingmaker.UI.UnitSettings;
using UnityEngine;

namespace TabletopTweaks.Core.NewUI {
    public class MechanicActionBarSlotSpellKenning : MechanicActionBarSlotSpontaneusConvertedSpell {

        public override Sprite GetIcon() {
            return Spell.Icon;
        }

        public override Sprite GetForeIcon() {
            return null;
        }

        public override Sprite GetDecorationSprite() {
            return UIUtility.GetDecorationBorderByIndex(Spell.m_ConvertedFrom.DecorationBorderNumber);
        }

        public override Color GetDecorationColor() {
            return UIUtility.GetDecorationColorByIndex(Spell.m_ConvertedFrom.DecorationColorNumber);
        }
    }
}
