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
        const int rate = 4;

        public bool initialized;

        public override string Texture => TBAC.TextureHoldplacer;

        public Player GetOwner => Main.player[Projectile.owner];

        public float mouseX;
        public float mouseY;

        public Vector2 mousePosition;
        public Vector2? oldMousePosition;

        public sealed override void SetDefaults()
        {
            mouseX = 0;
            mouseY = 0;
            oldMousePosition = null;
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
                plr.standAbilities.Clear();
                Projectile.Kill();
            }

            if (GetOwner.whoAmI == Main.myPlayer) {
                mouseX = Main.MouseWorld.X;
                mouseY = Main.MouseWorld.Y;
                if(Main.GameUpdateCount % rate == 0)
                    Projectile.netUpdate = true;
            }

            mousePosition = new Vector2(mouseX, mouseY);

            SafeAI();

            if (oldMousePosition == null || oldMousePosition != new Vector2(mouseX, mouseY)) {

                oldMousePosition = new Vector2(mouseX, mouseY);
            }
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

        public Vector2 GetMousePosition()
        {
            var result = mousePosition;

            Vector2 oldMousePos = oldMousePosition ?? Vector2.Zero;

            if (Projectile.owner != Main.myPlayer)
                result = Vector2.Lerp(oldMousePos, mousePosition, 0.12f);

            return result;
        }

    }
}
