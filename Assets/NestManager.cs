using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    [SerializeField]
    AnimationCurve animationCurve;
    [SerializeField]
    BoxManager _boxManager;
    [SerializeField]
    EconomyManager _economyManager;
    PanelManager _panelManager;

    private void Awake()
    {
        _panelManager = FindObjectOfType<PanelManager>();
    }
    public void OpenNest()
    {
        _panelManager.RequestShowPanel(_mainPanel);
    }
    public void CloseNest()
    {
        _mainPanel.SetActive(false);
    }
    public void ShowVideo()
    {
        GameEvents.PlayAd.Invoke("SpecialBox");
        CloseNest();
    }
    public void HardCoinPurchase()
    {
        if (_economyManager.SpendHardCoins(3))
        {
            _boxManager.RewardBox(4);
        }
        CloseNest();
    }
}