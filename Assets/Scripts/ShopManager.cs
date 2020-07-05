using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    EconomyManager _economyManager;
    [SerializeField]
    GameObject _shopPanel;
    MainGameSceneController _mainGameSceneController;
    [SerializeField]
    GameObject _shopButton;
    [SerializeField]
    GameObject _upgradeButton;
    [SerializeField]
    GameObject _purchaseDinoPanelPrefab;
    [SerializeField]
    Transform _panelParent;
    List<PurchaseDinoPanel> _dinoPanelManagers;
    string[] dinoNames = new string[] { "Pidgey","Caterpie","Magikarp","Abra","","", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

    int _fastPurchaseDinoType = 0;
    private void Awake()
    {
        _economyManager = FindObjectOfType<EconomyManager>();
        _mainGameSceneController = FindObjectOfType<MainGameSceneController>();
        _shopPanel.SetActive(false);
        GameEvents.EarnMoney.AddListener(RefreshButtons);
        if (UserDataController.GetBiggestDino() < 3)
        {
            _shopButton.SetActive(false);
            _upgradeButton.SetActive(false);
        }
    }
    void Start()
    {
        _dinoPanelManagers = new List<PurchaseDinoPanel>();
        for (int i = 0; i < UserDataController._currentUserData._dinosaurs.Length; i++)
        {
            GameObject nPanel = Instantiate(_purchaseDinoPanelPrefab, transform.position, Quaternion.identity);
            nPanel.transform.SetParent(_panelParent);
            nPanel.transform.localScale = Vector3.one;
            PurchaseDinoPanel p = nPanel.GetComponent<PurchaseDinoPanel>();
            p.SetProfits(_economyManager.GetEarningsByType(i).GetCurrentMoney());
            p.SetDinoImage(Resources.Load<Sprite>("Sprites/ShopSprites/"+0));
            p.SetDinoName(dinoNames[i]);
            if (i > UserDataController.GetBiggestDino())
            {
                p.LockPanel();
            }
            _dinoPanelManagers.Add(p);
        }
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
        for (int i = 0; i < _dinoPanelManagers.Count; i++)
        {
            bool canPurchase = UserDataController.HaveMoney(_economyManager.GetDinoCost(i));
            _dinoPanelManagers[i].SetProfits(_economyManager.GetDinoCost(i).GetCurrentMoney());
            _dinoPanelManagers[i].SetPurchaseCost(_economyManager.GetDinoCost(0).GetCurrentMoney());

            if (canPurchase && i <= UserDataController.GetBiggestDino())
            {
                _dinoPanelManagers[i].UnlockPanel();
            }
            else
            {
                _dinoPanelManagers[i].LockPanel();
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