using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public static List<int> experienceCost = new List<int>() {0, 4, 11, 27, 50, 109, 217, 394, 665, 1015, 1415, 1904, 2552, 3394, 4480, 4817, 5869, 6384, 7630};
    LevelUpPanelManager _levelUpPanelManager;
    bool waitingLvlUpPanel = false;

    void Start()
    {
        // Robot 3 -- Tutorial
        // Robot 7 -- Desbloquea Mejoras - Gratis x2Vel
        // Robot 9 -- Rankings
        _levelUpPanelManager = FindObjectOfType<LevelUpPanelManager>();
        GameEvents.MergeDino.AddListener(MergeDinoCallBack);
    }

    public void MergeDinoCallBack(int dinoType)
    {
        if (UserDataController.IsGoingToLvlUp(dinoType))
        {
            _levelUpPanelManager.LevelUp();
            StartCoroutine(WaitingCloseLvlUpPanel(dinoType));
        }
        else
        {
            UserDataController.AddExperiencePoints(dinoType);
        }
    }

    public void CloseLvlUpPanel()
    {
        waitingLvlUpPanel = false;
    }
    IEnumerator WaitingCloseLvlUpPanel(int dinoType)
    {
        waitingLvlUpPanel = true;
        while (waitingLvlUpPanel)
        {
            yield return null;
        }
        UserDataController.AddExperiencePoints(dinoType);
    }
}
