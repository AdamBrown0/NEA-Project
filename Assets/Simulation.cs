﻿using System;
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
        private static GameObject StepCounter = GameObject.Find("StepCounter");
        

        void Start()
        {
            StepCounter.SetActive(true);
            StepCounter.transform.SetParent(GridManager.GridManagerTransform);
            StepCounter.transform.localPosition = new Vector2(-1f, -1f);
        }
        public int StepCount = 0;
        public bool autoRun = false;
        void Update()
        {
            // Check if the autoRun is on
            if (autoRun)
            {
                // Start the Coroutine
                StartCoroutine(AutoRunSimulation(0.5f));
            }
        }
        public IEnumerator AutoRunSimulation(float stepDelay)
        {
            // This is the loop that will run the simulation automatically
            while (autoRun)
            {
                // Run the simulation code here
                StepCount++;
                StepCounter.GetComponent<Text>().text = $"Step: {StepCount.ToString()}";

                // Wait for the set amount of time before running the next iteration
                yield return new WaitForSeconds(stepDelay);
            }
        }
    }
}
