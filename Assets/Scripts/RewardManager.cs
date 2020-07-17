using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardManager : MonoBehaviour
{
    [SerializeField]
    Sprite[] _rewardSprites;
    [SerializeField]
    Image _rewardImage;
    [SerializeField]
    TextMeshProUGUI _txReward;
    [SerializeField]
    PanelManager _panelManager;
    [SerializeField]
    EconomyManager _economyManager;
    [SerializeField]
    SpeedUpManager _speedUpManager;
    [SerializeField]
    GameObject _mainPanel;
    Queue<RewardData> rewardDataQueue;
    bool canClose = false;
    bool panelIsOpen;

    private void Awake()
    {
        GameEvents.MergeDino.AddListener(MergeDinoCallBack);
        GameEvents.Purchase.AddListener(PurchaseDinoCallBack);
        GameEvents.DinoUp.AddListener(CheckDinoUp);
        rewardDataQueue = new Queue<RewardData>();
        panelIsOpen = false;
    }
    public void ShowPanel()
    {
        RewardData r = rewardDataQueue.Dequeue();
        if (!panelIsOpen)
        {
            _mainPanel.SetActive(true);
            panelIsOpen = true;
        }
        RefreshInfo(r);
        StartCoroutine(WaitToClose(1f));
    }
    public void ClosePanel()
    {
        if (canClose)
        {
            if (rewardDataQueue.Count > 0)
            {
                ShowPanel();
            }
            else
            {
                _mainPanel.SetActive(false);
                panelIsOpen = false;
            }
        }
    }

    public void CheckDinoUp(int dinoType)
    {
        if(dinoType == 5)
        {
            UnlockSpin();
        }
        if (dinoType == 6)
        {
            UnlockUpgrades();
        }
        if (dinoType == 7)
        {
            UnlockMissions();
        }
    }

    public void UnlockSpin()
    {
        rewardDataQueue.Enqueue(new RewardData(4, 0));
        if (!panelIsOpen)
        {
            ShowPanel();
        }
    }
    public void UnlockMissions()
    {
        rewardDataQueue.Enqueue(new RewardData(5, 0));
        if (!panelIsOpen)
        {
            ShowPanel();
        }
    }
    public void UnlockUpgrades()
    {
        rewardDataQueue.Enqueue(new RewardData(6, 0));
        if (!panelIsOpen)
        {
            ShowPanel();
        }
    }
    public void EarnSoftCoin(int seconds)
    {
        GameCurrency baseRewardPSec;
        baseRewardPSec = new GameCurrency(_economyManager.GetTotalEarningsPerSecond().GetIntList());
        baseRewardPSec.MultiplyCurrency(seconds);
        rewardDataQueue.Enqueue(new RewardData(0, baseRewardPSec));
        if (!panelIsOpen)
        {
            ShowPanel();
        }
    }

    public void EarnHardCoin(int amount)
    {
        rewardDataQueue.Enqueue(new RewardData(1, amount));
        if (!panelIsOpen)
        {
            ShowPanel();
        }
    }

    public void EarnSpeedUp(int seconds)
    {
        rewardDataQueue.Enqueue(new RewardData(2, seconds));
        if (!panelIsOpen)
        {
            ShowPanel();
        }
    }

    public void EarnDinoEarnings(int seconds)
    {
        rewardDataQueue.Enqueue(new RewardData(3, seconds));
        if (!panelIsOpen)
        {
            ShowPanel();
        }
        //TO DO
        //FUNCION DINOEARNINGS
    }

    IEnumerator WaitToClose(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        canClose = true;
    }

    public class RewardData
    {
        public int _rewardType;
        public GameCurrency _softCoinsAmount;
        public int _amount;

        public RewardData(int rewardType, int amount)
        {
            _rewardType = rewardType;
            _amount = amount;
        }
        public RewardData(int rewardType, GameCurrency coinsAmount)
        {
            _rewardType = rewardType;
            _softCoinsAmount = coinsAmount;
        }
    }

    public void RefreshInfo(RewardData r)
    {
        switch (r._rewardType)
        {
            case 0:
                _txReward.text = r._softCoinsAmount.GetCurrentMoney();
                _rewardImage.sprite = _rewardSprites[r._rewardType];
                _economyManager.EarnSoftCoins(r._softCoinsAmount);
                break;
            case 1:
                _txReward.text = string.Format(LocalizationController.GetValueByKey("REWARD_HARDCOINS"), r._amount);
                _rewardImage.sprite = _rewardSprites[r._rewardType];
                UserDataController.AddHardCoins(r._amount);
                break;
            case 2:
                _txReward.text = string.Format(LocalizationController.GetValueByKey("REWARD_SPEEDUP"), r._amount);
                _rewardImage.sprite = _rewardSprites[r._rewardType];
                _speedUpManager.SpeedUpCallback(r._amount);
                break;
            case 3:
                _txReward.text = string.Format(LocalizationController.GetValueByKey("REWARD_DINOREWARDS"), r._amount);
                _rewardImage.sprite = _rewardSprites[r._rewardType];
                break;
            case 4:
                _txReward.text = LocalizationController.GetValueByKey("UNLOCK_SPIN");
                _rewardImage.sprite = _rewardSprites[r._rewardType];
                break;
            case 5:
                _txReward.text = LocalizationController.GetValueByKey("UNLOCK_MISSIONS");
                _rewardImage.sprite = _rewardSprites[r._rewardType];
                break;
            case 6:
                _txReward.text = LocalizationController.GetValueByKey("UNLOCK_UPGRADES");
                _rewardImage.sprite = _rewardSprites[r._rewardType];
                break;
        }
    }

    public void MergeDinoCallBack(int dinoType)
    {
        UserDataController.AddDailyMerge();
    }
    public void PurchaseDinoCallBack(int dinoType)
    {
        UserDataController.AddDailyPurchase();
    }
}


