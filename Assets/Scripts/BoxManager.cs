using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    [SerializeField]
    CellManager _cellManager;
    [SerializeField]
    GameObject[] _boxPrefabs;
    [SerializeField]
    MainGameSceneController _mainGameSceneController;

    float _waitingTime;
    float _currentTime;
    bool canDrop = false;

    private void Start()
    {
        _waitingTime = Random.Range(15f, 30f);
        if (UserDataController._currentUserData._tutorialCompleted[7])
        {
            canDrop = true;
        }
    }
    public void CreateBox(int cellIndex, int dinoType)
    {
        GameObject box = Instantiate(_boxPrefabs[0], _cellManager.GetCellPosition(cellIndex), Quaternion.identity);
        _cellManager.GetCellInstanceByIndex(cellIndex).SetBox(dinoType, box);
        UserDataController.CreateBox(cellIndex, dinoType);
    }
    public void DropBox(int dinotype)
    {
        int firstEmptyCell = _mainGameSceneController.GetFirstEmptyCell();
        if (firstEmptyCell >= 0)
        {
            CreateBox(firstEmptyCell, dinotype);
        }
    }
    public void DropBox()
    {
        int firstEmptyCell = _mainGameSceneController.GetFirstEmptyCell();
        if (firstEmptyCell >= 0)
        {
            CreateBox(firstEmptyCell, 0);
        }
    }
    public void SetDropTrue()
    {
        canDrop = true;
    }
    void Update()
    {
        if (canDrop)
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= _waitingTime)
            {
                DropBox();
                _currentTime = 0f;
                _waitingTime = Random.Range(15f, 30f);
            }
        }
    }
}
