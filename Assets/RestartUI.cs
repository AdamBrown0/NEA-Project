using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartUI : MonoBehaviour
{
    [SerializeField] private string restartScene = "startScene";

    public void restartButton()
    {
        Simulation.StepCount = 0;
        Simulation.isRunning = false;
        SceneManager.LoadScene(restartScene);
    }
}
