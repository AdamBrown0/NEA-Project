using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class AutoRunToggle : MonoBehaviour
{
    private Simulation sim;

    private bool hasBeenClicked = false;

    void Awake()
    {
        //sim = gameObject.GetComponent<Simulation>();
    }

    public void autoRunToggle()
    {
        if (!hasBeenClicked)
        {
            Simulation.autoRun = !Simulation.autoRun;
            Debug.Log($"AutoRun set to {Simulation.autoRun}");
            hasBeenClicked = true;
        }
    }

    public void OnButtonClick()
    {
        autoRunToggle();
    }
}
