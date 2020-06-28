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
    EconomyManager _economyManager;
    List<int> _earningsTime = new List<int>() { 5, 4, 4, 3, 3, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
    Coroutine _workingCr;

    private void Awake()
    {
        _economyManager = FindObjectOfType<EconomyManager>();
    }
    private void Start()
    {
        _mainSceneController = FindObjectOfType<MainGameSceneController>();
    }
    public void ShowDinosaur(CellInstance cellInstance)
    {
        _referencedCell = cellInstance;
        dinoImage.sprite = cellInstance.GetDinoInstance().GetComponent<SpriteRenderer>().sprite;
        if (_workingCr != null)
        {
            StopCoroutine(WorkingCr());
        }
        _workingCr = StartCoroutine(WorkingCr());
    }
    public void HideDinosaur()
    {
        _referencedCell = null;
        dinoImage.sprite = null;
        StopCoroutine(_workingCr);
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
    public IEnumerator WorkingCr()
    {
        int dinoType = _referencedCell.GetDinoInstance().GetDinosaur();
        yield return new WaitForSeconds(_earningsTime[dinoType]);
        GameEvents.EarnMoney.Invoke(new GameEvents.MoneyEventData(transform.position, _earningsTime[dinoType] * _economyManager.GetEarningsByType(dinoType)));
        _economyManager.EarnSoftCoins(_earningsTime[dinoType] * _economyManager.GetEarningsByType(dinoType));
        _workingCr = StartCoroutine(WorkingCr());
    }
}
