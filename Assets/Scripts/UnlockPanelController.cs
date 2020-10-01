using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockPanelController : MonoBehaviour
{
    [SerializeField]
    List<GameObject> buttons;
    private void Awake()
    {
        UnlockButtons(UserDataController.GetBiggestDino());
        GameEvents.DinoUp.AddListener(UnlockButtons);
    }
    
    void UnlockButtons(int biggestDino)
    {
        if (biggestDino == 0)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].SetActive(false);
            }
        }

        else
        {
            if (biggestDino < 7)
            {
                int unlockIndex = 0;
                switch (biggestDino)
                {
                    case 0:
                    case 1:
                        break;
                    case 2:
                        unlockIndex = 1;
                        break;
                    case 3:
                        unlockIndex = 2;
                        break;
                    case 4:
                        unlockIndex = 4;
                        break;
                    case 5:
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
        if (UserDataController.IsVipUser())
        {
            buttons[3].SetActive(false); //CAMBIAR SEGUN ORDEN
        }
    }
}
