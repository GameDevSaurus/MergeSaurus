using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    [SerializeField]
    CellManager _cellManager;
    [SerializeField]
    GameObject _standardBox;
    [SerializeField]
    GameObject _lootBox;
    [SerializeField]
    GameObject _rewardBox;

    [SerializeField]
    MainGameSceneController _mainGameSceneController;

    float _standardWaitingTime;
    float _standardCurrentTime;
    float _lootCurrentTime;
    float _lootWaitingTime;
    bool canDrop = false;

    public enum BoxType {StandardBox, LootBox, RewardedBox };

    private void Start()
    {
        _standardWaitingTime = Random.Range(15f, 30f);
        _lootWaitingTime = Random.Range(60f, 120f);
        if (UserDataController._currentUserData._tutorialCompleted[7])
        {
            canDrop = true;
        }
    }
    public void CreateBox(BoxType boxType, int cellIndex, int dinoType)
    {
        GameObject boxToInstantiate = null;
        switch (boxType)
        {
            case BoxType.StandardBox:
                boxToInstantiate = _standardBox;
                break;

            case BoxType.LootBox:
                boxToInstantiate = _lootBox;
                break;

            case BoxType.RewardedBox:
                boxToInstantiate = _rewardBox;
                break;
        }
        
        GameObject box = Instantiate(boxToInstantiate, _cellManager.GetCellPosition(cellIndex), Quaternion.identity);
        _cellManager.GetCellInstanceByIndex(cellIndex).SetBox(boxType, dinoType, box);
        UserDataController.CreateBox(cellIndex, dinoType);
    }
    public bool DropStandardBox()
    {
        int biggestDino = UserDataController.GetBiggestDino();
        int standardDropDino = Mathf.Max(biggestDino - 5, 0);
        int firstEmptyCell = _mainGameSceneController.GetFirstEmptyCell();
        bool canDropStandardBox = true; 
        if (firstEmptyCell >= 0)
        {
            CreateBox(BoxType.StandardBox, firstEmptyCell, standardDropDino);
        }
        else
        {
            canDropStandardBox = false;
        }
        return canDropStandardBox;
        
    }
    public bool DropSpecificBox(int dinotype)
    {
        int firstEmptyCell = _mainGameSceneController.GetFirstEmptyCell();
        bool canDropStandardBox = true;
        if (firstEmptyCell >= 0)
        {
            CreateBox(BoxType.StandardBox, firstEmptyCell, dinotype);
        }
        else
        {
            canDropStandardBox = false;
        }
        return canDropStandardBox;

    }
    public bool DropLootBox()
    {
        int firstEmptyCell = _mainGameSceneController.GetFirstEmptyCell();
        bool canDropLootBox = true;
        if (firstEmptyCell >= 0)
        {
            CreateBox(BoxType.LootBox, firstEmptyCell, 0);
        }
        else
        {
            canDropLootBox = false;
        }
        return canDropLootBox;
    }

    public void SetDropTrue()
    {
        canDrop = true;
    }
    void Update()
    {
        if (canDrop)
        {
            _lootCurrentTime += Time.deltaTime;
            _standardCurrentTime += Time.deltaTime;
            if(_lootCurrentTime >= _lootWaitingTime)
            {
                if (DropLootBox())
                {
                    _lootCurrentTime = 0f;
                    _lootWaitingTime = Random.Range(60f, 120f);
                }
            }
            if (_standardCurrentTime >= _standardWaitingTime)
            {
                if (DropStandardBox())
                {
                    _standardCurrentTime = 0f;
                    _standardWaitingTime = Random.Range(15f, 30f);
                }
            }
        }
    }
}
