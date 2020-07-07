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
    GameObject dinoCopy;

    private void Awake()
    {
        _economyManager = FindObjectOfType<EconomyManager>();
        GameEvents.TouristWatchDino.AddListener(EarnMoney);
    }
    private void Start()
    {
        _mainSceneController = FindObjectOfType<MainGameSceneController>();
    }
    public void ShowDinosaur(CellInstance cellInstance)
    {
        _referencedCell = cellInstance;
        dinoCopy = Instantiate(_referencedCell.GetDinoInstance().gameObject, transform.position, Quaternion.identity);
        Destroy(dinoCopy.GetComponent<DinosaurInstance>());
    }
    public void SetReferencedCell(CellInstance targetCell)
    {
        _referencedCell = targetCell;
        if(targetCell != null)
        {
            Destroy(dinoCopy);
            dinoCopy = Instantiate(targetCell.GetDinoInstance().gameObject, transform.position, Quaternion.identity);
            dinoCopy.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }
    }

    public void HideDinosaur()
    {
        _referencedCell = null;
        Destroy(dinoCopy);
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
    public void EarnMoney(int expoIndex)
    {
        if(_referencedCell != null)
        {
            int dinoType = _referencedCell.GetDinoInstance().GetDinosaur();
            if (expoIndex == _expositorNumber)
            {
                if (dinoType >= 0)
                {
                    GameCurrency currentDinoEarnings = new GameCurrency(_economyManager.GetEarningsByType(dinoType).GetIntList());
                    _economyManager.EarnSoftCoins(currentDinoEarnings);
                    GameEvents.EarnMoney.Invoke(new GameEvents.MoneyEventData(transform.position, currentDinoEarnings));
                }
            }
        }
    }
}
