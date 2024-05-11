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
        public static ModKeybind OpenAbilityWheel { get; private set; }
        public static ModKeybind AbilityOne { get; private set; }
        public static ModKeybind AbilityTwo { get; private set; }
        public static ModKeybind AbilityThree { get; private set; }

        public override void Load()
        {
            SummonStand = KeybindLoader.RegisterKeybind(Mod, "SummonStand", "Z");
            OpenAbilityWheel = KeybindLoader.RegisterKeybind(Mod, "OpenAbilityWheel", "X");
            AbilityOne = KeybindLoader.RegisterKeybind(Mod, "AbilityOne", "C");
            AbilityTwo = KeybindLoader.RegisterKeybind(Mod, "AbilityTwo", "V");
            AbilityThree = KeybindLoader.RegisterKeybind(Mod, "AbilityThree", "B");
        }

        public override void Unload()
        {
            SummonStand = null;
            OpenAbilityWheel = null;
            AbilityOne = null;
            AbilityTwo = null;
            AbilityThree = null;
        }
    }
}
