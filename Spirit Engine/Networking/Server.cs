using Realsphere.Spirit.Mathematics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Realsphere.Spirit.Networking
{
    public class Server
    {
        internal UdpClient Udp;
        internal bool open;
        internal Dictionary<IPEndPoint,int> endPoints = new();

        public Func<string, bool> ClientConnectionRequested;
        public Action<int> ClientConnected;
        public Action<int> ClientDisconnected;
        public Action<int, Packet> PacketRecieved;

        internal Random r;

        internal int idOfEndPoint(IPEndPoint ep)
        {
            if (!endPoints.ContainsKey(ep)) return -1;

            return endPoints[ep];
        }

        internal IPEndPoint endPointOfId(int id)
        {
            if (!endPoints.ContainsValue(id)) return null;

            return endPoints.ElementAt(endPoints.Values.ToList().IndexOf(id)).Key;
        }

        internal void addClient(IPEndPoint ep)
        {
            int id = r.Next(int.MaxValue);
            while (endPoints.ContainsValue(id))
                id = r.Next(int.MaxValue);
            endPoints.Add(ep, id);
        }

        public Server(int port)
        {
            r = new();
            Udp = new UdpClient(port);
            open = true;
            // Create a Thread for receiving
            ThreadPool.QueueUserWorkItem(delegate
            {
                while(open)
                {
                    IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] buffer = Udp.Receive(ref clientEndPoint);
                    if (Encoding.ASCII.GetString(buffer) == "action.handshake")
                    {
                        if (ClientConnectionRequested(clientEndPoint.ToString()))
                        {
                            Udp.Send(Encoding.ASCII.GetBytes("response.accept"), clientEndPoint);
                        }
                        else
                            Udp.Send(Encoding.ASCII.GetBytes("response.reject"), clientEndPoint);
                    }
                    else if (Encoding.ASCII.GetString(buffer) == "action.disconnect")
                    {
                        if (ClientDisconnected != null) ClientDisconnected(idOfEndPoint(clientEndPoint));
                        endPoints.Remove(clientEndPoint);
                        Udp.Send(Encoding.ASCII.GetBytes("OK"), clientEndPoint);
                    }
                    else if (Encoding.ASCII.GetString(buffer) == "action.ping")
                    {
                        Udp.Send(Encoding.ASCII.GetBytes("response.pong"), clientEndPoint);
                    }
                    else if (Encoding.ASCII.GetString(buffer) == "action.handshakeRecievedReadyForPackages")
                    {
                        addClient(clientEndPoint);
                        if (ClientConnected != null) ClientConnected(idOfEndPoint(clientEndPoint));
                    }
                    else
                    {
                        Packet p = new(buffer);
                        PacketRecieved(idOfEndPoint(clientEndPoint), p);
                        p.Dispose();
                    }
                }
            });
        }

        /// <summary>
        /// Sends a Packet to a specific connected client.
        /// </summary>
        public void SendTo(int id, Packet packet)
        {
            if(!endPoints.ContainsValue(id))
                throw new InvalidOperationException("Client with id " + id + " has left or never connected.");

            Udp.Send(packet.GetBytes(), endPointOfId(id));
        }

        /// <summary>
        /// Sends a Packet to all connected clients.
        /// </summary>
        public void Broadcast(Packet packet)
        {
            foreach (var ep in endPoints.Keys)
                SendTo(idOfEndPoint(ep), packet);
        }

        public void Close()
        {
            Packet p = new();
            p.Write("action.serverclose");
            Broadcast(p);
            p.Dispose();
            while(endPoints.Count > 0) { }
        }
    }
}
