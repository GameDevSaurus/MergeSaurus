using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    [SerializeField]
    AnimationCurve animationCurve;
    PanelManager _panelManager;
    public void OpenShop()
    {
        _panelManager.RequestShowPanel(_mainPanel);
    }
    public void CloseShop()
    {
        _mainPanel.SetActive(false);
    }

    private void Start()
    {
        _panelManager = FindObjectOfType<PanelManager>();
    }

 
}
