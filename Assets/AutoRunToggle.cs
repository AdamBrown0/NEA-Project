using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class AutoRunToggle : MonoBehaviour
{
    // Create a static instance of the script
    private static AutoRunToggle instance;
    private Simulation sim;

    void Awake()
    {
        sim = gameObject.GetComponent<Simulation>();
        // Set the static instance to the current instance of the script
        instance = this;
    }
    public void autoRunToggle()
    {
        sim.autoRun = !sim.autoRun;
        Debug.Log($"AutoRun set to {sim.autoRun}");
    }
    public void OnButtonClick()
    {
        autoRunToggle();
    }
}
