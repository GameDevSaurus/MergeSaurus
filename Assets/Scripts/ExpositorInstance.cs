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
    bool _clicking;

    private void Start()
    {
        _mainSceneController = FindObjectOfType<MainGameSceneController>();
    }
    public void ShowDinosaur(CellInstance cellInstance)
    {
        _referencedCell = cellInstance;
        dinoImage.sprite = cellInstance.GetDinoInstance().GetComponent<SpriteRenderer>().sprite;
    }
    public void HideDinosaur()
    {
        _referencedCell = null;
        dinoImage.sprite = null;
    }
    public void SetExpositor(int expoNumber)
    {
        _expositorNumber = expoNumber;
    }
    public DinosaurInstance GetDinoInstance()
    {
        if(_referencedCell != null)
        {
            return _referencedCell.GetDinoInstance();
        }
        else
        {
            return null;
        }
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
        _clicking = false;
    }

    private void OnMouseDown()
    {
        if (_referencedCell != null)
        {
            _clicking = true;
            StartCoroutine(DisableClickingState());
        }
    }
    private void OnMouseUp()
    {
        //if (_referencedCell != null && _clicking)
        //{
        //    if (CurrentSceneManager._canTakeBackByExpositor)
        //    {
        //        _mainSceneController.StopShowDino(_referencedCell.GetCellNumber());
        //    }
        //}
    }
    IEnumerator DisableClickingState()
    {
        yield return new WaitForSeconds(0.25f);
        _clicking = false;
    }
}
