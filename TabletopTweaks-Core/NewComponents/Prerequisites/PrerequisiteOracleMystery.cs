using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Text;

namespace TabletopTweaks.Core.NewComponents.Prerequisites {
    [TypeId("064cb6d3927e4972a6d2846fb4fef335")]
    public class PrerequisiteOracleMystery : Prerequisite {

        public ReferenceArrayProxy<BlueprintFeature, BlueprintFeatureReference> Features => m_Features;
        public BlueprintFeature Bypass => m_BypassFeature?.Get();

        public override bool ConsiderFulfilled(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state) {
            //Handles alternate capstone
            return Bypass != null && unit.Progression.CharacterLevel == 20 && unit.Progression.Features.HasFact(Bypass);
        }

        public override bool CheckInternal(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state) {
            int num = 0;
            foreach (BlueprintFeature blueprintFeature in this.Features) {
                if ((!(selectionState != null) || !selectionState.IsSelectedInChildren(blueprintFeature)) && unit.HasFact(blueprintFeature)) {
                    num++;
                    if (num >= this.Amount) {
                        return true;
                    }
                }
            }
            return false;
        }

        public override string GetUITextInternal(UnitDescriptor unit) {
            StringBuilder stringBuilder = new StringBuilder();
            if (this.Amount <= 1) {
                stringBuilder.Append(string.Format("{0}:\n", UIStrings.Instance.Tooltips.OneFrom));
            } else {
                stringBuilder.Append(string.Format(UIStrings.Instance.Tooltips.FeaturesFrom, this.Amount));
                stringBuilder.Append(":\n");
            }
            for (int i = 0; i < this.Features.Length; i++) {
                stringBuilder.Append(this.Features[i].Name);
                if (i < this.Features.Length - 1) {
                    stringBuilder.Append("\n");
                }
            }
            return stringBuilder.ToString();
        }

        public BlueprintFeatureReference[] m_Features;
        public BlueprintFeatureReference m_BypassFeature;
        public int Amount = 1;
    }
}
