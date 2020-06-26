using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinosaurInstance : MonoBehaviour
{
    int _dinoType;
    int _cellIndex;
    bool _dragging;
    Camera _camera;
    [SerializeField]
    GameObject _otherCell;
    MainGameSceneController _mainGameSceneController;
    CellManager _cellManager;
    bool _clicking;
    int _nClicks;
    bool _working;
    Coroutine _clickCr;

    void Start()
    {
        _camera = Camera.main;
        _mainGameSceneController = FindObjectOfType<MainGameSceneController>();
        _cellManager = FindObjectOfType<CellManager>();
    }

    void Update()
    {
        Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        if (_dragging)
        {
            transform.position = mousePos;
        }
    }

    IEnumerator DisableClickingState()
    {
        yield return new WaitForSeconds(0.25f);
        _clicking = false;
        _nClicks = 0;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_dragging)
        {
            if (col.CompareTag("Cell") && col.gameObject.GetComponent<CellInstance>().GetCellNumber() != _cellIndex)
            {
                _otherCell = col.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (_dragging)
        {
            if (col.CompareTag("Cell"))
            {
                _otherCell = null;
            }
        }
    }
    private void OnMouseDown()
    {
        if (CurrentSceneManager._canPickDinosaur  && !_working)
        {
            _dragging = true;
            _otherCell = null;
        }

        if(_clickCr != null)
        {
            StopCoroutine(_clickCr);
        }
        _clicking = true;
        _clickCr = StartCoroutine(DisableClickingState());

    }

    public void StartWorking()
    {
        if (CurrentSceneManager._canWorkDinosaur)
        {
            _working = true;
            GameEvents.WorkDino.Invoke();
            GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
    public void StopWorking()
    {
        _working = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    private void OnMouseUp()
    {
        if (_clicking)
        {
            if ((transform.position - (Vector3)_cellManager.GetCellPosition(_cellIndex)).magnitude < 1f)
            {
                if (_working)
                {
                    StopWorking();
                }
                else
                {
                    _nClicks++;
                    if(_nClicks == 2)
                    {
                        _nClicks = 0;
                        StartWorking();
                    }
                }
            }
        }
        _clicking = false;

        _dragging = false;
        if(_otherCell != null)
        {
            CellInstance collisionCell = _otherCell.GetComponent<CellInstance>();
            if (collisionCell.GetDinosaur() == 0)
            {
                if (CurrentSceneManager._canMoveDinosaur)
                {
                    UserDataController.MoveDinosaur(_cellIndex, collisionCell.GetCellNumber());
                    _cellManager.SetDinosaurInCell(0, _cellIndex);
                    SetCell(collisionCell.GetCellNumber());
                    collisionCell.SetDinosaur(_dinoType);
                }
            }
            else
            {
                if(collisionCell.GetDinosaur() == _dinoType)
                {
                    if (CurrentSceneManager._canMergeDinosaur)
                    {
                        _mainGameSceneController.Merge(this, collisionCell.GetCellNumber());
                    }
                }
            }
        }
        _mainGameSceneController.UpdatePositions();
    }
    public void SetCell(int nCell)
    {
        _cellIndex = nCell;
    }
    public void SetDino(int nDino)
    {
        _dinoType = nDino;
    }
    public int GetDinosaur()
    {
        return _dinoType;
    }
    public int GetCellNumber()
    {
        return _cellIndex;
    }
}
