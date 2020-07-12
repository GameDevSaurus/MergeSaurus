using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    PanelManager _panelManager;

    public enum UpgradeTypes {Discount, DinoEarnings, Coolness , PassiveEarnings}

    public void OpenUpgrades()
    {
        _panelManager.RequestShowPanel(_mainPanel);
    }
    public void CloseUpgrades()
    {
        _mainPanel.SetActive(false);
    }

    private void Start()
    {
        _panelManager = FindObjectOfType<PanelManager>();
    }
    public int GetDinoLevelForUpgrade(UpgradeTypes upgradeType, int level)
    {
        int requiredDinoLevel=7;
        requiredDinoLevel += (int)upgradeType;
        requiredDinoLevel += (4 * level);
        return requiredDinoLevel;
    }

}
