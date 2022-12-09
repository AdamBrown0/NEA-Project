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
    private int timesClicked = 0;

    public int steps = 0;

    void Awake()
    {
        //sim = gameObject.GetComponent<Simulation>();
    }

    public void autoRunToggle()
    {
        Debug.Log($"timeClicked: {timesClicked}");
        if (timesClicked % 2 == 0)
        {
            //GUI.color = Color.clear;
            //GetComponent<Image>().color = Color.clear;
        }
        else
        {
            //GUI.color = new Color32(255, 142, 0, 100);
            GetComponent<Image>().color = new Color32(255, 142, 0, 100);
        }
        Simulation.autoRun = !Simulation.autoRun;
        Debug.Log($"AutoRun set to {Simulation.autoRun}");
        timesClicked++;
    }

    public void OnButtonClick()
    {
        autoRunToggle();
    }
    private void Update()
    {
        steps = Simulation.StepCount;
    }
}
