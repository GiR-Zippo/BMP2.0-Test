using BardMusicPlayer.Jamboree.PartyManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroTier.Sockets;

namespace BardMusicPlayer.Jamboree.ZeroTier
{
    public class ZeroTierPartyServer
    {
        private SocketServer svcWorker { get; set; } = null;

        public ZeroTierPartyServer(IPEndPoint iPEndPoint)
        {
            BackgroundWorker objWorkerServerDiscovery = new BackgroundWorker();
            objWorkerServerDiscovery.WorkerReportsProgress = true;
            objWorkerServerDiscovery.WorkerSupportsCancellation = true;

            svcWorker = new SocketServer(ref objWorkerServerDiscovery, iPEndPoint);
            objWorkerServerDiscovery.DoWork += new DoWorkEventHandler(svcWorker.Start);
            objWorkerServerDiscovery.ProgressChanged += new ProgressChangedEventHandler(logWorkers_ProgressChanged);
            objWorkerServerDiscovery.RunWorkerAsync();
        }

        public void SetPlayerData(byte type, string name)
        {
            svcWorker.SetPlayerData(type, name);
        }

        public void SendToAll(byte[] pck)
        {
            svcWorker.SendToAll(pck);
        }

        public void Close()
        {
            svcWorker.Stop();
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

        private BackgroundWorker worker = null;
        private List<ZeroTierPartySocket> sessions = new List<ZeroTierPartySocket>();
        List<ZeroTierPartySocket> removed_sessions = new List<ZeroTierPartySocket>();

        private PartyClientInfo _clientInfo = new PartyClientInfo();

        public SocketServer(ref BackgroundWorker w, IPEndPoint localEndPoint)
        {
            worker = w;
            iPEndPoint = localEndPoint;
            worker.ReportProgress(1, "Server");
            
            //Temporary
            PartyManager.Instance.Add(_clientInfo);
        }

        public void Start(object sender, DoWorkEventArgs e)
        {
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
                        //Aufm server erstellt, also ist server = true
                        ZeroTierPartySocket session = new ZeroTierPartySocket(handler, true);
                        sessions.Add(session);
                    }
                }

                //Update the sessions
                foreach (ZeroTierPartySocket session in sessions)
                {
                    if (!session.Update())
                        removed_sessions.Add(session);
                }
                
                //Remove dead sessions
                foreach (ZeroTierPartySocket session in removed_sessions)
                {
                    sessions.Remove(session);
                    SendToAll(ZeroTierPacketBuilder.SMSG_LEAVE_PARTY(session.PartyClient.Performer_Type, session.PartyClient.Performer_Name));
                }
                //And clear the list
                removed_sessions.Clear();
                Task.Delay(5);
            }

            //Finished serving - close all
            foreach (ZeroTierPartySocket s in sessions)
            {
                // Release the socket.
                s.CloseConnection();
            }
            listener.Close();
            return;
        }

        public void SendToAll(byte[] pck)
        {
            foreach (ZeroTierPartySocket session in sessions)
                session.SendPacket(pck);
        }

        public void Stop()
        {
            this.disposing = true;
        }

        public void SetPlayerData(byte type, string name)
        {
            _clientInfo.Performer_Type = type;
            _clientInfo.Performer_Name = name;
        }
    }
}
