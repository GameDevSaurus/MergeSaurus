using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionsManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;

    public void OpenMissions()
    {
        _mainPanel.SetActive(true);
    }
    public void CloseMissions()
    {
        _mainPanel.SetActive(false);
    }
}
