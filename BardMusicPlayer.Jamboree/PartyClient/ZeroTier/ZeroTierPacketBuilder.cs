using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BardMusicPlayer.Jamboree.ZeroTier
{
    public static class ZeroTierPacketBuilder
    {
        public static byte[] PerformanceStart()
        {
            long milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            string data = " " + milliseconds.ToString();
            byte[] msg = Encoding.ASCII.GetBytes(data);
            msg[0] = (byte)ZeroTierPartyOpcodes.OpcodeEnum.SMSG_PERFORMANCE_START;
            return msg;
        }

        public static byte[] CMSG_JOIN_PARTY(string performer_name)
        {
            string data = " " + performer_name;
            byte[] msg = Encoding.ASCII.GetBytes(data);
            msg[0] = (byte)ZeroTierPartyOpcodes.OpcodeEnum.CMSG_JOIN_PARTY;
            return msg;
        }

    }
}
