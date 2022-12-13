using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.SimulationStuff
{
    public class MainSimulation
    {
        public static float foodDecreaseRate = -1f;
        public static float energyDecreaseRate = -1f;
        public static float waterDecreaseRate = -1f;

        private static int[] howToMove(int x, int y)
        {
            int[] move = new int[2]; //switching pos 0 for pos 1 to see if work
            switch (x)
            {
                case 0:
                    move[1] = -1;
                    switch (y)
                    {
                        case 0:
                            move[0] = -1;
                            break;
                        case 1:
                            move[0] = 0;
                            break;
                        case 2:
                            move[0] = 1;
                            break;
                    }
                    break;
                case 1:
                    move[1] = 0;
                    switch (y)
                    {
                        case 0:
                            move[0] = -1;
                            break;
                        case 1:
                            move[0] = 0;
                            break;
                        case 2:
                            move[0] = 1;
                            break;
                    }
                    break;
                case 2:
                    move[1] = 1;
                    switch (y)
                    {
                        case 0:
                            move[0] = -1;
                            break;
                        case 1:
                            move[0] = 0;
                            break;
                        case 2:
                            move[0] = 1;
                            break;
                    }
                    break;
                default:
                    Debug.Log($"howToMove gone wrong, MOVE: {move[0]}, {move[1]}");
                    break;
            }
            //Debug.Log($"RETURN: {move[1]}, {move[0]}");
            return move;
        }

        public static object[,] moveEntities(string[,] grid, object[,] entityGrid) //move entities by move distance, grid and entityGrid passed to make possible
        {
            object[,] newEntityGrid = new object[entityGrid.GetLength(0), entityGrid.GetLength(1)]; //this is needed to stop the for loop detecting moved entities and causing infinite loop
            Prey tempPrey = new Prey();
            string[,] bigBlockArray = tempPrey.createBlockGridBigArray(grid);

            //looping through to find an entity, if entity found will move to a valid space according to grid
            for (int rows = 0; rows < entityGrid.GetLength(0); rows++) //GetLength(0) for rows
            {
                for (int cols = 0; cols < entityGrid.GetLength(1); cols++) //GetLength(1) for cols
                {
                    Type t;
                    if (entityGrid[rows, cols] != null) //if entity is at row, col then check which type it is
                    {
                        t = entityGrid[rows, cols].GetType();
                        if (t == typeof(Prey)) //if type prey, prey cannot eat predators, so will only move to grass blocks
                        {
                            Debug.Log("TEST AT PREY");
                            Prey prey = (Prey)entityGrid[rows, cols];
                            string[,] blockNeighbours;
                            object[,] entityNeighbours;
                            if (grid != null && entityGrid != null)
                            {
                                Debug.Log("TEST");
                                blockNeighbours = prey.getBlockNeighbours(grid); //get block neighbours 3x3 string array
                                entityNeighbours = prey.getEntityNeighbours(entityGrid); //get entity neighbours 3x3 object array
                            }
                            else
                            {
                                Debug.Log($"Grid and entityGrid were null");
                                blockNeighbours = prey.getBlockNeighbours(grid); //get block neighbours 3x3 string array
                                entityNeighbours = prey.getEntityNeighbours(entityGrid); //get entity neighbours 3x3 object array
                            }

                            //Prey cannot eat predators, so we will search blockNeighbours for a valid space, if grass is found it will check entityNeighbours to see if there is an entity already occupying it
                            bool done = false;
                            int count = 0;
                            while (!done && count < 8)
                            {

                                Debug.Log($"DONE: {done} COUNT: {count}");
                                int randomX = UnityEngine.Random.Range(0, blockNeighbours.GetLength(0));
                                int randomY = UnityEngine.Random.Range(0, blockNeighbours.GetLength(1));

                                //debugBlockNeighbours:;
                                //Debug.Log($"Block Neighbours PREY while: {blockNeighbours[randomX, randomY]}, rows: {randomX} cols: {randomY}");
                                //goto debugBlockNeighbours;

                                if (blockNeighbours[randomX, randomY] == "G" && bigBlockArray[prey.getPos()[1] + howToMove(randomX, randomY)[0] + 1, prey.getPos()[0] + howToMove(randomX, randomY)[1] + 1] == "G") //checks if random spot is Grass (G) block
                                {
                                    Debug.Log($"BLOCK: {blockNeighbours[randomX, randomY]} GRID: {grid[prey.getPos()[0], prey.getPos()[1]]} at PREY WHILE\nRANDOMX: {randomX}, RANDOMY: {randomY}");
                                    if (entityNeighbours[randomX, randomY] == null) //if block is Grass, then check if block is not currently occupied by another entity
                                    {
                                        //move prey to new location and delete old
                                        try
                                        {
                                            newEntityGrid[rows + howToMove(randomX, randomY)[0], cols + howToMove(randomX, randomY)[1]] = prey;
                                            Debug.Log($"Prey moved to {rows + howToMove(randomX, randomY)[0]}, {cols + howToMove(randomX, randomY)[1]}\nTile was {blockNeighbours[randomX, randomY]}");
                                            done = true;
                                            goto outsideFallBack;
                                        }
                                        catch (Exception ex)
                                        {
                                            Debug.Log($"Error at Prey while loop, error: {ex}");
                                        }
                                    }
                                    else
                                    {
                                        //Debug.Log($"Couldn't move to {rows + howToMove(randomX, randomY)[0]}, {cols + howToMove(randomX, randomY)[1]} due to entity being {entityGrid[rows + howToMove(randomX, randomY)[0], cols + howToMove(randomX, randomY)[1]]}");
                                    }
                                }
                                else
                                {
                                    //Debug.Log($"Couldn't move to {rows + howToMove(randomX, randomY)[0]}, {cols + howToMove(randomX, randomY)[1]} due to tile being {grid[rows + howToMove(randomX, randomY)[0], cols + howToMove(randomX, randomY)[1]]}");
                                }
                                count++;
                            }
                            if (!done) //if the entity fails even this, then it is boxed in and cannot move at all
                            {
                                //if count has exceeded 8, we fall back to just choose the first option that pops up, this case will most likely never happen
                                for (int rowsBackup = 0; rowsBackup < blockNeighbours.GetLength(0); rowsBackup++)
                                {
                                    for (int colsBackup = 0; colsBackup < blockNeighbours.GetLength(1); colsBackup++)
                                    {
                                        //debugBlockNeighbours:;
                                        //Debug.Log($"Block Neighbours PREY fall back: {blockNeighbours[rowsBackup, colsBackup]}, rows: {rowsBackup} cols: {colsBackup}");
                                        //goto debugBlockNeighbours;

                                        if (blockNeighbours[rowsBackup, colsBackup] == "G" && bigBlockArray[prey.getPos()[1] + howToMove(rowsBackup, colsBackup)[0] + 1, prey.getPos()[0] + howToMove(rowsBackup, colsBackup)[1] + 1] == "G")
                                        {
                                            Debug.Log($"BLOCK: {blockNeighbours[rowsBackup, colsBackup]} GRID: {grid[prey.getPos()[0], prey.getPos()[1]]} at PREY FALL BACK");
                                            if (entityNeighbours[rowsBackup, colsBackup] == null)
                                            {
                                                //move prey to new location and delete old
                                                try
                                                {
                                                    newEntityGrid[rowsBackup + howToMove(rowsBackup, colsBackup)[0], colsBackup + howToMove(rowsBackup, colsBackup)[1]] = prey;
                                                    Debug.Log($"Prey moved to {rowsBackup + howToMove(rowsBackup, colsBackup)[0]}, {colsBackup + howToMove(rowsBackup, colsBackup)[1]}\nRelied on fall back option\n Tile was {blockNeighbours[rowsBackup , colsBackup]}");
                                                    done = true;
                                                    goto outsideFallBack;
                                                }
                                                catch (Exception ex)
                                                {
                                                    Debug.Log($"Error at Prey fall back loop, error: {ex}");
                                                }
                                            }
                                            else
                                            {
                                                Debug.Log($"Could not move prey to {rowsBackup + howToMove(rowsBackup, colsBackup)[0]}, {colsBackup + howToMove(rowsBackup, colsBackup)[1]}. Entity occupying was {entityNeighbours[rowsBackup, colsBackup]}\nTile was {blockNeighbours[rowsBackup, colsBackup]}");
                                            }
                                        }
                                        else
                                        {
                                            Debug.Log($"Could not move prey to {rowsBackup + howToMove(rowsBackup, colsBackup)[0]}, {colsBackup + howToMove(rowsBackup, colsBackup)[1]}. Tile was {blockNeighbours[rowsBackup, colsBackup]}");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Debug.Log($"Skipped fall back option");
                            }

                            outsideFallBack:;
                            goto outsideTypeStatement;
                        }
                        else if (t == typeof(Predator)) //if type prey, prey cannot eat predators, so will only move to grass blocks
                        {
                            Debug.Log("TEST AT PREDATOR");
                            Predator predator = (Predator)entityGrid[rows, cols];
                            string[,] blockNeighbours = new string[3, 3];
                            object[,] entityNeighbours = new object[3, 3];
                            if (grid != null && entityGrid != null)
                            {
                                Debug.Log("TEST");
                                blockNeighbours = predator.getBlockNeighbours(grid); //get block neighbours 3x3 string array
                                entityNeighbours = predator.getEntityNeighbours(entityGrid); //get entity neighbours 3x3 object array
                                Debug.Log($"predator x: {predator.getPos()[0]} y: {predator.getPos()[1]}");
                                foreach (object obj in entityNeighbours)
                                {
                                    Debug.Log($"EntityNeighbours: {obj}");
                                }
                            }
                            else
                            {
                                Debug.Log($"Grid and entityGrid were null");
                                
                            }

                            //Prey cannot eat predators, so we will search blockNeighbours for a valid space, if grass is found it will check entityNeighbours to see if there is an entity already occupying it
                            bool done = false;
                            int count = 0;
                            while (!done && count < 8)
                            {
                                Debug.Log($"DONE: {done} COUNT: {count}");
                                int randomX = UnityEngine.Random.Range(0, blockNeighbours.GetLength(0));
                                int randomY = UnityEngine.Random.Range(0, blockNeighbours.GetLength(1));
                                if (blockNeighbours[randomX, randomY] == "G" && bigBlockArray[predator.getPos()[1] + howToMove(randomX, randomY)[0] + 1, predator.getPos()[0] + howToMove(randomX, randomY)[1] + 1] == "G") //checks if random spot is Grass (G) block
                                {
                                    //debugBlockNeighbours:;
                                    //Debug.Log($"Block Neighbours PREDATOR while: {blockNeighbours[randomX, randomY]}, rows: {randomX} cols: {randomY}");
                                    //goto debugBlockNeighbours;

                                    Debug.Log($"BLOCK: {blockNeighbours[randomX, randomY]} GRID: {grid[predator.getPos()[0], predator.getPos()[1]]} at PREDATOR WHILE\nRANDOMX: {randomX}, RANDOMY: {randomY}");
                                    if (entityNeighbours[randomX, randomY] != null)
                                    {
                                        try
                                        {
                                            if (entityNeighbours[randomX, randomY].GetType() == typeof(Prey))
                                            {
                                                try
                                                {
                                                    newEntityGrid[rows + howToMove(randomX, randomY)[0], cols + howToMove(randomX, randomY)[1]] = predator;
                                                    Debug.Log($"Predator moved to {rows + howToMove(randomX, randomY)[0]}, {cols + howToMove(randomX, randomY)[1]}\nPredator has eaten Prey\n{blockNeighbours[randomX, randomY]}");
                                                    done = true;
                                                    goto outsideFallBack;
                                                }
                                                catch (Exception ex)
                                                {
                                                    Debug.Log($"Error at Predator while loop, error: {ex}");
                                                }
                                            }
                                            else if (entityNeighbours[randomX, randomY].GetType() == typeof(Predator))
                                            {
                                                Debug.Log($"Cannot move predator to {rows + howToMove(randomX, randomY)[0]}, {cols + howToMove(randomX, randomY)[1]} due to Predator occupying space\n{blockNeighbours[randomX, randomY]}");
                                            }
                                            else
                                            {
                                                Debug.Log($"Error at entityNeighbours[randomX, randomY] != null, entityNeighbours: {entityNeighbours[randomX, randomY]}");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Debug.Log($"Error at entityNeighbours[randomX, randomY] != null, entityNeighbours: {entityNeighbours[randomX, randomY]}");
                                        }
                                        
                                    }
                                    else
                                    {
                                        if (rows + howToMove(randomX, randomY)[0] > newEntityGrid.GetLength(0) - 1 || cols + howToMove(randomX, randomY)[1] > newEntityGrid.GetLength(1) - 1 || rows + howToMove(randomX, randomY)[0] < 0 || cols + howToMove(randomX, randomY)[1] < 0)
                                        {
                                            Debug.Log($"Out of Bounds at predator while loop, rows: {rows + howToMove(randomX, randomY)[0]} cols: {cols + howToMove(randomX, randomY)[1]}");
                                            goto outsideOutOfBoundsAreaWhile;
                                        }
                                        //code will only execute if above condition is not met, ie inside array
                                        newEntityGrid[rows + howToMove(randomX, randomY)[0], cols + howToMove(randomX, randomY)[1]] = predator;
                                        Debug.Log($"Predator moved to {rows + howToMove(randomX, randomY)[0]}, {cols + howToMove(randomX, randomY)[1]}\nTile is {blockNeighbours[randomX, randomY]}");
                                        done = true;
                                        Debug.Log($"entityNeighbours was default at Predator");
                                    }
                                    outsideOutOfBoundsAreaWhile:;
                                }
                                else
                                {
                                    //Debug.Log($"Couldn't move to {rows + howToMove(randomX, randomY)[0]}, {cols + howToMove(randomX, randomY)[1]} due to tile being {grid[rows + howToMove(randomX, randomY)[0], cols + howToMove(randomX, randomY)[1]]}");
                                }
                                count++;
                            }
                            if (!done) //if the entity fails even this, then it is boxed in and cannot move at all
                            {
                                //if count has exceeded 8, we fall back to just choose the first option that pops up, this case will most likely never happen
                                for (int rowsBackup = 0; rowsBackup < blockNeighbours.GetLength(0); rowsBackup++)
                                {
                                    for (int colsBackup = 0; colsBackup < blockNeighbours.GetLength(1); colsBackup++)
                                    {
                                        //debugBlockNeighbours:;
                                        //Debug.Log($"Block Neighbours PREDATOR fall back: {blockNeighbours[rowsBackup, colsBackup]}, rows: {rowsBackup} cols: {colsBackup}");
                                        //goto debugBlockNeighbours;

                                        if (blockNeighbours[rowsBackup, colsBackup] == "G" && bigBlockArray[predator.getPos()[1] + howToMove(rowsBackup, colsBackup)[0] + 1, predator.getPos()[0] + howToMove(rowsBackup, colsBackup)[1] + 1] == "G")
                                        {
                                            Debug.Log($"BLOCK: {blockNeighbours[rowsBackup, colsBackup]} GRID: {grid[predator.getPos()[0], predator.getPos()[1]]} at PREDATOR FALL BACK");
                                            if (entityNeighbours[rowsBackup, colsBackup] == null)
                                            {
                                                //move prey to new location and delete old
                                                Debug.Log($"BLOCKNEIGHBOURS: {blockNeighbours[rowsBackup, colsBackup]}");
                                                Debug.Log($"rows: {rowsBackup + howToMove(rowsBackup, colsBackup)[0]}, cols: {colsBackup + howToMove(rowsBackup, colsBackup)[1]}");
                                                try
                                                {
                                                    newEntityGrid[rowsBackup + howToMove(rowsBackup, colsBackup)[0], colsBackup + howToMove(rowsBackup, colsBackup)[1]] = predator;
                                                    Debug.Log($"Predator moved to {rowsBackup + howToMove(rowsBackup, colsBackup)[0]}, {colsBackup + howToMove(rowsBackup, colsBackup)[1]}\nRelied on fall back option\nTile is {blockNeighbours[rowsBackup, colsBackup]}");
                                                    done = true;
                                                    goto outsideFallBack;
                                                }
                                                catch (Exception ex)
                                                {
                                                    Debug.Log($"Error at Predator fall back loop, error: {ex}");
                                                }
                                            }
                                            else
                                            {
                                                Debug.Log($"Could not move predator to {rowsBackup + howToMove(rowsBackup, colsBackup)[0]}, {colsBackup + howToMove(rowsBackup, colsBackup)[1]}. Entity occupying was {entityNeighbours[rowsBackup, colsBackup]}");
                                            }
                                        }
                                        else
                                        {
                                            Debug.Log($"Could not move predator to {rowsBackup + howToMove(rowsBackup, colsBackup)[0]}, {colsBackup + howToMove(rowsBackup, colsBackup)[1]}. Tile was {blockNeighbours[rowsBackup, colsBackup]}");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Debug.Log($"Skipped fall back option");
                            }

                            outsideFallBack:;
                            goto outsideTypeStatement;
                        }
                        else
                        {
                            Debug.Log($"No Entity to Move");
                        }

                        outsideTypeStatement:;

                    }
                    else
                    {
                        Debug.Log($"moveEntities tried to move null");
                    }
                }
            }
            entityGrid = newEntityGrid; //this is to stop the loop detecting the new entities and forever moving them
            return entityGrid;
        }

        public static object[,] reproduceEntities(string[,] grid, object[,] entityGrid) //if 2 Prey are next to each other and have requirements to reproduce, then have chance to actually reproduce
        {
            object[,] newEntityGrid = new object[entityGrid.GetLength(0), entityGrid.GetLength(1)];

            Prey[,] preyGrid = new Prey[entityGrid.GetLength(0), entityGrid.GetLength(1)];

            for (int row = 0; row < entityGrid.GetLength(0); row++)
            {
                for (int col = 0; col < entityGrid.GetLength(1); col++)
                {
                    if (entityGrid[row, col] != null && entityGrid[row, col].GetType() == typeof(Prey))
                    {
                        Prey p = (Prey) entityGrid[row, col];
                        object[,] pEntityNeighbours = p.getEntityNeighbours(entityGrid);
                        for (int rows = 0; rows < pEntityNeighbours.GetLength(0); rows++)
                        {
                            for (int cols = 0; cols < pEntityNeighbours.GetLength(1); cols++)
                            {
                                int[] rowscols = new int[2] { rows, cols };
                                int[] middle = new int[2] { 2, 2 };
                                if (pEntityNeighbours[rows, cols] != null && pEntityNeighbours[rows, cols].GetType() == typeof(Prey) && rowscols != middle)
                                {
                                    Prey mate = (Prey)pEntityNeighbours[rows, cols];
                                    if (p.canReproduce() && mate.canReproduce())
                                    {
                                        int reproductionChance = Math.Min(p.getReproductionProb(), mate.getReproductionProb());

                                        int chance = UnityEngine.Random.Range(0, 100);
                                        if (reproductionChance > chance)
                                        {
                                            //Energy
                                            p.updateNumOffSprings(1);
                                            mate.updateNumOffSprings(1);
                                            float newEnergy = (p.getEnergy() + mate.getEnergy()) / 3;
                                            p.updateEnergy(newEnergy);
                                            mate.updateEnergy(newEnergy);

                                            //add Prey to entityGrid at random position (can be next to parent or random pos)
                                            float foodLevel = UnityEngine.Random.Range(5f, 10f);
                                            float waterLevel = UnityEngine.Random.Range(5f, 10f);
                                            int maxOffsprings = UnityEngine.Random.Range(0, 6);
                                            int reproductionProb = UnityEngine.Random.Range(0, 100);
                                            int numOffsprings = 0;
                                            float minReproductionEnergy = UnityEngine.Random.Range(2f, 10f);
                                            string name = GridManager.names[UnityEngine.Random.Range(0, GridManager.names.Count + 1)];
                                            GridManager.names.Remove(name);
                                            int x = UnityEngine.Random.Range(0, rows);
                                            int y = UnityEngine.Random.Range(0, cols);
                                            entityGrid[x, y] = new Prey(newEnergy, foodLevel, waterLevel, maxOffsprings, reproductionProb, numOffsprings, minReproductionEnergy, name, x, y);
                                            Debug.Log($"Prey created at {x}, {y}. Name: {name}. Bred from {p.getName()}, {mate.getName()}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return entityGrid;
        }

        public static void changeEnergyEntities(string EnergyType, object entity, float foodLevel, float energyLevel, float waterLevel)
        {
            if (entity == null)
            {
                return;
            }
            else
            {
                switch (EnergyType.ToUpper())
                {
                    case "FOOD":
                        if (entity.GetType() == typeof(Prey))
                        {
                            Prey prey = (Prey)entity;
                            prey.updateFood(foodLevel);
                        }
                        else
                        {
                            Predator predator = (Predator)entity;
                            predator.updateFood(foodLevel);
                        }
                        break;
                    case "ENERGY":
                        if (entity.GetType() == typeof(Prey))
                        {
                            Prey prey = (Prey)entity;
                            prey.updateEnergy(energyLevel);
                        }
                        else
                        {
                            Predator predator = (Predator)entity;
                            predator.updateEnergy(energyLevel);
                        }
                        break;
                    case "WATER":
                        if (entity.GetType() == typeof(Prey))
                        {
                            Prey prey = (Prey)entity;
                            prey.updateWater(waterLevel);
                        }
                        else
                        {
                            Predator predator = (Predator)entity;
                            predator.updateWater(waterLevel);
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        public static object decreaseFoodLevel(object entity)
        {
            if (entity.GetType() == typeof(Prey))
            {
                Prey prey = (Prey)entity;
                prey.updateFood(foodDecreaseRate);
            }
            else
            {
                Predator predator = (Predator)entity;
                predator.updateFood(foodDecreaseRate);
            }
            return entity;
        }
        public static object decreaseEnergyLevel(object entity)
        {
            if (entity.GetType() == typeof(Prey))
            {
                Prey prey = (Prey)entity;
                prey.updateEnergy(energyDecreaseRate);
            }
            else
            {
                Predator predator = (Predator)entity;
                predator.updateEnergy(energyDecreaseRate);
            }
            return entity;
        }

        public static void feedEntities(object[,] entityGrid, float foodLevel)
        {
            for (int row = 0; row < entityGrid.GetLength(0); row++)
            {
                for (int col = 0; col < entityGrid.GetLength(1); col++)
                {
                    if (entityGrid[row, col] == null)
                    {
                        goto outside;
                    }
                    else
                    {
                        changeEnergyEntities("FOOD", entityGrid[row, col], foodLevel, 0, 0);
                    }
                    outside:;
                }
            }
        }

        public static object[,] killEntities(object[,] entityGrid)
        {
            object[,] newEntityGrid = new object[entityGrid.GetLength(0), entityGrid.GetLength(1)];

            for (int row = 0; row < entityGrid.GetLength(0); row++)
            {
                for (int col = 0; col < entityGrid.GetLength(1); col++)
                {
                    if (entityGrid[row, col] == null)
                    {
                        goto outside;
                    }
                    else
                    {
                        if (entityGrid[row, col].GetType() == typeof(Prey))
                        {
                            Prey prey = (Prey)entityGrid[row, col];
                            if (prey.isDead())
                            {
                                newEntityGrid[row, col] = null;
                            }
                            else
                            {
                                newEntityGrid[row, col] = entityGrid[row, col];
                            }
                        }
                        else
                        {
                            Predator predator = (Predator)entityGrid[row, col];
                            if (predator.isDead())
                            {
                                newEntityGrid[row, col] = null;
                            }
                            else
                            {
                                newEntityGrid[row, col] = entityGrid[row, col];
                            }
                        }
                    }
                    outside:;
                }
            }
            return newEntityGrid;
        }
    }
}
