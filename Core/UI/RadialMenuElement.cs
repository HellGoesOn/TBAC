﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAC.Content.Systems.Players;
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
        private float rotation;
        public bool visible;

        public RadialOption[] menuOptions;

        float angleDiff => MathHelper.TwoPi / optionCount;

        float baseAngle = MathHelper.PiOver2;

        public RadialMenuElement(params string[] options)
        {
            visible = true;
            deadzoneRadius = 80.0f;
            this.options = options;
            optionCount = this.options.Length;
            menuOptions = new RadialOption[optionCount];

            float baseAngle = MathHelper.PiOver2 * 0;
            float endAngle = baseAngle + MathHelper.TwoPi / optionCount;
            for(int i = 0; i < optionCount; i++) {
                menuOptions[i] = new RadialOption(options[i], baseAngle, endAngle);
                baseAngle = endAngle;
                endAngle = baseAngle + MathHelper.TwoPi / optionCount;
            }

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            TBAPlayer plr = TBAPlayer.Get(Main.LocalPlayer);
            ResetAvailableOptions(plr);

            rotation += 0.06f;

            visible = plr.Player.controlUseTile;

            if (!visible || optionCount <= 0)
                return;

            Vector2 myPosition = this.GetDimensions().Position();
            Vector2 mouse = myPosition - Main.MouseScreen;

            if (Vector2.Distance(myPosition, Main.MouseScreen) <= deadzoneRadius) {
                highlightedOption = -1;
                return;
            }

            float inter = MathHelper.TwoPi / optionCount;
            for (int i = 0; i < optionCount; i++) {

                if (CheckPoint(mouse, menuOptions[i].startAngle, menuOptions[i].endAngle, inter)) {
                    highlightedOption = i;
                }
            }

        }

        private void ResetAvailableOptions(TBAPlayer plr)
        {
            if (plr.standAbilities.Count != optionCount) {

                optionCount = plr.standAbilities.Count;
                options = new string[optionCount];

                for (int i = 0; i < optionCount; i++) {
                    options[i] = plr.standAbilities[i].name;
                }

                rotation = MathHelper.PiOver2 * 0;

                float interval = MathHelper.TwoPi / optionCount;
                float baseAngle = 0;
                float endAngle = baseAngle + interval;

                menuOptions = new RadialOption[optionCount];
                for (int i = 0; i < optionCount; i++) {
                    menuOptions[i] = new RadialOption(options[i], baseAngle, endAngle);
                    baseAngle = endAngle;
                    endAngle = baseAngle + interval;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!visible || optionCount <= 0)
                return;

            Vector2 myPosition = this.GetDimensions().Position();

            float rotation = (Main.MouseScreen - myPosition).ToRotation() - MathHelper.PiOver2;
            float dist = Vector2.Distance(myPosition, Main.MouseScreen);

            spriteBatch.Draw(TextureAssets.MagicPixel.Value, myPosition, new Rectangle(0, 0, 1, (int)dist), Color.White, rotation, new Vector2(0.5f, 0), new Vector2(4, 1), SpriteEffects.None, 0f);

            for(int i = 0; i < optionCount; i++) {
                menuOptions[i].Draw(spriteBatch, myPosition, radius, this.rotation, i == highlightedOption ? Color.Goldenrod : Color.White);
            }
        }

        private bool CheckPoint(Vector2 mousePos, float startangle, float endAngle, float interval)
        {
            float checkedAngle = mousePos.ToRotation() + rotation;

            if(checkedAngle < 0) {
                checkedAngle += MathHelper.TwoPi;
            }


            //courtesy of Ryan, check out his Wii Tanks Rebirth game
            bool weirdoCheck = (startangle < MathHelper.PiOver2 && endAngle > MathHelper.PiOver2 * 3);
            bool isMouseWeirdoChecked = checkedAngle > endAngle ? (checkedAngle > endAngle && endAngle < MathHelper.TwoPi) 
                : (checkedAngle < startangle && checkedAngle > 0);

            return weirdoCheck ? isMouseWeirdoChecked : (checkedAngle > startangle && checkedAngle < endAngle);
        }

    }

    public class RadialOption
    {
        public string name;
        public float startAngle, endAngle;

        public RadialOption(string name, float startAngle, float endAngle)
        {
            this.name = name;
            this.startAngle = startAngle;
            this.endAngle = endAngle;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 startPosition, float length, float rotation, Color color = default) 
        {
            if (color == default)
                color = Color.White;

            spriteBatch.Draw(TextureAssets.MagicPixel.Value, startPosition, new Rectangle(0, 0, (int)length, 1), color, startAngle + MathHelper.Pi-rotation, new Vector2(0.5f), new Vector2(1, 4), SpriteEffects.None, 0f);

            Utils.DrawBorderString(spriteBatch, name, startPosition - new Vector2(333, 0).RotatedBy(startAngle-rotation), color, 1.5f, 0.5f);
        }

        public float Wrap(float angle)
        {
            angle = MathHelper.WrapAngle(angle);

            if (angle < 0)
                angle += MathHelper.TwoPi;

            return angle;
        }
    }
}
