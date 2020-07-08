﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainGameSceneController : MonoBehaviour
{
    [SerializeField]
    GameObject[] _dinoPrefabs;
    List<DinosaurInstance> _dinosIngame;
    [SerializeField]
    CellManager _cellManager;
    [SerializeField]
    Tutorial _tutorial;
    DinosaurInstance _pickedDinosaur;
    CellInstance _currentCell;
    ExpositorInstance _currentExpositor;
    bool _isPicking;
    Camera _camera;
    EconomyManager _economyManager;
    bool waitingForAnimation = false;
    [SerializeField]
    BoxManager _boxManager;
    TrashBin _trashBin;
    int deleteCount = 0;
    float deleteTimer = 0;

    private void Awake()
    {
        _economyManager = FindObjectOfType<EconomyManager>();
        _trashBin = FindObjectOfType<TrashBin>();
    }
    private void Start()
    {
        CreateStartingDinosaurs();
        _camera = Camera.main;
    }
    public void Purchase(int dinosaurIndex, GameCurrency cost)
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
                    _cellManager.SetDinosaurInCell(dinoInst, i);
                    _dinosIngame.Add(dinoInst);
                    break;
                }
            }
            UserDataController.BuyDinosaur(dinosaurIndex, cost);
            GameEvents.Purchase.Invoke(dinosaurIndex);
        }
    }
    public void CreateStartingDinosaurs()
    {
        _dinosIngame = new List<DinosaurInstance>();
        for(int i = 0; i<UserDataController._currentUserData._unlockedCells; i++)
        {
            int dinoType = UserDataController._currentUserData._dinosaurs[i];
            if (dinoType >= 0 && dinoType < 100)
            {
                CreateDinosaur(i , dinoType);
                int expositor = UserDataController.GetExpositorIndexByCell(i);
                if (expositor >= 0)
                {
                    ShowDinosaur(i, expositor);
                }
            }
            else
            {
                if(dinoType >= 100 && dinoType < 200)
                {
                    _boxManager.CreateBox(BoxManager.BoxType.StandardBox, i, dinoType-100);
                }
                else
                {
                    if (dinoType >= 200 && dinoType < 300)
                    {
                        _boxManager.CreateBox(BoxManager.BoxType.RewardedBox, i, dinoType - 200);
                    }
                    else{
                        if(dinoType == 300)
                        {
                            UserDataController.CreateDinosaur(i, -1);
                        }
                    }
                }
            }
        }
        _economyManager.SetDinosInGame(_dinosIngame);
    }

    public void CreateDinosaur(int cellIndex, int dinoType)
    {
        GameObject dino = Instantiate(_dinoPrefabs[dinoType], _cellManager.GetCellPosition(cellIndex), Quaternion.identity);
        DinosaurInstance dinoInst = dino.GetComponent<DinosaurInstance>();
        dinoInst.SetCell(cellIndex);
        dinoInst.SetDino(dinoType);
        _cellManager.SetDinosaurInCell(dinoInst, cellIndex);
        _dinosIngame.Add(dinoInst);
        UserDataController.CreateDinosaur(cellIndex, dinoType);
    }

    public void UpdatePositions() 
    {
        foreach (GameObject c in _cellManager.GetCellInstances())
        {
            CellInstance cellInst = c.GetComponent<CellInstance>();
            GameObject cellBox = cellInst.HaveBox();
            if (cellBox != null)
            {
                cellBox.transform.position = _cellManager.GetCellPosition(cellInst.GetCellNumber());
            }
            else
            {
                if(cellInst.GetDinoInstance() != null)
                {
                    cellInst.GetDinoInstance().transform.position = _cellManager.GetCellPosition(cellInst.GetCellNumber());
                }
            }         
        }
    }

    public int GetDinosSum()
    {
        int sum = 0;
        for(int i = 0; i<_dinosIngame.Count; i++)
        {
            if(_dinosIngame[i].GetDinosaur() == 0)
            {
                sum++;
            }
            if (_dinosIngame[i].GetDinosaur() > 0)
            {
                sum += (int)Mathf.Pow(2, _dinosIngame[i].GetDinosaur());
            }
        }
        return sum;
    }
    public void Merge(DinosaurInstance dinoInstance1, int targetCellIndex)
    {
        GameObject dino = Instantiate(_dinoPrefabs[dinoInstance1.GetDinosaur()+1], _cellManager.GetCellPosition(targetCellIndex), Quaternion.identity);
        DinosaurInstance dinoInstance2 = GetDinoInstanceByCell(targetCellIndex);
        DinosaurInstance dinoInst = dino.GetComponent<DinosaurInstance>();
        dinoInst.SetDino(dinoInstance1.GetDinosaur()+1);
        dinoInst.SetCell(targetCellIndex);
        _dinosIngame.Add(dinoInst);
        _dinosIngame.Remove(dinoInstance1);
        _dinosIngame.Remove(dinoInstance2);

        if(UserDataController.MergeDinosaurs(dinoInstance1.GetCellNumber(), dinoInstance2.GetCellNumber(), dinoInstance1.GetDinosaur()))
        {
            StartCoroutine(WaitForUnlockNewDino( dinoInstance1, dinoInst, dinoInstance2, targetCellIndex, dino ));
        }
        else
        {
            EndMerge(dinoInstance1, dinoInst, dinoInstance2, targetCellIndex, dino);
        }
    }

    public void Swap(DinosaurInstance dino1, DinosaurInstance dino2)
    {
        int auxDino1Cell = dino1.GetCellNumber();
        int auxDino2Cell = dino2.GetCellNumber();
        UserDataController.MoveDinosaur(auxDino1Cell, auxDino2Cell);
        _cellManager.SetDinosaurInCell(dino1, auxDino2Cell);
        _cellManager.SetDinosaurInCell(dino2, auxDino1Cell);
        dino1.SetCell(auxDino2Cell);
        dino2.SetCell(auxDino1Cell);
        if (dino2.IsWorking())
        {
            UserDataController.ShowCell(dino2.GetCellNumber(), _cellManager.GetCellInstanceByIndex(auxDino2Cell).GetTargetExpositor().GetExpositorNumber());
            UserDataController.StopShowCell(auxDino2Cell);
            _cellManager.GetCellInstanceByIndex(auxDino1Cell).SetExpositor(_cellManager.GetCellInstanceByIndex(auxDino2Cell).GetTargetExpositor());
            _cellManager.GetCellInstanceByIndex(auxDino2Cell).GetTargetExpositor().SetReferencedCell(_cellManager.GetCellInstanceByIndex(auxDino1Cell));
            _cellManager.GetCellInstanceByIndex(auxDino2Cell).SetExpositor(null);
        }
    }

    IEnumerator WaitForUnlockNewDino(DinosaurInstance dinoInstance1, DinosaurInstance dinoInst, DinosaurInstance dinoInstance2, int targetCellIndex, GameObject dino)
    {
        waitingForAnimation = true;
        while (waitingForAnimation)
        {
            yield return null;
        }
        EndMerge(dinoInstance1, dinoInst, dinoInstance2, targetCellIndex, dino);
    }

    public void StopWaitingAnim()
    {
        waitingForAnimation = false;
    }
    public void EndMerge(DinosaurInstance dinoInstance1, DinosaurInstance dinoInst, DinosaurInstance dinoInstance2, int targetCellIndex, GameObject dino)
    {
        _cellManager.SetDinosaurInCell(null, dinoInstance1.GetCellNumber());
        _cellManager.SetDinosaurInCell(dinoInst, dinoInstance2.GetCellNumber());
        Destroy(dinoInstance1.gameObject);
        Destroy(dinoInstance2.gameObject);
        GameEvents.MergeDino.Invoke(dinoInstance1.GetDinosaur() + 1);
    }
    DinosaurInstance GetDinoInstanceByCell(int cell)
    {
        foreach(DinosaurInstance d in _dinosIngame)
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
        return _dinosIngame[0].transform.position;
    }
    public void DeleteGameData()
    {
        deleteCount++;
        deleteTimer = 0;
        if(deleteCount == 10)
        {
            GameEvents.LoadScene.Invoke("Reset");
        }
    }

    public Vector3 GetDinoPositionsUIByCell(int cellIndex)
    {
        return Camera.main.WorldToScreenPoint(_cellManager.GetCellPosition(cellIndex));
    }

    private void Update()
    {
        deleteTimer += Time.deltaTime;
        if(deleteTimer > 2f)
        {
            deleteCount = 0;
        }
        Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        if (_isPicking)
        {
            _pickedDinosaur.transform.position = new Vector3(mousePos.x, mousePos.y, _pickedDinosaur.transform.position.z);
            if (Input.GetMouseButtonUp(0))
            {
                _isPicking = false;

                if (_currentCell == null)
                {
                    if (_currentExpositor != null)
                    {
                        if(_currentExpositor.GetDinoInstance() == null)
                        {
                            ShowDinosaur(_pickedDinosaur.GetCellNumber(), _currentExpositor.GetExpositorNumber());
                        }
                    }
                    else
                    {
                        if (_trashBin.IsOverTrashBin())
                        {
                            _economyManager.EarnSoftCoins(_economyManager.GetInitialCost(_pickedDinosaur.GetDinosaur()));
                            UserDataController.DeleteDino(_pickedDinosaur.GetCellNumber());
                            _dinosIngame.Remove(_pickedDinosaur);
                            Destroy(_pickedDinosaur.gameObject);
                        }
                    }
                }
                else
                {
                    if(_currentCell.GetCellNumber() != _pickedDinosaur.GetCellNumber())
                    {
                        if (_currentCell.GetDinoInstance() != null)
                        {
                            if (_currentCell.GetDinoInstance().GetDinosaur() == _pickedDinosaur.GetDinosaur())
                            {
                                if (_currentCell.GetDinoInstance().IsWorking())
                                {
                                    StopShowDino(_currentCell.GetCellNumber());
                                    if (CurrentSceneManager._canMergeDinosaur)
                                    {
                                        Merge(_pickedDinosaur, _currentCell.GetCellNumber());
                                    }
                                }
                                else
                                {
                                    if (CurrentSceneManager._canMergeDinosaur)
                                    {
                                        Merge(_pickedDinosaur, _currentCell.GetCellNumber());
                                    }
                                }                        
                            }
                            else
                            {
                                Swap(_pickedDinosaur, _currentCell.GetDinoInstance());
                            }
                        }
                        else
                        {
                            if (_currentCell.GetBoxNumber() < 0)
                            {
                                if (CurrentSceneManager._canMoveDinosaur)
                                {
                                    UserDataController.MoveDinosaur(_pickedDinosaur.GetCellNumber(), _currentCell.GetCellNumber());
                                    _cellManager.SetDinosaurInCell(null, _pickedDinosaur.GetCellNumber());
                                    _pickedDinosaur.SetCell(_currentCell.GetCellNumber());
                                    _currentCell.SetDinosaur(_pickedDinosaur);
                                }
                            }
                        }
                    }
                }
                _currentCell = null;
                UpdatePositions();
            }
        }
    }
    public void PickDinosaur(DinosaurInstance pickedDino) 
    {
        _pickedDinosaur = pickedDino;
        _isPicking = true;
    }
    public void EnterCell(CellInstance enteredCell)
    {
        if (_isPicking)
        {
            _currentCell = enteredCell;
        }
    }
    public void ExitCell()
    {
        _currentCell = null;
    }
    public void EnterExpositor(ExpositorInstance enteredExpo)
    {
        if (_isPicking)
        {
            _currentExpositor = enteredExpo;
        }
    }
    public void ExitExpositor()
    {
        _currentExpositor = null;
    }
    public void ShowDinosaur(int cell, int expo)
    {
        _cellManager.GetCellInstanceByIndex(cell).ExposeDinosaur(_cellManager.GetExpoInstanceByIndex(expo));
        _cellManager.GetCellInstanceByIndex(cell).GetDinoInstance().StartWorking();
        UserDataController.ShowCell(cell, expo);
    }
    public void ShowDinosaurInFirstExpo(int cell)
    {
        if(UserDataController.GetEmptyExpositors() > 0)
        {
            if (CurrentSceneManager._canShowDinosaurByTouch)
            {
                ShowDinosaur(cell, GetFirstEmptyExpositor());
            }
        }
        else
        {
            GameEvents.ShowAdvice.Invoke("ADVICE_NOEXPOSITORS");
        }
    }
    
    public void StopShowDino(int cell)
    {
        UserDataController.StopShowCell(_cellManager.GetCellInstanceByIndex(cell).GetTargetExpositor().GetExpositorNumber());
        _cellManager.GetCellInstanceByIndex(cell).StopExpose();
        _cellManager.GetCellInstanceByIndex(cell).GetDinoInstance().StopWorking();
        GameEvents.TakeBack.Invoke();
    }
    
    public int GetFirstEmptyExpositor()
    {
        int selectedExpo = -1;
        for (int i = 0; i < UserDataController._currentUserData._unlockedExpositors; i++)
        {
            if (UserDataController._currentUserData._workingCellsByExpositor[i] == -1)
            {
                selectedExpo = i;
                break;
            }
        }
        return selectedExpo;
    }
    public int GetFirstEmptyCell()
    {
        int selectedCell = -1;
        for (int i = 0; i < UserDataController._currentUserData._unlockedCells; i++)
        {
            if (UserDataController._currentUserData._dinosaurs[i] < 0)
            {
                selectedCell = i;
                break;
            }
        }
        return selectedCell;
    }
    public int GetFirstWorkerCell()
    {
        int selectedCell = -1;
        for (int i = 0; i < UserDataController._currentUserData._unlockedCells; i++)
        {
            int expositor = UserDataController.GetExpositorIndexByCell(i);
            if (expositor  >= 0)
            {
                selectedCell = i;
                break;
            }
        }
        return selectedCell;
    }

    public List<int> GetMergeableDinosCellIndex()
    {
        int selectedDino1;
        int selectedDino2;
        int selectedCell1 = -1;
        int selectedCell2 = -1;
        bool founded = false;

        for (int i = 0; i < UserDataController._currentUserData._unlockedCells && !founded; i++)
        {
            selectedDino1 = UserDataController._currentUserData._dinosaurs[i];
            if (selectedDino1 >= 0 && selectedDino1 < 100)
            {
                for (int j = i+1; j < UserDataController._currentUserData._unlockedCells && !founded; j++)
                {
                    selectedDino2 = UserDataController._currentUserData._dinosaurs[j];
                    if (selectedDino1 == selectedDino2)
                    {
                        selectedCell1 = i;
                        selectedCell2 = j;
                        founded = true;
                    }
                }
            }
        }
        if (founded)
        {
            return new List<int>() {selectedCell1, selectedCell2};
        }
        else
        {
            return null;
        }
    }
}
