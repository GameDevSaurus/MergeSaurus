using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public static List<int> experienceCost = new List<int>() {0, 4, 11, 27, 50, 109, 217, 394, 665, 1015, 1415, 1904, 2552, 3394, 4480, 4817, 5869, 6384, 7630, 1000000};
    LevelUpPanelManager _levelUpPanelManager;
    bool waitingLvlUpPanel = false;

    void Start()
    {
        _levelUpPanelManager = FindObjectOfType<LevelUpPanelManager>();
        GameEvents.MergeDino.AddListener(MergeDinoCallBack);
    }

    public void MergeDinoCallBack(int dinoType)
    {
        int preLevel = UserDataController.GetLevel();
        UserDataController.AddExperiencePoints(dinoType);
        int postLevel = UserDataController.GetLevel();
        if (postLevel > preLevel)
        {
            _levelUpPanelManager.LevelUp();
        }
    }

    public void CloseLvlUpPanel()
    {
        waitingLvlUpPanel = false;
    }
}
