using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstLevelUI : MonoBehaviour
{
    //5 x 8 

    private GridManager grid;
    

    public void SetSize()
    {
        GridManager.rows = 5;
        Debug.Log($"Grid Rows set to {GridManager.rows}");

        GridManager.cols = 8;
        Debug.Log($"Grid Columns set to {GridManager.cols}");

        GridManager.minPreyCount = 0;
        Debug.Log($"Min Prey count set to {GridManager.minPreyCount}");

        GridManager.minPredatorCount = 1;
        Debug.Log($"Min Predator count set to {GridManager.minPredatorCount}");
    }
}
