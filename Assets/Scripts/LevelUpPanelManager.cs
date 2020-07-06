﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;

public class LevelUpPanelManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _levelUpTx;
    [SerializeField]
    GameObject _levelUpPanel;
    bool canDisable;
    ExperienceManager _experienceManager;
    [SerializeField]
    Transform _rewardsPanel;
    [SerializeField]
    Sprite[] _rewardsIcons;
    [SerializeField]
    GameObject _rewardsPrefab;
    private void Start()
    {
        _experienceManager = FindObjectOfType<ExperienceManager>();
    }
    public void LevelUp()
    {
        StartCoroutine(ShowNewLevelInfo());
        LevelUpRewards(UserDataController.GetLevel() + 1);
    }

    IEnumerator ShowNewLevelInfo()
    {
        _levelUpPanel.SetActive(true);
        canDisable = false;
        int level = UserDataController.GetLevel() + 1;
        _levelUpTx.text = level.ToString();
        yield return new WaitForSeconds(0.5f);
        canDisable = true;
    }

    public void DisableMainPanel()
    {
        if (canDisable)
        {
            _levelUpPanel.SetActive(false);
            _experienceManager.CloseLvlUpPanel();
        }
    }
    public void LevelUpRewards(int lvl)
    {
        switch (lvl)
        {
            case 2:
            case 6:
                ShowRewards(1,1,0);
                break;
            case 4:
            case 20:
                ShowRewards(1,0,0);
                break;
            case 5:
            case 7:
            case 9:
            case 11:
            case 15:
                ShowRewards(1,0,3);
                break;
            case 8:
            case 10:
                ShowRewards(0, 1, 0);
                break;
            case 13:
            case 17:
            case 19:
                ShowRewards(0, 0, 3);
                break;
        }
    }

    public void ShowRewards(int cells, int expositors, int gems)
    {
        if (cells > 0)
        {

            GameObject nReward = Instantiate(_rewardsPrefab, _rewardsPanel.position, Quaternion.identity);
            nReward.GetComponent<RewardInstance>().SetRewards(_rewardsIcons[0], cells);
            nReward.transform.SetParent(_rewardsPanel);
            nReward.transform.localScale = Vector3.one;
        }
        if (expositors > 0)
        {
            GameObject nReward = Instantiate(_rewardsPrefab, _rewardsPanel.position, Quaternion.identity);
            nReward.GetComponent<RewardInstance>().SetRewards(_rewardsIcons[1], expositors);
            nReward.transform.SetParent(_rewardsPanel);
            nReward.transform.localScale = Vector3.one;
        }
        if (gems > 0)
        {
            GameObject nReward = Instantiate(_rewardsPrefab, _rewardsPanel.position, Quaternion.identity);
            nReward.GetComponent<RewardInstance>().SetRewards(_rewardsIcons[2], gems);
            nReward.transform.SetParent(_rewardsPanel);
            nReward.transform.localScale = Vector3.one;
        }
    }
}
