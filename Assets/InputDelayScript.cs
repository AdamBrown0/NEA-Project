using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputDelayScript : MonoBehaviour
{
    public void input(InputField _input)
    {
        Simulation.stepDelay = float.Parse(_input.text);
        Debug.Log($"Step Delay set to: {Simulation.stepDelay}");
    }
}
