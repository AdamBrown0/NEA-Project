using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using UnityEditorInternal.Profiling;
using UnityEditor;
using UnityEngine.Rendering;

namespace Assets.SimulationStuff
{
    /*
     * Simulation Needs:
     * 
     * Moving Entities to new space;
     *  Requirements:
     *      string[,] GridManager.grid <-- base block grid, used for checking whether space to move to is valid (grass)
     *      object[,] GridManager.entityGrid <-- base entity grid, used for finding entities as described below
     *      GameObject[,] GridManager.gameObjectGrid <-- base GameObject version of entity grid, mostly optional, could be used to check further if an entity exists
     *      Entity.getBlockNeighbours(string[,] blockGrid) <-- sub routine to find the blocks surrounding an entity, linked to a specific entity so doesnt need position passed
     *      Entity.getEntityNeighbours(object[,] entityGrid) <-- sub routine to find the entities surround an entity, linked to a specific entity so doesnt need position passed
     * 
     *  Create temporary entityGrid which is a copy of GridManager.entityGrid, this will be used for all changes made in the MoveEntities method
     *  
     *  Loop through GridManager.entityGrid (MUST be GridManager.entityGrid otherwise may move entities more than once and end up with errors) (now using a readonly variable), check if [x,y] is an Object (check against GridManager.gameObjectGrid to further check if needed)
     *  2 random numbers (0-2) to choose space on 3x3 grid (subject to change) (could use EntityNeighbours array to check)
     *  Check if space is already occupied (use entityGrid)
     *      If space is already occupied and Prey entity is trying to move to it, block that space from being moved to and try another space
     *      If space is already occupied and Predator entity is trying to move to it, check if space is occupied by Prey
     *          If space is occupied by Prey, delete Prey entity from entityGrid (possibly also delete GameObject), and move Predator to that space, update all necessary values
     *          If space is occupied by Predator, block that space from being moved to and try another space
     *  Update GridManager.entityGrid to this new entityGrid 
     * 
     * Reproducing Prey;
     *  Requirements:
     *      object[,] entityGrid <-- needs to find entity neighbours
     *      object[,] Entity.getEntityNeighbours(object[,] entityGrid) <-- needs to check if prey is within range for reproducing
     *
     * Create temporary entityGrid which is a copy of GridManager.entityGrid (makes it so it cant catch a new Prey), this is returned to update the main entityGrid
     *
     * Loop through tempEntityGrid, if there is an Entity check if it is Prey, if it is Prey check entityNeighbours, if entityNeighbours shows Prey then take average reproductionChance
     * compare average reproductionChance against random number generated, if bigger then reproduce and place next to one of parents
     * 
     * Useful Info:
     * int rowsOrHeight = ary.GetLength(0); This would be a Y value
     * int colsOrWidth = ary.GetLength(1); This would be an X value
     * 
     * Array2D[rows, cols] => Array2D[y, x] since rows is height
     * 
     */
    internal class NewSimulation
    {
        public static string[,] BaseBlockGrid;
        public static object[,] BaseEntityGrid;
        public void runSimulation()
        {
            MoveEntities moveEntities = new MoveEntities(BaseBlockGrid, BaseEntityGrid);
            Simulation.entityGrid = moveEntities.Move(); // move entities working!
            Debug.Log("NEWSIM MOVE DONE");
            ReproduceEntities reproduceEntities = new ReproduceEntities(BaseBlockGrid, BaseEntityGrid);
            Simulation.entityGrid = reproduceEntities.Reproduce();
            Debug.Log("NEWSIM REPRODUCTION DONE");
        }

    }

    internal class MoveEntities
    {
        // defining variables
        private GridManager MoveEntitiesGridManager = new GridManager();
        public static string[,] BaseBlockGrid;
        public static object[,] BaseEntityGrid;
        private object[,] tempEntityGrid; //= new object[BaseEntityGrid.GetLength(0), BaseEntityGrid.GetLength(1)];
        //private GameObject[,] tempGameObjectGrid;
        internal MoveEntities(string[,] baseBlockGrid, object[,] baseEntityGrid)
        {
            BaseBlockGrid = baseBlockGrid;
            BaseEntityGrid = baseEntityGrid;
            tempEntityGrid = BaseEntityGrid;
        }

