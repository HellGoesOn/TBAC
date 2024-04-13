using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace TBAC.Core.UI
{
    public class RadialMenuElement : UIElement
    {
        public int optionCount;
        public int highlightedOption;
        public float deadzoneRadius;
        
        public string[] options;
        private float radius = 333.0f;
        public RadialMenuElement(params string[] options)
        {
            deadzoneRadius = 80.0f;
            this.options = options;
            optionCount = this.options.Length;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //highlightedOption = 0;

            Vector2 myPosition = this.GetDimensions().Position();
            Vector2 mouse = myPosition - Main.MouseScreen;
            if (Vector2.Distance(myPosition, Main.MouseScreen) <= deadzoneRadius) {
                highlightedOption = -1;
                return;
            }

            float angleDiff = MathHelper.TwoPi / optionCount;
            float percent = 100.0f / optionCount;
            for (int i = 0; i < optionCount; i++) {

                if (CheckPoint(mouse, angleDiff * i, angleDiff*i + angleDiff)) {
                    highlightedOption = i;
                    //break;
                }
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 myPosition = this.GetDimensions().Position();

            float angleDiff = MathHelper.TwoPi / optionCount;

            float rotation = (Main.MouseScreen - myPosition).ToRotation() - MathHelper.PiOver2;
            float dist = Vector2.Distance(myPosition, Main.MouseScreen);

            spriteBatch.Draw(TextureAssets.MagicPixel.Value, myPosition, new Rectangle(0, 0, 1, (int)dist), Color.White, rotation, new Vector2(0.5f, 0), new Vector2(4, 1), SpriteEffects.None, 0f);

            Color[] colors = new[] { Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Orange, Color.Purple, Color.Cyan,
                Color.Beige, Color.BlueViolet, Color.Brown, Color.DarkBlue, Color.Crimson, Color.Magenta
            };

            for (int i = 0; i < optionCount; i++) {
                bool isHighlighted = i == highlightedOption;
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, myPosition - new Vector2(deadzoneRadius, 0).RotatedBy(angleDiff * i), new Rectangle(0, 0, 1, (int)radius), colors[i], angleDiff * i + MathHelper.PiOver2, new Vector2(0.5f, 0), new Vector2(4, 1) , SpriteEffects.None, 0f);
                Utils.DrawBorderString(spriteBatch, options[i], myPosition - new Vector2(radius, 0).RotatedBy(angleDiff * i + MathHelper.Pi / optionCount), isHighlighted  ? Color.Yellow : Color.White, isHighlighted ? 2f : 1.5f, 0.5f);
            }


        }

        private bool CheckPoint(Vector2 mousePos, float startangle, float endAngle)
        {
            float polarAngle = mousePos.ToRotation();//Math.Abs((float)Math.Atan2(y, x));

            if(polarAngle < 0) {
                polarAngle += MathHelper.TwoPi;
            }
            if (polarAngle >= startangle && polarAngle <= endAngle)
                return true;

            return false;
        }
    }
}
