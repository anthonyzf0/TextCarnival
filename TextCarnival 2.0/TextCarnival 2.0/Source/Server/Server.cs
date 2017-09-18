using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
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
            List<String> ips = GetLocalIPAddress();


            if (ips.Count == 1)
            {
                localAddr = IPAddress.Parse(ips[0]);
            }
            else
            {
                // Prompt user to select an ip from list
                Console.WriteLine("Multiple ips found - select a server IP");
                for (int i = 0; i < ips.Count; i++)
                {
                    Console.WriteLine("{0}. {1}", i, ips[i]);
                }
                Console.Write("\nWhich IP would you like to start the server on? ({0}-{1}): ", 0, ips.Count - 1);
                localAddr = IPAddress.Parse(ips[Convert.ToInt32(Console.ReadLine())]);

            }
            Console.WriteLine("Starting server on local ip: " + localAddr.ToString());

            //Create server object
            server = new TcpListener(localAddr, port);

            server.Start();
            
            run = new Thread(new ThreadStart(runServer));
            run.Start();
            Console.WriteLine("Server started");
        }

        //Gets local ip
        public static List<string> GetLocalIPAddress()
        {
            List<string> ips = new List<string>();
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ips.Add(ip.ToString());
                }
            }
            return ips;
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
        public static bool runningDebug = false;

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

        //Runs the client
        public void runClient()
        {
            if (!runningDebug)
                try
                {
                    play();
                }
                catch
                {
                    Console.WriteLine("ERROR...ERROR...CARNIVAL GAME CAUGHT ON FIRE!");
                    Console.WriteLine("(there was an error in the carnival game code)");
                    Console.WriteLine("(run this in debug mode to get an error message)");
                    runClient();
                }
            else
                play();


        }

        //Actualy runs the client data
        private void play()
        {

            while (true)
            {
                int index = -1;

                loadGames();
                do
                {
                    writeData("\nGames at this carnival:");

                    for (int i = 0; i < allGames.Length; i++)
                        writeData("[" + i + "] " + allGames[i].getName());

                }
                while (!int.TryParse(readData(), out index));

                Debug.Assert(index != -1, "Server is on fire - index should not be -1");
                
                // int index = Convert.ToInt32(readData());

                allGames[index].setup(send, readData);
                allGames[index].play();

                //Resets the color data
                writeData("|f0");
            }

        }

    }
}
