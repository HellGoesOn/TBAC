using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAC.Core;
using Terraria.ModLoader;
using Terraria;

namespace TBAC.Content.Systems.Players
{
    public partial class TBAPlayer
    {

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)PacketType.PlayerSync);
            packet.Write((byte)Player.whoAmI);
            packet.Write((int)currentStand);
            packet.Write((int)usedComboId);
            packet.Write((bool)isStandActive);
            packet.Send(toWho, fromWho);
        }

        public override void CopyClientState(ModPlayer targetCopy)
        {
            TBAPlayer clone = (TBAPlayer)targetCopy;
            clone.currentStand = currentStand;
            clone.usedComboId = usedComboId;
            clone.isStandActive = isStandActive;
        }

        public void ReceivePlayerSync(BinaryReader reader)
        {
            currentStand = reader.ReadInt32();
            usedComboId = reader.ReadInt32();
            isStandActive = reader.ReadBoolean();
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            TBAPlayer clone = (TBAPlayer)clientPlayer;

            if (currentStand != clone.currentStand || isStandActive != clone.isStandActive || clone.usedComboId != usedComboId) {
                SyncPlayer(-1, Main.myPlayer, false);
            }
        }

    }
}
