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
    Image lastQualityBar;
    [SerializeField]
    Image currentQualityBar;
    Tutorial _tutorial;
    MainGameSceneController _mainGameSceneController;
    [SerializeField]
    Image dinoImage;
    [SerializeField]
    Sprite[] dinoMergeUpSprites;

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
        dinoImage.sprite = dinoMergeUpSprites[dinoType-1];
        canDisable = false;
        _dinoTypeTx.text = ShopManager.dinoNames[dinoType-1];
        currentQualityBar.fillAmount = (float)(dinoType-1f) / (float)UserDataController._currentUserData._dinosaurs.Length;
        lastQualityBar.fillAmount = (float)dinoType / (float)UserDataController._currentUserData._dinosaurs.Length;     
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
