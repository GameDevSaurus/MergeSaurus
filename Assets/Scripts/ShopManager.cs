using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    Button[] _dinoShopButtons;
    [SerializeField]
    Button _fastPurchaseButton;
    [SerializeField]
    TextMeshProUGUI[] _txProfitsPerSec;
    [SerializeField]
    TextMeshProUGUI[] _txCurrentCost;
    [SerializeField]
    Image[] _dinoImages;
    [SerializeField]
    Sprite[] _dinoSprites;
    EconomyManager _economyManager;
    [SerializeField]
    GameObject _shopPanel;
    MainGameSceneController _mainGameSceneController;
    [SerializeField]
    TextMeshProUGUI txPurchaseCost;
    int _fastPurchaseDinoType = 0;
    private void Awake()
    {
        _economyManager = FindObjectOfType<EconomyManager>();
        _mainGameSceneController = FindObjectOfType<MainGameSceneController>();
        _shopPanel.SetActive(false);
        GameEvents.EarnMoney.AddListener(RefreshButtons);
    }
    void Start()
    {
        for (int i = 0; i < _txProfitsPerSec.Length; i++)
        {
            string earnings = _economyManager.GetEarningsByType(i).GetCurrentMoney() + "/sec";
            _txProfitsPerSec[i].text = earnings;
        }
        for (int i = 0; i < _dinoImages.Length; i++)
        {
            _dinoImages[i].sprite = _dinoSprites[i];
        }
        RefreshButtons(null);
    }
    public void Close()
    {
        _shopPanel.SetActive(false);
    }
    public void Open()
    {
        _shopPanel.SetActive(true);
    }
    public void RefreshButtons(GameEvents.MoneyEventData e)
    {
        for (int i = 0; i < _dinoShopButtons.Length; i++)
        {
            bool canPurchase = UserDataController.HaveMoney(_economyManager.GetDinoCost(i));
            _txCurrentCost[i].text = _economyManager.GetDinoCost(i).GetCurrentMoney();

            if (i == 0)
            {
                txPurchaseCost.text = _economyManager.GetDinoCost(0).GetCurrentMoney();
            }
            if (canPurchase)
            {
                if (i == 0) //Cambiar indice del fastPurchase
                {
                    _fastPurchaseButton.interactable = true;
                }
                _dinoShopButtons[i].interactable = true;
            }
            else
            {
                if (i == 0)
                {
                    _fastPurchaseButton.interactable = false;
                }
                _dinoShopButtons[i].interactable = false;
            }
        }
    }
    public void Purchase(int dinoType)
    {
        if (UserDataController.GetEmptyCells() > 0)
        {
            _mainGameSceneController.Purchase(dinoType, _economyManager.GetDinoCost(dinoType));
            RefreshButtons(null);
        }
        else
        {
            GameEvents.ShowAdvice.Invoke("ADVICE_NOEMPTYCELLS");
        }
    }

    public void FastPurchase()
    {
        Purchase(_fastPurchaseDinoType);
    }
}
