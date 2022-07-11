using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] private string startScene = "SampleScene";

    public void StartButton()
    {
        SceneManager.LoadScene(startScene);
    }
}
