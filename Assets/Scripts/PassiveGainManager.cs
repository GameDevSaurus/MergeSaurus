using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class PassiveGainManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    EconomyManager _economyManager;
    PanelManager _panelManager;
    [SerializeField]
    TextMeshProUGUI txCurrentCoins;
    [SerializeField]
    TextMeshProUGUI txTargetCoins;
    RewardManager _rewardManager;

    GameCurrency baseRewardPSec;
    GameCurrency targetPossibleCoins;
    int secondsSinceLastSave;
    public void OpenPassiveEarningsPanel()
    {
        _mainPanel.SetActive(true);
    }
    public void ClosePassiveEarningsPanel()
    {
        EarnMoney();
        DefaultClose();
    }
    public void DefaultClose()
    {
        _panelManager.ClosePanel();
    }

    public void SpendHardCoins(int cost)
    {
        if (_economyManager.SpendHardCoins(cost))
        {
            EarnTripleMoney();
            DefaultClose();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _panelManager.RequestShowPanel(_mainPanel);
        }
    }

    public IEnumerator Start()
    {
        _panelManager = FindObjectOfType<PanelManager>();
        _economyManager = FindObjectOfType<EconomyManager>();
        _rewardManager = FindObjectOfType<RewardManager>();
        yield return null;
        CheckLastSaveTime();
    }

    public void CheckLastSaveTime()
    {
        secondsSinceLastSave = UserDataController.GetSecondsSinceLastSave();
        if (secondsSinceLastSave < 0)
        {
            DateTime lastSave = UserDataController.GetLastSave();
            DateTime now = System.DateTime.Now;
            secondsSinceLastSave = (int)now.Subtract(lastSave).TotalSeconds;
        }

        if(UserDataController.GetBiggestDino() >= 4)
        {           
            if (secondsSinceLastSave > 60)
            {
                if(secondsSinceLastSave > 7200)
                {
                    secondsSinceLastSave = 7200;
                }
                baseRewardPSec = _economyManager.GetTotalEarningsPerSecond();
                baseRewardPSec.MultiplyCurrency(secondsSinceLastSave);
               // baseRewardPSec.MultiplyCurrency(1 + (UpgradesManager.GetExtraPassiveEarnings() / 100));
                _panelManager.RequestShowPanel(_mainPanel);
                txCurrentCoins.text = "+ " + baseRewardPSec.GetCurrentMoneyConvertedTo3Chars();
                targetPossibleCoins = new GameCurrency(baseRewardPSec.GetIntList());
                targetPossibleCoins.MultiplyCurrency(3f);
                txTargetCoins.text = "+ " + targetPossibleCoins.GetCurrentMoneyConvertedTo3Chars();
            }
        }
    }
    public void EarnMoney()
    {
        _rewardManager.EarnSoftCoin(secondsSinceLastSave);
    }
    public void EarnTripleMoney()
    {
        _rewardManager.EarnSoftCoin(secondsSinceLastSave * 3);
    }
}
