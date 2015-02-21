using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Main
{
    class KiwisDeath
    {
        static Random randomNum = new Random();
        static int kiwiRow = 2;
        static int kiwiCol = 40;
        const int maxLives = 10;
        const double maxSpeed = 300;
        const int maxPulse = 255;
        static string gameBeginning = System.IO.File.ReadAllText("../../../GameBeginningFile.txt");
        static string gameOver = System.IO.File.ReadAllText("../../../GameOverFile.txt");
        static char[,] kiwi = new char[4,5] 
        {
            {'?', '\"', '?', '\"', '?'},
            {'\\', '(', '?', ')', '/'},
            {'?', '?', '@', '?', '?'},
            {'?', '?', '|', '?', '?'}
        };

        struct Stuff
        {
            public int x;
            public int y;
            public string shape;
            public ConsoleColor color;
        }
        static void Main()
        {
            int menuStartX = 91;
            int menuStartY = 10;
            int heigth = Console.BufferHeight = Console.WindowHeight = 40;
            int width = Console.BufferWidth = Console.WindowWidth = 120;
            int travelled = 0;
            int currentLives = 3;
            double currentSpeed = 10;
            int currentPulse = 40;

            //PrintOnPosition(0, 5, gameBeginning, ConsoleColor.Cyan);
            //PrintOnPosition(0, 5, gameOver, ConsoleColor.Red);
            //ConsoleKeyInfo key = Console.ReadKey(true);

            char[,] gameField = new char[heigth, width - 30];

            

            /*
            ?"?"?
            \(?)/
            ??@??
            ??|??
             */
            while (true)
            {
                int chance = randomNum.Next(0, 101);

                //if (chance == 0 && chance = <= 20)
                // game field set up
                FillGameField(gameField);

                // Draw KIWI
                SetKiwiPosition(gameField);

                PrintOnPosition(menuStartX, menuStartY, "I killed the kiwi", ConsoleColor.Cyan);

                PrintGameField(gameField);

               
                //Console.SetCursorPosition(0, 39);
                Console.Clear();
               

            }
        }

        private static void SetKiwiPosition(char[,] gameField)
        {
            for (int currentRow = kiwiRow, i = 0; i < kiwi.GetLength(0); currentRow++, i++)
            {
                for (int currenCol = kiwiCol, j = 0; j < kiwi.GetLength(1); currenCol++, j++)
                {
                    if (gameField[kiwiRow, kiwiCol] == '0' || gameField[kiwiRow, kiwiCol] == '?')
                    {
                        gameField[currentRow, currenCol] = kiwi[i, j];
                    }
                    else
                    {
                        //TO DO COLLISION
                    }
                }
            }
        }

        private static void FillGameField(char[,] gameField)
        {



            for (int row = 0; row < gameField.GetLength(0); row++)
            {
                for (int col = 0; col < gameField.GetLength(1); col++)
                {
                    gameField[row, col] = '0';
                }
            }
        }


        private static void PrintGameField(char[,] gameField)
        {
            for (int row = 0; row < gameField.GetLength(0); row++)
            {
                for (int col = 0; col < gameField.GetLength(1); col++)
                {
                    if (gameField[row, col] == '?' || gameField[row, col] == '0')
                    {
                        Console.Write(gameField[row, col] = ' ');
                    }
                   
                    else
                    {
                        Console.Write(gameField[row, col]);
                    }
                    if (col == gameField.GetLength(1) - 1)
                    {
                        Console.Write(gameField[row, col] = '|');
                    }
                }
                Console.WriteLine();
            }
            Thread.Sleep(500);
        }
        static void PrintOnPosition(int x, int y, string shape, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.WriteLine(shape);
        }
    }
}
