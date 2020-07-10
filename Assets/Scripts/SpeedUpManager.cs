using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SpeedUpManager : MonoBehaviour
{
    [SerializeField]
    GameObject _speedUpMain;
    [SerializeField]
    TextMeshProUGUI _remainingTimeTx;
    [SerializeField]
    Transform _speedUpButton;
    [SerializeField]
    Button gemsPurchaseButton;
    [SerializeField]
    Transform _adButton;
    [SerializeField]
    RectTransform _activeFeedbackPanel;
    [SerializeField]
    TextMeshProUGUI _activeTime;
    [SerializeField]
    AnimationCurve animationCurve;
    [SerializeField]
    VFXFireworksPool _VFXFireworksPool;

    float _speedUpTime = 0;
    bool _speedingUp = false;
    bool _panelIsOpen = false;

    private void Start()
    {
        DateTime _lastSpeedUp = UserDataController.GetLastSpeedUpTime();
        int remainingSecs = (int)System.DateTime.Now.Subtract(_lastSpeedUp).TotalSeconds;

        int finalTime = UserDataController.GetSpeedUpRemainingSecs() - remainingSecs;

        if (finalTime > 0)
        {
            SpeedUpCallback(finalTime);
        }
    }
    public void OpenSpeedUpPanel()
    {
        StartCoroutine(CrOpen());
        CheckGemsButton();
    }
    IEnumerator CrOpen()
    {
        RectTransform rt = _speedUpMain.GetComponent<RectTransform>();
        rt.localScale = Vector3.zero;
        _speedUpMain.SetActive(true);
        _panelIsOpen = true;
        for (float i = 0; i < 0.25f; i += Time.deltaTime)
        {
            rt.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, animationCurve.Evaluate(i / 0.25f));
            yield return null;
        }
        rt.localScale = Vector3.one;
    }
    public void CloseSpeedUpPanel()
    {
        _speedUpMain.SetActive(false);
        _panelIsOpen = false;
    }

    public void SpeedUpCallback(int time)
    {
        _speedingUp = true;
        _speedUpTime += time;
        CurrentSceneManager.SetGlobalSpeed(2);
        _VFXFireworksPool.StartTheParty();
        UserDataController.UpdateSpeedUpData((int)_speedUpTime);
    }

    public void SpeedUpShowVideo()
    {
        GameEvents.PlayAd.Invoke("SpeedUp");
    }

    public void GemsPurchase()
    {
        if(UserDataController._currentUserData._hardCoins >= 3)
        {
            SpeedUpCallback(200);
            UserDataController._currentUserData._hardCoins -= 3;
        }
        CheckGemsButton();
    }
    public void CheckGemsButton()
    {
        if (UserDataController._currentUserData._hardCoins >= 3)
        {
            gemsPurchaseButton.interactable = true;
        }
        else
        {
            gemsPurchaseButton.interactable = false;
        }
    }
    private void Update()
    {
        if (_panelIsOpen) 
        {
            _remainingTimeTx.text = GetRemainingTime();
        }
        if(_speedingUp)
        {
            _activeFeedbackPanel.gameObject.SetActive(true);
            _activeTime.text = GetRemainingTime();
            _speedUpTime -= Time.deltaTime;
            if(_speedUpTime < 0)
            {
                _speedingUp = false;
                _speedUpTime = 0;
                _VFXFireworksPool.StopTheParty();
                CurrentSceneManager.SetGlobalSpeed(1);
                _activeFeedbackPanel.gameObject.SetActive(false);
            }
        }
        else
        {
            _activeFeedbackPanel.gameObject.SetActive(false);
        }
    }

    public string GetRemainingTime()
    {
        int initialUnits = Mathf.FloorToInt(_speedUpTime);
        int minutes = initialUnits / 60;
        int seconds = initialUnits % 60;
        string time = minutes.ToString("00") + ":" + seconds.ToString("00");
        return time;
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
