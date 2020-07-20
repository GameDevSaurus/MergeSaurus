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
    [SerializeField]
    RectTransform[] _dailyRewards;
    [SerializeField]
    Image _reborder;
    RewardManager _rewardManager;

    private void Awake()
    {
        _panelManager = FindObjectOfType<PanelManager>();
        GameEvents.DinoUp.AddListener(DinoUpCallback);
        _rewardManager = FindObjectOfType<RewardManager>();
    }
    private void Start()
    {
        DateTime lastDay = UserDataController.GetLastPlayedDay();
        DateTime today = System.DateTime.Now;
        if(lastDay.Day != today.Day || lastDay.Month != today.Month)
        {
            OpenPanel();
            UserDataController.RestoreDailyMissions();
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
        switch (rewardDay)
        {
            case 0:
                _rewardManager.EarnSpeedUp(300);
                break;
            case 1:
                _rewardManager.EarnSoftCoin(3600);
                break;
            case 2:
                //TO DO
                //300s DE GANANCIAS x5
                break;
            case 3:
                _rewardManager.EarnSoftCoin(7200);
                break;
            case 4:
                _rewardManager.EarnSpeedUp(300);
                //TO DO
                //300s DE GANANCIAS x5
                break;
            case 5:
                _rewardManager.EarnSoftCoin(14400);
                break;
            case 6:
                _rewardManager.EarnHardCoin(50);
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
                    _reborder.rectTransform.sizeDelta = new Vector2(265, 365);
                }
                _reborder.transform.SetParent(_dailyRewards[i]);
                _reborder.transform.SetAsLastSibling();
                _reborder.rectTransform.anchoredPosition = Vector3.zero;
                dailyButton.interactable = true;
            }
        }
    }
}