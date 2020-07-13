using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradesManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    PanelManager _panelManager;
    [SerializeField]
    TextMeshProUGUI[] _upgradesCurrentPercentTx;
    [SerializeField]
    TextMeshProUGUI[] _upgradesTargetPercentTx;
    [SerializeField]
    TextMeshProUGUI[] _dinoRequiredTypeTx;
    [SerializeField]
    TextMeshProUGUI[] _dinoCostTx;
    [SerializeField]
    TextMeshProUGUI[] _currentLevelsTx;
    [SerializeField]
    Button[] _upgradeButtons;
    [SerializeField]
    MainGameSceneController _mainGameSceneController;
    int discountIncrement = 5, earningsIncrement = 7, coolnesIncrement = 6, passiveEarningsIncrement = 4;

    public enum UpgradeTypes {Discount, DinoEarnings, TouristSpeed , PassiveEarnings}

    public void OpenUpgrades()
    {
        _panelManager.RequestShowPanel(_mainPanel);
        RefreshUpgradesInfo();
    }
    public void CloseUpgrades()
    {
        _mainPanel.SetActive(false);
    }

    private void Start()
    {
        _panelManager = FindObjectOfType<PanelManager>();
    }

    public void RefreshUpgradesInfo()
    {
        int dinoLevelForDiscount = GetDinoLevelForUpgrade(UpgradeTypes.Discount, UserDataController.GetDiscountUpgradeLevel()) ;
        int dinoLevelForEarnings = GetDinoLevelForUpgrade(UpgradeTypes.DinoEarnings, UserDataController.GetExtraEarningsLevel()) ;
        int dinoLevelForTouristSpeed = GetDinoLevelForUpgrade(UpgradeTypes.TouristSpeed, UserDataController.GetExtraTouristSpeedLevel());
        int dinoLevelForPassiveEarnings = GetDinoLevelForUpgrade(UpgradeTypes.PassiveEarnings, UserDataController.GetExtraPassiveEarningsLevel());

        _dinoRequiredTypeTx[0].text = "Dino " + (dinoLevelForDiscount + 1);
        _dinoRequiredTypeTx[1].text = "Dino " + (dinoLevelForEarnings + 1);
        _dinoRequiredTypeTx[2].text = "Dino " + (dinoLevelForTouristSpeed + 1);
        _dinoRequiredTypeTx[3].text = "Dino " + (dinoLevelForPassiveEarnings + 1);

        _currentLevelsTx[0].text = "Nvl. " + (UserDataController.GetDiscountUpgradeLevel() +1);
        _currentLevelsTx[1].text = "Nvl. " + (UserDataController.GetExtraEarningsLevel()+1);
        _currentLevelsTx[2].text = "Nvl. " + (UserDataController.GetExtraTouristSpeedLevel()+1);
        _currentLevelsTx[3].text = "Nvl. " + (UserDataController.GetExtraPassiveEarningsLevel()+1);

        _dinoCostTx[0].text = UserDataController.GetDinoAmountByType(dinoLevelForDiscount) + "/3";
        _dinoCostTx[1].text = UserDataController.GetDinoAmountByType(dinoLevelForEarnings) + "/3";
        _dinoCostTx[2].text = UserDataController.GetDinoAmountByType(dinoLevelForTouristSpeed) + "/3";
        _dinoCostTx[3].text = UserDataController.GetDinoAmountByType(dinoLevelForPassiveEarnings) + "/3";

        _upgradesCurrentPercentTx[0].text = discountIncrement * UserDataController.GetDiscountUpgradeLevel() + "%";
        _upgradesTargetPercentTx[0].text = " -> " + discountIncrement * (UserDataController.GetDiscountUpgradeLevel()+1) + "%";

        _upgradesCurrentPercentTx[1].text = earningsIncrement * UserDataController.GetExtraEarningsLevel() + "%";
        _upgradesTargetPercentTx[1].text = " -> " + earningsIncrement * (UserDataController.GetExtraEarningsLevel() + 1) + "%";

        _upgradesCurrentPercentTx[2].text = coolnesIncrement * UserDataController.GetExtraTouristSpeedLevel() + "%";
        _upgradesTargetPercentTx[2].text = " -> " + coolnesIncrement * (UserDataController.GetExtraTouristSpeedLevel() + 1) + "%";

        _upgradesCurrentPercentTx[3].text = passiveEarningsIncrement * UserDataController.GetExtraPassiveEarningsLevel() + "%";
        _upgradesTargetPercentTx[3].text = " -> " + passiveEarningsIncrement * (UserDataController.GetExtraPassiveEarningsLevel() + 1) + "%";

        if (UserDataController.GetDinoAmountByType(dinoLevelForDiscount) >= 3)
        {
            _upgradeButtons[0].interactable = true;
        }
        else
        {
            _upgradeButtons[0].interactable = false;
        }

        if (UserDataController.GetDinoAmountByType(dinoLevelForEarnings) >= 3)
        {
            _upgradeButtons[1].interactable = true;
        }
        else
        {
            _upgradeButtons[1].interactable = false;
        }

        if (UserDataController.GetDinoAmountByType(dinoLevelForTouristSpeed) >= 3)
        {
            _upgradeButtons[2].interactable = true;
        }
        else
        {
            _upgradeButtons[2].interactable = false;
        }

        if (UserDataController.GetDinoAmountByType(dinoLevelForPassiveEarnings) >= 3)
        {
            _upgradeButtons[3].interactable = true;
        }
        else
        {
            _upgradeButtons[3].interactable = false;
        }
    }

    public int GetDinoLevelForUpgrade(UpgradeTypes upgradeType, int level)
    {
        int requiredDinoLevel= 0;
        requiredDinoLevel += (int)upgradeType;
        requiredDinoLevel += (4 * level);
        return requiredDinoLevel;
    }

    public void UpgradeLevel(int type)
    {
        int requiredDinoLevel = 0;
        UpgradeTypes upgradeType = (UpgradeTypes)type;

        switch (upgradeType)
        {
            case UpgradeTypes.Discount:
                requiredDinoLevel = GetDinoLevelForUpgrade(upgradeType, UserDataController.GetDiscountUpgradeLevel());              
                break;
            case UpgradeTypes.DinoEarnings:
                requiredDinoLevel = GetDinoLevelForUpgrade(upgradeType, UserDataController.GetExtraEarningsLevel());
                break;
            case UpgradeTypes.TouristSpeed:
                requiredDinoLevel = GetDinoLevelForUpgrade(upgradeType, UserDataController.GetExtraTouristSpeedLevel());
                break;
            case UpgradeTypes.PassiveEarnings:
                requiredDinoLevel = GetDinoLevelForUpgrade(upgradeType, UserDataController.GetExtraPassiveEarningsLevel());
                break;
        }
        List<int> firstThreeDinos = UserDataController.GetFirstThreeDinosByType(requiredDinoLevel);
        if(firstThreeDinos.Count == 3)
        {
            for (int i = 0; i<firstThreeDinos.Count; i++)
            {
                _mainGameSceneController.DestroyDinosaur(firstThreeDinos[i]);
            }
            UserDataController.LevelUpUpgrade(upgradeType);
            RefreshUpgradesInfo();
        }
    }

    public static int GetDiscount() 
    {
        int increment = 5;
        int discount = increment * UserDataController.GetDiscountUpgradeLevel();
        return discount;
    }
    public static int GetExtraEarnings()
    {
        int increment = 7;
        int extra = increment * UserDataController.GetExtraEarningsLevel();
        return extra;
    }
    public static int GetExtraTouristSpeed()
    {
        int increment = 6;
        int extra = increment * UserDataController.GetExtraTouristSpeedLevel();
        return extra;
    }
    public static int GetExtraPassiveEarnings()
    {
        int increment = 4;
        int extra = increment * UserDataController.GetExtraPassiveEarningsLevel();
        return extra;
    }
}
