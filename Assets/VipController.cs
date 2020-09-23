using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class VipController : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel, _vipButton;
    PanelManager _panelManager;
    RewardManager _rewardManager;
    EconomyManager _economyManager;

    private void Start()
    {
        _panelManager = FindObjectOfType<PanelManager>();
        _rewardManager = FindObjectOfType<RewardManager>();
        _economyManager = FindObjectOfType<EconomyManager>();
    }

    public void OpenVip()
    {
        _panelManager.RequestShowPanel(_mainPanel);  
    }

    public void CloseVip()
    {
        _panelManager.ClosePanel();
    }

    public void VipPurchase()
    {
        if (_economyManager.SpendHardCoins(2000))
        {
            _rewardManager.EarnHardCoin(75);
            _rewardManager.EarnSpeedUp(400);
            //TO DO AÑADIR RESTO DE MEJORAS
            for(int i = 0; i<UserDataController.GetBiggestDino(); i++)
            {
                UserDataController.UnlockSpecialCard(i);
            }
            UserDataController.SetVip();
            UserDataController.SetFreeSpinTries(2);
            _vipButton.SetActive(false);
            CloseVip();
        }
    }
}
