using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopManager : MonoBehaviour
{
    EconomyManager _economyManager;
    [SerializeField]
    Button _fastPurchase;
    [SerializeField]
    Image _fastPurchaseFace;
    [SerializeField]
    TextMeshProUGUI _fastPurchaseCost;
    [SerializeField]
    TextMeshProUGUI _fastPurchaseName;
    [SerializeField]
    GameObject _shopPanel;
    MainGameSceneController _mainGameSceneController;
    [SerializeField]
    GameObject _shopButton;
    [SerializeField]
    GameObject _purchaseDinoPanelPrefab;
    [SerializeField]
    Transform _panelParent;
    List<PurchaseDinoPanel> _dinoPanelManagers;
    public static string[] dinoNames = new string[] { "Pidgey","Caterpie","Magikarp","Abra","Bulbasaur","Squirtle", "Charmander", "Growlithe", "Farfetch'd", "Gastly", "Geodude", "Machop", "Lickitung", "Cubone", "Metapod", "Pikachu", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

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
            int index = i;
            p.GetDinoButton().onClick.AddListener(()=>Purchase(index));
            p.SetProfits(_economyManager.GetEarningsByType(i).GetCurrentMoney());
            p.SetPurchaseCost(_economyManager.GetDinoCost(i).GetCurrentMoney());
            p.SetDinoImage(Resources.Load<Sprite>("Sprites/ShopSprites/"+ i));
            p.SetDinoName(dinoNames[i]);
            if (i > UserDataController.GetBiggestDino())
            {
                p.LockPanel();
            }
            _dinoPanelManagers.Add(p);
            RefreshButtons(null);
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
        int biggestDino = UserDataController.GetBiggestDino();
        int fastPurchaseIndex = Mathf.Max(biggestDino-5,0);
        for (int i = 0; i < _dinoPanelManagers.Count; i++)
        {
            if (i == fastPurchaseIndex)
            {
                _fastPurchase.interactable = UserDataController.HaveMoney(_economyManager.GetDinoCost(i));
                _fastPurchaseCost.text = _economyManager.GetDinoCost(i).GetCurrentMoney();
                _fastPurchaseFace.sprite = Resources.Load<Sprite>("Sprites/ShopSprites/" + i);
                _fastPurchaseFace.overrideSprite = Resources.Load<Sprite>("Sprites/ShopSprites/" + i);
                _fastPurchaseName.text = dinoNames[i];
                _fastPurchaseDinoType = i;
                
            }
            if (i > UserDataController.GetBiggestDino())
            {
                _dinoPanelManagers[i].LockPanel();
                _dinoPanelManagers[i].LockPurcharse();
            }
            else
            {
                _dinoPanelManagers[i].UnlockPanel();
                bool canPurchase = UserDataController.HaveMoney(_economyManager.GetDinoCost(i));
                _dinoPanelManagers[i].SetProfits(_economyManager.GetEarningsByType(i).GetCurrentMoney());
                _dinoPanelManagers[i].SetPurchaseCost(_economyManager.GetDinoCost(i).GetCurrentMoney());

                if (canPurchase && i <= UserDataController.GetBiggestDino())
                {
                    _dinoPanelManagers[i].UnlockPurcharse();
                }
                else
                {
                    _dinoPanelManagers[i].LockPurcharse();
                }
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