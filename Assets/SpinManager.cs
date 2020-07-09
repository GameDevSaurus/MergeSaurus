using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SpinManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    int _currentTries = 3;
    bool spinning = false;
    [SerializeField]
    RectTransform _spin;
    SpeedUpManager _speedUpManager;
    BoxManager _boxManager;
    EconomyManager _economyManager;
    enum SpinRewards {Boxes, SmallSpeedTime, BigSpeedTime, Money2H, Money4H, Gems};
    SpinRewards _obtainedReward;

    private void Start()
    {
        _speedUpManager = FindObjectOfType<SpeedUpManager>();
        _boxManager = FindObjectOfType<BoxManager>();
        _economyManager = FindObjectOfType<EconomyManager>();
    }
    public void OpenSpin()
    {
        _mainPanel.SetActive(true);
    }
    public void CloseSpin()
    {
        _mainPanel.SetActive(false);
    }
    public void ShowVideo()
    {
        if(_currentTries > 0)
        {
            GameEvents.PlayAd.Invoke("SpinReward");
        }
        else
        {
            GameEvents.ShowAdvice.Invoke("ADVICE_NO_AD");
        }
    }
    public void SpinCallBack()
    {
        _currentTries--;
        Spin();
    }
    public void Spin()
    {
        spinning = true;
        StartCoroutine(SpinRouleteCr());
    }
    public void Update()
    {
        if (spinning)
        {
            _spin.Rotate(new Vector3(0,0, Time.deltaTime * 180));
        }
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
                break;
            case SpinRewards.BigSpeedTime:
                _speedUpManager.SpeedUpCallback(400);
                break;
            case SpinRewards.Boxes:
                _boxManager.RewardBox(4);
                break;
            case SpinRewards.Gems:
                UserDataController.AddHardCoins(5);
                break;
            case SpinRewards.Money2H:
                baseRewardPSec = _economyManager.GetTotalEarningsPerSecond();
                baseRewardPSec.MultiplyCurrency(720);
                _economyManager.EarnSoftCoins(baseRewardPSec);
                print(baseRewardPSec.GetCurrentMoney());
                break;
            case SpinRewards.Money4H:
                baseRewardPSec = _economyManager.GetTotalEarningsPerSecond();
                baseRewardPSec.MultiplyCurrency(1440);
                _economyManager.EarnSoftCoins(baseRewardPSec);
                print(baseRewardPSec.GetCurrentMoney());
                break;
        }
        CloseSpin();
    }
}