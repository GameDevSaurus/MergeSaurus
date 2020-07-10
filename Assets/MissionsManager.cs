using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionsManager : MonoBehaviour
{
    [SerializeField]
    AnimationCurve animationCurve;
    [SerializeField]
    GameObject _mainPanel;

    public void OpenMissions()
    {
        StartCoroutine(CrOpen());
    }
    public void CloseMissions()
    {
        _mainPanel.SetActive(false);
    }
    IEnumerator CrOpen()
    {
        RectTransform rt = _mainPanel.GetComponent<RectTransform>();
        rt.localScale = Vector3.zero;
        _mainPanel.SetActive(true);
        for (float i = 0; i < 0.25f; i += Time.deltaTime)
        {
            rt.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, animationCurve.Evaluate(i / 0.25f));
            yield return null;
        }
        rt.localScale = Vector3.one;
    }
}
