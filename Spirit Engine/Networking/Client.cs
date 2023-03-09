using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Realsphere.Spirit.Networking
{
    /// <summary>
    /// A High-Level UDP Client.
    /// </summary>
    public class Client
    {
        internal string serverIP;
        internal int serverPort;
        internal IPEndPoint serverEndPoint;
        internal bool handshakeCompleted;
        internal UdpClient Udp;
        internal bool open;

        public Action<Packet> PacketReceived;
        public Action ServerClosed;

        public Client(string serverIP, int serverPort)
        {
            this.serverIP = serverIP;
            this.serverPort = serverPort;
            this.serverEndPoint = new(IPAddress.Parse(serverIP), serverPort);
            this.Udp = new();
            open = true;
            ThreadPool.QueueUserWorkItem(delegate
            {
                while(open)
                {
                    try
                    {
                        byte[] recieved = Udp.Receive(ref serverEndPoint);
                        Packet tmp = new(recieved);
                        if (tmp.ReadString() == "action.serverclose")
                        {
                            if (ServerClosed != null) ServerClosed();
                            open = false;
                            Udp.Dispose();
                            handshakeCompleted = false;
                        }
                        Packet p = new(recieved);
                        if (PacketReceived != null) PacketReceived(p);
                        p.Dispose();
                    }catch(InvalidOperationException ex) { }
                    catch(SocketException) { }
                }
            });
        }

        /// <summary>
        /// Performs a "handshake" with the UDP Server, to tell the Server that the Client is now connected.
        /// This needs to be called before any other calls occur.
        /// </summary>
        public HandshakeResult Handshake()
        {
            if(handshakeCompleted)
                return HandshakeResult.Success;

            // Connect the Client
            try
            {
                Udp.Send(Encoding.ASCII.GetBytes("action.handshake"), serverEndPoint);
                byte[] response = Udp.Receive(ref serverEndPoint);
                if(Encoding.ASCII.GetString(response) == "response.reject")
                    return HandshakeResult.Rejected;
                else if(Encoding.ASCII.GetString(response) == "response.accept")
                {
                    Udp.Send(Encoding.ASCII.GetBytes("action.handshakeRecievedReadyForPackages"), serverEndPoint);
                    handshakeCompleted = true;
                    return HandshakeResult.Success;
                }
                else
                {
                    throw new("Error: Server responded with an Invalid Packet!\nPlease make sure you dont send any Packets in Server.ClientConnected as this will lead to Packet loss!");
                }
            }catch(SocketException)
            {
                return HandshakeResult.ServerUnreachable;
            }catch(Exception)
            {
                return HandshakeResult.Unknown;
            }
        }

        /// <summary>
        /// Disconnects the Client from the Server.
        /// </summary>
        public void Disconnect()
        {
            if(handshakeCompleted)
            {
                open = false;
                Udp.Send(Encoding.ASCII.GetBytes("action.disconnect"), serverEndPoint);
                Udp.Receive(ref serverEndPoint);
                Udp.Dispose();
                handshakeCompleted = false;
            }
        }

        /// <summary>
        /// Sends a Packet to the Server.
        /// </summary>
        public void SendPacket(Packet packet)
        {
            if (!handshakeCompleted) throw new("Handshake was not successful or never happened.");

            byte[] toSend = packet.GetBytes();
            Udp.Send(toSend, serverEndPoint);
        }
    }
}
