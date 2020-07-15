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
    List<Sprite> dinoMergeUpSprites;
    [SerializeField]
    AnimationCurve animationCurve;
    PanelManager _panelManager;
    private void Awake()
    {
        dinoMergeUpSprites = new List<Sprite>();
        for(int i = 0; i < UserDataController.GetDinoAmount(); i++)
        {
            string path = Application.productName + "/Sprites/DefaultSprites/" + i;
            dinoMergeUpSprites.Add(Resources.Load<Sprite>(path));
        }
        _panelManager = FindObjectOfType<PanelManager>();
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
        dinoImage.sprite = dinoMergeUpSprites[dinoType - 1];
        canDisable = false;
        _dinoTypeTx.text = DayCareManager.dinoNames[dinoType - 1];
        currentQualityBar.fillAmount = (float)(dinoType - 1f) / (float)UserDataController.GetDinoAmount();
        lastQualityBar.fillAmount = (float)dinoType / (float)UserDataController.GetDinoAmount();
        _panelManager.RequestShowPanel(_mergeUpPanel);
        yield return new WaitForSeconds(0.5f);
        canDisable = true;
    }

    public void ClosePanel()
    {
        _panelManager.ClosePanel();
        GameEvents.CloseDinoUpPanel.Invoke();
    }
}
