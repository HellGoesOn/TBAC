using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;

namespace TBAC.Core.UI
{
    public class AbilityUIState : UIState
    {
        public override void OnInitialize()
        {
            RadialMenuElement radialMenuElement = new RadialMenuElement();
            radialMenuElement.HAlign = 0.5f;
            radialMenuElement.VAlign = 0.5f;
            this.Append(radialMenuElement);
            this.Recalculate();
        }
    }
}
