using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextCarnival.Source.Server.CarnivalGames
{
    class ExampleGame : CarnivalGame
    {

        //Init your carnival game
        public ExampleGame(ConnectedClient client) : base(client)
        {
            //Text triggered by 
            title =  "==============================================\n" +
                     "EXAMPLE PROJECT TITLE :)                      \n" +
                     "==============================================\n";
            
            //Run initialization code here
        }

        //Gets the name of this game
        public override string getName()
        {
            return "Example Game";
        }
        
        //Plays your game in entirety, return to exit the game
        public override void play()
        {
            //Show the title
            changeTextColor("Cyan");
            showTitle();

            //Sets text color *Check the Carnival Game class for the list of accepted colors
            changeTextColor("Red");

            WriteLine("This is a test game that will show you how to make a basic game");
            WriteLine("Did you know you could look at the code to see how this is done?");

            //Gets a yes/no answer
            bool understands = ReadYesNo();

            if (understands)
                WriteLine("Well arn\'t you a smart one!");
            else
                WriteLine("Well you can!");

            WriteLine("You see I just did a yes / no question th|11ere? There is a command for that.");
            WriteLine("Write a number [one] [two] or [three], or dont, see if I care");
            
            //Gets a choice of one of a set of options
            String choice = readOption("one", "two", "three");

            WriteLine("Thats cool, right? With a simple command it makes you write a specific input.");
            WriteLine("Hey, Wait for a sec...");

            //Waits for a certain amount of time
            pause(3);

            WriteLine("That was actually 3 secconds, but you get the point. You can even do it dramaticly");

            //Dramaticly waits for a certain amount of time
            dramaticPause();

            WriteLine("This is so easy, why dont you give it a try?");
            ReadYesNo();
            
        }
    }
}
