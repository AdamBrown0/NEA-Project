using Assets;
using System;

/// <summary>
/// Prey class:
/// Allows use of objects to identify each prey entity
/// </summary>
internal class Predator : Entity
    {
        //set predator entity to predetermined values
        public Predator(float energyLevel, float foodLevel, float waterLevel, int maxOffsprings, float reproductionProb, int numOffsprings, float minReproductionEnergy, string name, int x, int y)
        {
            this.energyLevel = energyLevel;
            this.foodLevel = foodLevel;
            this.waterLevel = waterLevel;
            this.maxOffsprings = maxOffsprings;
            this.reproductionProb = reproductionProb;
            this.numOffsprings = numOffsprings;
            this.minReproductionEnergy = minReproductionEnergy;
            this.name = name;
            this.x = x;
            this.y = y;
        }

        //default predator entity values TBD
        public Predator()
        {

        }

    }

