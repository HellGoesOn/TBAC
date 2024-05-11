using Microsoft.Xna.Framework;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TBAC.Content.Projectiles.StarPlatinum;
using TBAC.Content.Projectiles.Test;
using TBAC.Core;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace TBAC.Content.Systems.Players
{
    public partial class TBAPlayer : ModPlayer
    {
        public int currentStand;
        public int usedAbilityId;
        public bool isStandActive;
        public List<StandAbility> standAbilities;

        public override void Initialize()
        {
            usedComboId = -1;
            usedAbilityId = -1;
            currentStand = ModContent.ProjectileType<StarPlatinumProjectile>();
            Combos = new List<InputCombo>();
            standAbilities = new List<StandAbility>();
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

            if(usedAbilityId != -1 && standAbilities.Count > usedAbilityId) {
                standAbilities[usedAbilityId].OnUse?.Invoke();
                usedAbilityId = -1;
            }

            if (Combos.Any(x => x.IsSuccessfull())) {
                usedComboId = Combos.FindIndex(x => x.IsSuccessfull());
                isComboAlive = false;
                foreach (var item in Combos) {
                    item.ResetProgress();
                }

                SendUsedComboPacket();
            }
        }

        public void SendUsedComboPacket()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)PacketType.UsedCombo);
                packet.Write((byte)Player.whoAmI);
                packet.Write((byte)usedComboId);
                packet.Send();
            }
        }

        public void SendUsedAbilityPacket()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)PacketType.UsedAbility);
                packet.Write((byte)Player.whoAmI);
                packet.Write((byte)usedAbilityId);
                packet.Send();
            }
        }

        public InputCombo AddCombo(params TimedInput[] inputs)
        {
            var combo = new InputCombo(inputs);
            Combos.Add(combo);
            return combo;
        }

        public StandAbility AddAbility(string name, string descrpition = "")
        {
            var result = new StandAbility(name, descrpition);
            standAbilities.Add(result);
            return result;
        }

        public static TBAPlayer Get(Player player) => player.GetModPlayer<TBAPlayer>();
    }
}
