using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    [SerializeField]
    Image _blackBackgroundImage;
    [SerializeField]
    PanelManager _panelManager;
    private void Awake()
    {
        _panelManager = FindObjectOfType<PanelManager>();
        GameEvents.DinoUp.AddListener(DinoUpCallback);
    }
    public void DinoUpCallback(int dino)
    {
        if(dino == 6)
        {
            OpenPanel();
        }
    }
    public void CloseDaily()
    {
        _mainPanel.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenPanel();
        }
    }
    public void OpenPanel()
    {
        _panelManager.RequestShowPanel(_mainPanel.gameObject);
        //StartCoroutine(CrOpenPanel());
    }

    IEnumerator CrOpenPanel()
    {
        _mainPanel.SetActive(true);
        float fadeTime = 0.25f;
        Color semiTransparentBlack = new Color(0, 0, 0, 0.75f);
        Color transparentBlack = new Color(0, 0, 0, 0f);
        for (float i = 0; i < fadeTime; i += Time.deltaTime)
        {
            _blackBackgroundImage.color = Color.Lerp(transparentBlack, semiTransparentBlack, i / fadeTime);
            yield return null;
        }
        _blackBackgroundImage.color = semiTransparentBlack;
    }
    public void CheckForDailyReward()
    {

    }
}