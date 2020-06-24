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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_dragging)
        {
            if (col.CompareTag("Cell"))
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
        _dragging = true;
        _otherCell = null;
    }
    private void OnMouseUp()
    {
        _dragging = false;
        if(_otherCell != null)
        {
            CellInstance collisionCell = _otherCell.GetComponent<CellInstance>();
            if (collisionCell.GetDinosaur() == 0)
            {
                UserDataController.MoveDinosaur(_cellIndex, collisionCell.GetCellNumber());
                _cellManager.SetDinosaurInCell(0 ,_cellIndex);
                SetCell(collisionCell.GetCellNumber());
                collisionCell.SetDinosaur(_dinoType);
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
