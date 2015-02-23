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
        static int kiwiPositionX = 5;
        static int kiwiPositionY = 10;
        const int maxLives = 10;
        const int maxSpeed = 30;
        const int maxPulse = 50;
        static int heigth = Console.BufferHeight = Console.WindowHeight = 25;
        static int width = Console.BufferWidth = Console.WindowWidth = 90;
        static int gameFieldWidth = width - 10;
        static char[,] gameField = new char[heigth, gameFieldWidth];
        //static char[,] menuField = new char[heigth, 10];

        static string gameBeginning = System.IO.File.ReadAllText("../../../GameBeginningFile.txt");
        static string gameOver = System.IO.File.ReadAllText("../../../GameOverFile.txt");
        static char[,] kiwi = new char[4,5] 
        {
            {'?', '\"', '?', '\"', '?'},
            {'\\', '(', '?', ')', '/'},
            {'?', '?', '@', '?', '?'},
            {'?', '?', '|', '?', '?'}
        };
        static char[,] smallBor = new char[3, 4]
        {
            {'?', '/', '\\', '?'}, 
            {'/', '?', '?', '\\'}, 
            {'?', '|', '|', '?'}
        };
        /*
         * ?/\?
         * /??\
         * ?||?
         */

        static char[,] bigBor = new char[4, 6]
        {
            {'?', '?', '/', '\\', '?', '?'},
            {'?', '/', '/', '\\', '\\', '?'},
            {'/', '/', '/', '\\', '\\', '\\'},
            {'?', '?', '|', '|', '?', '?'},

        };         
        /*
         *  ??/\??
         *  ?//\\?
         *  ///\\\
         *  ??||??
         */
        static void Main()
        {
            int menuStartX = 80;
            int menuStartY = 2;
            int travelled = 0;
            int currentLives = 3;
            int currentSpeed = 10;
            int currentPulse = 40;



            PrintOnPosition(0, 5, gameBeginning, ConsoleColor.Cyan);
            //PrintOnPosition(0, 5, gameOver, ConsoleColor.Red);
            ConsoleKeyInfo key = Console.ReadKey(true);

            Console.CursorVisible = false;
            while (true)
            {
                if (currentSpeed == maxSpeed)
                {
                    if (currentPulse == maxPulse)
                    {
                        Console.Clear();
                        PrintOnPosition(0, 5, gameOver, ConsoleColor.Red);
                        return;
                    }
                    currentSpeed = maxSpeed;
                    currentPulse++;
                }
                else
                {
                    currentSpeed++;
                }
                
                int chance = randomNum.Next(0, 101);

                //TO DO: Spawn chances
                //if (chance == 0 && chance <= 20 || smallBorCheck)
                //{

                //}

                // game field set up
                FillGameField(gameField);

                // Draw KIWI
                SetKiwiPosition(gameField);

                // Check for boundries - NOT WORKING / TO DO
                ColissionWithBoundries();

                // Move KIWI
                // Checks if anything is pressed so the game doesn't wait on us pressing anything
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey(true);

                    // Until there's keypressed stored in the buffer, it will read it
                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                    }
                    MoveKiwi(pressedKey);
                }


                

                //PrintGameField(gameField);

                Thread.Sleep(150);
                //Console.SetCursorPosition(0, 39);
                Console.Clear();

                // Redraw the gameField after the clear(). Fixes movement tearing, BUT causes blue dot bug.
                // Blue dot bug is fixed if this goes at the end of MoveKiwi, but tearing reaper
                PrintGameField(gameField);
                PrintMenu(menuStartX, menuStartY, travelled, currentLives, currentPulse, currentSpeed);
                

            }
        }
        // Not working
        static void ColissionWithBoundries()
        {

        }

        private static void MoveKiwi(ConsoleKeyInfo pressedKey)
        {
            if (pressedKey.Key == ConsoleKey.LeftArrow)
            {
                if (kiwiPositionY - 2 >= 0)
                {
                    kiwiPositionY = kiwiPositionY - 2;
                }
            }
            if (pressedKey.Key == ConsoleKey.RightArrow)
            {
                if (kiwiPositionY + 2 <= gameFieldWidth - 5)
                {
                    kiwiPositionY = kiwiPositionY + 2;
                }
            }
            if (pressedKey.Key == ConsoleKey.UpArrow)
            {
                if (kiwiPositionX - 2 >= 0)
                {
                    kiwiPositionX = kiwiPositionX - 2;
                }
            }
            if (pressedKey.Key == ConsoleKey.DownArrow)
            {
                if (kiwiPositionX + 2 <= heigth - 4)
                {
                    kiwiPositionX = kiwiPositionX + 2;
                }
            }
        }

        private static void SetKiwiPosition(char[,] gameField)
        {
            for (int currentRow = kiwiPositionX, i = 0; i < kiwi.GetLength(0); currentRow++, i++)
            {
                for (int currenCol = kiwiPositionY, j = 0; j < kiwi.GetLength(1); currenCol++, j++)
                {
                    if (gameField[kiwiPositionX, kiwiPositionY] == '0' || gameField[kiwiPositionX, kiwiPositionY] == '?')
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
                    if (col == gameField.GetLength(1) - 2)
                    {
                        Console.Write(gameField[row, col] = '|');
                    }
                }
                Console.WriteLine();
            }
        }
        private static void PrintMenu(int positionX, int positionY, int travelled, int currentLives, int currentPulse, int currentSpeed)
        {
            PrintOnPosition(positionX, positionY, "Lives:", ConsoleColor.Cyan);
            PrintOnPosition(positionX, positionY + 1, currentLives.ToString(), ConsoleColor.Cyan);
            PrintOnPosition(positionX, positionY + 4, "Speed:", ConsoleColor.Cyan);
            PrintOnPosition(positionX, positionY + 5, currentSpeed.ToString(), ConsoleColor.Cyan);
            PrintOnPosition(positionX, positionY + 8, "Pulse:", ConsoleColor.Cyan);
            PrintOnPosition(positionX, positionY + 9, currentPulse.ToString(), ConsoleColor.Cyan);
            PrintOnPosition(positionX, positionY + 12, "Travelled:", ConsoleColor.Cyan);
            PrintOnPosition(positionX, positionY + 13, travelled.ToString(), ConsoleColor.Cyan);
        }
        static void PrintOnPosition(int x, int y, string text, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.WriteLine(text);
        }
    }
}
