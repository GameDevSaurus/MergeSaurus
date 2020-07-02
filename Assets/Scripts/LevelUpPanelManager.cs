using System.Collections;
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

    public void LevelUp()
    {
        StartCoroutine(ShowNewLevelInfo());
    }

    IEnumerator ShowNewLevelInfo()
    {
        _levelUpPanel.SetActive(true);
        canDisable = false;
        _levelUpTx.text = UserDataController.GetLevel().ToString();
        yield return new WaitForSeconds(0.5f);
        canDisable = true;
    }

    public void DisableMainPanel()
    {
        if (canDisable)
        {
            _levelUpPanel.SetActive(false);
        }
    }
}
