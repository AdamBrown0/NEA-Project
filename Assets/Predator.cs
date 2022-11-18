using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    internal class Predator : Entity
    {
        //set predator entity to predetermined values
        public Predator(float energyLevel, float foodLevel, float waterLevel, int maxOffsprings, float reproductionProb, int numOffsprings, float minReproductionEnergy)
        {
            this.energyLevel = energyLevel;
            this.foodLevel = foodLevel;
            this.waterLevel = waterLevel;
            this.maxOffsprings = maxOffsprings;
            this.reproductionProb = reproductionProb;
            this.numOffsprings = numOffsprings;
            this.minReproductionEnergy = minReproductionEnergy;
        }

        //default predator entity values TBD
        public Predator()
        {

        }

    }
}
