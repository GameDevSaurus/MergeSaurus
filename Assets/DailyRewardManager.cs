using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    [SerializeField]
    Image _blackBackgroundImage;
    [SerializeField]
    PanelManager _panelManager;
    SpeedUpManager _speedUpManager;
    EconomyManager _economyManager;


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
        TimeSpan elapsedTime = today.Subtract(lastDay);
        if(elapsedTime.TotalDays > 0)
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
        _mainPanel.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenPanel();
        }
    }
    public void OpenPanel()
    {
        _panelManager.RequestShowPanel(_mainPanel.gameObject);
        //DIA 1 300s SPEEDUP
        //DIA 2 1 HORA DE GANANCIAS
        //DIA 3 300s DE GANANCIAS x5
        //DIA 4 2 HORAS DE GANANCIAS
        //DIA 5 300s SPEEDUP Y GANANCIAS x5
        //DIA 6 4 HORAS DE GANANCIAS
        //DIA 7 50 HARDCOINS

    }

    public void ObtainReward(int rewardDay)
    {
        GameCurrency baseRewardPSec;
        switch (rewardDay)
        {
            case 0:
                _speedUpManager.SpeedUpCallback(300);
                break;
            case 1:
                baseRewardPSec = _economyManager.GetTotalEarningsPerSecond();
                baseRewardPSec.MultiplyCurrency(3600);
                _economyManager.EarnSoftCoins(baseRewardPSec);
                break;
            case 2:
                //TO DO
                //300s DE GANANCIAS x5
                break;
            case 3:
                baseRewardPSec = _economyManager.GetTotalEarningsPerSecond();
                baseRewardPSec.MultiplyCurrency(7200);
                _economyManager.EarnSoftCoins(baseRewardPSec);
                break;
            case 4:
                _speedUpManager.SpeedUpCallback(300);
                //TO DO
                //300s DE GANANCIAS x5
                break;
            case 5:
                baseRewardPSec = _economyManager.GetTotalEarningsPerSecond();
                baseRewardPSec.MultiplyCurrency(14400);
                _economyManager.EarnSoftCoins(baseRewardPSec);
                break;
            case 6:
                UserDataController.AddHardCoins(50);
                break;
        }
        UserDataController.AddPlayedDay();
    }
}