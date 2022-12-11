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
        public static int StepCount = 0;
        public static float stepDelay = 5f;
        public static bool isRunning = false;

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
            if (autoRun && !isRunning)
            {
                isRunning = true;
                stepDelay = stepDelay;
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

                // Wait for the set amount of time before running the next iteration
                yield return new WaitForSeconds(stepDelay);
            }
        }
        public static void StepSimulation()
        {
            StepCount++;
            StepCounterText.GetComponent<Text>().text = $"Simulation Step: {StepCount.ToString()}";
        }
    }
}
