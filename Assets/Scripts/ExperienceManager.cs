using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public static List<int> experienceCost = new List<int>() {0, 4, 11, 27, 50, 109, 217, 394, 665, 1015, 1415, 1904, 2552, 3394, 4480, 4817, 5869, 6384, 7630};
    LevelObserver _levelObserver;
    LevelUpPanelManager _levelUpPanelManager;
    void Start()
    {
        // Robot 3 -- Tutorial
        // Robot 7 -- Desbloquea Mejoras - Gratis x2Vel
        // Robot 9 -- Rankings
        _levelUpPanelManager = FindObjectOfType<LevelUpPanelManager>();
        _levelObserver = FindObjectOfType<LevelObserver>();
        GameEvents.MergeDino.AddListener(MergeDinoCallBack);
        GameEvents.LevelUp.AddListener(LevelUpCallBack);
    }

    public void MergeDinoCallBack(int dinoType)
    {
        _levelObserver.UpdateBar();
        UserDataController.AddExperiencePoints(dinoType);
    }
    public void LevelUpCallBack(int level)
    {
        _levelUpPanelManager.LevelUp();
    }
}
