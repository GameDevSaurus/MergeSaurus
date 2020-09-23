using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    Queue<GameObject> _panelsToOpen;
    GameObject _currentPanel;
    bool _isAnyPanelOpen = false;
    [SerializeField]
    Image _blackBackground;
    VFXManager _vfxManager;
    public void RequestShowPanel(GameObject panel)
    {
        if (_panelsToOpen == null)
        {
            _panelsToOpen = new Queue<GameObject>();
        }
        _panelsToOpen.Enqueue(panel);
        if (!_isAnyPanelOpen)
        {
            ShowPanel();
        }
    }
    private void Awake()
    {
        _vfxManager = FindObjectOfType<VFXManager>();
    }
    public void ShowPanel()
    {
        StartCoroutine(CrShowPanel());
    }

    public void ClosePanel()
    {
        _currentPanel.SetActive(false);
        _blackBackground.gameObject.SetActive(false);
        if (_panelsToOpen.Count > 0)
        {
            ShowPanel();
        }
        else
        {
            _isAnyPanelOpen = false;
        }
    }


    public bool GetPanelState()
    {
        return _isAnyPanelOpen;
    }
    IEnumerator CrShowPanel()
    {
        yield return null;
        GameEvents.PlaySFX.Invoke("Click");
        GameObject panelToShow = _panelsToOpen.Dequeue();
        Color transparentBlack = new Color(0, 0, 0, 0f);
        panelToShow.SetActive(true);
        Color semiTransparentBlack = new Color(0, 0, 0, 0.8f);
        _blackBackground.color = transparentBlack;
        _blackBackground.gameObject.SetActive(true);
        _isAnyPanelOpen = true;
        _currentPanel = panelToShow;
        _blackBackground.color = semiTransparentBlack;
    }
}
