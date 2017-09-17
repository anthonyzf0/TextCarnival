﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TextCarnivalV2.Source.CarnivalGames
{
    abstract class CarnivalGame
    {
        //reading / writing functions
        private Func<String, bool> output;
        private Func<String> read;

        public CarnivalGame() {

        }

        //Writes a string on a new line
        public void writeLine(String line)
        {
            output("show" + line);
        }

        //Writes a string on the current line
        public void write(String text)
        {
            output("writ" + text);
        }

        //Beeps the client
        public void beep()
        {
            output("beep 600 1");
        }

        //Beeps the client with a certain frequency and time
        public void beep(float f, float t)
        {
            output("beep "+f+" "+t);
        }
        //Setup client
        public void setup(Func<String, bool> writeData, Func<String> readData)
        {
            output = writeData;
            read = readData;
        }

        //Shows this title at the begining of the game
        public void showTitle(String title)
        {
            writeLine("==================================");
            writeLine("|a0" + title + "|f0");
            writeLine("==================================");

        }

        //gets a yes or no value from the user using the getOption method
        public bool getYesNo()
        {
            return getOption("yes", "no")=="yes";
        }

        //Gets a generic input
        public String getInput()
        {
            return read().ToLower();
        }

        //waits for a certain number of secconds
        public void wait(double secs)
        {
            Thread.Sleep((int)(secs * 1000));
        }

        //Asks for an option in a list
        public String getOption(params String[] options)
        {
            String answer = getInput();

            if (options.Contains(answer))
                return answer;

            writeLine("Beep...Boop..Answer not valid :(");
            writeLine("Please answer as one of these: ");
            writeLine(String.Join(" ", options));
            return getOption(options);
        }
        
        //Called to play the game, have all of the main game code in here
        //When this method ends, the game is ended
        public abstract void play();        

        //Gets the name of the client for the menu
        public abstract string getName();

    }
}
