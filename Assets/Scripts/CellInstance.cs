using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellInstance : MonoBehaviour
{
    int _cellNumber;
    DinosaurInstance _placedDino;
    MainGameSceneController _mainSceneController;
    ExpositorInstance _targetExpositor;

    private void Start()
    {
        _mainSceneController = FindObjectOfType<MainGameSceneController>();    
    }
    public void SetDinosaur(DinosaurInstance dinosaur)
    {
        _placedDino = dinosaur;
    }
    public void ExposeDinosaur(ExpositorInstance targetExpositor)
    {
        _targetExpositor = targetExpositor;
        _targetExpositor.ShowDinosaur(this);
    }
    public void SetCell(int cellNumber)
    {
        _cellNumber = cellNumber;
    }
    public DinosaurInstance GetDinoInstance()
    {
        return _placedDino;
    }
    public int GetCellNumber()
    {
        return _cellNumber;
    }

    private void OnMouseDown()
    {
        if(_placedDino != null) 
        {
            if (!_placedDino.IsWorking())
            {
                _mainSceneController.PickDinosaur(_placedDino);
            }
        }
    }
    private void OnMouseEnter()
    {
        _mainSceneController.EnterCell(this);
    }
    private void OnMouseExit()
    {
        _mainSceneController.ExitCell();
    }
}
