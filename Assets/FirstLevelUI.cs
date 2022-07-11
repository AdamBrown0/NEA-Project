using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstLevelUI : MonoBehaviour
{
    //5 x 8 aka Default

    private GridManager grid;
    

    public void SetSize()
    {
        GridManager.rows = 5;
        Debug.Log($"Grid Rows set to {GridManager.rows}");

        GridManager.cols = 8;
        Debug.Log($"Grid Columns set to {GridManager.cols}");
    }
}