        public object[,] Move()
        {
            string str = null;
            str = null;
            for (int x = 0; x < tempEntityGrid.GetLength(0); x++)
            {
                for (int y = 0; y < tempEntityGrid.GetLength(1); y++)
                {
                    if (tempEntityGrid[x, y] != null)
                    {
                        str += (tempEntityGrid[x, y].GetType().ToString()) + " ";
                    }
                    else
                    {
                        str += "null ";
                    }
                }
                str += "\n";
            }
            str += "\n222";
            Debug.Log(str);
            //Loop through BaseBlockGrid
            for (int x = 0; x < BaseEntityGrid.GetLength(0); x++)
            {
                for (int y = 0; y < BaseEntityGrid.GetLength(1); y++)
                {
                    //str += y.ToString() + " ";

                    

                    if (BaseEntityGrid[x, y] != null) // find entities from base grid to stop chance of moving entities twice
                    {
                        
                        Type type = BaseEntityGrid[x, y].GetType(); // get the type of the location
                        if (type == typeof(Prey)) // && Regex.IsMatch(BaseGameObjectGrid[x, y].name, "Prey [A-Za-z0-9]+", RegexOptions.IgnoreCase)) // check if it is equal to Prey, this means there is a Prey entity at x, y  regex check to see if the GameObject at x, y includes "Prey"
                        {
                            Prey prey = (Prey)BaseEntityGrid[x, y];
                            prey.setPos(x, y);
                            string[,] PreyBlockNeighbours = prey.getBlockNeighbours(BaseBlockGrid); // THIS NEVER UPDATES
                            object[,] PreyEntityNeighbours = prey.getEntityNeighbours(tempEntityGrid);

                            bool moved = false;
                            int moveTries = 0;
                            while (!moved && moveTries <= 9)
                            {
                                int[] checkMoveReturn = checkMove(PreyBlockNeighbours, PreyEntityNeighbours, false);
                                int moveToX = x + checkMoveReturn[1];
                                int moveToY = y + checkMoveReturn[0];
                                if ((moveToX <= (BaseBlockGrid.GetLength(0) - 1) && moveToX > 0) && (moveToY <= (BaseBlockGrid.GetLength(1) - 1) && moveToY > 0))
                                {
                                    tempEntityGrid[x, y] = null;
                                    tempEntityGrid[moveToX, moveToY] = prey;
                                    moved = true;
                                    Debug.Log($"Moved to {moveToX} {moveToY}, moveTries {moveTries}, moved {moved}");
                                }
                                else
                                {
                                    Debug.Log($"Caught IndexOutOfRangeException at Prey section, trying to move again, tried to move to {moveToX} {moveToY}, moveTries {moveTries}, moved {moved}, BaseBlockGrid.GetLength {BaseBlockGrid.GetLength(0) - 1} {BaseBlockGrid.GetLength(1) - 1}");
                                    moveTries++;
                                }
                                //try
                                //{
                                //    tempEntityGrid[moveToX, moveToY] = prey;
                                //    tempEntityGrid[x, y] = null;
                                //    moved = true;
                                //    Debug.Log($"Moved to {moveToX} {moveToY}, moveTries {moveTries}, moved {moved}");
                                //}
                                //catch (IndexOutOfRangeException)
                                //{
                                //    Debug.Log($"Caught IndexOutOfRangeException at Prey section, trying to move again, tried to move to {moveToX} {moveToY}, moveTries {moveTries}, moved {moved}");
                                //    moveTries++;
                                //}
                            }
                            
                        }
                        else if (type == typeof(Predator)) //&& Regex.IsMatch(BaseGameObjectGrid[x, y].name, "Predator [A-Za-z0-9]+", RegexOptions.IgnoreCase)) // check if it is equal to Predator, this means there is a Prey entity at x, y  regex check to see if the GameObject at x, y includes "Predator"
                        {
                            Predator predator = (Predator)BaseEntityGrid[x, y];
                            predator.setPos(x, y);
                            string[,] PredatorBlockNeighbours = predator.getBlockNeighbours(BaseBlockGrid); 
                            string bn = "";
                            for (int xi = 0; xi < PredatorBlockNeighbours.GetLength(0); xi++)
                            {
                                for (int yi = 0; yi < PredatorBlockNeighbours.GetLength(1); yi++)
                                {
                                    bn += $"{PredatorBlockNeighbours[yi, xi]}";
                                }

                                bn += "\n";
                            }
                            Debug.Log($"BLOCKNEIGHBOURS222 \n {bn} x: {x} y: {y}");
                            object[,] PredatorEntityNeighbours = predator.getEntityNeighbours(tempEntityGrid);

                            bool moved = false;
                            int moveTries = 0;
                            while (!moved && moveTries <= 9)
                            {
                                int[] checkMoveReturn = checkMove(PredatorBlockNeighbours, PredatorEntityNeighbours, true);
                                int moveToX = x + checkMoveReturn[1];
                                int moveToY = y + checkMoveReturn[0];
                                if ((moveToX <= (BaseBlockGrid.GetLength(0) - 1) && moveToX > 0) && (moveToY <= (BaseBlockGrid.GetLength(1) - 1) && moveToY > 0))
                                {
                                    tempEntityGrid[x, y] = null;
                                    tempEntityGrid[moveToX, moveToY] = predator;
                                    moved = true;
                                    Debug.Log($"Moved to {moveToX} {moveToY}, moveTries {moveTries}, moved {moved}");
                                }
                                else
                                {
                                    Debug.Log($"Caught IndexOutOfRangeException at Predator section, trying to move again, tried to move to {moveToX} {moveToY}, moveTries {moveTries}, moved {moved}, BaseBlockGrid.GetLength {BaseBlockGrid.GetLength(0) - 1} {BaseBlockGrid.GetLength(1) - 1}");
                                    moveTries++;
                                }
                            }

                            predator = null;
                        }
                        else // this shouldn't happen, but is here to Debug, this means that there is no Prey or Predator at x, y but there is still an Object
                        {
                            Debug.Log($"Object detected at {y}, {x}. Object type is: {type}. Error: 0001");
                        }
                    }
                    //if (tempEntityGrid[x, y] != null)
                    //{
                    //    str += (tempEntityGrid[x, y].GetType().ToString()) + " ";
                    //    //Console.Write($"{BaseEntityGrid[x, y].GetType().ToString()} ");
                    //}
                    //else
                    //{
                    //    str += "null ";
                    //    //Console.Write("null ");
                    //}
                    //try
                    //{
                    //    str += (tempEntityGrid[x, y].GetType().ToString()) + " ";
                    //}
                    //catch (Exception)
                    //{
                    //    str += "null ";
                    //}
                    //Console.Write("\n");
                }
                //str += "\n";
            }
            //Debug.Log(str);

