using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TextCarnival.Source.Server.CarnivalGames;

namespace TextCarnival.Source.Server
{
    class ConnectedClient
    {
        //Network data
        private NetworkStream stream;
        private Byte[] bytes;

        public bool end = false;
        private bool debugView;

        //Thread for connection
        private Thread run;
        
        
        //INIT for a client
        public ConnectedClient(NetworkStream connection, bool debugView = false)
        {
            stream = connection;

            this.debugView = debugView;
            if (debugView) return;

            //Run it
            run = new Thread(new ThreadStart(runClient));
            run.Start();
        }
        
        //Reads the data from the client
        public String readData()
        {
            if (debugView)
                return Console.ReadLine();

            bytes = new Byte[1024];
            int i = stream.Read(bytes, 0, bytes.Length);
            String data = (Encoding.ASCII.GetString(bytes, 0, i));

            return data;
        }

        //Sends data out to a client
        public void sendData(String data)
        {
            //If you are running this localy without a server, parse it here
            if (debugView)
            {
                String info = data.Substring(0, 4);
                if (info == "show")
                    Console.WriteLine(data.Substring(4));
                if (info == "writ")
                    Console.Write(data.Substring(4));
                if (info == "colr")
                    Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), data.Substring(4));
                return;
            }
            
            //Uses [`] to split commands because its an uncommon character
            bytes = Encoding.ASCII.GetBytes("`"+data+"`");
            stream.Write(bytes, 0, bytes.Length);
        }

        private void writeLine(String s)
        {
            sendData("show" + s);
        }

        private String readLine()
        {
            sendData("read");
            return readData();
        }

        //disconnect the server
        public void disconnect()
        {
            end = true;
        }

        //Run client connection
        public void runClient()
        {
            //Gets all the types within the "Carnival games" file
            Type[] typelist = CarnivalGame.getGames(Assembly.GetExecutingAssembly(), "TextCarnival.Source.Server.CarnivalGames");
            CarnivalGame[] allGames = typelist.Select(i => (CarnivalGame)Activator.CreateInstance(i,this)).ToArray();
            
            while (true)
            {
                //List the games
                writeLine("All of these are carnival games ready to play, select one you think is interesting!");
                for (int i = 0; i < allGames.Length; i++)
                    writeLine("[" + (i+1) + "] " + allGames[i].getName());
                writeLine("\n");

                //Get player game choice
                String choice = readLine();
                
                //Play the game
                bool played = false;
                for (int i = 0; i < allGames.Length; i++)
                    if (choice == (i+1) + "")
                    {
                        allGames[i].play();
                        played = true;
                    }
                
                if (!played)
                    writeLine("That wasnt a game on the list, please write the number on the left to play it!");
                writeLine("\n\n");
                
            }
            
        }


    }
}
