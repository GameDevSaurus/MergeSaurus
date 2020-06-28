using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinosaurInstance : MonoBehaviour
{
    int _dinoType;
    int _cellIndex;
    [SerializeField]
    GameObject _otherCell;
    bool _working;

    public void StartWorking()
    {
        _working = true;
        GameEvents.WorkDino.Invoke();
        GetComponent<SpriteRenderer>().color = Color.green;
    }
    public void StopWorking()
    {
        _working = false;
        GetComponent<SpriteRenderer>().color = Color.white;
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
    public bool IsWorking()
    {
        return _working;
    }
}
