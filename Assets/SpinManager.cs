using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using TMPro;
public class SpinManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    [SerializeField]
    TextMeshProUGUI _txRemainingTime;

    int _currentTries;
    bool spinning = false;
    [SerializeField]
    RectTransform _spin;
    SpeedUpManager _speedUpManager;
    BoxManager _boxManager;
    EconomyManager _economyManager;
    enum SpinRewards {Boxes, SmallSpeedTime, BigSpeedTime, Money2H, Money4H, Gems};
    SpinRewards _obtainedReward;
    float _nextAdTime;
    int timeToNextAd = 3600;
    PanelManager _panelManager;
    private void Start()
    {
        _speedUpManager = FindObjectOfType<SpeedUpManager>();
        _boxManager = FindObjectOfType<BoxManager>();
        _economyManager = FindObjectOfType<EconomyManager>();
        _currentTries = UserDataController.GetSpinRemainingAds();
        _panelManager = FindObjectOfType<PanelManager>();
        int finalCount = (int)System.DateTime.Now.Subtract(UserDataController.GetSpinLastViewTime()).TotalSeconds;
        
        int nAds = finalCount / timeToNextAd;
        int remainingTime = finalCount % timeToNextAd;
        _currentTries = Mathf.Min((_currentTries + nAds),3);
        if(_currentTries < 3)
        {
            _nextAdTime = timeToNextAd - remainingTime;
        }
    }
    public void Open()
    {
        _panelManager.RequestShowPanel(_mainPanel);
    }
    public void CloseSpin()
    {
        _mainPanel.SetActive(false);
    }
    public void ShowVideo()
    {
        if (!spinning)
        {
            if (_currentTries > 0)
            {
                GameEvents.PlayAd.Invoke("SpinReward");
            }
            else
            {
                GameEvents.ShowAdvice.Invoke("ADVICE_NO_AD");
            }
        }
    }
    public void SpinCallBack()
    {
        _currentTries--;
        if(_currentTries == 2)
        {
            _nextAdTime = (float)timeToNextAd;
        }
        Spin();
    }

    public void HardCoinSpin()
    {
        if (!spinning)
        {
            if (_economyManager.SpendHardCoins(3))
            {
                Spin();
            }
        }
    }

    public void Spin()
    {
        if (!spinning)
        {
            spinning = true;
            StartCoroutine(SpinRouleteCr());
        }
    }
    public void Update()
    {
        if (spinning)
        {
            _spin.Rotate(new Vector3(0,0, Time.deltaTime * 180));
        }
        if (_currentTries < 3)
        {
            _nextAdTime -= Time.deltaTime;
            _txRemainingTime.text = GetRemainingTime() + " (" + _currentTries + "/3)"; 
            if(_nextAdTime <= 0f)
            {
                _currentTries++;
                _nextAdTime = timeToNextAd;
                UserDataController.UpdateSpinData(_currentTries);
            }
        }
        else
        {
            _txRemainingTime.text = "(" + _currentTries + "/3)";
        }
    }
    public string GetRemainingTime()
    {
        int initialUnits = Mathf.FloorToInt(_nextAdTime);
        int minutes = initialUnits / 60;
        int seconds = initialUnits % 60;
        string time = minutes.ToString("00") + ":" + seconds.ToString("00");
        return time;
    }

    IEnumerator SpinRouleteCr()
    {
        yield return new WaitForSeconds(3f);
        spinning = false;
        _obtainedReward = (SpinRewards)Random.Range(0,6);
        print(_obtainedReward);
        GameCurrency baseRewardPSec;
        switch (_obtainedReward)
        {
            case SpinRewards.SmallSpeedTime:
                _speedUpManager.SpeedUpCallback(200);
                GameEvents.ShowAdvice.Invoke("SPIN_REWARD_SPEEDUP_200");
                break;
            case SpinRewards.BigSpeedTime:
                _speedUpManager.SpeedUpCallback(400);
                GameEvents.ShowAdvice.Invoke("SPIN_REWARD_SPEEDUP_400");
                break;
            case SpinRewards.Boxes:
                _boxManager.RewardBox(4);
                GameEvents.ShowAdvice.Invoke("SPIN_REWARD_BOXES");
                break;
            case SpinRewards.Gems:
                UserDataController.AddHardCoins(5);
                GameEvents.ShowAdvice.Invoke("SPIN_REWARD_HARDCOINS");
                break;
            case SpinRewards.Money2H:
                baseRewardPSec = _economyManager.GetTotalEarningsPerSecond();
                baseRewardPSec.MultiplyCurrency(7200);
                _economyManager.EarnSoftCoins(baseRewardPSec);
                GameEvents.ShowAdvice.Invoke("SPIN_REWARD_SOFTCOINS_2");
                break;
            case SpinRewards.Money4H:
                baseRewardPSec = _economyManager.GetTotalEarningsPerSecond();
                baseRewardPSec.MultiplyCurrency(14400);
                _economyManager.EarnSoftCoins(baseRewardPSec);
                GameEvents.ShowAdvice.Invoke("SPIN_REWARD_SOFTCOINS_4");
                break;
        }
        UserDataController.UpdateSpinData(_currentTries);
        CloseSpin();
    }
}