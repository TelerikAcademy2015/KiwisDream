// BUGS SO FAR :
/*
 * 1. Когато се вземе живот или спийд със първия му чар (удар на първия чар на пилето и първия чар на живота/спийда), се отчита, че се е взел както живот, така и спийд.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KiwiGame
{
    class KiwisDeath
    {
        static Random randomNum = new Random();
        static int kiwiPositionX = 5;
        static int kiwiPositionY = 10;
        const int maxLives = 10;
        static int currentLives = 4;
        const int maxSpeed = 200;
        const int minSpeed = 10;
        const int maxPulse = 180;
        const int minPulse = 40;
        static int heigth = Console.BufferHeight = Console.WindowHeight = 35;
        static int width = Console.BufferWidth = Console.WindowWidth = 100;
        static int gameFieldWidth = width - 35;
        static int gameFieldHeigth = heigth;
        static char[,] gameField = new char[gameFieldHeigth, gameFieldWidth];  
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

        static bool treeCollision;


        static string gameBeginning = System.IO.File.ReadAllText("../../../GameBeginningFile.txt");
        static string gameOver = System.IO.File.ReadAllText("../../../GameOverFile.txt");
        static char[,] kiwi = new char[4, 5] 
        {
            {'?', '\"', '?', '\"', '?'},
            {'\\', '(', '?', ')', '/'},
            {'?', '?', '@', '?', '?'},
            {'?', '?', '|', '?', '?'}
        };

        static char[] lifeUp = new char[3] { '1', 'u', 'p' };
        static char[] speedDown = new char[4] { 'S', 'p', 'd', 'D' };

        static void Main()
        {
            SoundPlayer player = new SoundPlayer();
            try
            {
                player.SoundLocation = @"..\..\..\music\Final2.wav";
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found!");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Path not found!");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("You do not have permit to use this file!");
            }

            player.PlayLooping();

            int menuStartX = 70;
            int menuStartY = 2;
            int travelled = 0;
            int currentSpeed = minSpeed;
            int currentPulse = minPulse;

            // Hides the annoying cursor
            Console.CursorVisible = false;
            PrintMessages(0, 5, gameBeginning, ConsoleColor.Cyan);
            ConsoleKeyInfo key = Console.ReadKey(true);

            Tree leftTree = new Tree(Tree.bigBor, gameFieldHeigth - 1, randomNum.Next(1, gameFieldWidth / 5 - Tree.bigBor.GetLength(1)));
            Tree middleTree = new Tree(Tree.bigDub, gameFieldHeigth - 1, randomNum.Next(gameFieldWidth * 1 / 5, gameFieldWidth * 2 / 5 - Tree.bigDub.GetLength(1)));
            Tree rightTree = new Tree(Tree.mediumBuk, gameFieldHeigth - 1, randomNum.Next(gameFieldWidth * 2 / 5, gameFieldWidth * 3 / 5 - Tree.mediumBuk.GetLength(1)));
            Tree lastTree = new Tree(Tree.bigBuk, gameFieldHeigth - 1, randomNum.Next(gameFieldWidth * 3 / 5, gameFieldWidth * 4 / 5 - Tree.bigBuk.GetLength(1)));
            Tree lastLastTree = new Tree(Tree.lipa, gameFieldHeigth - 1, randomNum.Next(gameFieldWidth * 4 / 5, gameFieldWidth - Tree.lipa.GetLength(1)));

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
                    if (gameField[lifeSpawnHeight, lifeSpawnWidth] == '0' ||
                        gameField[lifeSpawnHeight, lifeSpawnWidth] == '?')
                    {
                        SpawnLifeUp(lifeSpawnWidth, lifeSpawnHeight);
                        spawnedLife = true;
                    }

                }
                else if (chance >= 11 && chance <= 20 && spawnedSpeed == false)
                {
                    // spawn speed down
                    speedSpawnWidth = randomNum.Next(1, gameFieldWidth - speedDown.Length);
                    speedSpawnHeight = randomNum.Next(3, gameFieldHeigth - 1);

                    // A check to make sure theres NOTHING on the spawn point.
                        if (gameField[speedSpawnHeight, speedSpawnWidth] == '0' ||
                            gameField[speedSpawnHeight, speedSpawnWidth] == '?')
                        {
                            SpawnSpeedDown(speedSpawnWidth, speedSpawnHeight);
                            spawnedSpeed = true;
                        }
                }
                else if (chance >= 21 && chance <= 35 && leftTree.SpawnedTree == false)
                {
                    leftTree.SpawnWidth = randomNum.Next(1, gameFieldWidth / 5 - leftTree.TypeOfTree.GetLength(1));
                    leftTree.SpawnHeigth = gameFieldHeigth - 1;
                    SpawnTree(leftTree);
                    leftTree.SpawnedTree = true;
                }
                else if (chance >= 36 && chance <= 50 && middleTree.SpawnedTree == false)
                {
                    middleTree.SpawnWidth = randomNum.Next(gameFieldWidth / 5, gameFieldWidth * 2 / 5 - middleTree.TypeOfTree.GetLength(1));
                    middleTree.SpawnHeigth = gameFieldHeigth - 1;
                    SpawnTree(middleTree);
                    middleTree.SpawnedTree = true;
                }
                else if (chance >= 51 && chance <= 65 && rightTree.SpawnedTree == false)
                {
                    rightTree.SpawnWidth = randomNum.Next(gameFieldWidth * 2 / 5, gameFieldWidth * 3 / 5 - rightTree.TypeOfTree.GetLength(1));
                    rightTree.SpawnHeigth = gameFieldHeigth - 1;
                    SpawnTree(rightTree);
                    rightTree.SpawnedTree = true;
                }
                else if (chance >= 66 && chance <= 80 && lastTree.SpawnedTree == false)
                {
                    lastTree.SpawnWidth = randomNum.Next(gameFieldWidth * 3 / 5, gameFieldWidth * 4 / 5 - lastTree.TypeOfTree.GetLength(1));
                    lastTree.SpawnHeigth = gameFieldHeigth - 1;
                    SpawnTree(lastTree);
                    lastTree.SpawnedTree = true;
                }
                else if (chance >= 81 && chance <= 100 && lastLastTree.SpawnedTree == false)
                {
                    lastLastTree.SpawnWidth = randomNum.Next(gameFieldWidth * 4 / 5, gameFieldWidth * 5 / 5 - lastLastTree.TypeOfTree.GetLength(1));
                    lastLastTree.SpawnHeigth = gameFieldHeigth - 1;
                    SpawnTree(lastLastTree);
                    lastLastTree.SpawnedTree = true;
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

                MoveTree(leftTree);
                MoveTree(middleTree);
                MoveTree(rightTree);
                MoveTree(lastTree);
                MoveTree(lastLastTree);

                // Check for collisions 
                CollisionWithLifeUp(kiwiPositionX, kiwiPositionY);
                CollisionWithSpeedDown(kiwiPositionX, kiwiPositionY);
                CheckCollisionWithTree(kiwiPositionX, kiwiPositionY, leftTree);
                CheckCollisionWithTree(kiwiPositionX, kiwiPositionY, middleTree);
                CheckCollisionWithTree(kiwiPositionX, kiwiPositionY, rightTree);
                CheckCollisionWithTree(kiwiPositionX, kiwiPositionY, lastTree);
                CheckCollisionWithTree(kiwiPositionX, kiwiPositionY, lastLastTree);

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

                // Slow down game
                Thread.Sleep((20 - currentSpeed) >= 50 ? (200 - currentSpeed) : 150);
                Console.Clear();

                // Redraw the gameField after the clear().
                PrintGameField(gameField);
                PrintStaticMenu(menuStartX, menuStartY);
                PrintDynamicMenu(menuStartX, menuStartY, travelled, currentLives, currentPulse, currentSpeed);

                if (treeCollision == true)
                {
                    if (leftTree.IsHitted)
                    {
                        DeleteTree(leftTree);
                    }
                    if (middleTree.IsHitted)
                    {
                        DeleteTree(middleTree);
                    }
                    if (rightTree.IsHitted)
                    {
                        DeleteTree(rightTree);
                    }
                    if (lastTree.IsHitted)
                    {
                        DeleteTree(lastTree);
                    }
                    if (lastLastTree.IsHitted)
                    {
                        DeleteTree(lastLastTree);
                    }

                    Console.Beep();
                    treeCollision = false;
                    currentLives--;
                    Thread.Sleep(300);
                    if (currentLives == 0)
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
                if (kiwiPositionX + 2 <= heigth - 1 - kiwi.GetLength(0))
                {
                    kiwiPositionX += 2;
                }
            }
            if (pressedKey.Key == ConsoleKey.P)
            {
                Pause();
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

        private static void SpawnTree(Tree tree)
        {
            if (tree.Counter < tree.TypeOfTree.GetLength(0))
            {
                tree.Counter++;
            }
            for (int row = 0, i = tree.SpawnHeigth; row < tree.Counter; row++, i++)
            {
                for (int col = 0, j = tree.SpawnWidth; col < tree.TypeOfTree.GetLength(1); col++, j++)
                {
                    gameField[i, j] = tree.TypeOfTree[row, col];
                }
            }
        }

        public static void MoveTree(Tree tree)
        {
            if (tree.SpawnedTree)
            {
                tree.SpawnHeigth--;
                if (tree.SpawnHeigth <= 2)
                {
                    DespawnTree(tree);
                    if (tree.Counter == 0)
                    {
                        tree.SpawnedTree = false;
                    }
                }
                else
                {
                    SpawnTree(tree);
                }
            }
        }

        private static void DespawnTree(Tree tree)
        {
            if (tree.Counter > 0)
            {
                tree.Counter--;
            }
            tree.DespawnCounter = tree.TypeOfTree.GetLength(0) - 1;
            tree.DespawnCounter = (tree.DespawnCounter - tree.Counter) + 1;

            for (int row = tree.Counter, i = tree.SpawnHeigth; row > 0; row--, i++)
            {
                for (int col = 0, j = tree.SpawnWidth; col < tree.TypeOfTree.GetLength(1); col++, j++)
                {
                    gameField[i, j] = tree.TypeOfTree[tree.DespawnCounter, col];
                }
                tree.DespawnCounter++;
            }
        }

        private static void DeleteTree(Tree tree)
        {
                tree.SpawnedTree = false;
                tree.Counter = -1;
                tree.DespawnCounter = tree.TypeOfTree.GetLength(0) - 1;
                tree.IsHitted = false;
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
                    //SetColors(row, col);
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
                        Console.Write(gameField[row, col] = '|');
                    }
                }
            }
        }
        private static void PrintDynamicMenu(int positionX, int positionY, int travelled, int currentLives, int currentPulse, int currentSpeed)
        {
            if (currentLives == 1)
            {
                PrintMessages(positionX + 6, positionY + 4, currentLives.ToString(), ConsoleColor.Red);
            }
            else if (currentLives > 1 && currentLives <= 3)
            {
                PrintMessages(positionX + 6, positionY + 4, currentLives.ToString(), ConsoleColor.Yellow);
            }
            else
            {
                PrintMessages(positionX + 6, positionY + 4, currentLives.ToString(), ConsoleColor.Green);
            }

            if (currentSpeed < maxSpeed)
            {
                PrintMessages(positionX + 6, positionY + 10, currentSpeed.ToString(), ConsoleColor.Cyan);
            }
            else
            {
                PrintMessages(positionX + 6, positionY + 10, currentSpeed.ToString(), ConsoleColor.Red);
            }
            PrintMessages(positionX + 6, positionY + 16, currentPulse.ToString(), ConsoleColor.Red);
            PrintMessages(positionX + 6, positionY + 22, travelled.ToString(), ConsoleColor.Cyan);
        }

        private static void PrintStaticMenu(int positionX, int positionY)
        {
            PrintMessages(positionX , positionY + 1, "╔══════════════════════╗", ConsoleColor.White);
            PrintMessages(positionX , positionY + 2, "║", ConsoleColor.White);
            PrintMessages(positionX + 23, positionY + 2, "║", ConsoleColor.White);
            PrintMessages(positionX +4, positionY + 2, "Lives: ", ConsoleColor.White);
            PrintMessages(positionX , positionY + 3, "║", ConsoleColor.White);
            PrintMessages(positionX + 23, positionY + 3, "║", ConsoleColor.White);
            PrintMessages(positionX + 6, positionY + 3, "Curr /", ConsoleColor.DarkGray);
            PrintMessages(positionX + 13, positionY + 3, "Max", ConsoleColor.DarkGray);
            PrintMessages(positionX + 11, positionY + 4, "/", ConsoleColor.DarkGray);
            PrintMessages(positionX , positionY + 4, "║", ConsoleColor.White);
            PrintMessages(positionX + 23, positionY + 4, "║", ConsoleColor.White);
            PrintMessages(positionX + 13, positionY + 4, maxLives.ToString(), ConsoleColor.Gray);
            PrintMessages(positionX , positionY + 5, "╚══════════════════════╝", ConsoleColor.White);


            PrintMessages(positionX, positionY + 7, "╔══════════════════════╗", ConsoleColor.White);
            PrintMessages(positionX + 4, positionY + 8, "Speed:", ConsoleColor.White);
            PrintMessages(positionX, positionY + 8, "║", ConsoleColor.White);
            PrintMessages(positionX + 23, positionY + 8, "║", ConsoleColor.White);
            PrintMessages(positionX + 6, positionY + 9, "Curr /", ConsoleColor.DarkGray);
            PrintMessages(positionX + 13, positionY + 9, "Max", ConsoleColor.DarkGray);
            PrintMessages(positionX, positionY + 9, "║", ConsoleColor.White);
            PrintMessages(positionX + 23, positionY + 9, "║", ConsoleColor.White);
            PrintMessages(positionX + 11, positionY + 10, "/", ConsoleColor.DarkGray);
            PrintMessages(positionX + 13, positionY + 10, maxSpeed.ToString(), ConsoleColor.Gray);
            PrintMessages(positionX , positionY + 10, "║", ConsoleColor.White);
            PrintMessages(positionX + 23, positionY + 10, "║", ConsoleColor.White);
            PrintMessages(positionX , positionY + 11, "║", ConsoleColor.White);
            PrintMessages(positionX + 23, positionY + 11, "║", ConsoleColor.White);
            PrintMessages(positionX , positionY + 11, "╚══════════════════════╝", ConsoleColor.White);


            PrintMessages(positionX, positionY + 13, "╔══════════════════════╗", ConsoleColor.White);
            PrintMessages(positionX, positionY + 14, "║", ConsoleColor.White);
            PrintMessages(positionX + 23, positionY + 14, "║", ConsoleColor.White);
            PrintMessages(positionX +4, positionY + 14, "Pulse:", ConsoleColor.White);
            PrintMessages(positionX + 6, positionY + 15, "Curr /", ConsoleColor.DarkGray);
            PrintMessages(positionX + 13, positionY + 15, "Max", ConsoleColor.DarkGray);
            PrintMessages(positionX, positionY + 15, "║", ConsoleColor.White);
            PrintMessages(positionX + 23, positionY + 15, "║", ConsoleColor.White);
            PrintMessages(positionX + 11, positionY + 16, "/", ConsoleColor.DarkGray);
            PrintMessages(positionX + 13, positionY + 16, maxPulse.ToString(), ConsoleColor.Gray);
            PrintMessages(positionX, positionY + 16, "║", ConsoleColor.White);
            PrintMessages(positionX + 23, positionY + 16, "║", ConsoleColor.White);
            PrintMessages(positionX, positionY + 17, "╚══════════════════════╝", ConsoleColor.White);


            PrintMessages(positionX, positionY + 19, "╔══════════════════════╗", ConsoleColor.White);
            PrintMessages(positionX, positionY + 20, "║", ConsoleColor.White);
            PrintMessages(positionX + 23, positionY + 20, "║", ConsoleColor.White);
            PrintMessages(positionX +4, positionY + 20, "Travelled:", ConsoleColor.White);
            PrintMessages(positionX, positionY + 21, "║", ConsoleColor.White);
            PrintMessages(positionX + 23, positionY + 21, "║", ConsoleColor.White);
            PrintMessages(positionX, positionY + 22, "║", ConsoleColor.White);
            PrintMessages(positionX + 23, positionY + 22, "║", ConsoleColor.White);
            PrintMessages(positionX, positionY + 23, "╚══════════════════════╝", ConsoleColor.White);
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

        private static void CheckCollisionWithTree(int currentRow, int currentCol, Tree tree)
        {
            for (int row = currentRow, i = 0; i < kiwi.GetLength(0); row++, i++)
            {
                for (int col = currentCol, j = 0; j < kiwi.GetLength(1); col++, j++)
                {
                    for (int treeRow = 0; treeRow < tree.TypeOfTree.GetLength(0); treeRow++)
                    {
                        for (int treeCol = 0; treeCol < tree.TypeOfTree.GetLength(1); treeCol++)
                        {
                            if ((gameField[row + 1, col] == tree.TypeOfTree[treeRow, treeCol]) && (tree.TypeOfTree[treeRow, treeCol] != '?'))
                            {
                                treeCollision = true;
                                tree.IsHitted = true;
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

        private static void IndexOutOfRangeException(IndexOutOfRangeException e)
        {
            Console.WriteLine(e.Message);
        }

        static void SetColors(int row, int col)
        {
            if (gameField[row, col] == '\\' || gameField[row, col] == '/' || gameField[row, col] == '(' || gameField[row, col] == ')' || gameField[row, col] == '(' || gameField[row, col] == '@')
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

        static void Pause()
        {
           PrintMessages(0, 2,"The game is paused ", ConsoleColor.Green);
            System.Diagnostics.Process pauseProc =
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = "cmd",
                    Arguments = "/C pause",
                    UseShellExecute = false
                });
            pauseProc.WaitForExit();
            Console.ForegroundColor = ConsoleColor.Cyan;
        }
    }
}
