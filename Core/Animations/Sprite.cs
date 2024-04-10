using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace TBAC.Core.Animations
{
    public class Sprite
    {
        public string texturePath;

        public int currentFrame;
        public int frameTime;
        public int FrameCount => Frames.Count;

        public List<Frame> Frames { get; private set; } = new();

        public Sprite(string texturePath)
        {
            this.texturePath = texturePath;
            currentFrame = 0;
            frameTime = 0;
        }

        public void Update()
        {
            if (Frames.Count == 0)
                return;

            if (++frameTime >= Frames[currentFrame].frameLength) {

                if (++currentFrame >= FrameCount) {
                    currentFrame = 0;
                }

                frameTime = 0;
            }
        }

        public void Draw(int X, int Y, float rotation, SpriteEffects spriteEffects = default)
        {
            var texture = ModContent.Request<Texture2D>(texturePath).Value;

            if (Frames.Count == 0 || texture == null) // do not draw if texture isn't loaded or we have no animations frames added.
                return;

            Vector2 position = new (X, Y); // we are using integers to remove jittering that appears when using decimals
            Frame frame = Frames[currentFrame]; // use current frame for drawing
            SpriteEffects effects = spriteEffects == default ? frame.effects : spriteEffects; /* if we don't override spriteEffects, we'll be using the one supplied by the frame.
                                                                                               * useful in case we want an animation that wants to always face a certain direction*/

            Main.EntitySpriteDraw(texture, position - Main.screenPosition, frame.GetRect(), frame.color, rotation, frame.origin, frame.scale, effects, 0);
        }
    }
}
