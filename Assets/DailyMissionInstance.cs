using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DailyMissionInstance : MonoBehaviour
{
    [SerializeField]
    int _missionType;

    [SerializeField]
    TextMeshProUGUI _titleTx;
    [SerializeField]
    TextMeshProUGUI _progressTx;
    [SerializeField]
    Image _progressFill;
    [SerializeField]
    Image _rewardIcon;
    [SerializeField]
    TextMeshProUGUI _rewardAmountTx;
    [SerializeField]
    GameObject _claimButton;
    [SerializeField]
    EconomyManager _economyManager;

    int level = 0;
    int target = 0;
    int currentProgress = 0;

    void OnEnable()
    {
        Refresh();
    }

    public void ClaimMergeReward()
    {
        GameCurrency baseRewardPSec;
        baseRewardPSec = new GameCurrency(_economyManager.GetTotalEarningsPerSecond().GetIntList());
        baseRewardPSec.MultiplyCurrency(300 * (level + 1));
        _economyManager.EarnSoftCoins(baseRewardPSec);
        UserDataController.AddDailyMergeLevel();
        Refresh();
    }
    public void ClaimAdReward()
    {
        _economyManager.EarnHardCoins(3*level);
        UserDataController.AddDailyAdLevel();
        Refresh();
    }
    public void ClaimPurchaseReward()
    {
        GameCurrency bigBaseRewardPSec;
        bigBaseRewardPSec = new GameCurrency(_economyManager.GetTotalEarningsPerSecond().GetIntList());
        bigBaseRewardPSec.MultiplyCurrency(600 * (level + 1));
        _economyManager.EarnSoftCoins(bigBaseRewardPSec);
        UserDataController.AddDailyPurchaseLevel();
        Refresh();
    }

    public void Refresh()
    {
        string title = "";
        switch (_missionType)
        {
            case 0://merge
                level = UserDataController.GetDailyMergeLevel();
                target = 10 + (10 * level);
                title = string.Format(LocalizationController.GetValueByKey("DAILYMISSION_MERGE"), target);
                currentProgress = UserDataController.GetDailyMerges();
                GameCurrency baseRewardPSec;
                baseRewardPSec = new GameCurrency(_economyManager.GetTotalEarningsPerSecond().GetIntList());
                baseRewardPSec.MultiplyCurrency(300 * (level + 1));
                
                _rewardAmountTx.text = "x " + baseRewardPSec.GetCurrentMoney();
                break;
            case 1://AD
                level = UserDataController.GetDailyAdLevel();
                target = 5 + (5 * level);
                title = string.Format(LocalizationController.GetValueByKey("DAILYMISSION_AD"), target);
                currentProgress = UserDataController.GetDailyAds();
                break;
            case 2://Purchase
                level = UserDataController.GetDailyPurchaseLevel();
                target = 10 + (10 * level);
                title = string.Format(LocalizationController.GetValueByKey("DAILYMISSION_PURCHASE"), target);
                currentProgress = UserDataController.GetDailyPurchases();
                GameCurrency bigBaseRewardPSec;
                bigBaseRewardPSec = new GameCurrency(_economyManager.GetTotalEarningsPerSecond().GetIntList());
                bigBaseRewardPSec.MultiplyCurrency(600 * (level +1));
                _rewardAmountTx.text = "x "+ bigBaseRewardPSec.GetCurrentMoney();
                break;
        }
        if (currentProgress >= target)
        {
            currentProgress = target;
            _claimButton.SetActive(true);
        }
        else
        {
            _claimButton.SetActive(false);
        }
        _titleTx.text = title;
        _progressFill.fillAmount = currentProgress / (float)target;
        _progressTx.text = currentProgress + "/" + target;
    }
}
