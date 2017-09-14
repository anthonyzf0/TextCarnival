using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextCarnival.Source.Client;
using TextCarnival.Source.Server;
using TextCarnival.Source.Server.CarnivalGames;

namespace TextCarnival.Source
{
    class Program
    {
        //Set to a specific type to debug that CarnivalGame
        //public static Type debugType = typeof(ExampleGame);   //Uncomment this to text the example game without a server running
        public static Type debugType = null;


        //Run to start the program itself
        static void Main(string[] args)
        {
            if (debugType != null)
            {
                DebugGame server = new DebugGame(debugType);
                server.play();
                return;
            }

            //Introductory info
            Console.WriteLine("Welcome to the Text Carnival!");
            Console.WriteLine("Start a [server] or a [client]: ");

            //Determine if you should run as a server or a client
            if (Console.ReadLine() == "server")
            {
                GameServer server = new GameServer();
                server.start();
            }
            else
            {
                GameClient client = new GameClient();
                client.start();
            }
            
            Console.WriteLine("Press [return] to exit...");
            Console.ReadLine();
        }
    }
}