            str = null;
            for (int x = 0; x < tempEntityGrid.GetLength(0); x++)
            {
                for (int y = 0; y < tempEntityGrid.GetLength(1); y++)
                {
                    if (tempEntityGrid[x, y] != null)
                    {
                        str += (tempEntityGrid[x, y].GetType().ToString()) + " ";
                    }
                    else
                    {
                        str += "null ";
                    }
                }
                str += "\n";
            }
            str += "\n111";
            Debug.Log(str);
        
            return tempEntityGrid;
        }

        // debug
        private int[] checkMove(string[,] blockNeighbours, object[,] entityNeighbours, bool isPred)
        {
            string bn = "";
            for (int x = 0; x < blockNeighbours.GetLength(0); x++)
            {
                for (int y = 0; y < blockNeighbours.GetLength(1); y++)
                {
                    bn += $"{blockNeighbours[y, x]}";
                }

                bn += "\n";
            }
            Debug.Log($"BLOCKNEIGHBOURS \n {bn}");
            bool done = false;
            int[] possibleMove = new int[2];
            bool possibleMoveFound = false;
            int predatorMoveTries = 0; // allows predator to try find a Prey instead of moving to completely random space
            int[] move = new int[2];
            int moveTries = 0;
            if (isSurrounded(blockNeighbours))
            {
                return new int[2] { 0, 0 };
            }
            while (!done)
            {
                skipTo:

                int randomX = RandomNumberGenerator.GetInt32(0, 3); // from inclusive to exclusive
                int randomY = RandomNumberGenerator.GetInt32(0, 3);
                if (blockNeighbours[randomX, randomY] != "G" && predatorMoveTries <= 9 && moveTries <= 9) // returns null if tile is not Grass
                {
                    if (isPred)
                    {
                        predatorMoveTries++;
                    }
                    goto skipTo;
                }
                if (entityNeighbours[randomX, randomY] != null && !isPred) // returns null if trying to move to space with 
                {
                    if (isPred)
                    {
                        goto isPredIf;
                    }
                    goto skipTo;
                }
                isPredIf:
                if (isPred) // predator
                {
                    if (!possibleMoveFound)
                    {
                        int x = translateMove(randomX, randomY)[0];
                        int y = translateMove(randomX, randomY)[1];
                        possibleMove[0] = x; possibleMove[1] = y; // if it cannot find a Prey to eat, will rely back on this
                        possibleMoveFound = true;
                    }
                    if (predatorMoveTries > 9)
                    {
                        move = possibleMove; // if the predator cannot find a Prey, it will just return a possible move
                        done = true;
                    }
                    else
                    {
                        if (entityNeighbours[randomX, randomY] != null)
                        {
                            if (entityNeighbours[randomX, randomY].GetType() == typeof(Prey)) // finds Prey entity
                            {
                                int x = translateMove(randomX, randomY)[0];
                                int y = translateMove(randomX, randomY)[1];
                                move[0] = x; move[1] = y;
                                done = true;
                            }
                            else
                            {
                                predatorMoveTries++;
                            }
                        }
                        else
                        {
                            predatorMoveTries++;
                        }
                    }
                }
                else if (!isPred) // prey
                {
                    if (entityNeighbours[randomX, randomX] == null)
                    {
                        int x = translateMove(randomX, randomY)[0];
                        int y = translateMove(randomX, randomY)[1];
                        move[0] = x; move[1] = y;
                        done = true;
                    }
                }
                else // should never get here, just here for error handling
                {
                    Debug.Log($"Error at checkMove isPred section, isPred: {isPred}");
                }

                if (move[0] > BaseBlockGrid.GetLength(0) - 1 || move[1] > BaseBlockGrid.GetLength(1) - 1 || move[0] < 0 || move[1] < 0) // this should stop entities trying to move outside of the grid
                {
                    done = false;
                }
                moveTries++;

                if (moveTries > 9)
                {
                    Debug.Log($"Relied on backup move");
                    move = backupMove(blockNeighbours, entityNeighbours);
                    done = true;
                }

            }
            return move; // returns how to move, but returns 1 to the left of where it is supposed to be .... OR the displaying is not correct .... its not that its displaying the wrong thing, it displays before it moves. 
        }

