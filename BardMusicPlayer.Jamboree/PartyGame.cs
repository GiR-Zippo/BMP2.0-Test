/*
 * Copyright(c) 2021 MoogleTroupe
 * Licensed under the GPL v3 license. See https://github.com/BardMusicPlayer/BardMusicPlayer/blob/develop/LICENSE for full license information.
 */

using BardMusicPlayer.Jamboree.ZeroTier;
using System;
using System.Text;
using ZeroTier.Sockets;

namespace BardMusicPlayer.Jamboree
{
    public class PartyGame
    {
        public Socket Socket { get{ return _socket; } }

        private Socket _socket = null;

        internal PartyGame(Socket socket)
        {
            _socket = socket;
        }

        public bool Update()
        {
            byte[] bytes = new byte[1024];
            if (_socket.Available == -1)
                return false;
            if (_socket.Poll(1, System.Net.Sockets.SelectMode.SelectRead))
            {
                int bytesRec = 0;
                try
                {
                    bytesRec = _socket.Receive(bytes);
                    if (bytesRec == -1)
                    {
                        _socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                        _socket.Close();
                        return false;
                    }
                    else
                    {
                        ZeroTierPartyOpcodes.OpcodeEnum opcode = (ZeroTierPartyOpcodes.OpcodeEnum)bytes[0];
                        string trunk = Encoding.ASCII.GetString(bytes, 1, bytesRec);
                        switch (opcode)
                        {
                            case ZeroTierPartyOpcodes.OpcodeEnum.CMSG_JOIN_PARTY:
                                Console.WriteLine(trunk);
                                break;
                            default:
                                break;
                        };
                        Console.WriteLine("Client: Recv: {0}", trunk);
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

        public void SendPacket(byte[] pck)
        {
            if (_socket.Available == -1)
                return;

            try
            {
                _socket.Send(pck);
            }
            catch
            {
            }
        }
    }
}
