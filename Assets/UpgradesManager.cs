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

    public enum UpgradeTypes {Discount, DinoEarnings, Coolness , PassiveEarnings}

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
        _dinoRequiredTypeTx[0].text = "Dino " + GetDinoLevelForUpgrade(UpgradeTypes.Discount, UserDataController.GetDiscountUpgradeLevel());
        _dinoRequiredTypeTx[1].text = "Dino " + GetDinoLevelForUpgrade(UpgradeTypes.DinoEarnings, UserDataController.GetExtraEarningsLevel());
        _dinoRequiredTypeTx[2].text = "Dino " + GetDinoLevelForUpgrade(UpgradeTypes.Coolness, UserDataController.GetExtraCoolnessLevel());
        _dinoRequiredTypeTx[3].text = "Dino " + GetDinoLevelForUpgrade(UpgradeTypes.PassiveEarnings, UserDataController.GetExtraPassiveEarningsLevel());
       
        _currentLevelsTx[0].text = "Nvl. " + UserDataController.GetDiscountUpgradeLevel()+1;
        _currentLevelsTx[1].text = "Nvl. " + UserDataController.GetExtraEarningsLevel()+1;
        _currentLevelsTx[2].text = "Nvl. " + UserDataController.GetExtraCoolnessLevel()+1;
        _currentLevelsTx[3].text = "Nvl. " + UserDataController.GetExtraPassiveEarningsLevel()+1;
    }

    public int GetDinoLevelForUpgrade(UpgradeTypes upgradeType, int level)
    {
        int requiredDinoLevel=7;
        requiredDinoLevel += (int)upgradeType;
        requiredDinoLevel += (4 * level);
        return requiredDinoLevel;
    }

}