        private int[] backupMove(string[,] blockNeighbours, object[,] entityNeighbours)
        {
            int[] move = new int[2];
            for (int x = 0; x < blockNeighbours.GetLength(0); x++)
            {
                for (int y = 0; y < blockNeighbours.GetLength(1); y++)
                {
                    if (entityNeighbours[x, y] == null)
                    {
                        if (blockNeighbours[x, y] == "G")
                        {
                            int xi = translateMove(x, y)[0];
                            int yi = translateMove(x, y)[1];
                            move[0] = xi; move[1] = yi;
                            goto endOfBackup;
                        }
                    }
                }
            }
            endOfBackup:
            return move;
        }

        private bool isSurrounded(string[,] blockNeighbours) // makes sure entity isnt surrounded to prevent infinite loops
        {
            int count = 0;
            for (int x = 0; x < blockNeighbours.GetLength(0); x++)
            {
                for (int y = 0; y < blockNeighbours.GetLength(1); y++)
                {
                    if (blockNeighbours[x, y] != "G")
                    {
                        count++;
                    }
                }
            }
            if (count == 8)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal int[] translateMove(int x, int y) // takes in the randomX and randomY and outputs how to get to that location
        {
            if (x == 0 && y == 0)
            {
                return new int[2] { -1, -1 };
            }
            else if (x == 0 && y == 1)
            {
                return new int[2] { -1, 0 };
            }
            else if (x == 0 && y == 2)
            {
                return new int[2] { -1, 1 };
            }
            else if (x == 1 && y == 0)
            {
                return new int[2] { 0, -1 };
            }
            else if (x == 1 && y == 1)
            {
                return new int[2] { 0, 0 };
            }
            else if (x == 1 && y == 2)
            {
                return new int[2] { 0, 1 };
            }
            else if (x == 2 && y == 0)
            {
                return new int[2] { 1, -1 };
            }
            else if (x == 2 && y == 1)
            {
                return new int[2] { 1, 0 };
            }
            else if (x == 2 && y == 2)
            {
                return new int[2] { 1, 1 };
            }
            else
            {
                Debug.Log($"Error at translateMove() function, x or y value passed were not in range");
                return null;
            }
        }
    }

    internal class ReproduceEntities
    {
        public static string[,] BaseBlockGrid;
        public static object[,] BaseEntityGrid;
        private object[,] tempEntityGrid;

        internal ReproduceEntities(string[,] baseBlockGrid, object[,] baseEntityGrid)
        {
            BaseBlockGrid = baseBlockGrid;
            BaseEntityGrid = baseEntityGrid;
            tempEntityGrid = BaseEntityGrid;
        }

