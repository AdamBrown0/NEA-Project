using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Text.RegularExpressions;

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
     * 
     * Useful Info:
     * int rowsOrHeight = ary.GetLength(0); This would be a Y value
     * int colsOrWidth = ary.GetLength(1); This would be an X value
     */
    internal class NewSimulation
    {
        
        public void runSimulation()
        {

        }

    }

    internal class MoveEntities
    {
        // defining variables
        private static readonly GridManager MoveEntitiesGridManager = new GridManager();
        private readonly string[,] BaseBlockGrid = MoveEntitiesGridManager.grid;
        private readonly object[,] BaseEntityGrid = MoveEntitiesGridManager.entityGrid;
        private readonly GameObject[,] BaseGameObjectGrid = MoveEntitiesGridManager.gameObjectGrid;
        private string[,] tempBlockGrid;
        private object[,] tempEntityGrid;
        private GameObject[,] tempGameObjectGrid;
        private string[,] BlockNeighbours;
        internal MoveEntities()
        {
            tempBlockGrid = BaseBlockGrid;
            tempEntityGrid = BaseEntityGrid;
            tempGameObjectGrid = BaseGameObjectGrid;
            BlockNeighbours = new Prey().getBlockNeighbours(BaseBlockGrid); // since blocks never change, this can be made at the start
        }

        public object[,] Move()
        {
            //Loop through BaseBlockGrid
            for (int x = 0; x < BaseEntityGrid.GetLength(1); x++)
            {
                for (int y = 0; y < BaseEntityGrid.GetLength(0); y++)
                {
                    if (BaseEntityGrid[x, y] != null) // find entities from base grid to stop chance of moving entities twice
                    {
                        Type type = BaseEntityGrid[x, y].GetType(); // get the type of the location
                        // !!!! might need to change regex below !!!!
                        if (type == typeof(Prey) && new Regex("Prey [a-zA-Z]+", RegexOptions.IgnoreCase).IsMatch(BaseGameObjectGrid[x, y].name)) // check if it is equal to Prey, this means there is a Prey entity at x, y  regex check to see if the GameObject at x, y includes "Prey"
                        {

                        }
                        else if (type == typeof(Predator) && new Regex("Predator [a-zA-Z]+", RegexOptions.IgnoreCase).IsMatch(BaseGameObjectGrid[x, y].name)) // check if it is equal to Predator, this means there is a Prey entity at x, y  regex check to see if the GameObject at x, y includes "Predator"
                        {

                        }
                        else // this shouldn't happen, but is here to Debug, this means that there is no Prey or Predator at x, y but there is still an Object
                        {
                            Debug.Log($"Object detected at {x}, {y}. Object type is: {type}. Error: 0001");
                        }
                    }
                }
            }


        }
        
    }
}
