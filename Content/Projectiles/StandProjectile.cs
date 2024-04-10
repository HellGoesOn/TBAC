using Microsoft.Xna.Framework;
using TBAC.Content.Systems;
using TBAC.Content.Systems.Players;
using Terraria;
using Terraria.ModLoader;

namespace TBAC.Content.Projectiles
{
    public abstract class StandProjectile : ModProjectile
    {
        public override string Texture => TBAC.TextureHoldplacer;

        public Player GetOwner() => Main.player[Projectile.owner];

        public sealed override void SetDefaults()
        {
            base.SetDefaults();

            if (!StandLoader.Stands.Contains(Projectile.type))
                StandLoader.Stands.Add(Projectile.type);

            SafeSetDefaults();
        }

        public virtual void SafeSetDefaults() { }

        public sealed override void AI() // sealed override to make sure some code is forced to be inhereted
        {
            if (TBAPlayer.Get(GetOwner()).currentStand != Projectile.type || !TBAPlayer.Get(GetOwner()).isStandActive) {
                TBAPlayer.Get(GetOwner()).isStandActive = false;
                Projectile.Kill();
            }

            SafeAI();
        }

        public virtual void SafeAI() // use this instead of overriding AI
        {

        }

        public override bool PreDraw(ref Color lightColor) /* I advise against relying on vanilla drawing because it's bad in my opinion.
                                                            * use Sprite & Frame classes & draw what you need manually */
        {
            return false;
        }
    }
}
