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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            OpenNest();
        }
    }
    public void CloseNest()
    {
        _panelManager.ClosePanel();
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