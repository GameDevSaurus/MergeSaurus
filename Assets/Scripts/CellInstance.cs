using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellInstance : MonoBehaviour
{
    int _cellNumber;
    DinosaurInstance _placedDino;
    MainGameSceneController _mainSceneController;
    ExpositorInstance _targetExpositor;
    bool _clicking;
    int _nClicks;
    Coroutine _clickCr;
    int _boxNumber;
    GameObject _currentBox;

    private void Awake()
    {
        _boxNumber = -1;
    }
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
    public void StopExpose()
    {
        _targetExpositor.HideDinosaur();
        _targetExpositor = null;
        _placedDino.StopWorking();
    }
    public void SetCell(int cellNumber)
    {
        _cellNumber = cellNumber;
    }
    public void SetBox(int boxNumber, GameObject box)
    {
        _boxNumber = boxNumber;
        _currentBox = box;
    }
    public DinosaurInstance GetDinoInstance()
    {
        return _placedDino;
    }
    public int GetCellNumber()
    {
        return _cellNumber;
    }
    public int GetBoxNumber()
    {
        return _boxNumber;
    }

    private void OnMouseDown()
    {
        if(_placedDino != null) 
        {
            if (CurrentSceneManager._canPickDinosaur)
            {
                if (!_placedDino.IsWorking())
                {
                    _mainSceneController.PickDinosaur(_placedDino);
                }
            }
        }
        if (_clickCr != null)
        {
            StopCoroutine(_clickCr);
        }
        _clicking = true;
        _clickCr = StartCoroutine(DisableClickingState());
    }
    private void OnMouseEnter()
    {
        _mainSceneController.EnterCell(this);
    }
    private void OnMouseExit()
    {
        _mainSceneController.ExitCell();
        _clicking = false;
        _nClicks = 0;
    }
    IEnumerator DisableClickingState()
    {
        yield return new WaitForSeconds(0.25f);
        _clicking = false;
        _nClicks = 0;
    }
    private void OnMouseUp()
    {
        if (_clicking)
        {
            if(_placedDino != null)
            {
                if (_placedDino.IsWorking())
                {
                    if (CurrentSceneManager._canTakeBackByCell)
                    {
                        _mainSceneController.StopShowDino(_cellNumber);
                    }
                }
                else
                {
                    _nClicks++;
                    if (_nClicks == 2)
                    {
                        _nClicks = 0;
                        _mainSceneController.ShowDinosaurInFirstExpo(_cellNumber);
                    }
                }
            }
            else
            {
                if (_boxNumber >= 0)
                {
                    OpenBox();
                }
            }
        }
        _clicking = false;
    }

    public ExpositorInstance GetTargetExpositor()
    {
        return _targetExpositor;
    }

    public GameObject HaveBox()
    {
        return _currentBox;
    }
    public void OpenBox()
    {
        if (CurrentSceneManager._canOpenBox)
        {
            Destroy(_currentBox);
            _currentBox = null;
            _mainSceneController.CreateDinosaur(_cellNumber, _boxNumber);
            _boxNumber = -1;
            GameEvents.OpenBox.Invoke();
        }
    }
}
