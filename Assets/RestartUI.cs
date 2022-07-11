using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartUI : MonoBehaviour
{
    [SerializeField] private string restartScene = "startScene";

    public void restartButton()
    {
        SceneManager.LoadScene(restartScene);
    }
}
