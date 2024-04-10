using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace TBAC.Content.Systems
{
    public class TBAInput : ModSystem
    {
        public static ModKeybind SummonStand { get; private set; }
        public static ModKeybind GetStand { get; private set; }

        public override void Load()
        {
            SummonStand = KeybindLoader.RegisterKeybind(Mod, "SummonStand", "Z");
            GetStand = KeybindLoader.RegisterKeybind(Mod, "GetRandomStand", "X");
        }

        public override void Unload()
        {
            SummonStand = null;
            GetStand = null;
        }
    }
}
