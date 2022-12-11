using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSecretSettings : MonoBehaviour
{
    public void SuperSecretSettingsToggle()
    {
        GridManager.superSecretSettings = !GridManager.superSecretSettings;
        Debug.Log($"Super Secret Settings set to {GridManager.superSecretSettings}");
    }
}
