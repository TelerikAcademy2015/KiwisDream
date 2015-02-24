// BUGS SO FAR :
/*
 * 1. Когато се вземе живот или спийд със първия му чар (удар на първия чар на пилето и първия чар на живота/спийда), се отчита, че се е взел както живот, така и спийд.
 */ 

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
        const int maxSpeed = 250;
        const int minSpeed = 10;
        const int maxPulse = 180;
        const int minPulse = 40;
        static int heigth = Console.BufferHeight = Console.WindowHeight = 30;
        static int width = Console.BufferWidth = Console.WindowWidth = 90;
        static int gameFieldWidth = width - 10;
        static int gameFieldHeigth = heigth - 5;
        static char[,] gameField = new char[gameFieldHeigth, gameFieldWidth];   // 25,80
        // life variables
        static int lifeSpawnWidth;
        static int lifeSpawnHeight;
        static bool spawnedLife = false;
        static bool spawnedLifeTaken = false;
        static int lifeTimeCounter = 80;
        // speed down variables
        static int speedSpawnWidth;
        static int speedSpawnHeight;
        static bool spawnedSpeed = false;
        static bool spawnedSpeedTaken = false;
        static int speedTimeCounter = 80;

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
        static char[] lifeUp = new char[3] { '1', 'u', 'p' };
        static char[] speedDown = new char[4] { 'S', 'p', 'd', 'D' };
 
        static void Main()
        {
            int menuStartX = 80;
            int menuStartY = 2;
            int travelled = 0;
            int currentLives = 3;
            int currentSpeed = minSpeed;
            int currentPulse = minPulse;

            // Hides the annoying cursor
            Console.CursorVisible = false;
            PrintOnPosition(0, 5, gameBeginning, ConsoleColor.Cyan);
            ConsoleKeyInfo key = Console.ReadKey(true);

            while (true)
            {
                travelled++;
                if (currentSpeed == maxSpeed)
                {
                    if (currentPulse == maxPulse)
                    {
                        Console.Clear();
                        PrintOnPosition(0, 10, gameOver, ConsoleColor.Red);
                        Console.ReadKey();
                        return;
                    }
                    currentSpeed = maxSpeed;
                    currentPulse++;
                }
                else
                {
                    currentSpeed++;
                }
                
                // game field set up
                FillGameField(gameField);

                // Spawn chances
                int chance = randomNum.Next(0, 101);

                if (chance >= 1 && chance <= 10 && spawnedLife == false) // bool check to spawn ONLY 1 life at a time
                {
                    // spawn life up;
                    lifeSpawnWidth = randomNum.Next(1, gameFieldWidth - 3);
                    lifeSpawnHeight = randomNum.Next(3, gameFieldHeigth - 1);

                    // A check to make sure theres NOTHING on the spawn point
                    // TO DO (MATRIX WIDE 5x5 atleast CHECK, not just the first char !!!!!!!)

                    // HERE IS THE REASON FOR BUG 1.
                    try
                    {
                        if (gameField[lifeSpawnHeight, lifeSpawnWidth] == '0' ||
                            gameField[lifeSpawnHeight, lifeSpawnWidth] == '?')
                        {
                            SpawnLifeUp(lifeSpawnWidth, lifeSpawnHeight);
                            spawnedLife = true;
                        }
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        IndexOutOfRangeException(e); 
                    }
                }
                else if (chance >= 11 && chance <= 20 && spawnedSpeed == false)
                {
                    // spawn speed down
                    speedSpawnWidth = randomNum.Next(1, gameFieldWidth - 4);
                    speedSpawnHeight = randomNum.Next(3, gameFieldHeigth - 1);

                    // A check to make sure theres NOTHING on the spawn point
                    // TO DO (MATRIX WIDE 5x5 atleast CHECK, not just the first char !!!!!!!)

                    // HERE IS THE REASON FOR BUG 1.
                    try
                    {
                        if (gameField[speedSpawnHeight, speedSpawnWidth] == '0' ||
                            gameField[speedSpawnHeight, speedSpawnWidth] == '?')
                        {
                            SpawnSpeedDown(speedSpawnWidth, speedSpawnHeight);
                            spawnedSpeed = true;
                        }
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        IndexOutOfRangeException(e);
                    }
                }


                // Checks if life is spawned, IF SO it keeps it there while the player takes is OR the counter ENDS
                // if the counter reaches zero, on the next itteration the life will be GONE, and a new one can be spawned. When 1 life is spawned, another one CANNOT be spanwed.
                if (spawnedLife)
                {
                    if (lifeTimeCounter == 0 || spawnedLifeTaken)
                    {
                        if (spawnedLifeTaken)
                        {
                            currentLives++;
                            if (currentLives >= maxLives)
                            {
                                currentLives = maxLives;
                            }
                        }
                        lifeTimeCounter = 80;
                        spawnedLife = false;
                        spawnedLifeTaken = false;
                    }
                    else
                    {
                        lifeTimeCounter--;
                        SpawnLifeUp(lifeSpawnWidth, lifeSpawnHeight); // Keep spawning the life at the same place
                    }
                }
                if (spawnedSpeed)
                {
                    if (speedTimeCounter == 0 || spawnedSpeedTaken)
                    {
                        if (spawnedSpeedTaken)
                        {
                            currentSpeed -= 100;
                            currentPulse = minPulse;
                            if (currentSpeed <= minSpeed)
                            {
                                currentSpeed = minSpeed;
                            }
                        }
                        speedTimeCounter = 80;
                        spawnedSpeed = false;
                        spawnedSpeedTaken = false;
                    }
                    else
                    {
                        speedTimeCounter--;
                        SpawnSpeedDown(speedSpawnWidth, speedSpawnHeight); // Keep spawning the speed at the same place
                    }
                }


                // Draw KIWI
                SetKiwiPosition(gameField);


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

                // Check for collisions
                CollisionWithLifeUp(kiwiPositionX, kiwiPositionY);
                CollisionWithSpeedDown(kiwiPositionX, kiwiPositionY);

                // Slow down game
                Thread.Sleep(150);
                Console.Clear();

                // Redraw the gameField after the clear(). Fixes movement tearing, BUT causes blue dot bug.
                // Blue dot bug is fixed if this goes at the end of MoveKiwi, but tearing reapers
                PrintGameField(gameField);
                PrintMenu(menuStartX, menuStartY, travelled, currentLives, currentPulse, currentSpeed);
                
            }
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
                if (kiwiPositionX + 2 <= heigth - 9)
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

        private static void SpawnLifeUp(int spawnWidth, int spawnHeight)
        {
            for (int index = 0, j = spawnWidth; index < lifeUp.Length; index++, j++)
            {
                gameField[spawnHeight, j] = lifeUp[index];
            }
        }

        private static void SpawnSpeedDown(int spawnWidth, int spawnHeight)
        {
            for (int index = 0, j = spawnWidth; index < speedDown.Length; index++, j++)
            {
                gameField[spawnHeight, j] = speedDown[index];
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
                        Console.Out.Write(gameField[row, col] = default(char));
                    }
                    else
                    {
                        Console.Out.Write(gameField[row, col]);
                    }
                    if (col == gameField.GetLength(1) - 1)
                    {
                        Console.Out.Write(gameField[row, col] = ' ');
                    }
                }
                Console.Out.WriteLine();
            }
        }
        private static void PrintMenu(int positionX, int positionY, int travelled, int currentLives, int currentPulse, int currentSpeed)
        {
            PrintOnPosition(positionX, positionY, "Lives:", ConsoleColor.White);
            if (currentLives == 1)
            {
                PrintOnPosition(positionX, positionY + 1, currentLives.ToString(), ConsoleColor.Red);
            }
            else if (currentLives > 1 && currentLives <= 3)
            {
                PrintOnPosition(positionX, positionY + 1, currentLives.ToString(), ConsoleColor.Yellow);
            }
            else
            {
                PrintOnPosition(positionX, positionY + 1, currentLives.ToString(), ConsoleColor.Green);
            }

            PrintOnPosition(positionX, positionY + 4, "Speed:", ConsoleColor.White);
            if (currentSpeed < maxSpeed)
            {
                PrintOnPosition(positionX, positionY + 5, currentSpeed.ToString(), ConsoleColor.Cyan);
            }
            else
            {
                PrintOnPosition(positionX, positionY + 5, currentSpeed.ToString(), ConsoleColor.Red);
            }
            PrintOnPosition(positionX, positionY + 8, "Pulse:", ConsoleColor.White);
            PrintOnPosition(positionX, positionY + 9, currentPulse.ToString(), ConsoleColor.Red);
            PrintOnPosition(positionX, positionY + 12, "Travelled:", ConsoleColor.White);
            PrintOnPosition(positionX, positionY + 13, travelled.ToString(), ConsoleColor.Cyan);
        }
        static void PrintOnPosition(int x, int y, string text, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.WriteLine(text);
        }
        
        // Collision checks below
        private static void CollisionWithLifeUp(int currentRow, int currenCol)
        {
            for (int i = 0, row = currentRow; i < kiwi.GetLength(0); i++, row++)
            {
                for (int j = 0, col = currenCol; j < kiwi.GetLength(1); j++, col++)
                {
                    for (int k = 0; k < lifeUp.Length; k++)
                    {
                        if (gameField[row, col] == lifeUp[k])
                        {
                            spawnedLifeTaken = true;
                        }
                    }
                }
            }
        }

        private static void CollisionWithSpeedDown(int currentRow, int currentCol)
        {
            for (int i = 0, row = currentRow; i < kiwi.GetLength(0); i++, row++)
            {
                for (int j = 0, col = currentCol; j < kiwi.GetLength(1); j++, col++)
                {
                    for (int k = 0; k < speedDown.Length; k++)
                    {
                        if (gameField[row, col] == speedDown[k])
                        {
                            spawnedSpeedTaken = true;
                        }
                    }
                }
            }
        }

        // Format exceptions below
        private static void IndexOutOfRangeException(IndexOutOfRangeException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
