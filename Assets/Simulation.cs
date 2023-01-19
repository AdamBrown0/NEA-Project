using Assets.SimulationStuff;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class Simulation : MonoBehaviour
    {
        
        private static Text StepCounterText;
        public static bool autoRun;
        [SerializeField] public static int StepCount;
        public static float stepDelay = 5f;
        public static bool isRunning = false;

        public static string[,] grid;
        public static object[,] entityGrid;

        private void Awake()
        {
            autoRun = false;
        }
        void Start()
        {
            StepCounterText = GameObject.Find("StepCounterText").GetComponent<Text>();
        }
        
        void Update()
        {
            if (autoRun && !isRunning) //!isRunning stops the coroutine from starting again if already running.
            {
                isRunning = true;
                stepDelay = stepDelay; //to try update stepDelay if InputField changes it. 
                StartCoroutine(AutoRunSimulation(stepDelay));
            }
            
        }
        public IEnumerator AutoRunSimulation(float stepDelay)
        {
            // This is the loop that will run the simulation automatically
            while (autoRun)
            {
                // Run the simulation code here
                StepSimulation();
                StepCount++;
                StepCounterText.GetComponent<Text>().text = $"Simulation Step: {StepCount.ToString()}";

                // Wait for the set amount of time before running the next iteration
                yield return new WaitForSeconds(stepDelay);
            }
        }
        public static void StepSimulation()
        {
            ////get newEntityGrid after moves
            //entityGrid = SimulationStuff.MainSimulation.moveEntities(grid, entityGrid);
            ////reproduce entities if possible
            //entityGrid = SimulationStuff.MainSimulation.reproduceEntities(grid, entityGrid);
            ////feed entities
            //SimulationStuff.MainSimulation.feedEntities(entityGrid, 1f); //can get input from user in future
            ////kill entities
            //SimulationStuff.MainSimulation.killEntities(entityGrid);

            //NEW SIMULATION
            NewSimulation sim = new NewSimulation();
            sim.runSimulation();


            //display changes
            GameObject gridManagerObject = GameObject.Find("GridHolder");
            GridManager gridManager = gridManagerObject.GetComponent<GridManager>();
            gridManager.entityGrid = entityGrid;
            gridManager.displayEntities(entityGrid);
            gridManager.updateNewSim();
            StepCount++;
            StepCounterText.GetComponent<Text>().text = $"Simulation Step: {StepCount.ToString()}";
        }
    }
}
