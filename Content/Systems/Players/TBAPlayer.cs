using Microsoft.Xna.Framework;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TBAC.Content.Projectiles.Test;
using TBAC.Core;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace TBAC.Content.Systems.Players
{
    public partial class TBAPlayer : ModPlayer
    {
        public int currentStand;
        public bool isStandActive;

        public override void Initialize()
        {
            usedComboId = -1;
            currentStand = -1;
            Combos = new List<InputCombo>();
        }

        public override void PostUpdate()
        {
            if(isStandActive) {
                if (Player.ownedProjectileCounts[currentStand] < 1)
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, currentStand, 0, 0, Player.whoAmI);
            }

            if (usedComboId != -1 && Combos.Count > usedComboId) {
                Combos[usedComboId].activationEffect?.Invoke();
                usedComboId = -1;
            }

            if (Combos.Any(x => x.IsSuccessfull())) {
                usedComboId = Combos.FindIndex(x => x.IsSuccessfull());
                isComboAlive = false;
                foreach (var item in Combos) {
                    item.ResetProgress();
                }
            }
        }

        public InputCombo AddCombo(params TimedInput[] inputs)
        {
            var combo = new InputCombo(inputs);

            Combos.Add(combo);

            return combo;
        }

        public static TBAPlayer Get(Player player) => player.GetModPlayer<TBAPlayer>();
    }
}
