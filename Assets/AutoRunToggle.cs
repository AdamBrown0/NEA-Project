using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class AutoRunToggle : MonoBehaviour
{
    private bool on = false;
    private bool off = true;

    private void autoRunToggle()
    {
        if (on)
        {
            Simulation.autoRun = true;
        }
        else if (off)
        {
            Simulation.autoRun = false;
        }
        else
        {
            Debug.Log($"Error at autoRunToggle(), on: {on} off: {off}");
        }
        Debug.Log($"AutoRun set to {Simulation.autoRun}");
    }
    public void onButtonClicked()
    {
        on = true;
        off = false;
        autoRunToggle();
    }
    public void offButtonClicked()
    {
        on = false;
        off = true;
        autoRunToggle();
    }
}
