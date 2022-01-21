/*
 * Copyright(c) 2021 MoogleTroupe
 * Licensed under the GPL v3 license. See https://github.com/BardMusicPlayer/BardMusicPlayer/blob/develop/LICENSE for full license information.
 */

using BardMusicPlayer.Jamboree.Events;
using BardMusicPlayer.Jamboree.ZeroTier;
using System;
using System.Text;
using ZeroTier.Sockets;

namespace BardMusicPlayer.Jamboree
{
    public class PartyGame
    {
        public Socket Socket { get{ return _socket; } }

        /// <summary>
        /// Is this session a (0) bard, (1) dancer
        /// </summary>
        public byte Performer_Type { get; set; } = 254;
        public string Performer_Name { get; set; } = "Unknown";


        private Socket _socket = null;
        private bool _server = false;

        internal PartyGame(Socket socket, bool server)
        {
            _socket = socket;
            _server = server;
        }

        public bool Update()
        {
            byte[] bytes = new byte[1024];
            if (_socket.Available == -1)
                return false;
            if (_socket.Poll(100, System.Net.Sockets.SelectMode.SelectRead))
            {
                int bytesRec = 0;
                try
                {
                    bytesRec = _socket.Receive(bytes);
                    if (bytesRec == -1)
                    {
                        CloseConnection();
                        return false;
                    }
                    else
                    {
                        ZeroTierPartyOpcodes.OpcodeEnum opcode = (ZeroTierPartyOpcodes.OpcodeEnum)bytes[0];
                        if (_server)
                            serverOpcodeHandling(opcode, bytes, bytesRec);
                        else
                            clientOpcodeHandling(opcode, bytes, bytesRec);
                    }
                }
                catch (SocketException err)
                {
                    Console.WriteLine(
                            "ServiceErrorCode={0} SocketErrorCode={1}",
                            err.ServiceErrorCode,
                            err.SocketErrorCode);
                    return false;
                }
            }

            return true;
        }

        public bool SendPacket(byte[] pck)
        {
            if (_socket.Available == -1)
                return false;

            try { _socket.Send(pck); }
            catch { return false; }
            return true;
        }

        private void serverOpcodeHandling(ZeroTierPartyOpcodes.OpcodeEnum opcode, byte[] bytes, int bytesRec)
        {
            switch (opcode)
            {
                case ZeroTierPartyOpcodes.OpcodeEnum.CMSG_JOIN_PARTY:
                    Performer_Type = bytes[1];
                    Performer_Name = Encoding.ASCII.GetString(bytes, 2, bytesRec);
                    break;
                default:
                    break;
            };
        }

        private void clientOpcodeHandling(ZeroTierPartyOpcodes.OpcodeEnum opcode, byte[] bytes, int bytesRec)
        {
            switch (opcode)
            {
                case ZeroTierPartyOpcodes.OpcodeEnum.SMSG_PERFORMANCE_START:
                    BmpJamboree.Instance.PublishEvent(new PerformanceStartEvent(Convert.ToInt64(Encoding.ASCII.GetString(bytes, 1, bytesRec))));
                    break;
                case ZeroTierPartyOpcodes.OpcodeEnum.SMSG_JOIN_PARTY:
                    Performer_Type = bytes[1];
                    Performer_Name = Encoding.ASCII.GetString(bytes, 2, bytesRec);
                    break;
                default:
                    break;
            };
        }

        public void CloseConnection()
        {
            _socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
            _socket.Close();
        }
    }
}
