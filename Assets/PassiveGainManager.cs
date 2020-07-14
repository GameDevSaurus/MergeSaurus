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

    GameCurrency baseRewardPSec;
    GameCurrency targetPossibleCoins;
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

    public void WatchAdd()
    {
        GameEvents.PlayAd.Invoke("PassiveEarnings");
    }

    public void VideoWatchedCallBack()
    {
        EarnTripleMoney();
        DefaultClose();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _panelManager.RequestShowPanel(_mainPanel);
        }
    }

    public void Start()
    {
        _panelManager = FindObjectOfType<PanelManager>();
        _economyManager = FindObjectOfType<EconomyManager>();
        CheckLastSaveTime();
    }

    public void CheckLastSaveTime()
    {
        int secondsSinceLastSave = UserDataController.GetSecondsSinceLastSave();
        if (secondsSinceLastSave < 0)
        {
            DateTime lastSave = UserDataController.GetLastSave();
            DateTime now = System.DateTime.Now;
            secondsSinceLastSave = (int)now.Subtract(lastSave).TotalSeconds;
        }

        print("Han pasado " + secondsSinceLastSave + " desde la última vez que se guardó");
        if(UserDataController.GetBiggestDino() >= 4)
        {           
            if (secondsSinceLastSave > 60)
            {
                if(secondsSinceLastSave > 7200)
                {
                    secondsSinceLastSave = 7200;
                }
                baseRewardPSec = new GameCurrency(_economyManager.GetTotalEarningsPerSecond().GetIntList());
                baseRewardPSec.MultiplyCurrency(secondsSinceLastSave);
                baseRewardPSec.MultiplyCurrency(1 + (UpgradesManager.GetExtraPassiveEarnings() / 100));
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
        _economyManager.EarnSoftCoins(baseRewardPSec);
    }
    public void EarnTripleMoney()
    {
        _economyManager.EarnSoftCoins(targetPossibleCoins);
    }
}
