using HarmonyLib;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaks.Core.NewEvents {
    public interface IAgeNegateHandler : IUnitSubscriber {
        void OnAgeNegateChanged();
    }
}
