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
    [SerializeField]
    Tutorial _tutorial;
    DinosaurInstance _pickedDinosaur;
    CellInstance _currentCell;
    ExpositorInstance _currentExpositor;
    bool _isPicking;
    Camera _camera;

    private void Start()
    {
        CreateStartingDinosaurs();
        _camera = Camera.main;
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
                    _cellManager.SetDinosaurInCell(dinoInst, i);
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
                _cellManager.SetDinosaurInCell(dinoInst, i);
                _dinoIngame.Add(dinoInst);
                if (UserDataController._currentUserData._workingCellsByExpositor[i] >= 0)
                {
                    ShowDinosaur(i, UserDataController._currentUserData._workingCellsByExpositor[i]);
                }
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
        _cellManager.SetDinosaurInCell(null, dinoInstance1.GetCellNumber());
        _cellManager.SetDinosaurInCell(dinoInst, dinoInstance2.GetCellNumber());
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

    private void Update()
    {
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
                        ShowDinosaur(_pickedDinosaur.GetCellNumber(), _currentExpositor.GetExpositorNumber());
                    }
                }
                else
                {
                    if(_currentCell.GetDinoInstance() != null)
                    {
                        if (_currentCell.GetDinoInstance().GetDinosaur() == _pickedDinosaur.GetDinosaur())
                        {
                            if (CurrentSceneManager._canMergeDinosaur)
                            {
                                Merge(_pickedDinosaur, _currentCell.GetCellNumber());
                            }
                        }
                    }
                    else
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
        _cellManager.GetCellInstanceByIndex(cell).ExposeDinosaur(_currentExpositor);
        _pickedDinosaur.StartWorking();
        UserDataController.ShowCell(cell, expo);
    }
}
