using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockPanelController : MonoBehaviour
{
    [SerializeField]
    List<GameObject> buttons;
    private void Awake()
    {
        GameEvents.DinoUp.AddListener(UnlockButtons);
        UnlockButtons(UserDataController.GetBiggestDino());
    }

    
    void UnlockButtons(int biggestDino)
    {
        if(biggestDino < 2) //PONER A 8
        {
            int unlockIndex = 7;
            switch (biggestDino)
            {
                case 0:
                case 1:
                case 2:
                    unlockIndex = 0;
                    break;
                case 3:
                case 4:
                    unlockIndex = 2;
                    break;
                case 5:
                    unlockIndex = 4;
                    break;
                case 6:
                    unlockIndex = 6;
                    break;
                case 7:
                    unlockIndex = 7;
                    break;
            }
            for (int i = 0; i < buttons.Count; i++)
            {
                if (i < unlockIndex)
                {
                    buttons[i].SetActive(true);
                }
                else
                {
                    buttons[i].SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].SetActive(true);
            }
        }
    }
}
