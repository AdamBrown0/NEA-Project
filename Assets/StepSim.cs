using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSim : MonoBehaviour
{
    public void StepSimulation()
    {
        Simulation sim = gameObject.AddComponent<Simulation>();
        sim.StepSimulation();
    }
}
