﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameSceneController : MonoBehaviour
{
    [SerializeField]
    GameObject[] _dinoPrefabs;
    List<DinosaurInstance> _dinoIngame;
    [SerializeField]
    CellManager _cellManager;
    [SerializeField]
    Tutorial _tutorial;

    private void Start()
    {
        CreateStartingDinosaurs();
    }
    public void FastPurchase(int dinosaurIndex, int cost)
    {
        if (CurrentSceneManager._canPurchase)
        {
            for (int i = 0; i < UserDataController._currentUserData._unlockedCells; i++)
            {
                if (UserDataController._currentUserData._dinosaurs[i] == -1)
                {
                    GameObject dino = Instantiate(_dinoPrefabs[dinosaurIndex], _cellManager.GetCellPosition(i), Quaternion.identity);
                    DinosaurInstance dinoInst = dino.GetComponent<DinosaurInstance>();
                    dinoInst.SetCell(i);
                    dinoInst.SetDino(dinosaurIndex);
                    _cellManager.SetDinosaurInCell(dinosaurIndex, i);
                    _dinoIngame.Add(dinoInst);
                    break;
                }
            }
            UserDataController.BuyDinosaur(dinosaurIndex, cost);
            GameEvents.FastPurchase.Invoke();
        }
    }
    public void CreateStartingDinosaurs()
    {
        _dinoIngame = new List<DinosaurInstance>();
        for(int i = 0; i<UserDataController._currentUserData._unlockedCells; i++)
        {
            int dinoType = UserDataController._currentUserData._dinosaurs[i];
            if (dinoType >= 0)
            {
                GameObject dino = Instantiate(_dinoPrefabs[dinoType], _cellManager.GetCellPosition(i), Quaternion.identity);
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

    public void Merge(DinosaurInstance dinoInstance1, int targetCellIndex)
    {
        GameObject dino = Instantiate(_dinoPrefabs[dinoInstance1.GetDinosaur()+1], _cellManager.GetCellPosition(targetCellIndex), Quaternion.identity);
        DinosaurInstance dinoInstance2 = GetDinoInstanceByCell(targetCellIndex);
        DinosaurInstance dinoInst = dino.GetComponent<DinosaurInstance>();
        dinoInst.SetDino(dinoInstance1.GetDinosaur()+1);
        dinoInst.SetCell(targetCellIndex);
        _dinoIngame.Add(dinoInst);
        _dinoIngame.Remove(dinoInstance1);
        _dinoIngame.Remove(dinoInstance2);
        UserDataController.MergeDinosaurs(dinoInstance1.GetCellNumber(), dinoInstance2.GetCellNumber(), dinoInstance1.GetDinosaur());
        _cellManager.SetDinosaurInCell(-1, dinoInstance1.GetCellNumber());
        _cellManager.SetDinosaurInCell(dinoInstance1.GetDinosaur()+1, dinoInstance2.GetCellNumber());
        Destroy(dinoInstance1.gameObject);
        Destroy(dinoInstance2.gameObject);
        GameEvents.MergeDino.Invoke();
    }

    DinosaurInstance GetDinoInstanceByCell(int cell)
    {
        foreach(DinosaurInstance d in _dinoIngame)
        {
            if(d.GetCellNumber() == cell)
            {
                return d;
            }
        }
        return null;
    }

    public Vector2 GetFirstDinoPosition()
    {
        return _dinoIngame[0].transform.position;
    }
    public void DeleteGameData()
    {
        UserDataController.DeleteFile();
        GameEvents.LoadScene.Invoke("Splash");
    }

    public Vector3 GetDinoPositionsUIByCell(int cellIndex)
    {
        return Camera.main.WorldToScreenPoint(_cellManager.GetCellPosition(cellIndex));
    }
}
