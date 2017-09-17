﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TextCarnivalV2.Source.CarnivalGames;

namespace TextCarnivalV2.Source.Server
{
    class Server
    {   
        //Basic Server information
        private static int port = 13000;
        private static TcpListener server;
        private static IPAddress localAddr;

        //All the clients on the server currently
        private static List<ServerConnection> clients;

        //The thread this is running on
        private static Thread run;

        //Called to start up the server
        public static void startServer()
        {
            Console.WriteLine("Running server");
            //Server
            clients = new List<ServerConnection>();

            //Gets ip
            String ip = GetLocalIPAddress();
            localAddr = IPAddress.Parse(ip);
            Console.WriteLine("Starting server on local ip: " + ip);

            //Create server object
            server = new TcpListener(localAddr, port);

            server.Start();
            
            run = new Thread(new ThreadStart(runServer));
            run.Start();
            Console.WriteLine("Server started");
        }

        //Gets local ip
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        //Runs the server itself
        public static void runServer()
        {
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();

                //Creact a client for our connection
                ServerConnection c = new ServerConnection(client.GetStream());
                clients.Add(c);
            }
        }

    }

    //2nd class in file, a new instance is created to store each individual client connected
    class ServerConnection
    {
        private CarnivalGame[] allGames;

        //Network data
        private NetworkStream stream;
        private Byte[] bytes;
        
        //Thread for connection
        private Thread run;

        public ServerConnection (NetworkStream connection)
        {
            stream = connection;
            loadGames();

            //Run it
            run = new Thread(new ThreadStart(runClient));
            run.Start();

        }

        private void loadGames()
        {
            //Gets all the types within the "Carnival games" file
            Type[] allTypes = Assembly.GetExecutingAssembly().GetTypes();
            Type[] typeList = allTypes.Where(t => String.Equals(t.Namespace, "TextCarnivalV2.Source.CarnivalGames.AllCarnivalGames", StringComparison.Ordinal)).ToArray();

            //Seperates and parses those types into those of Carnival Game
            allGames = typeList.Select(i => (CarnivalGame)Activator.CreateInstance(i)).ToArray();

        }

        //Sends data out to a client
        private bool send(String data)
        {
            //Uses [`] to split commands because its an uncommon character
            bytes = Encoding.ASCII.GetBytes("`" + data + "`");
            stream.Write(bytes, 0, bytes.Length);

            return true;
        }

        //Reads the data from the client
        public String readData()
        {
            send("read");
            bytes = new Byte[1024];
            int i = stream.Read(bytes, 0, bytes.Length);
            String data = (Encoding.ASCII.GetString(bytes, 0, i));

            return data;
        }

        //Writes data
        public bool writeData(String data)
        {
            send("show" + data);
            return true;
        }

        //Runs the server
        public void runClient()
        {
            while (true)
            {
                loadGames();
                writeData("\nGames at this carnival:");

                for (int i = 0; i < allGames.Length; i++)
                    writeData("["+i+"] "+allGames[i].getName());
            
                int index = Convert.ToInt32(readData());

                allGames[index].setup(send, readData);
                allGames[index].play();

                //Resets the color data
                writeData("|f0");
            }

        }

    }
}