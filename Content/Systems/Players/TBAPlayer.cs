using Microsoft.Xna.Framework;
using TBAC.Content.Projectiles.Test;
using Terraria;
using Terraria.ModLoader;

namespace TBAC.Content.Systems.Players
{
    public class TBAPlayer : ModPlayer
    {
        public static TBAPlayer Get(Player player) => player.GetModPlayer<TBAPlayer>();

        public override void PostUpdate()
        {
            int testProjType = ModContent.ProjectileType<SpriteTestProjectile>();

            if (Player.ownedProjectileCounts[testProjType] < 1)
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, testProjType, 0, 0, Player.whoAmI);
        }
    }
}
