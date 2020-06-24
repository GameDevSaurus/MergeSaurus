using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellInstance : MonoBehaviour
{
    int _dinosaur = 0;
    int _cellNumber;

    public void SetDinosaur(int dinosaur)
    {
        _dinosaur = dinosaur;
    }
    public void SetCell(int cellNumber)
    {
        _cellNumber = cellNumber;
    }
    public int GetDinosaur()
    {
        return _dinosaur;
    }
    public int GetCellNumber()
    {
        return _cellNumber;
    }
}
