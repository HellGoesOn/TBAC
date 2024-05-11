using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TBAC.Content.Projectiles.StarPlatinum;

namespace TBAC.Content.Systems.TimeStop
{
    public partial class TimeStopSystem : ModSystem
    {
        public List<TimeStopInstance> timeStops;

        public List<int> immunityWhiteList;

        public override void PostUpdateEverything()
        {
            base.PostUpdateEverything();

            foreach(var timeStop in timeStops) {
                timeStop.Update();
            }

            if (IsTimeStopped) {
                Main.time -= 1;
            }

            timeStops.RemoveAll(x => x.duration <= 0);
        }

        public override void Load()
        {
            immunityWhiteList = new List<int>() {
                ModContent.ProjectileType<StarPlatinumProjectile>()
            };
            timeStops = new List<TimeStopInstance>();
            LoadOnEdits();
        }

        public override void Unload()
        {
            timeStops.Clear();
            UnloadEdits();
        }

        public static TimeStopInstance StopTimeFor(byte who, int duration)
        {
            var result = new TimeStopInstance(who, duration);
            Get().timeStops.Add(result);
            return result;
        }

        public bool IsTimeStopped => timeStops.Count > 0;

        public static bool IsEntityImmuned(byte who) => Get().timeStops.Any(x => x.owner == who);

        public static TimeStopSystem Get() => ModContent.GetInstance<TimeStopSystem>();
    }

    public class TimeStopInstance
    {
        public byte owner;
        public int duration;
        public int progress;
        public const int progressMax = 60;

        public TimeStopInstance(byte owner, int duration)
        {
            this.owner = owner;
            this.duration = duration;
        }

        public void Update()
        {
            if (progress <= progressMax) {
                progress++;
                return;
            }

            if (duration > 0)
                duration--;
        }
    }
}
