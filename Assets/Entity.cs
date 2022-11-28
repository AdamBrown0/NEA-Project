using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    internal abstract class Entity
    {
        //energy related variables
        protected float energyLevel;
        protected float foodLevel;
        protected float waterLevel;

        //breeding related variables
        protected int maxOffsprings;
        protected float reproductionProb;
        protected int numOffsprings;
        protected float minReproductionEnergy;

        //miscellaneous variables
        protected string name;
        protected int x;
        protected int y;

        public float getEnergy() { return energyLevel; }

        public float getFoodLevel() { return foodLevel; }

        public float getWaterLevel() { return waterLevel; }

        public int getMaxOffsprings() { return maxOffsprings; }

        public float getReproductionProb() { return reproductionProb; }

        public int getNumOffsprings() { return numOffsprings; }

        public float getMinReproductionEnergy() { return minReproductionEnergy; }

        public bool canReproduce() { return energyLevel == minReproductionEnergy; }

        public bool isDead() { return foodLevel == 0 || waterLevel == 0; }

        public string getName() { return name; }

        public int[] getPos() { return new int[2] { x, y }; }


        /* create 3x3 array for both entities and blocks
         * for i in that array check whether entity grid has entities, if it does update 3x3 entity array
         * also check which blocks are next to entity
         * return both arrays
         */

        //<array>.GetLength(x) where x is the dimension: 0 for rows, 1 for columns

        private string[,] createBlockGridBigArray(string[,] blockGrid)
        {
            string[,] bigArray = new string[blockGrid.GetLength(0) + 2, blockGrid.GetLength(1) + 2];
            for (int rows = 0; rows < bigArray.GetLength(0); rows++)
            {
                for (int cols = 0; cols < bigArray.GetLength(1); cols++)
                {
                    bigArray[rows, cols] = null;
                }
            }
            for (int rows = 0; rows < bigArray.GetLength(0); rows++)
            {
                for (int cols = 0; cols < bigArray.GetLength(1); cols++)
                {
                    if (rows == 0 || cols == 0 || rows == bigArray.GetLength(0) - 1 || cols == bigArray.GetLength(1) - 1)
                    {
                        Debug.Log($"Do nothing at row: {rows} col: {cols}");
                    }
                    else
                    {
                        Debug.Log($"Placed {blockGrid[rows - 1, cols - 1]} at row: {rows} col: {cols}");
                        bigArray[rows, cols] = blockGrid[rows - 1, cols - 1];
                    }
                }
            }
            return bigArray;
        }

        public string[,] getBlockNeighbours(string[,] blockGrid)
        {
            string[,] bigArray = createBlockGridBigArray(blockGrid);  //creates array that is 1 bigger on each side to eliminate need for edge cases
            string[,] neighbours = new string[3, 3];

            int[] rowsarr = { -1, -1, -1, 0, 0, 0, +1, +1, +1 };
            int[] colsarr = { -1, 0, +1, -1, 0, +1, -1, 0, +1 };

            int yB = 0;
            int xB = 0;
            for (int i = 0; i < 9; i++)
            {
                
                neighbours[xB, yB] = bigArray[y + rowsarr[i], x + colsarr[i]]; //
                if (i == 2 || i == 5)
                {
                    xB = 0;
                    yB += 1;
                    //Debug.Log($"x: {xB} y: {yB}");
                }
                else
                {
                    xB += 1;
                    //Debug.Log($"x: {xB} y: {yB}");
                }
            }

            for (int cols = 0; cols < neighbours.GetLength(0); cols++)
            {
                for (int rows = 0; rows < neighbours.GetLength(1); rows++)
                {
                    Debug.Log($"Tile {neighbours[cols, rows]} at row: {rows} col: {cols}");
                }
            }

            return neighbours;
        }

        private object[,] createEntityGridBigArray(object[,] entityGrid)
        {
            object[,] bigArray = new object[entityGrid.GetLength(0) + 2, entityGrid.GetLength(1) + 2];
            for (int rows = 0; rows < bigArray.GetLength(0); rows++)
            {
                for (int cols = 0; cols < bigArray.GetLength(1); cols++)
                {
                    bigArray[rows, cols] = null;
                }
            }
            for (int rows = 0; rows < bigArray.GetLength(0); rows++)
            {
                for (int cols = 0; cols < bigArray.GetLength(1); cols++)
                {
                    if (rows == 0 || cols == 0 || rows == bigArray.GetLength(0) - 1 || cols == bigArray.GetLength(1) - 1)
                    {
                        Debug.Log($"Do nothing at row: {rows} col: {cols}");
                    }
                    else
                    {
                        if (entityGrid[rows - 1, cols - 1] != null)
                        {
                            Type t = entityGrid[rows - 1, cols - 1].GetType();
                            Debug.Log($"Placed {t.ToString()} at row: {rows} col: {cols}");
                        }
                        else
                        {
                            Debug.Log($"Placed null at row: {rows} col: {cols}");
                        }
                        bigArray[rows, cols] = entityGrid[rows - 1, cols - 1];
                    }
                }
            }
            return bigArray;
        }

        public object[,] getEntityNeighbours(object[,] entityGrid)
        {
            object[,] bigArray = createEntityGridBigArray(entityGrid); //creates array that is 1 bigger on each side to eliminate need for edge cases

            object[,] neighbours = new object[3, 3];

            int[] rowsarr = { -1, -1, -1, 0, 0, 0, +1, +1, +1 };
            int[] colsarr = { -1, 0, +1, -1, 0, +1, -1, 0, +1 };

            int yB = 0;
            int xB = 0;
            for (int i = 0; i < 9; i++)
            {

                neighbours[xB, yB] = bigArray[y + rowsarr[i], x + colsarr[i]];
                if (i == 2 || i == 5)
                {
                    xB = 0;
                    yB += 1;
                    //Debug.Log($"x: {xB} y: {yB}");
                }
                else
                {
                    xB += 1;
                    //Debug.Log($"x: {xB} y: {yB}");
                }
            }

            for (int cols = 0; cols < neighbours.GetLength(0); cols++)
            {
                for (int rows = 0; rows < neighbours.GetLength(1); rows++)
                {
                    if (neighbours[cols, rows] != null)
                    {
                        Type t = neighbours[cols, rows].GetType();
                        Debug.Log($"Neighbour Tile {t.ToString()} at row: {rows} col: {cols}");
                    }
                    else
                    {
                        Debug.Log($"Neighbour Tile null at row: {rows} col: {cols}");
                    }
                }
            }

            return neighbours;
        }
    }
}
