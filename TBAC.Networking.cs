using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TBAC.Content.Systems.Players;
using TBAC.Core;
using Terraria;
using Terraria.ID;

namespace TBAC
{
    public partial class TBAC
    {
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            PacketType packetType = (PacketType)reader.ReadByte();

            switch(packetType) {
                case PacketType.PlayerSync:
                    byte playerNum = reader.ReadByte();
                    TBAPlayer plr = TBAPlayer.Get(Main.player[playerNum]);
                    plr.ReceivePlayerSync(reader);

                    if(Main.netMode == NetmodeID.Server) {
                        plr.SyncPlayer(-1, whoAmI, false);
                    }
                    break;
            }
        }
    }
}
