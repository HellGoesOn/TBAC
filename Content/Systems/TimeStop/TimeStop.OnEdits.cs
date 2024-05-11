using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ModLoader;

namespace TBAC.Content.Systems.TimeStop
{
    public partial class TimeStopSystem : ModSystem
    {
        public void UnloadEdits()
        {
            On_NPC.VanillaAI -= On_NPC_VanillaAI;
            On_NPC.AI -= On_NPC_AI;
            On_Projectile.AI -= On_Projectile_AI;
            On_Rain.Update -= On_Rain_Update;
            On_Dust.UpdateDust -= On_Dust_UpdateDust;
            On_Liquid.UpdateLiquid -= On_Liquid_UpdateLiquid;
            On_Gore.Update -= On_Gore_Update;
        }

        public void LoadOnEdits()
        {
            On_NPC.VanillaAI += On_NPC_VanillaAI;
            On_NPC.AI += On_NPC_AI;
            On_Projectile.AI += On_Projectile_AI;
            On_Rain.Update += On_Rain_Update;
            On_Dust.UpdateDust += On_Dust_UpdateDust;
            On_Liquid.UpdateLiquid += On_Liquid_UpdateLiquid;
            On_Gore.Update += On_Gore_Update;
        }

        private void On_Gore_Update(On_Gore.orig_Update orig, Gore self)
        {
            if (timeStops.Count > 0) {
                var timestop = timeStops[0];
                int time = timestop.progress;
                int denom = GetDenom(time, 20, 40);

                if (time < TimeStopInstance.progressMax)
                    orig.Invoke(self);
                else {
                    //self.position -= self.velocity;
                }

                if (time % denom == 0) {
                    self.position -= self.velocity;
                    return;
                }

                return;
            }

            orig?.Invoke(self);
        }

        private void On_Liquid_UpdateLiquid(On_Liquid.orig_UpdateLiquid orig)
        {
            if (timeStops.Count > 0) {
                var timestop = timeStops[0];
                int time = timestop.progress;

                if (time < TimeStopInstance.progressMax)
                    orig.Invoke();

                return;
            }

            orig?.Invoke();
        }

        private void On_Dust_UpdateDust(On_Dust.orig_UpdateDust orig)
        {
            if (timeStops.Count > 0) {
                var timestop = timeStops[0];
                int time = timestop.progress;

                if (time < TimeStopInstance.progressMax)
                    orig.Invoke();

                return;
            }

            orig?.Invoke();
        }

        private void On_Rain_Update(On_Rain.orig_Update orig, Rain self)
        {
            if (timeStops.Count > 0) {
                var timestop = timeStops[0];
                int time = timestop.progress;
                int denom = GetDenom(time, 20, 40);

                if (time < TimeStopInstance.progressMax)
                    orig.Invoke(self);
                else {
                    //self.position -= self.velocity;
                }

                if (time % denom == 0) {
                    self.position -= self.velocity;
                    return;
                }

                return;
            }

            orig?.Invoke(self);
        }

        private int GetDenom(int value, int num1, int num2)
        {
            return (value < num1) ? 6 : (value >= num2 && value < num2) ? 4 : 2;
        }

        private void On_Projectile_AI(On_Projectile.orig_AI orig, Projectile self)
        {
            if (timeStops.Count > 0) {

                List<int> whiteList = new List<int>();
                foreach (var instance in timeStops) {
                    whiteList.Add(instance.owner);
                }

                if(TimeStopGlobalProjectile.localTimeStopProgress[self.whoAmI] <= 2 || (whiteList.Contains(self.owner) && self.friendly && Get().immunityWhiteList.Contains(self.type))) {
                    orig.Invoke(self);
                    return;
                }

                int time = TimeStopGlobalProjectile.localTimeStopProgress[self.whoAmI];
                int denom = (time < 20) ? 5 : (time >= 20 && time < 40) ? 3 : 2;
                self.timeLeft++;

                if (time < TimeStopInstance.progressMax)
                    orig.Invoke(self);
                else {
                    self.position = self.oldPosition;
                    self.frameCounter = 0;
                }

                if (time % denom == 0) {
                    self.position = self.oldPosition;
                    self.frameCounter -= 1;
                    return;
                }

                return;
            }

            orig?.Invoke(self);
        }

        private void On_NPC_AI(On_NPC.orig_AI orig, NPC self)
        {
            if (timeStops.Count > 0) {
                var timestop = timeStops[0];
                int time = timestop.progress;
                int denom = GetDenom(time, 20, 40);

                if (time < TimeStopInstance.progressMax)
                    orig.Invoke(self);
                else {
                    self.position = self.oldPosition;
                    self.frameCounter = 0;
                }

                if (time % denom == 0) {
                    self.position = self.oldPosition;
                    self.frameCounter -= 1;
                    return;
                }

                return;
            }

            orig?.Invoke(self);
        }

        private void On_NPC_VanillaAI(On_NPC.orig_VanillaAI orig, NPC self)
        {
            if (timeStops.Count > 0) {
                var timestop = timeStops[0];
                int time = timestop.progress;
                int denom = GetDenom(time, 20, 40);

                if (time < TimeStopInstance.progressMax)
                    orig.Invoke(self);
                else {
                    self.position = self.oldPosition;
                    self.frameCounter = 0;
                }

                if (time % denom == 0) {
                    self.position = self.oldPosition;
                    self.frameCounter -= 1;
                    return;
                }

                return;
            }

            orig?.Invoke(self);
        }
    }
}
