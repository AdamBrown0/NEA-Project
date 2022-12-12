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

        private static int[] howToMove(int x, int y)
        {
            int[] move = new int[2]; //switching pos 0 for pos 1 to see if work
            switch (x)
            {
                case 0:
                    move[0] = -1;
                    switch (y)
                    {
                        case 0:
                            move[1] = -1;
                            break;
                        case 1:
                            move[1] = 0;
                            break;
                        case 2:
                            move[1] = 1;
                            break;
                    }
                    break;
                case 1:
                    move[0] = 0;
                    switch (y)
                    {
                        case 0:
                            move[1] = -1;
                            break;
                        case 1:
                            move[1] = 0;
                            break;
                        case 2:
                            move[1] = 1;
                            break;
                    }
                    break;
                case 2:
                    move[0] = 1;
                    switch (y)
                    {
                        case 0:
                            move[1] = -1;
                            break;
                        case 1:
                            move[1] = 0;
                            break;
                        case 2:
                            move[1] = 1;
                            break;
                    }
                    break;
                default:
                    Debug.Log($"howToMove gone wrong, MOVE: {move[0]}, {move[1]}");
                    break;
            }
            Debug.Log($"RETURN: {move[1]}, {move[0]}");
            return move;
        }

        public static object[,] moveEntities(string[,] grid, object[,] entityGrid) //move entities by move distance, grid and entityGrid passed to make possible
        {
            object[,] newEntityGrid = new object[entityGrid.GetLength(0), entityGrid.GetLength(1)]; //this is needed to stop the for loop detecting moved entities and causing infinite loop

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
                                int randomX = UnityEngine.Random.Range(0, blockNeighbours.GetLength(0));
                                int randomY = UnityEngine.Random.Range(0, blockNeighbours.GetLength(1));
                                if (blockNeighbours[randomX, randomY] == "G") //checks if random spot is Grass (G) block
                                {
                                    if (entityNeighbours[randomX, randomY] == null) //if block is Grass, then check if block is not currently occupied by another entity
                                    {
                                        //move prey to new location and delete old
                                        try
                                        {
                                            newEntityGrid[rows + howToMove(randomX, randomY)[0], cols + howToMove(randomX, randomY)[1]] = prey;
                                            Debug.Log($"Prey moved to {rows + howToMove(randomX, randomY)[0]}, {cols + howToMove(randomX, randomY)[1]}\nTile was {blockNeighbours[randomX, randomY]}");
                                            done = true;
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
                                        if (blockNeighbours[rowsBackup, colsBackup] == "G")
                                        {
                                            if (entityNeighbours[rowsBackup, colsBackup] == null)
                                            {
                                                //move prey to new location and delete old
                                                try
                                                {
                                                    newEntityGrid[rowsBackup + howToMove(rowsBackup, colsBackup)[0], colsBackup + howToMove(rowsBackup, colsBackup)[1]] = prey;
                                                    Debug.Log($"Prey moved to {rowsBackup + howToMove(rowsBackup, colsBackup)[0]}, {colsBackup + howToMove(rowsBackup, colsBackup)[1]}\nRelied on fall back option\n Tile was {blockNeighbours[rowsBackup , colsBackup]}");
                                                    done = true;
                                                    break;
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
                                Debug.Break();
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
                                int randomX = UnityEngine.Random.Range(0, blockNeighbours.GetLength(0));
                                int randomY = UnityEngine.Random.Range(0, blockNeighbours.GetLength(1));
                                if (blockNeighbours[randomX, randomY] == "G") //checks if random spot is Grass (G) block
                                {
                                    Type typePred;
                                    if (entityNeighbours[randomX, randomY] != null)
                                    {
                                        typePred = entityNeighbours[randomX, randomY].GetType();
                                        if (entityNeighbours[randomX, randomY].GetType() == typeof(Prey))
                                        {
                                            try
                                            {
                                                newEntityGrid[rows + howToMove(randomX, randomY)[0], cols + howToMove(randomX, randomY)[1]] = predator;
                                                Debug.Log($"Predator moved to {rows + howToMove(randomX, randomY)[0]}, {cols + howToMove(randomX, randomY)[1]}\nPredator has eaten Prey\n{blockNeighbours[randomX, randomY]}");
                                                done = true;
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
                                            try
                                            {
                                                newEntityGrid[rows + howToMove(randomX, randomY)[0], cols + howToMove(randomX, randomY)[1]] = predator;
                                                Debug.Log($"Predator moved to {rows + howToMove(randomX, randomY)[0]}, {cols + howToMove(randomX, randomY)[1]}\nTile is {blockNeighbours[randomX, randomY]}");
                                                done = true;
                                            }
                                            catch (Exception ex)
                                            {
                                                Debug.Log($"Error at Predator while loop, error: {ex}");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Debug.Log($"entityNeighbours was default at Predator");
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
                                        if (blockNeighbours[rowsBackup, colsBackup] == "G")
                                        {
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
                                                    break;
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
                        }
                        else
                        {
                            Debug.Log($"No Entity to Move");
                        }
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
        }

    }
}
