using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SpeedUpManager : MonoBehaviour
{
    [SerializeField]
    GameObject _speedUpMain;
    [SerializeField]
    TextMeshProUGUI _remainingTimeTx;
    [SerializeField]
    GameObject _speedUpButtonFeedback;
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
    VFXFireworksPool _VFXFireworksPool;

    float _speedUpTime = 0;
    bool _speedingUp = false;
    bool _panelIsOpen = false;
    bool activeShown = false;
    private void Start()
    {
        if (UserDataController.GetBiggestDino() < 3)
        {
            _speedUpButton.gameObject.SetActive(false);
            _speedUpButtonFeedback.SetActive(false);
        }
    }
    public void OpenSpeedUpPanel()
    {
        _speedUpMain.SetActive(true);
        _panelIsOpen = true;
        CheckGemsButton();
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
        _VFXFireworksPool.StartTheParty();
    }

    public void GemsPurchase()
    {
        if(UserDataController._currentUserData._hardCoins >= 3)
        {
            SpeedUp();
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
            _activeFeedbackPanel.anchoredPosition = new Vector2(-270, 80);
            _activeTime.text = GetRemainingTime();
            _speedUpTime -= Time.deltaTime;
            if(_speedUpTime < 0)
            {
                _speedingUp = false;
                _speedUpTime = 0;
                _VFXFireworksPool.StopTheParty();
                CurrentSceneManager.SetGlobalSpeed(1);
                _activeFeedbackPanel.anchoredPosition = new Vector2(-270, 0);
            }
        }
        else
        {
            _activeFeedbackPanel.anchoredPosition = new Vector2(-270, 0);
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
