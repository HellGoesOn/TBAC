using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using TBAC.Content.Systems;
using TBAC.Core.Animations;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TBAC.Content.Projectiles.Test
{
    public class SpriteTestProjectile : StandProjectile
    {
        Sprite sprite;

        public override void SafeSetDefaults()
        {
            sprite = new Sprite(TBAC.AssetPath + "Textures/Test");

            for (int i = 0; i < 14; i++) {
                Frame frame = new Frame(0, 80 * i, 50, 80);
                frame.origin = new Vector2(25, 40);
                sprite.Frames.Add(frame);
            }

            Projectile.friendly = true;
            Projectile.width = 50;
            Projectile.height = 80;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
        }

        public override void SafeAI()
        {
            Projectile.timeLeft = 60;

            Projectile.Center = GetOwner().Center + new Vector2(50 * -GetOwner().direction, -20);

            sprite.Update();
        }

        public override void PostDraw(Color lightColor)
        {
            base.PostDraw(lightColor);

            SpriteEffects effects = GetOwner().direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            sprite.Draw((int)Projectile.Center.X, (int)Projectile.Center.Y, 0, effects);
        }
    }
}
