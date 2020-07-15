using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;

    [SerializeField]
    GameObject[] _profilePanels;
    [SerializeField]
    AnimationCurve animationCurve;
    public void OpenProfile()
    {
        StartCoroutine(CrOpen());
    }
    public void CloseProfile()
    {
        _mainPanel.SetActive(false);
    }

    public void OpenPanel(int panel)
    {
        for(int i = 0; i<_profilePanels.Length; i++)
        {
            _profilePanels[i].SetActive(false);
        }
        _profilePanels[panel].SetActive(true);
    }
    IEnumerator CrOpen()
    {
        RectTransform rt = _mainPanel.GetComponent<RectTransform>();
        rt.localScale = Vector3.zero;
        _mainPanel.SetActive(true);
        OpenPanel(0);
        for (float i = 0; i < 0.25f; i += Time.deltaTime)
        {
            rt.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, animationCurve.Evaluate(i / 0.25f));
            yield return null;
        }
        rt.localScale = Vector3.one;
    }
}
