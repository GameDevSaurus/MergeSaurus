using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;

    public void OpenUpgrades()
    {
        _mainPanel.SetActive(true);
    }
    public void CloseUpgrades()
    {
        _mainPanel.SetActive(false);
    }
}
