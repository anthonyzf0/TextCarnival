﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextCarnivalV2.Source.CarnivalGames.AllCarnivalGames
{
    class ExampleGame : CarnivalGame
    {
        public ExampleGame() : base()
        {
            
        }

        public override string getName()
        {
            return "The Example Game";
        }
        
        public override void play()
        {
            //Shows the green title text
            showTitle("WELCOME TO THE EXAMPLE GAME!");

            writeLine("This is a basic tutorial on how to use this text basted engine and make games");
            writeLine("Follow along with it both in the console and in the code to see what to do");

            //Color example
            writeLine("|70Cool that I can color things right?|f0");

            //To add color put the character ' | ' somewhere in a string
            //then follow it by two hexadecimal characters that represend color
            //|a0 would represend changing the string to foreground color 'a' and background color '0'
            //Experiment to see what characters give what color!
            //black is '0' and white is 'f'
            //this is not part of normal c#, the client side of this program actualy parses the string and changes color
            //On a normal c# project this will not work
            writeLine("|50The Code shows how to do this!|f0");
            
            writeLine("|e0Hey, yea you, are you getting this so far?");

            //Asks for a yes or no responce and return if it was yes
            bool getsIt = getYesNo();

            if (getsIt)
                writeLine("Cool, you get it dont you!");
            else
                writeLine("You dont :( how sad, look at the code then.");

            writeLine("Try not answering yes or no to this next one, see how that goes.");
            writeLine("Are you a enjoying this so far?");

            //Gets yes / no
            bool likesIt = getYesNo();

            writeLine("You see how you can controll what the user types so that it has to be something like I requested");

            writeLine("It doesnt have to be yes or no either.");

            writeLine("Are you a [boy] or a [girl]?");      //OMG dont get triggered, its an example

            //Gets a option
            String gender = getOption("boy", "girl");       //YES! in this case there are two options, this is for learning

            writeLine("See there how I restricted it to whatever I wanted, not just yes or no");

            writeLine("You can also have the user wait for something, or for a specific amount of time");

            //Waits x amoutn of seconds, doesnt have to be a whole number
            wait(3.1);

            writeLine("That was 3 seconds");

            wait(2);

            writeLine("And that was another 2");

            writeLine("Do you get it so far?");

            //Gets yes no
            bool stillGetsIt = getYesNo();

            writeLine("Oh, by the way, whats your name");

            String name = getInput();

            writeLine("See, " + name + " this can also get raw inputs too");

            wait(1);

            writeLine("If you want you can also make the console play sounds");

            //Makes the console beep
            beep();
            
            writeLine("They are fairly basic, but you can specify frequency and durration if you want");

            //Using the beep command you can specify frequency and durration
            beep(700, 2);
            beep(800, 0.5f);
            beep(900, 0.4f);
            beep(600, 1);

            writeLine("Thats about all that this can do.");
            
            writeLine("Have fun and build an amazing game!");

            // To make your own game, dont edit this, but rather create another file in this same locaiton
            // Name the class whatever you want, but make sure that you extend the main carnival game class (inheritance)
            //
            // class [your class name] : CarnivalGame
            //
            // Make sure its namespace at the top reads:
            //
            // namespace TextCarnivalV2.Source.CarnivalGames.AllCarnivalGames
            //
            // Have fun

        }

    }
}
