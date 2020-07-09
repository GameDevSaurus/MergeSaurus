using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;

    [SerializeField]
    GameObject[] _profilePanels;

    public void OpenProfile()
    {
        _mainPanel.SetActive(true);
        OpenPanel(0);
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
}