        public object[,] Reproduce()
        {
            for (int x = 0; x < BaseEntityGrid.GetLength(0); x++)
            {
                for (int y = 0; y < BaseEntityGrid.GetLength(1); y++)
                {
                    if (BaseEntityGrid[x, y] == null)
                    {
                        continue;
                    }

                    Type type = BaseEntityGrid[x, y].GetType();
                    if (type == typeof(Prey))
                    {
                        bool offspringSpawned = false;
                        Prey prey = (Prey)BaseEntityGrid[x, y];
                        object[,] PreyEntityNeighbours = prey.getEntityNeighbours(BaseEntityGrid);
                        for (int xi = 0; xi < PreyEntityNeighbours.GetLength(0); xi++)
                        {
                            if (offspringSpawned)
                            {
                                continue;
                            }
                            for (int yi = 0; yi < PreyEntityNeighbours.GetLength(1); yi++)
                            {
                                if (PreyEntityNeighbours[xi, yi] == null || offspringSpawned)
                                {
                                    continue;
                                }

                                Type typeInside = PreyEntityNeighbours[xi, yi].GetType();
                                if (typeInside == typeof(Prey))
                                {
                                    Prey otherPrey = (Prey)PreyEntityNeighbours[xi, yi];
                                    if (prey.getName() == otherPrey.getName())
                                    {
                                        Debug.Log("Passing reproduction due to same entities");
                                        continue;
                                    }
                                    int basePreyRepChance = prey.getReproductionProb();
                                    int otherPreyRepChance = otherPrey.getReproductionProb();
                                    int averageRepChance = (basePreyRepChance + otherPreyRepChance) / 2;
                                    int repChance = UnityEngine.Random.Range(0, 100);
                                    if (averageRepChance >= repChance && prey.getMaxOffsprings() > prey.getNumOffsprings() && otherPrey.getMaxOffsprings() > otherPrey.getNumOffsprings())
                                    {
                                        float energyLevel = UnityEngine.Random.Range(5f, 10f);
                                        float foodLevel = UnityEngine.Random.Range(5f, 10f);
                                        float waterLevel = UnityEngine.Random.Range(5f, 10f);
                                        int maxOffsprings = UnityEngine.Random.Range(1, 3);
                                        int reproductionProb = UnityEngine.Random.Range(0, 100);
                                        reproductionProb = 100;
                                        int numOffsprings = 0;
                                        float minReproductionEnergy = UnityEngine.Random.Range(2f, 10f);
                                        string name = GridManager.names[UnityEngine.Random.Range(0, GridManager.names.Count + 1)];
                                        GridManager.names.Remove(name);
                                        string[,] BasePreyBlockNeighbours = prey.getBlockNeighbours(BaseBlockGrid); // this allows us to find a valid spawn location for the new prey
                                        int xPrey;
                                        int yPrey;
                                        int spawnTries = 0;
                                        for (int xb = 0; xb < BasePreyBlockNeighbours.GetLength(0); xb++)
                                        {
                                            if (offspringSpawned || spawnTries > 9)
                                            {
                                                continue;
                                            }
                                            for (int yb = 0; yb < BasePreyBlockNeighbours.GetLength(1); yb++)
                                            {
                                                if (offspringSpawned || spawnTries > 9)
                                                {
                                                    continue;
                                                }
                                                if (BasePreyBlockNeighbours[xb, yb] == "G" && PreyEntityNeighbours[xb, yb] == null ) // checks if block is Grass AND parent does not occupy it
                                                {
                                                    MoveEntities moveEntities = new MoveEntities(null, null);
                                                    int[] whereToSpawn = moveEntities.translateMove(xb, yb);
                                                    xPrey = x + whereToSpawn[1];
                                                    yPrey = y + whereToSpawn[0];
                                                    if ((xPrey <= (BaseBlockGrid.GetLength(0) - 1) && xPrey > 0) &&
                                                        (yPrey <= (BaseBlockGrid.GetLength(1) - 1) && yPrey > 0))
                                                    {
                                                        if (BaseBlockGrid[prey.getPos()[0] + whereToSpawn[1], prey.getPos()[1] + whereToSpawn[0]] == "G")
                                                        {
                                                            tempEntityGrid[xPrey, yPrey] = new Prey(energyLevel, foodLevel, waterLevel, maxOffsprings, reproductionProb, numOffsprings, minReproductionEnergy, name, xPrey, yPrey);
                                                            Debug.Log($"Prey created at {x}, {y}. Name: {name}. Bred by {prey.getName()} {otherPrey.getName()}");
                                                            offspringSpawned = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        spawnTries++;
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

            return tempEntityGrid;
        }
    }

    internal class ChangeEntityValues
    {
        
    }
}
