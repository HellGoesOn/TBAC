using Microsoft.Xna.Framework;
using TBAC.Content.Projectiles.Test;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace TBAC.Content.Systems.Players
{
    public class TBAPlayer : ModPlayer
    {
        public int currentStand;
        public bool isStandActive;

        public override void Initialize()
        {
            currentStand = -1;
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
                isStandActive = true;

                if (Player.ownedProjectileCounts[currentStand] < 1)
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, currentStand, 0, 0, Player.whoAmI);
            }
        }

        public static TBAPlayer Get(Player player) => player.GetModPlayer<TBAPlayer>();
    }
}
