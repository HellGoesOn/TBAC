using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TBAC.Content.Systems.TimeStop
{
    public class TimeStopGlobalProjectile : GlobalProjectile
    {
        public static int[] localTimeStopProgress;

        public override void Load()
        {
            localTimeStopProgress = new int[Main.maxProjectiles];
        }

        public override void PostAI(Projectile projectile)
        {
            if (TimeStopSystem.Get().IsTimeStopped) {
                if (localTimeStopProgress[projectile.whoAmI] < TimeStopInstance.progressMax+2)
                    localTimeStopProgress[projectile.whoAmI]++;
            } else {
                localTimeStopProgress[projectile.whoAmI] = 0;
            }
        }

        public override void Kill(Projectile projectile, int timeLeft)
        {
            localTimeStopProgress[projectile.whoAmI] = 0;
        }

        public override bool PreKill(Projectile projectile, int timeLeft)
        {
            localTimeStopProgress[projectile.whoAmI] = 0;
            return base.PreKill(projectile, timeLeft);
        }
    }
}
