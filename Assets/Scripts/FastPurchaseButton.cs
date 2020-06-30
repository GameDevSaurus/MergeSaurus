using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FastPurchaseButton : MonoBehaviour
{
    [SerializeField]
    Button _fastPurchaseButton;
    [SerializeField]
    MainGameSceneController _mainGameSceneController;
    [SerializeField]
    int _purchaseType = 0;
    GameCurrency _purchaseCost;
    [SerializeField]
    TextMeshProUGUI txPurchaseCost;
    EconomyManager _economyManager;

    private void Awake()
    {
        _economyManager = FindObjectOfType<EconomyManager>();
        GameEvents.Purchase.AddListener(FastPurchaseCallBack);
        Refresh();
    }
    public void Purchase()
    {
        if(UserDataController.GetEmptyCells() > 0)
        {
            _mainGameSceneController.Purchase(_purchaseType, _purchaseCost);
        }
        else
        {
            GameEvents.ShowAdvice.Invoke("ADVICE_NOEMPTYCELLS");
        }
    }

    public void FastPurchaseCallBack(int dinoType)
    {
        if(dinoType == _purchaseType)
        {
            Refresh();
        }
    }

    private void Update()
    {
        if(UserDataController.HaveMoney(_purchaseCost))
        {
            _fastPurchaseButton.interactable = true;
        }
        else
        {
            _fastPurchaseButton.interactable = false;
        }
    }
    public void Refresh()
    {
        _purchaseCost = _economyManager.GetDinoCost(_purchaseType);
        txPurchaseCost.text = _purchaseCost.GetCurrentMoney();
    }
}
