using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaks.Core.NewComponents.Properties {
    [TypeId("08b794694fb94e85abf600faf6e5d92f")]
    public class MasterSimplePropertyGetter : PropertyValueGetter {

        public  override int GetBaseValue(UnitEntityData unit) {
            if (!unit.IsPet || unit.Master == null) { return 0; }
                
            return this.Property.GetInt(unit.Master);
        }

        public UnitProperty Property;
    }
}
