using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;

    public enum UpgradeTypes {Discount, DinoEarnings, Coolness , PassiveEarnings}

    public void OpenUpgrades()
    {
        _mainPanel.SetActive(true);
    }
    public void CloseUpgrades()
    {
        _mainPanel.SetActive(false);
    }

    public int GetDinoLevelForUpgrade(UpgradeTypes upgradeType, int level)
    {
        int requiredDinoLevel=7;
        requiredDinoLevel += (int)upgradeType;
        requiredDinoLevel += (4 * level);
        return requiredDinoLevel;
    }


}
