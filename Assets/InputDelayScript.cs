using System;
using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputDelayScript : MonoBehaviour
{
    private InputField inputField;

    private void Start()
    {
        inputField = GameObject.Find("InputDelayField").GetComponent<InputField>();
    }

    public void input(InputField _input)
    {
        try
        {
            Simulation.stepDelay = float.Parse(_input.text);
            inputField.placeholder.GetComponent<Text>().text = "Enter delay (s)";
            inputField.placeholder.GetComponent<Text>().color = Color.black;
        }
        catch (FormatException e)
        {
            inputField.text = "";
            inputField.placeholder.GetComponent<Text>().text = "Invalid Input";
            inputField.placeholder.GetComponent<Text>().color = Color.red;
            Debug.Log("User entered non-float value into Delay input field, " + e);
            throw;
        }
        Debug.Log($"Step Delay set to: {Simulation.stepDelay}");
    }
}
