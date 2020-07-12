using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionsManager : MonoBehaviour
{
    [SerializeField]
    AnimationCurve animationCurve;
    [SerializeField]
    GameObject _mainPanel;

    PanelManager _panelManager;
    public void OpenMissions()
    {
        _panelManager.RequestShowPanel(_mainPanel);
    }
    private void Start()
    {
        _panelManager = FindObjectOfType<PanelManager>();
    }
    public void CloseMissions()
    {
        _mainPanel.SetActive(false);
    }
}
