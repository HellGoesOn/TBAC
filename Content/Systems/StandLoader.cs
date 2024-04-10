using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TBAC.Content.Projectiles;
using Terraria;
using Terraria.ModLoader;

namespace TBAC.Content.Systems
{
    public static class StandLoader
    {
        public static List<int> Stands { get; set; } = new List<int>();

        public static void Load()
        {
            /*foreach(var projectile in ModContent.GetContent<ModProjectile>().OfType<StandProjectile>()) {
                Stands.Add(projectile.Type);
            }*/
        }


        public static void Unload()
        {
            Stands.Clear();
            Stands = null;
        }
    }
}
