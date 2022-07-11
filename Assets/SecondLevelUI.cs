using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondLevelUI : MonoBehaviour
{
    //10 x 16

    private GridManager grid;

    public void SetSize()
    {
        GridManager.rows = 10;
        Debug.Log($"Grid Rows set to {GridManager.rows}");

        GridManager.cols = 16;
        Debug.Log($"Grid Columns set to {GridManager.cols}");
    }
}
