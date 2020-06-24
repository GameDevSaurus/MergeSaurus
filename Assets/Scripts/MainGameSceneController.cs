using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameSceneController : MonoBehaviour
{
    [SerializeField]
    GameObject[] _dinoPrefabs;
    [SerializeField]
    List<GameObject> _dinoIngame;
    [SerializeField]
    CellManager _cellManager;

    private void Start()
    {
        UpdateDinosaurs();
    }
    public void FastPurchase(int dinosaurIndex, int cost)
    {
        UserDataController.BuyDinosaur(dinosaurIndex, cost);
        GameEvents.FastPurchase.Invoke();
        UpdateDinosaurs();
    }
    public void UpdateDinosaurs()
    {
        _dinoIngame = new List<GameObject>();
        for(int i = 0; i< UserDataController._currentUserData._unlockedCells; i++)
        {
            if (UserDataController._currentUserData._dinosaurs[i] > 0)
            {
                GameObject dino = Instantiate(_dinoPrefabs[UserDataController._currentUserData._dinosaurs[i] -1], _cellManager.GetCellPosition(i), Quaternion.identity);
                _dinoIngame.Add(dino);
            }
            
        }
    }
}
