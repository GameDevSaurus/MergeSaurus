using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    GameObject cellPrefab;
    [SerializeField]
    GameObject expoPanelPrefab;
    GameObject expoPrefab;
    [SerializeField]
    GameObject panelPrefab;
    int rows, cols;
    [SerializeField]
    int cellCount;
    [SerializeField]
    float verticalDist, horizontalDist;
    [SerializeField]
    float padding;
    [SerializeField]
    StreetManager _streetManager;
    List<List<int>> _cellPositionList;
    MainGameSceneController _mainGameSceneController;
    float panelWidth;
    float panelHeight;
    float expoSize = 1.5f;
    float expoPanelWidth;
    float expoPanelHeight;
    [SerializeField]
    SpriteRenderer backgroundTile;
    List<GameObject> _cells;
    List<ExpositorInstance> _expositors;

    private void Awake()
    {
        _mainGameSceneController = FindObjectOfType<MainGameSceneController>();
        GameEvents.LevelUp.AddListener(LevelUpCallBack);

        string cellPath = Application.productName + "/Environment/Cell";
        string expositorPath = Application.productName + "/Environment/Expositor";
        string bgPath = Application.productName + "/Environment/Background";
        cellPrefab = Resources.Load<GameObject>(cellPath);
        expoPrefab = Resources.Load<GameObject>(expositorPath);
        backgroundTile.sprite = Resources.Load<Sprite>(bgPath);

        panelWidth = 3 + (4f * horizontalDist);
        panelHeight = 5 + (6f * verticalDist);
        expoPanelWidth = panelWidth + (4f * padding) + (2f * expoSize);
        expoPanelHeight = panelHeight + (4f * padding) + (2f * expoSize);

        Camera c = Camera.main;
        float cameraHeight = (expoPanelHeight / 7f) * 10;
        float cameraWidth = c.aspect * cameraHeight;
        if(cameraWidth > 9)
        {
            c.orthographicSize = (expoPanelHeight / 7f) * 5f;
        }
        else
        {
            c.orthographicSize = (9f / c.aspect)/2f;
        }
        c.orthographicSize += 0.5f;
       
        float backGroundSize = c.orthographicSize * 2f;
        backgroundTile.gameObject.transform.localScale = new Vector3(backGroundSize * (c.aspect*(16/9f)), backGroundSize, 1);
        backgroundTile.gameObject.transform.position = new Vector3(0, -0.3f, 0);


        _cellPositionList = new List<List<int>>();
        _cellPositionList.Add(new List<int>() { 2, 2 });
        _cellPositionList.Add(new List<int>() { 2, 2, 1 });
        _cellPositionList.Add(new List<int>() { 2, 2, 2 });
        _cellPositionList.Add(new List<int>() { 2, 2, 2, 1 });
        _cellPositionList.Add(new List<int>() { 2, 2, 2, 2 });
        _cellPositionList.Add(new List<int>() { 2, 2, 2, 2, 1 });
        _cellPositionList.Add(new List<int>() { 2, 2, 2, 2, 2 });
        _cellPositionList.Add(new List<int>() { 3, 2, 2, 2, 2 });
        _cellPositionList.Add(new List<int>() { 3, 3, 2, 2, 2 });
        _cellPositionList.Add(new List<int>() { 3, 3, 3, 2, 2 });
        _cellPositionList.Add(new List<int>() { 3, 3, 3, 3, 2 });
        _cellPositionList.Add(new List<int>() { 3, 3, 3, 3, 3 });

        SetCellNumber(UserDataController._currentUserData._unlockedCells);
    }

    public void LevelUpCallBack(int lvl)
    {
        switch (lvl)
        {
            case 2:
            case 3:
            case 6:
                AddCell();
                AddExpositor();
                break;
            case 4:
            case 5:
            case 7:
            case 9:
            case 11:
            case 15:
            case 20:
                AddCell();
                break;
            case 8:
            case 10:
               // AddExpositor();
                break;
        }
    }

    public void SetCellNumber(int nCells)
    {
        _cells = new List<GameObject>();
        _expositors = new List<ExpositorInstance>();
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        cellCount = nCells;
        rows = _cellPositionList[cellCount - 4].Count;

        float verticalOrigin = panelHeight / 2f;
        float totalVerticalSpace = panelHeight - (float)rows;
        float verticalCellDistance = totalVerticalSpace / ((float)rows + 1f);

        for (int i = 0; i < rows; i++)
        {
            cols = _cellPositionList[cellCount - 4][i];

            float horizontalOrigin = -panelWidth / 2f;
            float totalHorizontalSpace = panelWidth - (float)cols;
            float horizontalCellDistance = totalHorizontalSpace / ((float)cols + 1f);

            for (int j = 0; j < cols; j++)
            {
                GameObject cell = Instantiate(cellPrefab, new Vector3(horizontalOrigin + horizontalCellDistance + 0.5f + ((horizontalCellDistance + 1f)*j), verticalOrigin - verticalCellDistance - 0.5f - ((verticalCellDistance + 1f) * i), 0.9f), Quaternion.identity);
                cell.transform.SetParent(transform);
                cell.name = i + " - " + j;
                cell.GetComponent<CellInstance>().SetCell(_cells.Count);
                _cells.Add(cell);
            }
        }
        //GameObject panel = Instantiate(panelPrefab, new Vector3(0,0,1), Quaternion.identity);
        //panel.transform.localScale = new Vector2(panelWidth, panelHeight);
        //panel.name = "DinoPanel";
        //GameObject expoPanel = Instantiate(expoPanelPrefab, new Vector3(0, 0, 2), Quaternion.identity);
        //expoPanel.transform.localScale = new Vector2(expoPanelWidth, expoPanelHeight);
        //expoPanel.name = "ExpoPanel";

        float xExpoPosition = (panelWidth / 2) + (((expoPanelWidth / 2) - (panelWidth / 2)) / 2);
        float yExpoPosition = (panelHeight/2) + (((expoPanelHeight / 2) - (panelHeight / 2)) / 2);

        GameObject expositorUp = Instantiate(expoPrefab, new Vector3(0, yExpoPosition, 2), Quaternion.identity);

        GameObject expositorDown = Instantiate(expoPrefab, new Vector3(0, -yExpoPosition, 2), Quaternion.identity);

        GameObject expositorUpRightCorner = Instantiate(expoPrefab, new Vector3(xExpoPosition, yExpoPosition, 2), Quaternion.identity);

        GameObject expositorDownRightCorner = Instantiate(expoPrefab, new Vector3(xExpoPosition, -yExpoPosition, 2), Quaternion.identity);

        GameObject expositorUpLeftCorner = Instantiate(expoPrefab, new Vector3(-xExpoPosition, yExpoPosition, 2), Quaternion.identity);

        GameObject expositorDownLeftCorner = Instantiate(expoPrefab, new Vector3(-xExpoPosition, -yExpoPosition, 2), Quaternion.identity);

        GameObject expositorUpLeftMiddle = Instantiate(expoPrefab, new Vector3(-xExpoPosition, (yExpoPosition*2f/3f)/2f, 2), Quaternion.identity);

        GameObject expositorDownLeftMiddle = Instantiate(expoPrefab, new Vector3(-xExpoPosition, -(yExpoPosition * 2f / 3f) / 2f, 2), Quaternion.identity);

        GameObject expositorUpRightMiddle = Instantiate(expoPrefab, new Vector3(xExpoPosition, (yExpoPosition * 2f / 3f) / 2f, 2), Quaternion.identity);

        GameObject expositorDownRightMiddle = Instantiate(expoPrefab, new Vector3(xExpoPosition, -(yExpoPosition * 2f / 3f) / 2f, 2), Quaternion.identity);

        _expositors.Add(expositorUpLeftMiddle.GetComponent<ExpositorInstance>());
        _expositors.Add(expositorUpRightMiddle.GetComponent<ExpositorInstance>());
        _expositors.Add(expositorDownLeftMiddle.GetComponent<ExpositorInstance>());
        _expositors.Add(expositorDownRightMiddle.GetComponent<ExpositorInstance>());
        _expositors.Add(expositorUp.GetComponent<ExpositorInstance>());
        //_expositors.Add(expositorDown.GetComponent<ExpositorInstance>());
        _expositors.Add(expositorUpLeftCorner.GetComponent<ExpositorInstance>());
        _expositors.Add(expositorUpRightCorner.GetComponent<ExpositorInstance>());
        //_expositors.Add(expositorDownLeftCorner.GetComponent<ExpositorInstance>());
        //_expositors.Add(expositorDownRightCorner.GetComponent<ExpositorInstance>());
        for (int i = 0; i < _expositors.Count; i++)
        {
            _expositors[i].SetExpositor(i);
        }
        for (int i = 0; i < _expositors.Count; i++)
        {
            if(i >= UserDataController._currentUserData._unlockedExpositors)
            {
                _expositors[i].gameObject.SetActive(false);
            }
        }
        _streetManager.Init(_expositors);
    }

    public void SetCells(int n)
    {
        cellCount += n;
        SetCellNumber(cellCount);
    }

    public Vector2 GetCellPosition(int nCell)
    {
        return _cells[nCell].transform.position;
    }
    public CellInstance GetCellInstanceByIndex(int index)
    {
        return _cells[index].GetComponent<CellInstance>();
    }
    public ExpositorInstance GetExpoInstanceByIndex(int index)
    {
        return _expositors[index];
    }

    public void SetDinosaurInCell(DinosaurInstance dinosaur, int cell)
    {
        _cells[cell].GetComponent<CellInstance>().SetDinosaur(dinosaur);
    }

    public void AddCell()
    {
        cellCount++;
        rows = _cellPositionList[cellCount - 4].Count;

        float verticalOrigin = panelHeight / 2f;
        float totalVerticalSpace = panelHeight - (float)rows;
        float verticalCellDistance = totalVerticalSpace / ((float)rows + 1f);

        int cellCounter = 0;
        for (int i = 0; i < rows; i++)
        {
            cols = _cellPositionList[cellCount - 4][i];

            float horizontalOrigin = -panelWidth / 2f;
            float totalHorizontalSpace = panelWidth - (float)cols;
            float horizontalCellDistance = totalHorizontalSpace / ((float)cols + 1f);

            for (int j = 0; j < cols; j++)
            {
                if(cellCounter >= _cells.Count)
                {
                    GameObject cell = Instantiate(cellPrefab, new Vector3(horizontalOrigin + horizontalCellDistance + 0.5f + ((horizontalCellDistance + 1f) * j), verticalOrigin - verticalCellDistance - 0.5f - ((verticalCellDistance + 1f) * i), 0.9f), Quaternion.identity);
                    cell.transform.SetParent(transform);
                    cell.name = i + " - " + j;
                    cell.GetComponent<CellInstance>().SetCell(_cells.Count);
                    _cells.Add(cell);
                }
                else
                {
                    _cells[cellCounter].transform.position = new Vector3(horizontalOrigin + horizontalCellDistance + 0.5f + ((horizontalCellDistance + 1f) * j), verticalOrigin - verticalCellDistance - 0.5f - ((verticalCellDistance + 1f) * i), 0.9f);
                }
                cellCounter++;
            }
        }
        _mainGameSceneController.UpdatePositions();
        UserDataController.AddCell();
    }

    public void AddExpositor()
    {
        UserDataController.AddExpositor();
        for (int i = 0; i < _expositors.Count; i++)
        {
            if (i >= UserDataController._currentUserData._unlockedExpositors)
            {
                _expositors[i].gameObject.SetActive(false);
            }
            else
            {
                _expositors[i].gameObject.SetActive(true);
            }
        }
    }
    public List<GameObject> GetCellInstances()
    {
        return _cells;
    }
}
