using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DayCareManager : MonoBehaviour
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
    bool watchedVideo = false;
    int smallGemCost = 3, bigGemCost = 4;

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
            p.GetGemsButton().onClick.AddListener(()=>GemsPurcharse(index));
            p.GetVideoButton().onClick.AddListener(()=>WatchVideo(index));
            p.SetDinoImage(Resources.Load<Sprite>("Sprites/ShopSprites/"+ i));
            p.SetDinoName(dinoNames[i]);
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
        RefreshButtons(null);
    }

    public void RefreshButtons(GameEvents.MoneyEventData e)
    {
        int fastPurchaseIndex = GetFastPurchaseIndex();
        int biggestDino = UserDataController.GetBiggestDino();
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
            }
            else
            {
                if (biggestDino == 3)
                {
                    _dinoPanelManagers[0].UnlockPanel(0);
                    _dinoPanelManagers[1].UnlockPanel(1);
                    _dinoPanelManagers[2].UnlockPanel(1);
                    _dinoPanelManagers[3].LockPurcharse();
                }
                else
                {
                    //Desbloqueo Corriente
                    if (biggestDino >= 4 && biggestDino < 8)
                    {
                        if (i == biggestDino || i == biggestDino -1)
                        {
                            _dinoPanelManagers[i].LockPurcharse(); //2 primeros bloqueados
                        }
                        if(i == biggestDino - 2)
                        {
                            _dinoPanelManagers[i].UnlockPanel(1); //Gemas
                            _dinoPanelManagers[i].SetGemsCost(bigGemCost);
                        }
                        if(i == biggestDino - 3)
                        {
                            _dinoPanelManagers[i].UnlockPanel(1); //Gemas
                            _dinoPanelManagers[i].SetGemsCost(smallGemCost);
                        }
                        if (i < biggestDino - 3)
                        {
                            _dinoPanelManagers[i].UnlockPanel(0);
                        }
                    }
                    else
                    {
                        if (biggestDino > 8)
                        {
                            if (i < biggestDino - 4)
                            {
                                _dinoPanelManagers[i].UnlockPanel(0); //Normal
                            }
                            else
                            {
                                if (i == biggestDino - 4)
                                {
                                    if (watchedVideo)
                                    {
                                        _dinoPanelManagers[i].UnlockPanel(2); //Video
                                    }
                                    else
                                    {
                                        _dinoPanelManagers[i].UnlockPanel(0); //Normal
                                    }
                                }
                                else
                                {
                                    if (i == biggestDino - 2)
                                    {
                                        _dinoPanelManagers[i].UnlockPanel(1); //Gemas
                                        _dinoPanelManagers[i].SetGemsCost(bigGemCost);
                                    }
                                    if (i == biggestDino - 3)
                                    {
                                        _dinoPanelManagers[i].UnlockPanel(1); //Gemas
                                        _dinoPanelManagers[i].SetGemsCost(smallGemCost);
                                    }
                                    if (i == biggestDino || i == biggestDino - 1)
                                    {
                                        _dinoPanelManagers[i].LockPurcharse(); //2 primeros bloqueados
                                    }
                                }
                            }
                        }
                    }

                    bool canPurchase = UserDataController.HaveMoney(_economyManager.GetDinoCost(i));
                    _dinoPanelManagers[i].SetProfits(_economyManager.GetEarningsByType(i).GetCurrentMoney());
                    _dinoPanelManagers[i].SetPurchaseCost(_economyManager.GetDinoCost(i).GetCurrentMoney());
                    _dinoPanelManagers[i].SetPurchaseState(canPurchase);
                }
            }
        }
    }

    public int GetFastPurchaseIndex()
    {
        int biggestDino = UserDataController.GetBiggestDino();
        int fastPurchaseIndex = 0;
        if (biggestDino < 7)
        {
            fastPurchaseIndex = biggestDino - 4;
        }
        else
        {
            if(biggestDino < 9)
            {
                fastPurchaseIndex = biggestDino - 5;
            }
            else
            {
                if (biggestDino < 11)
                {
                    fastPurchaseIndex = biggestDino - 6;
                }
                else
                {
                    fastPurchaseIndex = biggestDino - 7;
                }
            }
        }

        return Mathf.Max(fastPurchaseIndex, 0);
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
    public void GemsPurcharse(int dinoType)
    {
        print("Gems");
        if(_dinoPanelManagers[dinoType].GetGemCost() <= UserDataController._currentUserData._hardCoins)
        {
            if (UserDataController.GetEmptyCells() > 0)
            {
                UserDataController.SpendHardCoins(_dinoPanelManagers[dinoType].GetGemCost());
                _mainGameSceneController.Purchase(dinoType, null);
                RefreshButtons(null);
            }
            else
            {
                GameEvents.ShowAdvice.Invoke("ADVICE_NOEMPTYCELLS");
            }
        }
        else
        {
            GameEvents.ShowAdvice.Invoke("ADVICE_NOGEMS");
        }

    }
    public void WatchVideo(int dinoType)
    {
        if (UserDataController.GetEmptyCells() > 0)
        {
            //Mostrar publicidad y luego hacer esto:
            watchedVideo = true;
            _mainGameSceneController.Purchase(dinoType, null);
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