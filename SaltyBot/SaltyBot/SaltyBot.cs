using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;
using System.Collections;
namespace SaltyBot
{
    class SaltyBot
    {
        //E-mail and password for the bot
        private const string EMAIL = "saltybot@mailinator.com";
        private const string PWORD = "comacoma0";

        //The Browser
        private IE browser;

        //Current fighter names
        private string[] curFtrs = new string[2];

        //Hashtable of fight history
        Hashtable history = new Hashtable();

        //current winner
        private int winner = -1;

        //Initializer
        public SaltyBot()
        {
            // create a new Internet Explorer Instance
            browser = new IE();
        }

        //Login the bot
        private void Login()
        {
            // point it to login page
            browser.GoTo("http://www.saltybet.com/authenticate?signin=1");
            
            //If still logged in, get to main page
            if (!browser.Url.Equals("http://www.saltybet.com/authenticate?signin=1"))
            {
                browser.GoTo("http://www.saltybet.com");

                return;
            }

            browser.WaitForComplete();

            //Type in e-mail and pass
            browser.TextField(Find.ById("email")).TypeText(EMAIL);
            browser.TextField(Find.ById("pword")).TypeText(PWORD);

            //Submit info
            browser.Element(Find.ByValue("Sign In")).ClickNoWait();

            browser.WaitForComplete();
        }

        //Check if new fighters or get first set
        public bool CheckFighters()
        {
            if (curFtrs[0] == null)
            {
                return true;
            }
            else
            {
                return ((!curFtrs[0].Equals(browser.Element(Find.ById("p1name")).Text))
                    || (!curFtrs[1].Equals(browser.Element(Find.ById("p2name")).Text)));
            }
        }

        //Obtain fighter names
        public void SetFighters()
        {
            curFtrs[0] = browser.Element(Find.ById("p1name")).Text;
            curFtrs[1] = browser.Element(Find.ById("p2name")).Text;
        }

        //Prints an array of 2 fighters to console
        public void PrintFighters()
        {
            Console.WriteLine("Red Fighter: " +  curFtrs[0]);
            Console.WriteLine("Blue Fighter: " + curFtrs[1]);
        }

        //Returns 1 for player 1 and two for player 2
        public int GetWinner()
        {
            Element child;
            while (true)
            {
                if ((child = browser.Span(Find.ById("betstatus")).Children().First()) != null)
                {
                    if (child.ClassName.Equals("redtext"))
                    {
                        winner = 0;

                        return winner;
                    }
                    else if (child.ClassName.Equals("bluetext"))
                    {
                        winner = 1;

                        return winner;
                    }
                }
            }
        }

        //Save fight info into history
        public void SaveFight()
        {
            //history.Add(curFtrs[0], 
        }

        //Print winner info
        public void PrintWinner(int winner)
        {
            Console.WriteLine(curFtrs[winner] + " is the winner!");
        }
        
        //Closes the browser and exits
        public void End()
        {
            browser.Close();
            Environment.Exit(0);
        }




        //The main running of the bot thread
        [STAThread]
        static void Main(string[] args)
        {
            SaltyBot bot = new SaltyBot();

            bot.Login();

            
            

            //print 10 sets of fighters
            int x = 1;
            
            while (x < 11)
            {
                //check if new fighters
                if (bot.CheckFighters())
                {
                    //Set the fighters
                    bot.SetFighters();

                    //Write the print
                    Console.WriteLine("Fight " + x);
                    bot.PrintFighters();
                    bot.PrintWinner(bot.GetWinner());


                    Console.WriteLine();

                    //Increase the fight count
                    x++;  
                }

                else
                {
                    System.Threading.Thread.Sleep(10000);
                }

                              
            }

            bot.End();
            
        }
    }
}

/*
To write Hashtable on file:

HashTable xxx = new Hashtable();
BinaryFormatter bfw = new BinaryFormatter();
FileStream file = new FileStream(x,x,x);
StreamWriter ws = new StreamWriter(file);   
bfw.Serialize(ws.BaseStream, xxx);


To Read Hashtable from file:

Hashtable xxx = null;
FileStream file = new FileStream(x,x,x);
StreamReader readMap = new StreamReader(file);
BinaryFormatter bf = new BinaryFormatter();
xxx= (Hashtable)bf.Deserialize(readMap.BaseStream);
*/