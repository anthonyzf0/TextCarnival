using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TextCarnival.Source.Server
{
    class GameServer
    {
        //Server information
        private int port = 13000;   //TODO change
        private TcpListener server;
        private IPAddress localAddr;

        //All the clients on the server currently
        private List<ConnectedClient> clients;

        public GameServer()
        {
            //Server
            clients = new List<ConnectedClient>();
            localAddr = IPAddress.Parse("127.0.0.1");   //TODO change

            //Create server object
            server = new TcpListener(localAddr, port);
            
        }
        
        public void start()
        {
            //Start the server
            server.Start();
            update();
        }

        public void acceptClient()
        {
            TcpClient client = server.AcceptTcpClient();

            //Creact a client for our connection
            ConnectedClient c = new ConnectedClient(client.GetStream());
            clients.Add(c);

        }

        //Infinite loop to add clients to the server
        public void update()
        {
            while (true)
            {
                //Accept clients if any are connecting
                if (server.Pending())
                    acceptClient();

                //remove disconnected cliens
                for (int i = 0; i < clients.Count; i++)
                    if (clients[i].end)
                        clients.RemoveAt(i);
            }
        }
    }
}
