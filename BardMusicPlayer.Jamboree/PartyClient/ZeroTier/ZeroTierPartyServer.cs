using BardMusicPlayer.Jamboree.PartyClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroTier.Sockets;

namespace BardMusicPlayer.Jamboree.ZeroTier
{
    public class ZeroTierPartyServer
    {
        private SocketServer svcServer { get; set; } = null;
        public ZeroTierPartyServer(IPEndPoint iPEndPoint)
        {
            BackgroundWorker objWorkerServerDiscovery = new BackgroundWorker();
            objWorkerServerDiscovery.WorkerReportsProgress = true;
            objWorkerServerDiscovery.WorkerSupportsCancellation = true;

            svcServer = new SocketServer(ref objWorkerServerDiscovery, iPEndPoint);
            objWorkerServerDiscovery.DoWork += new DoWorkEventHandler(svcServer.Start);
            objWorkerServerDiscovery.ProgressChanged += new ProgressChangedEventHandler(logWorkers_ProgressChanged);
            objWorkerServerDiscovery.RunWorkerAsync();
        }

        public void Close()
        {
            svcServer.Stop();
        }

        public void SendToAll(ZeroTierPartyOpcodes.OpcodeEnum opcode, string data)
        {
            svcServer.SendToAll(opcode, data);
        }

        public void SendToAll(byte[] pck)
        {
            svcServer.SendToAll(pck);
        }

        private void logWorkers_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Console.WriteLine(e.UserState.ToString());
        }
    }

    public class SocketServer
    {
        public bool disposing = false;
        public IPEndPoint iPEndPoint;
        public int ServerPort = 0;

        private BackgroundWorker worker;
        private List<PartyGame> sessions = new List<PartyGame>();
        List<PartyGame> removed_sessions = new List<PartyGame>();

        public SocketServer(ref BackgroundWorker worker, IPEndPoint localEndPoint)
        {
            this.worker = worker;
            this.iPEndPoint = localEndPoint;
            worker.ReportProgress(1, "Server");
        }

        public void Start(object sender, DoWorkEventArgs e)
        {
            // Data buffer for incoming data.
            byte[] bytes = new byte[1024];

            Console.WriteLine(iPEndPoint.ToString());
            Socket listener = new Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
 
            listener.Bind(iPEndPoint);
            listener.Listen(10);
            
            while (this.disposing == false)
            {
                if (listener.Poll(100, System.Net.Sockets.SelectMode.SelectRead))
                {
                    //Incomming connection
                    Socket handler = listener.Accept();
                    bool isInList = false;
                    Parallel.ForEach(sessions, session =>
                    {
                        if (session.Socket == handler)
                            isInList = true;
                    });
                    if (!isInList)
                    {
                        PartyGame session = new PartyGame(handler);
                        sessions.Add(session);
                    }
                }

                //Update the sessions
                foreach (PartyGame session in sessions)
                {
                    if (!session.Update())
                        removed_sessions.Add(session);
                }
                
                //Remove dead sessions
                foreach (PartyGame session in removed_sessions)
                {
                    sessions.Remove(session);
                }
                //And clear the list
                removed_sessions.Clear();
                Task.Delay(5);
            }

            //Finished serving - close all
            foreach (PartyGame s in sessions)
            {
                // Release the socket.
                s.Socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                s.Socket.Close();
            }
            listener.Close();
            return;
        }

        public void SendToAll(ZeroTierPartyOpcodes.OpcodeEnum opcode, string data)
        {
            foreach (PartyGame session in sessions)
            {
                data = " " + data;
                byte[] msg = Encoding.ASCII.GetBytes(data);
                msg[0] = (byte)opcode;
                session.SendPacket(msg);
            }
        }

        public void SendToAll(byte[] pck)
        {
            foreach (PartyGame session in sessions)
                session.SendPacket(pck);
        }

        public void Stop()
        {
            this.disposing = true;
        }
    }
}
