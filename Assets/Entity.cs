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

        public string[,] getNeighbours(string[,] grid)
        {
            bool leftEdge = false;
            bool rightEdge = false;
            bool topEdge = false;
            bool bottomEdge = false;

            if (x == 0)
            {
                leftEdge = true;
            }
            else if (x == grid.GetLength(0))
            {
                rightEdge = true;
            }
            else if (y == 0)
            {
                topEdge = true;
            }
            else if (y == grid.GetLength(0))
            {
                bottomEdge = true;
            }
            else
            {
                Debug.Log($"{name} is not on edge");
            }
            return null;
        }
    }
}
