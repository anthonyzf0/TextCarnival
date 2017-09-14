﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TextCarnival.Source.Client
{
    class GameClient
    {
        //Connection Data
        private int port = 13000;
        private TcpClient client;
        private NetworkStream stream;

        //Used for data transfer
        private static Byte[] bytes = new Byte[1024];
        
        public GameClient()
        {
            //Connects to the server
            client = new TcpClient("localhost", port);      //TODO change
            stream = client.GetStream();
        }

        //Gets all the commands the server has sent
        public String[] getServerData()
        {
            int d = stream.Read(bytes, 0, bytes.Length);
            String data = Encoding.ASCII.GetString(bytes, 0, d);
            
            return data.Split('`');
        }

        //Sends a message to the server
        public void sendMsg(String s)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(s);
            stream.Write(data, 0, data.Length);
        }

        //Runs the client itself
        public void start()
        {
            while (true)
            {
                //Gets the data from the server
                String[] data = getServerData();

                foreach (string s in data) {

                    if (s == "") continue;

                    String type = s.Substring(0, 4);

                    switch (type)
                    {
                        //Server wants to read some client data
                        case "read":

                            sendMsg(Console.ReadLine());

                            break;

                        //Server wants to display some text
                        case "show":

                            Console.WriteLine(s.Substring(4));

                            break;

                        //Server wants to display some text
                        case "colr":

                            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), s.Substring(4));

                            break;
                        //Server wants to display some text
                        case "writ":

                            Console.Write(s.Substring(4));

                            break;

                    }
                }
            }
        }

        
    }
}