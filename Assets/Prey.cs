﻿using Assets;
using System;

/// <summary>
/// Prey class:
/// Allows use of objects to identify each prey entity
/// </summary>
internal class Prey : Entity
{

	//set prey entity to predetermined values
	public Prey(float energyLevel, float foodLevel, float waterLevel, int maxOffsprings, int reproductionProb, int numOffsprings, float minReproductionEnergy, string name, int x, int y)
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

	//default prey entity values TBD
	public Prey()
	{
		
	}

}
