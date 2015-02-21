using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kiwi
{
    class Tree
    {
        public int x;
        public int y;
        public string shape;
        public ConsoleColor color;

        public Tree(int x, int y, string shape, ConsoleColor color)//Constructor.
        {
            this.x = x;//"this" is used not to overlap the variables.
            this.y = y;
            this.shape = shape;
            this.color = color;
        }
    }

    class myKiwi
    {
        public int x;
        public int y;
        public string shape;
        public ConsoleColor color;

        public myKiwi(int x, int y, string shape, ConsoleColor color)//Constructor.
        {
            this.x = x;//"this" is used not to overlap the variables.
            this.y = y;
            this.shape = shape;
            this.color = color;
        }
    }

    class KiwiGame
    {
        public const int playFieldWidth = 90;
        const int kiwiStartPositionX = 45;
        const int kiwiStartPositionY = 0;
        static double speed = 20;
        const double maxSpeed = 200;
        static double pulse = 60;
        const int maxPulse = 250;
        static int lifes = 5;
        const int maxLifes = 5;
        static string dieFromHeartAttack = "The kiwi just died because of a heart attack :(";
        static string dieFromNoMoreLifes = "The kiwi just died. No more lifes :(";
        static string gameOver = System.IO.File.ReadAllText("../../TextFile1.txt");
        static string gameBeginning = System.IO.File.ReadAllText("../../TextFile2.txt");

        static void Main()
        {
            Console.BufferHeight = Console.WindowHeight = 40;
            Console.BufferWidth = Console.WindowWidth = 120;

            myKiwi myKiwi = new myKiwi(kiwiStartPositionX, kiwiStartPositionY, "kiwi", ConsoleColor.Cyan);

            Random rand = new Random();
            List<Tree> trees = new List<Tree>();

            PrintOnPosition(0, 20, gameBeginning, ConsoleColor.Red);
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter)
            {
                Console.Clear();
                Console.SetCursorPosition(30, 20);
                Console.WriteLine("READY!!!");
                Thread.Sleep(150);
            }
            else
            {
                Console.Clear();
                Console.SetCursorPosition(30, 20);
                return;
            }
            while (true)
            {
                // Generate spawning chance 
                Random randomGenerator = new Random();
                int chance = randomGenerator.Next(0, 101);
                if (chance == 0 && chance <=20)
                {
                    // Spawn nothing
                }
                else if (chance >= 21 && chance <= 40)
                {
                    Tree pineTree = new Tree(rand.Next(0, playFieldWidth), Console.WindowHeight - 1, "pine", ConsoleColor.DarkRed);
                    Tree limeTree = new Tree(rand.Next(0, playFieldWidth), Console.WindowHeight - 1, "lime", ConsoleColor.Green);
                    Tree coconutTree = new Tree(rand.Next(0, playFieldWidth), Console.WindowHeight - 1, "coconut", ConsoleColor.White);

                    Tree[] treeTypes = new Tree[] { pineTree, limeTree, coconutTree };
                    trees.Add(treeTypes[rand.Next(0, treeTypes.Length)]);

                }

                

                

                if (Console.KeyAvailable)//checks if there is pressed button in the console.
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                    //If we are in the cycle, there is pressed button and we check what is the value of it.
                    //the bool param "true" means that the value of the pressed button will not be printed in the console(The program will use it just for the body of the game).
                    while (Console.KeyAvailable) //This cycle takes out the additional info from pressed buttons in the buffer.
                    {
                        Console.ReadKey(true);
                    }

                    if (pressedKey.Key == ConsoleKey.LeftArrow)
                    {
                        if (myKiwi.x - 1 >= 0)
                        {
                            myKiwi.x--;

                        }
                    }
                    if (pressedKey.Key == ConsoleKey.RightArrow)
                    {
                        if (myKiwi.x < playFieldWidth - myKiwi.shape.Length)
                        {
                            myKiwi.x++;
                        }
                    }
                }

                

                for (int i = 0; i < trees.Count; i++)
                {
                    Tree oldTree = trees[i];
                    Tree currentTree = new Tree(oldTree.x, oldTree.y - 1, oldTree.shape, oldTree.color);
                    trees.Remove(oldTree);
                    if (currentTree.y == myKiwi.y && currentTree.x == myKiwi.x)//Checks if Kiwi is hitted by trees.
                    {
                        lifes--;
                        Console.SetCursorPosition(30, 20);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("{0} LIFES REMAINING. THE KIWI IS GOING TO DIE SOON!", lifes);
                        Console.Beep(700, 700);
                        Thread.Sleep(300);
                    }
                    if (currentTree.y > 0)//Checks if the tree is still in the console playfield.
                    {
                        trees.Add(currentTree);
                    }
                }

                Console.Clear();//Cleans the console.

                PrintOnPosition(myKiwi.x, myKiwi.y, myKiwi.shape, myKiwi.color);//Redraw  playfield
                foreach (var tree in trees)//Redraw  playfield
                {
                    PrintOnPosition(tree.x, tree.y, tree.shape, tree.color);
                }

                if (speed == maxSpeed)
                {
                    pulse += 1;
                }
                if (pulse > 100)
                {
                    if (pulse % 50 == 0)
                    {
                        Console.SetCursorPosition(20, 20);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("THE PULSE OF THE SCARED KIWI IS TOO HIGH. IT'S HEART WILL BLOW OUT VERY SOON!");
                        Console.Beep(700, 700);
                        Thread.Sleep(300);
                    }
                    if (pulse == maxPulse)
                    {
                        DieKiwi(dieFromHeartAttack);
                        break;
                    }
                }

                if (speed < maxSpeed)
                {
                    speed += 1;
                }
                if (lifes == 0)
                {
                    DieKiwi(dieFromNoMoreLifes);
                    break;
                }
                PrintInfo(lifes, speed, pulse, ConsoleColor.Blue);

                Thread.Sleep(100);//Slow down program
                
            }

        }
        static void DieKiwi(string reason)
        {
            Console.Clear();
            PrintOnPosition(0, 20, gameOver, ConsoleColor.Red);
            Console.SetCursorPosition(40, 30);
            Console.WriteLine(reason);
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
        }

        static void PrintOnPosition(int x, int y, string shape, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.WriteLine(shape);
        }


        //optional[uses char array instead of string]-when we understand how to put the whole char of the symbols.
        static void PrintOnPosition(int x, int y, char[,] text, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.WriteLine("      {0}", text);
        }

        static void PrintInfo(int lifes, double speed, double pulse, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(playFieldWidth, 5);
            Console.Write("Lifes: {0}", lifes);
            Console.SetCursorPosition(playFieldWidth, 6);
            Console.Write("Speed: {0}", speed);
            Console.SetCursorPosition(playFieldWidth, 7);
            Console.Write("Pulse: {0}", pulse);
        }
    }


}
