using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBAC.Content.Systems
{
    public class StandAbility
    {
        // TO-DO: Implement Localization;
        // Priority: low
        public readonly string name;
        public readonly string description;

        public Action OnUse;

        public StandAbility(string name, string description = "")
        {
            this.name = name;
            this.description = description;
        }
    }
}
