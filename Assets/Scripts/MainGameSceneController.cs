using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameSceneController : MonoBehaviour
{
    [SerializeField]
    GameObject[] _dinoPrefabs;
    List<DinosaurInstance> _dinoIngame;
    [SerializeField]
    CellManager _cellManager;

    private void Start()
    {
        CreateDinosaurs();
    }
    public void FastPurchase(int dinosaurIndex, int cost)
    {
        UserDataController.BuyDinosaur(dinosaurIndex, cost);
        GameEvents.FastPurchase.Invoke();
        UpdatePositions();
    }
    public void CreateDinosaurs()
    {
        _dinoIngame = new List<DinosaurInstance>();
        for(int i = 0; i<UserDataController._currentUserData._unlockedCells; i++)
        {
            int dinoType = UserDataController._currentUserData._dinosaurs[i];
            if (dinoType > 0)
            {
                GameObject dino = Instantiate(_dinoPrefabs[dinoType -1], _cellManager.GetCellPosition(i), Quaternion.identity);
                DinosaurInstance dinoInst = dino.GetComponent<DinosaurInstance>();
                dinoInst.SetCell(i);
                dinoInst.SetDino(dinoType);
                _cellManager.SetDinosaurInCell(dinoType, i);
                _dinoIngame.Add(dinoInst);
            }
        }
    }
    public void UpdatePositions() 
    {
        foreach (DinosaurInstance d in _dinoIngame)
        {
            d.transform.position = _cellManager.GetCellPosition(d.GetCellNumber());
        }
    }
}
