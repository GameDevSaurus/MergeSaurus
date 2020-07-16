using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    [SerializeField]
    TextMeshProUGUI[] txExtras;
    [SerializeField]
    TextMeshProUGUI txSProfits, txTProfits;
    [SerializeField]
    Button[] _panelButtons;
    [SerializeField]
    EconomyManager _economyManager;
    [SerializeField]
    GameObject[] _profilePanels;
    [SerializeField]
    AnimationCurve animationCurve;
    bool profileOpen = false;
    public void OpenProfile()
    {
        if (!profileOpen)
        {
            StartCoroutine(CrOpen());
            profileOpen = true;
            string extra0 = string.Format(LocalizationController.GetValueByKey("PROFILE_EXTRAS_1"), (5 * UserDataController.GetDiscountUpgradeLevel()));
            txExtras[0].text = extra0;

            string extra1 = string.Format(LocalizationController.GetValueByKey("PROFILE_EXTRAS_2"), (7 * UserDataController.GetExtraEarningsLevel()));
            txExtras[1].text = extra1;

            string extra2 = string.Format(LocalizationController.GetValueByKey("PROFILE_EXTRAS_3"), (6 * UserDataController.GetDiscountUpgradeLevel()));
            txExtras[2].text = extra2;

            string extra3 = string.Format(LocalizationController.GetValueByKey("PROFILE_EXTRAS_4"), (4 * UserDataController.GetDiscountUpgradeLevel()));
            txExtras[3].text = extra3;

            txSProfits.text = _economyManager.GetEarningsPerSecond();
            txTProfits.text = UserDataController.GetTotalEarnings().GetCurrentMoney();
        }
    }
    public void CloseProfile()
    {
        _mainPanel.SetActive(false);
        profileOpen = false;
    }

    public void OpenPanel(int panel)
    {
        for(int i = 0; i<_profilePanels.Length; i++)
        {
            _profilePanels[i].SetActive(false);
            _panelButtons[i].GetComponent<Image>().color = Color.gray;
        }
        _profilePanels[panel].SetActive(true);
        _panelButtons[panel].GetComponent<Image>().color = Color.green;
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
