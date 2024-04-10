using Microsoft.Xna.Framework;
using Newtonsoft.Json.Converters;
using System.IO;
using TBAC.Content.Projectiles.Test;
using TBAC.Core;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace TBAC.Content.Systems.Players
{
    public partial class TBAPlayer : ModPlayer
    {
        public int currentStand;
        public bool isStandActive;

        public override void Initialize()
        {
            currentStand = -1;
        }

        public override void PostUpdate()
        {
            if(isStandActive) {
                if (Player.ownedProjectileCounts[currentStand] < 1)
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, currentStand, 0, 0, Player.whoAmI);
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if(TBAInput.GetStand.JustPressed) {
                currentStand = StandLoader.Stands[Main.rand.Next(0, StandLoader.Stands.Count)];
                Main.NewText($"Total Stand Count: {StandLoader.Stands.Count}; Got Stand {currentStand}");
            }

            if (currentStand == -1) // do nothing if we have no stand selected
                return;

            if(TBAInput.SummonStand.JustPressed) {
                isStandActive = !isStandActive;
            }
        }

        public static TBAPlayer Get(Player player) => player.GetModPlayer<TBAPlayer>();
    }
}
