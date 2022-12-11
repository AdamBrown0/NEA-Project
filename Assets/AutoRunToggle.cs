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
            Simulation.isRunning = false;
        }
        else
        {
            Debug.Log($"Error at autoRunToggle(), on: {on} off: {off}");
        }
        Debug.Log($"AutoRun set to {Simulation.autoRun}");
    }
    public void onButtonClicked()
    {
        Button button = GameObject.Find("AutoRunOn").GetComponent<Button>();
        button.enabled = false;
        button.image.color = new Color32(0, 197, 18, 255);
        Button buttonOff = GameObject.Find("AutoRunOff").GetComponent<Button>();
        buttonOff.enabled = true;
        buttonOff.image.color = Color.clear;
        on = true;
        off = false;
        autoRunToggle();
    }
    public void offButtonClicked()
    {
        Button button = GameObject.Find("AutoRunOff").GetComponent<Button>();
        button.enabled = false;
        button.image.color = new Color32(255, 38, 0, 255);
        Button buttonOn = GameObject.Find("AutoRunOn").GetComponent<Button>();
        buttonOn.enabled = true;
        buttonOn.image.color = Color.clear;
        on = false;
        off = true;
        autoRunToggle();
    }
}
