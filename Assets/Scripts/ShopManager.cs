using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    PanelManager _panelManager;
    List<int> _gemRewards = new List<int>() { 40, 360, 840, 1920, 5100, 11400 };
    List<int> _coinRewards = new List<int>() { 40, 360, 840, 1920, 5100, 11400 };
    List<int> _realCosts = new List<int>() { 100, 449, 999, 1999, 4999, 9999 };
    List<int> _coinRewardSeconds = new List<int>() { 28800, 115200, 806400 };
    List<int> _coinGemCost = new List<int>() { 85, 399, 999 };
    [SerializeField]
    GameObject[] _gemProducts;
    [SerializeField]
    GameObject[] _coinProducts;
    EconomyManager _economyManager;
    RewardManager _rewardManager;
    [SerializeField]
    RectTransform _gridRectTransform;
    bool isOpen = false;
    [SerializeField]
    GameObject _confirmPurchase, _unlockedOfferPanel, _purchaseNull;

    public void OpenShop()
    {
        if (CurrentSceneManager._canOpenShop)
        {
            if (!isOpen)
            {
                isOpen = true;
                _gridRectTransform.anchoredPosition = Vector3.zero;
                if (_panelManager.GetPanelState())
                {
                    _panelManager.ClosePanel();
                }
                _panelManager.RequestShowPanel(_mainPanel);
                RefreshCoinPanels();
                if (UserDataController.CheckSpecialOffer())
                {
                    _unlockedOfferPanel.SetActive(true);
                    _purchaseNull.SetActive(false);
                }
            }
        }
    }
    public void CloseShop()
    {
        isOpen = false;
        _panelManager.ClosePanel();
    }

    private void Start()
    {
        _panelManager = FindObjectOfType<PanelManager>();
        _economyManager = FindObjectOfType<EconomyManager>();
        _rewardManager = FindObjectOfType<RewardManager>();

        for (int i = 0; i<_gemProducts.Length; i++)
        { 
            _gemProducts[i].GetComponent<ShopProductInstance>().Init(_gemRewards[i].ToString(), _realCosts[i], i, true, this);
        }
        RefreshCoinPanels();
    }
    public void Purchase(bool isGem, int index)
    {
        if (isGem)
        {
            _rewardManager.EarnHardCoin(_gemRewards[index]);
        }
        else
        {
            if (_economyManager.SpendHardCoins(_coinGemCost[index]))
            {
                _rewardManager.EarnSoftCoin(_coinRewardSeconds[index]);
            }
        }
    }

    public void RefreshCoinPanels()
    {
        for(int i = 0; i<_coinProducts.Length; i++)
        {
            GameCurrency baseRewardPSec;
            baseRewardPSec = new GameCurrency(_economyManager.GetTotalEarningsPerSecond().GetIntList());
            baseRewardPSec.MultiplyCurrency(_coinRewardSeconds[i]);
            _coinProducts[i].GetComponent<ShopProductInstance>().Init(baseRewardPSec.GetCurrentMoney(), _coinGemCost[i], i, false, this);
        }
    }

    public void SpecialOffer()
    {
        _confirmPurchase.SetActive(true);
    }

    public void ConfirmSpecialOffer()
    {
        if (_economyManager.SpendHardCoins(150))
        {
            UserDataController.PurchaseSpecialOffer();
            _unlockedOfferPanel.SetActive(true);
            _purchaseNull.SetActive(false);
            CloseConfirmPurchase();
        }
    }

    public void CloseConfirmPurchase()
    {
        _confirmPurchase.SetActive(false);
    }
}
