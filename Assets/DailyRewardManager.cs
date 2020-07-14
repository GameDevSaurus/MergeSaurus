using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DailyRewardManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    [SerializeField]
    Image _blackBackgroundImage;
    PanelManager _panelManager;
    SpeedUpManager _speedUpManager;
    EconomyManager _economyManager;
    [SerializeField]
    RectTransform[] _dailyRewards;
    [SerializeField]
    Image _reborder;
    private void Awake()
    {
        _panelManager = FindObjectOfType<PanelManager>();
        GameEvents.DinoUp.AddListener(DinoUpCallback);
        _speedUpManager = FindObjectOfType<SpeedUpManager>();
        _economyManager = FindObjectOfType<EconomyManager>();
    }
    private void Start()
    {
        DateTime lastDay = UserDataController.GetLastPlayedDay();
        DateTime today = System.DateTime.Now;
        if(lastDay.Day != today.Day || lastDay.Month != today.Month)
        {
            OpenPanel();
        }     
    }
    public void DinoUpCallback(int dino)
    {
        if(dino == 6)
        {
            OpenPanel();
        }
    }
    public void CloseDaily()
    {
        _panelManager.ClosePanel();
    }
    public void OpenPanel()
    {
        int playedDays = UserDataController.GetPlayedDays();
        MarkButton(playedDays);
        _panelManager.RequestShowPanel(_mainPanel.gameObject);
    }

    public void ObtainReward(int rewardDay)
    {
        GameCurrency baseRewardPSec;
        switch (rewardDay)
        {
            case 0:
                _speedUpManager.SpeedUpCallback(300);
                GameEvents.ShowAdvice.Invoke(new GameEvents.AdviceEventData("SPIN_REWARD_SPEEDUP","300"));
                break;
            case 1:
                baseRewardPSec = new GameCurrency(_economyManager.GetTotalEarningsPerSecond().GetIntList());
                baseRewardPSec.MultiplyCurrency(3600);
                GameEvents.ShowAdvice.Invoke(new GameEvents.AdviceEventData("SPIN_REWARD_SOFTCOINS", "1"));
                _economyManager.EarnSoftCoins(baseRewardPSec);
                break;
            case 2:
                //TO DO
                //300s DE GANANCIAS x5
                break;
            case 3:
                baseRewardPSec = new GameCurrency(_economyManager.GetTotalEarningsPerSecond().GetIntList());
                baseRewardPSec.MultiplyCurrency(7200);
                GameEvents.ShowAdvice.Invoke(new GameEvents.AdviceEventData("SPIN_REWARD_SOFTCOINS", "2"));
                _economyManager.EarnSoftCoins(baseRewardPSec);
                break;
            case 4:
                _speedUpManager.SpeedUpCallback(300);
                //TO DO
                //300s DE GANANCIAS x5
                break;
            case 5:
                baseRewardPSec = new GameCurrency(_economyManager.GetTotalEarningsPerSecond().GetIntList());
                baseRewardPSec.MultiplyCurrency(14400);
                GameEvents.ShowAdvice.Invoke(new GameEvents.AdviceEventData("SPIN_REWARD_SOFTCOINS", "4"));
                _economyManager.EarnSoftCoins(baseRewardPSec);
                break;
            case 6:
                UserDataController.AddHardCoins(50);
                break;
        }
        UserDataController.AddPlayedDay();
        CloseDaily();
    }

    public void MarkButton(int buttonIndex)
    {
        for(int i = 0; i<_dailyRewards.Length; i++)
        {
            Button dailyButton = _dailyRewards[i].GetComponentInChildren<Button>();
            TextMeshProUGUI dailyText = _dailyRewards[i].GetComponentInChildren<TextMeshProUGUI>();
            string finalTx = string.Format(LocalizationController._localizedData["DAILY_REWARD_DAY"], (i+1).ToString());
            dailyText.text = finalTx;

            if (i != buttonIndex)
            {
                dailyButton.interactable = false;
            }
            else
            {
                if (i == 6)
                {
                    _reborder.rectTransform.sizeDelta = new Vector2(400, 350);
                }
                else
                {
                    _reborder.rectTransform.sizeDelta = new Vector2(250, 350);
                }
                _reborder.transform.SetParent(_dailyRewards[i]);
                _reborder.transform.SetAsLastSibling();
                _reborder.rectTransform.anchoredPosition = Vector3.zero;
                dailyButton.interactable = true;
            }
        }
    }
}