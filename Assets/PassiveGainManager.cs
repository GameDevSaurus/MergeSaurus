using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PassiveGainManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;

    [SerializeField]
    Image _blackBackgroundImage;

    PanelManager _panelManager;

    public void OpenPassiveEarningsPanel()
    {
        _mainPanel.SetActive(true);
    }
    public void ClosePassiveEarningsPanel()
    {
        _mainPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _panelManager.RequestShowPanel(_mainPanel);
        }
    }

    public void OpenPanel()
    {
        StartCoroutine(CrOpenPanel());
    }

    public void Start()
    {
        _panelManager = FindObjectOfType<PanelManager>();
        CheckLastSaveTime();
    }

    IEnumerator CrOpenPanel()
    {
        _mainPanel.SetActive(true);
        float fadeTime = 0.25f;
        Color semiTransparentBlack = new Color(0, 0, 0, 0.75f);
        Color transparentBlack = new Color(0, 0, 0, 0f);
        for (float i = 0; i < fadeTime; i += Time.deltaTime)
        {
            _blackBackgroundImage.color = Color.Lerp(transparentBlack, semiTransparentBlack, i/fadeTime);
            yield return null;
        }
        _blackBackgroundImage.color = semiTransparentBlack;
    }

    public void CheckLastSaveTime()
    {
        int secondsSinceLastSave = UserDataController.GetSecondsSinceLastSave();
        if (secondsSinceLastSave < 0)
        {
            DateTime lastSave = UserDataController.GetLastSave();
            DateTime now = System.DateTime.Now;
            secondsSinceLastSave = (int)now.Subtract(lastSave).TotalSeconds;
        }

        print("Han pasado " + secondsSinceLastSave + " desde la última vez que se guardó");
    }
}
