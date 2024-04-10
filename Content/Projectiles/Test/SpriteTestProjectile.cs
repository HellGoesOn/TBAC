using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBAC.Core.Animations;
using Terraria;
using Terraria.ModLoader;

namespace TBAC.Content.Projectiles.Test
{
    public class SpriteTestProjectile : ModProjectile
    {
        Sprite sprite;

        Player owner => Main.player[Projectile.owner];

        public override string Texture => TBAC.TextureHoldplacer;

        public override void SetDefaults()
        {
            sprite = new Sprite(TBAC.AssetPath + "Textures/Test");

            for (int i = 0; i < 14; i++) {
                Frame frame = new Frame(0, 80 * i, 50, 80);
                frame.origin = new Vector2(25, 40);
                sprite.Frames.Add(frame);
            }

            Projectile.friendly = true;
            Projectile.width = Projectile.height = 24;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
        }

        public override void AI()
        {
            Projectile.timeLeft = 60;

            Projectile.Center = owner.Center + new Vector2(50 * -owner.direction, -20);

            sprite.Update();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            base.PostDraw(lightColor);

            SpriteEffects effects = owner.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            sprite.Draw((int)Projectile.position.X, (int)Projectile.position.Y, 0, effects);
        }
    }
}
