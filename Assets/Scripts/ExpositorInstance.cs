using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpositorInstance : MonoBehaviour
{
    int _expositorNumber;
    MainGameSceneController _mainSceneController;
    CellInstance _referencedCell;
    [SerializeField]
    SpriteRenderer dinoImage;

    private void Start()
    {
        _mainSceneController = FindObjectOfType<MainGameSceneController>();
    }
    public void ShowDinosaur(CellInstance cellInstance)
    {
        _referencedCell = cellInstance;
        dinoImage.sprite = cellInstance.GetDinoInstance().GetComponent<SpriteRenderer>().sprite;
    }
    public void SetExpositor(int expoNumber)
    {
        _expositorNumber = expoNumber;
    }
    public DinosaurInstance GetDinoInstance()
    {
        return _referencedCell.GetDinoInstance();
    }
    public int GetExpositorNumber()
    {
        return _expositorNumber;
    }
    private void OnMouseEnter()
    {
        _mainSceneController.EnterExpositor(this);
    }
    private void OnMouseExit()
    {
        _mainSceneController.ExitExpositor();
    }
}
