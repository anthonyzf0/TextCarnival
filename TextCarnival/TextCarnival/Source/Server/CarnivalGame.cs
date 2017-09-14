using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;

namespace TextCarnival.Source.Server
{
    abstract class CarnivalGame
    {
        //The client running this game
        private ConnectedClient client;

        public string title = "";

        public CarnivalGame(ConnectedClient client)
        {
            this.client = client;
            
        }

        //Gets a yes or no input from the client
        public bool ReadYesNo()
        {
            client.sendData("read");
            String data = client.readData().ToLower();
            if (data == "yes")
                return true;
            if (data == "no")
                return false;

            WriteLine("Please answer [yes] or [no]");
            return ReadYesNo();
        }

        //Gets a range of options
        public string readOption(params String[] cases)
        {
            client.sendData("read");
            String data = client.readData().ToLower();

            if (cases.Contains(data))
                return data;

            WriteLine("Please answer with an acceptable value: ");

            String vals = "";
            foreach (String s in cases)
                vals +=(" [" + s + "] ");

            WriteLine(vals+"\n");
            return readOption(cases);
        }

        //pause for x secconds
        public void pause(double time)
        {
            Thread.Sleep((int)(time * 1000));
        }

        //dramatic pause for 3 secconds
        public void dramaticPause()
        {
            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(1000);
                Write(". ");
            }
            WriteLine("");
        }

        //Gets a string value from the user
        public string readLine()
        {
            client.sendData("read");
            String data = client.readData();
            return data;
        }

        //
        
        //Changes the text color
        public void changeTextColor(String name)
        {
            client.sendData("colr" + name);
        }
        /*  ALL THE VALID COLORS FOR THE CONSOLE, CASE SENSATIVE
         *  Black           DarkBlue            DarkGreen
            DarkCyan        DarkRed             DarkMagenta
            DarkYellow      Gray                DarkGray
            Blue            Green               Cyan
            Red             Magenta             Yellow
            White           Black               DarkBlue
            DarkGreen       DarkCyan            DarkRed
            DarkMagenta     DarkYellow          Gray
            DarkGray        Blue                Green
            Cyan            Red                 Magenta
            Yellow          White
         */


        //Write requested info
        public void Write(String s)
        {
            client.sendData("writ" + s);
        }

        //Write line of requested info
        public void WriteLine(String s)
        {
            client.sendData("show" + s);
        }

        //Shows the title
        public void showTitle()
        {
            WriteLine(title);
        }

        //Starts the game itself
        public abstract void play();

        //Gets the name of the game, used for menus
        public abstract string getName();
        
        //Static method for getting all the games that exist, dont edit or extend
        public static Type[] getGames(Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
        }   
    }
}
