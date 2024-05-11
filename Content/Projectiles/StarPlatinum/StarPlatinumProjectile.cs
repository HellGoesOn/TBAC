using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAC.Content.Systems;
using TBAC.Content.Systems.Players;
using TBAC.Content.Systems.TimeStop;
using TBAC.Core.Animations;
using Terraria;

namespace TBAC.Content.Projectiles.StarPlatinum
{
    public class StarPlatinumProjectile : StandProjectile
    {
        Dictionary<string, Sprite> sprites; // <-- stores all animations

        const string Anim_Idle = "Idle"; // <-- shorthands for animations, useful to avoid any typing mistakes & makes it easier to change later without having go through the entire code
        const string Anim_PunchMid1 = "PunchMid1";
        const string Anim_PunchMid2 = "PunchMid2";
        const string Anim_UpperCut = "UpperCut";

        private string _currentAnimation;

        public float PunchTimer {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        int elapsedTime;
        bool lastRMBState;
        bool lockedOverride;

        public override void SafeSetDefaults()
        {
            sprites = new Dictionary<string, Sprite>();
            
            string path = TBAC.AssetPath + "Textures/StarPlatinum/";

            var a_idle = new Sprite(path + "Idle"); // <-- create Sprite object. It will store frame data to be used for animation purposes

            for (int i = 0; i < 14; i++) { // <-- Adds 14 frames. Since spritesheet is strictly vertical and contains one animation, there's no need to be more specific than that
                Frame frame = new Frame(0, 80 * i, 50, 80, 4); // <-- create frame object; check Frame.cs for the data it contains.
                                                             
                frame.origin = new Vector2(25, 40); // <-- Point around which frame is rotated. It's set per frame, but can overriden later when drawn depending on use case
                a_idle.Frames.Add(frame); // <-- finally add the frame to the animation
            }

            a_idle.onAnimationStart += () => { Projectile.damage = 0; };

            var a_punchMid1 = new Sprite(path + "PunchMid1");

            for (int i = 0; i < 3; i++) {
                Frame frame = new Frame(0, 80 * i, 80, 80, 5);

                frame.origin = new Vector2(40, 40);
                a_punchMid1.Frames.Add(frame); 
            }

            a_punchMid1.onAnimationEnd += () => { SetAnimation(Anim_Idle); };
            a_punchMid1.onAnimationStart += () => { Projectile.damage = 50; };

            var a_uppercut = new Sprite(path + "Uppercut");

            for (int i = 0; i < 15; i++) { 
                Frame frame = new Frame(0, 80 * i, 80, 80, 4);

                frame.origin = new Vector2(40, 40);
                a_uppercut.Frames.Add(frame);
            }

            a_uppercut.onAnimationEnd += () => { lockedOverride = false; SetAnimation(Anim_Idle); };
            a_uppercut.onAnimationStart += () => { Projectile.damage = 300; };

            sprites.Add(Anim_Idle, a_idle); // <-- adds sprite to be used
            sprites.Add(Anim_PunchMid1, a_punchMid1);
            sprites.Add(Anim_UpperCut, a_uppercut);

            Projectile.friendly = true;
            Projectile.width = 50;
            Projectile.height = 80;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;

            SetAnimation(Anim_Idle);
        }

        public override bool PreAI()
        {
            if(!initialized) {
                var plr = TBAPlayer.Get(GetOwner);

                var test2 = plr.AddCombo(new(ValidInput.LMB, 0), new(ValidInput.LMB, 30, 90), new(ValidInput.LMB, 0));
                var test1 = plr.AddCombo(new(ValidInput.LMB, 0), new(ValidInput.LMB, 0), new(ValidInput.LMB, 0));

                test1.activationEffect += () => { Console.WriteLine("Test 1");  Main.NewText("Test 1"); };
                test2.activationEffect += () => {
                    SetAnimation(Anim_UpperCut);
                    var text = $"Player {plr.Player.name} used Test 2 for Projectile {Projectile.whoAmI} resulting in {_currentAnimation} being used";
                    Console.WriteLine(text);
                    Main.NewText(text);
                    lockedOverride = true;
                };

                plr.AddAbility("Time Stop").OnUse += () =>
                {
                    TimeStopSystem.StopTimeFor((byte)plr.Player.whoAmI, 300);
                    Main.NewText("STAR PLATINUM: THE WORLD!");
                };
                plr.AddAbility("Plot Armor");
                plr.AddAbility("'So they are the same type..'").OnUse += () => { Main.NewText($"<{plr.Player.name}>So they are the same type of stand.."); };
                plr.AddAbility("Test4");
                plr.AddAbility("Test5");

                initialized = true;
            }

            return base.PreAI();
        }

        public override void SafeAI()
        {
            Projectile.timeLeft = 60;

            var destination = GetOwner.Center + new Vector2(50 * -GetOwner.direction, -20);

            if (GetOwner.controlUseItem && !lastRMBState) {
                SetAnimation(Anim_PunchMid1);
                PunchTimer = 120;
            }

            if (PunchTimer > 0) {
                PunchTimer--;
                destination = GetMousePosition();
            }

            Projectile.Center = Vector2.SmoothStep(Projectile.Center, destination, 0.3f);
            elapsedTime++;
            lastRMBState = GetOwner.controlUseItem;
            sprites[_currentAnimation].Update();
        }

        public override void PostDraw(Color lightColor)
        {
            base.PostDraw(lightColor);

            SpriteEffects effects = GetOwner.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            sprites[_currentAnimation].Draw((int)Projectile.Center.X, (int)Projectile.Center.Y, 0, effects);
            Utils.DrawBorderString(Main.spriteBatch, $"{_currentAnimation}", Projectile.Center - Main.screenPosition - new Vector2(20, 40), Color.White, 0.75f);
        }

        public void SetAnimation(string value, bool allowSame = false)
        {
            if (lockedOverride)
                return;

            if (!string.IsNullOrWhiteSpace(_currentAnimation) && (value != _currentAnimation || allowSame)) { // <-- prevents resetting animation needlessly & a crash if we had no animation set before
                sprites[_currentAnimation].currentFrame = 0;
                sprites[_currentAnimation].frameTime = 0;
            }
            _currentAnimation = value;
            sprites[_currentAnimation].onAnimationStart?.Invoke();
        }
    }
}
