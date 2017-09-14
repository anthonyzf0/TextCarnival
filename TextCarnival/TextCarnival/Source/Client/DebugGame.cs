using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextCarnival.Source.Server;

namespace TextCarnival.Source.Client
{
    class DebugGame
    {
        //Carnival Game to debu
        private CarnivalGame game;

        public DebugGame (Type type)
        {
            //Creates a fake client that is set up to test the game localy
            ConnectedClient testClient = new ConnectedClient(null, true);
            game = (CarnivalGame)Activator.CreateInstance(type, testClient);

        }

        public void play()
        {
            game.play();
        }


    }
}
