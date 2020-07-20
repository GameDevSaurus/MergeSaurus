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
    TextMeshProUGUI txSProfits, txTProfits, txLevel;
    [SerializeField]
    Button[] _panelButtons;
    [SerializeField]
    EconomyManager _economyManager;
    [SerializeField]
    GameObject[] _profilePanels;
    [SerializeField]
    AnimationCurve animationCurve;
    [SerializeField]
    Image[] _avatarFaces;
    [SerializeField]
    Image _avatar;
    [SerializeField]
    Button _sfxButton, _musicButton;
    bool _sfxState = true, _musicState = true;
    [SerializeField]
    GameObject _selectedBorderPrefab;
    GameObject _currentSelectedBorder;
    [SerializeField]
    PanelManager _panelManager;
    bool profileOpen = false;

    public void OpenProfile()
    {
        if (!profileOpen)
        {
            _panelManager.RequestShowPanel(_mainPanel);
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
            txLevel.text = UserDataController.GetLevel().ToString();

            for(int i = 0; i < UserDataController.GetDinoAmount(); i++)
            {
                _avatarFaces[i].sprite = Resources.Load<Sprite>(Application.productName + "/Sprites/FaceSprites/" + i);
                if (i > UserDataController.GetBiggestDino())
                {
                    _avatarFaces[i].color = Color.black;
                }
                else
                {
                    _avatarFaces[i].color = Color.white;
                }
            }
            _avatar.sprite = Resources.Load<Sprite>(Application.productName + "/Sprites/FaceSprites/" + UserDataController.GetPlayerAvatar());
            _currentSelectedBorder = Instantiate(_selectedBorderPrefab, _avatarFaces[UserDataController.GetPlayerAvatar()].transform.parent);
        }
    }
    public void CloseProfile()
    {
        _panelManager.ClosePanel();
        profileOpen = false;
    }

    public void ChooseAvatar(int avatarIndex)
    {
        if(avatarIndex <= UserDataController.GetBiggestDino())
        {
            UserDataController.SetPlayerAvatar(avatarIndex);
            Destroy(_currentSelectedBorder);
            _currentSelectedBorder = Instantiate(_selectedBorderPrefab, _avatarFaces[UserDataController.GetPlayerAvatar()].transform.parent);
        }
        else
        {
            GameEvents.ShowAdvice.Invoke(new GameEvents.AdviceEventData("ADVICE_NOT_UNLOCKED"));
        }
    }

    public void SFXButton()
    {
        _sfxState = !_sfxState;
        if (_sfxState)
        {
            _sfxButton.GetComponent<Image>().color = Color.white;
        }
        else
        {
            _sfxButton.GetComponent<Image>().color = Color.black;
        }
    }
    public void MusicButton()
    {
        _musicState = !_musicState;
        if (_musicState)
        {
            _musicButton.GetComponent<Image>().color = Color.white;
        }
        else
        {
            _musicButton.GetComponent<Image>().color = Color.black;
        }
    }

    public void OpenPanel(int panel)
    {
        for(int i = 0; i<_profilePanels.Length; i++)
        {
            _profilePanels[i].SetActive(false);
            _panelButtons[i].GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 1f);
        }
        _profilePanels[panel].SetActive(true);
        _panelButtons[panel].GetComponent<Image>().color = new Color(0.1f, 0.7f, 0.07f, 1f); ;
        _avatar.sprite = Resources.Load<Sprite>(Application.productName + "/Sprites/FaceSprites/" + UserDataController.GetPlayerAvatar());
    }
}
