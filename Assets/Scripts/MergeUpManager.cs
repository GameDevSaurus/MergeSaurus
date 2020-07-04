using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MergeUpManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _dinoTypeTx;
    [SerializeField]
    GameObject _mergeUpPanel;
    bool canDisable;
    [SerializeField]
    Image fillQualityBar;
    Tutorial _tutorial;
    MainGameSceneController _mainGameSceneController;

    private void Awake()
    {
        _mergeUpPanel.SetActive(false);
        GameEvents.DinoUp.AddListener(MergeUpCallBack);
        _tutorial = FindObjectOfType<Tutorial>();
        _mainGameSceneController = FindObjectOfType<MainGameSceneController>();
    }
    public void MergeUpCallBack(int dinoType)
    {
        StartCoroutine(ShowNewMergeInfo(dinoType + 1));
    }

    IEnumerator ShowNewMergeInfo(int dinoType)
    {
        _mergeUpPanel.SetActive(true);
        canDisable = false;
        _dinoTypeTx.text = "Dino " + dinoType;
        fillQualityBar.fillAmount = (float)dinoType / (float)UserDataController._currentUserData._dinosaurs.Length;
        yield return new WaitForSeconds(0.5f);
        canDisable = true;
    }

    public void DisableMainPanel()
    {
        if (canDisable)
        {
            _mainGameSceneController.StopWaitingAnim();
            _mergeUpPanel.SetActive(false);
        }
    }
}
