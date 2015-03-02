using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiwiGame
{
    public class Tree
    {
        private char[,] typeOfTree;
        private int spawnWidth;
        private int spawnHeigth;
        private bool spawnedTree;
        private int counter;
        private int despawnCounter;
        private bool isHitted;

        public bool IsHitted
        {
            get { return this.isHitted; }
            set { this.isHitted = value; }
        }

        public char[,] TypeOfTree
        {
            get { return this.typeOfTree; }
            set { this.typeOfTree = value; }
        }

        public int SpawnWidth
        {
            get { return this.spawnWidth; }
            set { this.spawnWidth = value; }
        }

        public int SpawnHeigth
        {
            get { return this.spawnHeigth; }
            set { this.spawnHeigth = value; }
        }

        public bool SpawnedTree
        {
            get { return this.spawnedTree; }
            set { this.spawnedTree = value; }
        }

        public int Counter
        {
            get { return this.counter; }
            set { this.counter = value; }
        }

        public int DespawnCounter
        {
            get { return this.despawnCounter; }
            set { this.despawnCounter = value; }
        }

        public Tree(char[,] typeOfTree, int treeSpawnHeigth, int treeSpawnWidth)
        {

            this.typeOfTree = typeOfTree;
            this.spawnWidth = treeSpawnWidth;
            this.spawnHeigth = treeSpawnHeigth;
            this.spawnedTree = false;
            this.isHitted = false;
            this.counter = -1;
            despawnCounter = typeOfTree.GetLength(0) - 1;
        }



        public static char[,] bigDub = new char[4, 5]
        {
            {'?', '#', '#', '#', '?'},
            {'#', '#', '#', '#', '#'},
            {'#', '#', '#', '#', '#'},
            {'?', '?', '$', '?', '?'},

        };

        public static char[,] bigBor = new char[4, 5]
        {
            {'?', '?', '^', '?', '?'},
            {'?', '^', '^', '^', '?'},
            {'^', '^', '^', '^', '^'},
            {'?', '?', '$', '?', '?'},

        };

        public static char[,] mediumBuk = new char[4, 5]
        {
            {'?', '&', '&', '&', '?'},
            {'&', '&', '&', '&', '&'},
            {'?', '&', '&', '&', '?'},
            {'?', '?', '$', '?', '?'},
        };

        public static char[,] bigBuk = new char[4, 5]
        {
            {'G', 'G', 'G', 'G', 'G'},
            {'G', 'G', 'G', 'G', 'G'},
            {'?', 'G', 'G', 'G', '?'},
            {'?', '?', '$', '?', '?'},
        };

        public static char[,] lipa = new char[4, 5]
        {
            {'?', '8', '8', '8', '?'},
            {'8', '8', '8', '8', '8'},
            {'8', '?', '8', '?', '8'},
            {'?', '?', '$', '?', '?'},
        };
    }
}
