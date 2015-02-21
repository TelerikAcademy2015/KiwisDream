using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    class KiwisDeath
    {
        const int maxLives = 10;
        const double maxSpeed = 300;
        const int maxPulse = 255;
        static string gameBeginning = System.IO.File.ReadAllText("../../../GameBeginningFile.txt");
        static string gameOver = System.IO.File.ReadAllText("../../../GameOverFile.txt");
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

            PrintOnPosition(0, 5, gameBeginning, ConsoleColor.Cyan);
            ConsoleKeyInfo key = Console.ReadKey(true);

            char[,] gameField = new char[heigth, width - 30];

            PrintGameField(gameField);

            PrintOnPosition(menuStartX, menuStartY, "I killed the kiwi", ConsoleColor.DarkMagenta);
            Console.SetCursorPosition(0, 39);

            //Console.Clear();
        }


        private static void PrintGameField(char[,] gameField)
        {
            for (int row = 0; row < gameField.GetLength(0); row++)
            {
                for (int col = 0; col < gameField.GetLength(1); col++)
                {
                    if (col == gameField.GetLength(1) - 1)
                    {
                        Console.Write(gameField[row, col] = '|');
                    }
                    else
                    {
                        Console.Write(gameField[row, col] = '0');
                    }
                }
                Console.WriteLine();
            }
        }
        static void PrintOnPosition(int x, int y, string shape, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.WriteLine(shape);
        }
    }
}
