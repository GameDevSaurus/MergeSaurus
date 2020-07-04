﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class SpeedUpManager : MonoBehaviour
{
    [SerializeField]
    GameObject _speedUpMain;
    [SerializeField]
    TextMeshProUGUI _remainingTimeTx;
    [SerializeField]
    Transform _speedUpButton;
    [SerializeField]
    Transform _adButton;
    float _speedUpTime = 0;
    bool _speedingUp = false;
    bool _panelIsOpen = false;
    public void OpenSpeedUpPanel()
    {
        _speedUpMain.SetActive(true);
        _panelIsOpen = true;
    }
    public void CloseSpeedUpPanel()
    {
        _speedUpMain.SetActive(false);
        _panelIsOpen = false;
    }

    public void SpeedUp()
    {
        _speedingUp = true;
        _speedUpTime += 200;
        CurrentSceneManager.SetGlobalSpeed(2);
    }

    private void Update()
    {
        if (_panelIsOpen) 
        {
            ShowRemainingTime();
        }
        if(_speedingUp)
        {
            _speedUpTime -= Time.deltaTime;
            if(_speedUpTime < 0)
            {
                _speedingUp = false;
                _speedUpTime = 0;
                CurrentSceneManager.SetGlobalSpeed(1);
            }
        }
    }

    public void ShowRemainingTime()
    {
        int initialUnits = Mathf.FloorToInt(_speedUpTime);
        int minutes = initialUnits / 60;
        int seconds = initialUnits % 60;
        string time = minutes.ToString("00") + ":" + seconds.ToString("00");
        _remainingTimeTx.text = time;
    }

    public Vector3 GetSpeedUpPosition()
    {
        return _speedUpButton.position;
    }
    public Vector3 GetAdButtPosition()
    {
        return _adButton.position;
    }

    public bool IsPanelOpen()
    {
        return _panelIsOpen;
    }
}