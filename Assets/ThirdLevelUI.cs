using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdLevelUI : MonoBehaviour
{
    //100 x 160 
    //This takes a while to load, basically just showing it is truly dynamic

    private GridManager grid;

    public void SetSize()
    {
        GridManager.rows = 100;
        Debug.Log($"Grid Rows set to {GridManager.rows}");

        GridManager.cols = 160;
        Debug.Log($"Grid Columns set to {GridManager.cols}");
    }
}
