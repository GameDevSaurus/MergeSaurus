using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveGainManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;

    public void OpenPassiveEarningsPanel()
    {
        _mainPanel.SetActive(true);
    }
    public void ClosePassiveEarningsPanel()
    {
        _mainPanel.SetActive(false);
    }
}
