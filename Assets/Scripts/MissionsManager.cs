using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionsManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    [SerializeField]
    Transform dailyMissionsNull;
    [SerializeField]
    Transform achievementsNull;
    [SerializeField]
    GameObject missionPrefab;
    [SerializeField]
    SOAchievement[] _sOAchievements;
    PanelManager _panelManager;
    [SerializeField]
    Sprite softCoinsIcon, hardCoinsIcon;

    public void OpenMissions()
    {
        _panelManager.RequestShowPanel(_mainPanel);
    }
    private void Start()
    {
        _panelManager = FindObjectOfType<PanelManager>();
        CreateAchievements();
    }
    public void CloseMissions()
    {
        _mainPanel.SetActive(false);
    }

    public void CreateAchievements()
    {
        for(int i = 0; i< 10; i++)
        {
            if (!UserDataController.GetClaimedAchievement(i))
            {
                GameObject missionInstance = Instantiate(missionPrefab, achievementsNull.transform.parent);
                string achievementTitle = string.Format(LocalizationController.GetValueByKey("ACHIEVEMENT_TITLE"), (_sOAchievements[i].dinoLevel + 1));
                missionInstance.GetComponent<AchievementInstance>().SetMissionInstance(i, achievementTitle, UserDataController.GetObtainedDinosByDinotype(_sOAchievements[i].dinoLevel), _sOAchievements[i].amount, hardCoinsIcon, _sOAchievements[i].rewardAmount, _sOAchievements[i].dinoLevel);
            }      
        }

    }
}
