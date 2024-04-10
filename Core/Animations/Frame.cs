using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBAC.Core.Animations
{
    public class Frame
    {
        public int X, Y; // <-- X, Y on the spritesheet. 
        public int Width, Height; // <-- Size of the frame.
        public int frameLength; // <-- Time it takes for next frame to be displayed
        public SpriteEffects effects; // <-- Should frame be flipped or displayed as-is
        public Vector2 origin; // <-- Point around which the frame is rotated
        public Vector2 scale; // <-- Scale of the frame. Can be used to stretch it.
        public Color color; // <-- Tint of the frame.

        public Frame(int x, int y, int width, int height, int frameLength = 6)
        {
            X = x;
            Y = y;
            Width = width; 
            Height = height;
            this.frameLength = frameLength;
            effects = SpriteEffects.None;
            scale = Vector2.One;
            origin = Vector2.Zero;
            color = Color.White;
        }

        public Rectangle GetRect()
        {
            Rectangle result = new Rectangle(X, Y, Width, Height);

            return result;
        }
    }
}
