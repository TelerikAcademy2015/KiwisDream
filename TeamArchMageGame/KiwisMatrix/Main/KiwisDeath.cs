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
        static int currentLives = 3;
        const int maxSpeed = 200;
        const int minSpeed = 10;
        const int maxPulse = 180;
        const int minPulse = 40;
        static int heigth = Console.BufferHeight = Console.WindowHeight = 35;
        static int width = Console.BufferWidth = Console.WindowWidth = 100;
        static int gameFieldWidth = width - 15;
        static int gameFieldHeigth = heigth;
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

        static int treeSpawnWidth;
        static int treeSpawnHeigth;
        static bool borCollision;
        // left tree variables
        static bool spawnedLeftTree = false;
        static int leftTreeCounter = -1;
        static int leftTreeDespawnCounter = 3;
        // middle tree variables
        // right tree variables

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

        static char[,] bigBor = new char[4, 5]
        {
            {'?', '?', '^', '?', '?'},
            {'?', '^', '^', '^', '?'},
            {'^', '^', '^', '^', '^'},
            {'?', '?', '$', '?', '?'},

        };         
        /*             
         *  ??/\??     ??^??
         *  ?//\\?     ?^^^?
         *  ///\\\     ^^^^^
         *  ??||??     ??$??
         */
        static char[] lifeUp = new char[3] { '1', 'u', 'p' };
        static char[] speedDown = new char[4] { 'S', 'p', 'd', 'D' };
 
        static void Main()
        {
            int menuStartX = 85;
            int menuStartY = 2;
            int travelled = 0;
            int currentSpeed = minSpeed;
            int currentPulse = minPulse;

            // Hides the annoying cursor
            Console.CursorVisible = false;
            PrintMessages(0, 5, gameBeginning, ConsoleColor.Cyan);
            ConsoleKeyInfo key = Console.ReadKey(true);
            

            while (true)
            {
                travelled++;
                if (currentSpeed == maxSpeed)
                {
                    if (currentPulse == maxPulse)
                    {
                        GameOver();
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
                    lifeSpawnWidth = randomNum.Next(1, gameFieldWidth - lifeUp.Length);
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
                    speedSpawnWidth = randomNum.Next(1, gameFieldWidth - speedDown.Length);
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
                else if (chance >= 21 && chance <= 40 && spawnedLeftTree == false)
                {
                    treeSpawnWidth = randomNum.Next(1, (gameFieldWidth / 3)- bigBor.GetLength(1));
                    treeSpawnHeigth = gameFieldHeigth - 1;

                    SpawnLeftTree(treeSpawnWidth, treeSpawnHeigth);
                    spawnedLeftTree = true;
                }
                //else if (chance >= 41 && chance <= 60 && spawnedMiddleTree == false)
                //{
                //    speedSpawnWidth = randomNum.Next(1, gameFieldWidth - 4);
                //    speedSpawnHeight = randomNum.Next(3, gameFieldHeigth - 1);
                //}
                //else if (chance >= 61 && chance <= 80 && spawnedRightTree == false)
                //{
                //    speedSpawnWidth = randomNum.Next(1, gameFieldWidth - 4);
                //    speedSpawnHeight = randomNum.Next(3, gameFieldHeigth - 1);
                //}

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
                if (spawnedLeftTree)
                {
                    treeSpawnHeigth--;
                    if (treeSpawnHeigth <= 2)
                    {
                        DespawnLeftTree(treeSpawnHeigth, treeSpawnWidth);
                        if (leftTreeCounter == 0)
                        {
                            spawnedLeftTree = false;
                        }
                    }
                    else
                    {
                        SpawnLeftTree(treeSpawnHeigth, treeSpawnWidth);
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
                CollisionWithBigBor(kiwiPositionX, kiwiPositionY);



                // Slow down game
                Thread.Sleep((300- currentSpeed) >= 50 ? (200 - currentSpeed) : 150);
                Console.Clear();

                // Redraw the gameField after the clear(). Fixes movement tearing, BUT causes blue dot bug.
                // Blue dot bug is fixed if this goes at the end of MoveKiwi, but tearing reapers
                PrintGameField(gameField);
                PrintStaticMenu(menuStartX, menuStartY);
                PrintDynamicMenu(menuStartX, menuStartY, travelled, currentLives, currentPulse, currentSpeed);

                if (borCollision == true)
                {
                    Console.Beep();
                    Console.BackgroundColor = ConsoleColor.Red;
                    borCollision = false;
                    currentLives--;
                    travelled = 0;
                    kiwi[2, 2] = 'X';
                    Thread.Sleep(1000);
                    Console.BackgroundColor = ConsoleColor.White;
                    kiwi[2, 2] = '@';
                    if (currentLives <= 0)
                    {
                        GameOver();
                    }
                }
                
            }
        }


        private static void MoveKiwi(ConsoleKeyInfo pressedKey)
        {
            if (pressedKey.Key == ConsoleKey.LeftArrow)
            {
                if (kiwiPositionY - 2 >= 0)
                {
                    kiwiPositionY -= 2;
                }
            }
            if (pressedKey.Key == ConsoleKey.RightArrow)
            {
                if (kiwiPositionY + 2 <= gameFieldWidth - kiwi.GetLength(1) - 1)
                {
                    kiwiPositionY += 2;
                }
            }
            if (pressedKey.Key == ConsoleKey.UpArrow)
            {
                if (kiwiPositionX - 2 >= 0)
                {
                    kiwiPositionX -= 2;
                }
            }
            if (pressedKey.Key == ConsoleKey.DownArrow)
            {
                if (kiwiPositionX + 2 <= heigth - kiwi.GetLength(0))
                {
                    kiwiPositionX += 2;
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
                }
            }
        }

        // Spawning below
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

        private static void SpawnLeftTree(int spawnWidth, int spawnHeigth)
        {
            if (leftTreeCounter < bigBor.GetLength(0))
            {
                leftTreeCounter++;
            }
            for (int row = 0, i = spawnWidth; row < leftTreeCounter; row++, i++)
            {
                for (int col = 0, j = spawnHeigth; col < bigBor.GetLength(1); col++, j++)
                {
                        gameField[i, j] = bigBor[row, col];                  
                }
            }
        }
        // Despawn

        private static void DeleteTree(int spawnWidth, int spawnHeigth)
        {

        }
        
        private static void DespawnLeftTree(int spawnWidth, int spawnHeigth)
        {
            if (leftTreeCounter > 0)
            {
                leftTreeCounter--;
            }
            leftTreeDespawnCounter = 3;
            leftTreeDespawnCounter = (leftTreeDespawnCounter - leftTreeCounter) + 1;

            for (int row = leftTreeCounter, i = spawnWidth; row > 0; row--, i++)
            {
                for (int col = 0, j = spawnHeigth; col < bigBor.GetLength(1); col++, j++)
                {
                    gameField[i, j] = bigBor[leftTreeDespawnCounter, col];                  
                }
                leftTreeDespawnCounter++;
            }
        }

        // Filling game field
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

        // Printing below
        private static void PrintGameField(char[,] gameField)
        {


            for (int row = 0; row < gameField.GetLength(0); row++)
            {
                for (int col = 0; col < gameField.GetLength(1); col++)
                {
                    SetColors(row, col);
                    if (gameField[row, col] == '?' || gameField[row, col] == '0')
                    {
                        
                    }
                    else
                    {
                        Console.SetCursorPosition(col, row);
                        Console.Write(gameField[row, col]);
                    }
                    if (col == gameField.GetLength(1) - 1)
                    {
                        Console.SetCursorPosition(col, row);
                        Console.Out.Write(gameField[row, col] = '|');
                    }
                }
            }
        }
        private static void PrintDynamicMenu(int positionX, int positionY, int travelled, int currentLives, int currentPulse, int currentSpeed)
        {            
            if (currentLives == 1)
            {
                PrintMessages(positionX + 2, positionY + 4, currentLives.ToString(), ConsoleColor.Red);
            }
            else if (currentLives > 1 && currentLives <= 3)
            {
                PrintMessages(positionX + 2, positionY + 4, currentLives.ToString(), ConsoleColor.Yellow);
            }
            else
            {
                PrintMessages(positionX + 2, positionY + 4, currentLives.ToString(), ConsoleColor.Green);
            }
            
            if (currentSpeed < maxSpeed)
            {
                PrintMessages(positionX + 2, positionY + 10, currentSpeed.ToString(), ConsoleColor.Cyan);
            }
            else
            {
                PrintMessages(positionX + 2, positionY + 10, currentSpeed.ToString(), ConsoleColor.Red);
            }            
            PrintMessages(positionX + 2, positionY + 16, currentPulse.ToString(), ConsoleColor.Red);           
            PrintMessages(positionX + 2, positionY + 22, travelled.ToString(), ConsoleColor.Cyan);
        }

        private static void PrintStaticMenu(int positionX, int positionY)
        {
            PrintMessages(positionX, positionY + 2, "Lives:", ConsoleColor.White);
            PrintMessages(positionX + 2, positionY + 3, "Curr /", ConsoleColor.DarkGray);
            PrintMessages(positionX + 9, positionY + 3, "Max", ConsoleColor.DarkGray);
            PrintMessages(positionX + 7, positionY + 4, "/", ConsoleColor.DarkGray);
            PrintMessages(positionX + 9, positionY + 4, maxLives.ToString(), ConsoleColor.Gray);
            PrintMessages(positionX, positionY + 8, "Speed:", ConsoleColor.White);
            PrintMessages(positionX + 2, positionY + 9, "Curr /", ConsoleColor.DarkGray);
            PrintMessages(positionX + 9, positionY + 9, "Max", ConsoleColor.DarkGray);
            PrintMessages(positionX + 7, positionY + 10, "/", ConsoleColor.DarkGray);
            PrintMessages(positionX + 9, positionY + 10, maxSpeed.ToString(), ConsoleColor.Gray);
            PrintMessages(positionX, positionY + 14, "Pulse:", ConsoleColor.White);
            PrintMessages(positionX + 2, positionY + 15, "Curr /", ConsoleColor.DarkGray);
            PrintMessages(positionX + 9, positionY + 15, "Max", ConsoleColor.DarkGray);
            PrintMessages(positionX + 7, positionY + 16, "/", ConsoleColor.DarkGray);
            PrintMessages(positionX + 9, positionY + 16, maxPulse.ToString(), ConsoleColor.Gray);
            PrintMessages(positionX, positionY + 20, "Travelled:", ConsoleColor.White);
        }
        static void PrintMessages(int x, int y, string text, ConsoleColor color)
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

        private static void CollisionWithBigBor(int currentRow, int currentCol)
        {
            for (int row = currentRow, i = 0; i < kiwi.GetLength(0); row++, i++)
            {
                for (int col = currentCol, j = 0; j < kiwi.GetLength(1); col++, j++)
                {
                    for (int borRow = 0; borRow < bigBor.GetLength(0); borRow++)
                    {
                        for (int borCol = 0; borCol < bigBor.GetLength(1); borCol++)
                        {
                            if ((gameField[row+1, col] == bigBor[borRow, borCol]) && (bigBor[borRow, borCol] != '?'))
                            {
                                borCollision = true;
                            }
                        }
                    }
                }
            }
        }
        private static void GameOver()
        {
            Thread.Sleep(1000);
            Console.Clear();
            PrintMessages(0, 10, gameOver, ConsoleColor.Red);
            Console.ReadKey();
            Environment.Exit(0);
        }
        // Format exceptions below
        private static void IndexOutOfRangeException(IndexOutOfRangeException e)
        {
            Console.WriteLine(e.Message);
        }

        static void SetColors(int row, int col)
        {
            if ( gameField[row, col] == '\\' || gameField[row, col] == '/' || gameField[row, col] == '(' || gameField[row, col] == ')' || gameField[row, col] == '(' || gameField[row, col] == '@')
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
            }
            
             if (gameField[row, col] == '"')
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
            }

            if (gameField[row, col] == '|')
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            if (gameField[row, col] == '^')
            {
                Console.ForegroundColor = ConsoleColor.Green;

            }
            if (gameField[row, col] == '$')
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;

            }

            if (gameField[row, col] == '0')
            {
                Console.ForegroundColor = ConsoleColor.Blue;

            }
        }


    }
}
