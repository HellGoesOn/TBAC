using Microsoft.Xna.Framework;
using System.IO;
using TBAC.Content.Systems;
using TBAC.Content.Systems.Players;
using Terraria;
using Terraria.ModLoader;

namespace TBAC.Content.Projectiles
{
    public abstract class StandProjectile : ModProjectile
    {
        public bool initialized;

        public override string Texture => TBAC.TextureHoldplacer;

        public Player GetOwner => Main.player[Projectile.owner];

        public float mouseX;
        public float mouseY;

        public sealed override void SetDefaults()
        {
            mouseX = 0;
            mouseY = 0;
            base.SetDefaults();

            if (!StandLoader.Stands.Contains(Projectile.type))
                StandLoader.Stands.Add(Projectile.type);

            SafeSetDefaults();
        }

        public virtual void SafeSetDefaults() { }

        public sealed override void AI() // sealed override to make sure some code is forced to be inhereted
        {
            TBAPlayer plr = TBAPlayer.Get(GetOwner);
            if (plr.currentStand != Projectile.type || !plr.isStandActive) {
                plr.isStandActive = false;
                plr.Combos.Clear();
                Projectile.Kill();
            }

            if (GetOwner.whoAmI == Main.myPlayer) {
                mouseX = Main.MouseWorld.X;
                mouseY = Main.MouseWorld.Y;
            }

            SafeAI();
        }

        public sealed override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            writer.Write(mouseX);
            writer.Write(mouseY);
        }

        public sealed override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            mouseX = reader.ReadSingle();
            mouseY = reader.ReadSingle();
        }

        public virtual void SafeAI() // use this instead of overriding AI
        {

        }

        public virtual void SafeSendExtraAI(BinaryWriter writer)
        {

        }

        public virtual void SafeReceiveExtraAI(BinaryReader reader)
        {

        }

        public override bool PreDraw(ref Color lightColor) /* I advise against relying on vanilla drawing because it's bad in my opinion.
                                                            * use Sprite & Frame classes & draw what you need manually */
        {
            return false;
        }
    }
}
