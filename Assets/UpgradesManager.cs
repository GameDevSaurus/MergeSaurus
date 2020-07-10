using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    [SerializeField]
    AnimationCurve animationCurve;
    [SerializeField]
    GameObject _mainPanel;

    public enum UpgradeTypes {Discount, DinoEarnings, Coolness , PassiveEarnings}

    public void OpenUpgrades()
    {
        StartCoroutine(CrOpen());
    }
    public void CloseUpgrades()
    {
        _mainPanel.SetActive(false);
    }

    public int GetDinoLevelForUpgrade(UpgradeTypes upgradeType, int level)
    {
        int requiredDinoLevel=7;
        requiredDinoLevel += (int)upgradeType;
        requiredDinoLevel += (4 * level);
        return requiredDinoLevel;
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
