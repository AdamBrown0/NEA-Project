using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class AutoRunToggle : MonoBehaviour
{
    public void autoRunToggle()
    {
        Simulation sim = new Simulation();
        sim.AutoRun = !sim.AutoRun;
    }

    // variable to track whether the simulation is currently running automatically
    bool autoRun = false;

    // method to handle the toggle button's OnValueChanged event
    public void OnAutoRunToggleChanged(bool value)
    {
        Simulation sim = new Simulation();
        // update the autoRun variable
        autoRun = value;

        // if the simulation should run automatically
        if (autoRun)
        {
            // start the simulation running automatically with a delay of 0.5 seconds between steps
            StartCoroutine(sim.PerformSimulationStep(0.5f));
        }
        else
        {
            // stop the simulation from running automatically
            StopCoroutine(sim.PerformSimulationStep);
        }
    }
}
