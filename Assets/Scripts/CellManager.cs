using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    [SerializeField]
    GameObject cellPrefab;    
    [SerializeField]
    GameObject expoPanelPrefab;    
    [SerializeField]
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

    List<List<int>> _cellPositionList;

    float panelWidth;
    float panelHeight;
    float expoSize = 1.5f;
    float expoPanelWidth;
    float expoPanelHeight;
    int ncells = 4;

    List<GameObject> _cells;
    List<ExpositorInstance> _expositors;

    private void Awake()
    {
        panelWidth = 3 + (4f * horizontalDist);
        panelHeight = 5 + (6f * verticalDist);
        expoPanelWidth = panelWidth + (4f * padding) + (2f * expoSize);
        expoPanelHeight = panelHeight + (4f * padding) + (2f * expoSize);

        float cameraHeight = (expoPanelHeight / 7f) * 10;
        float cameraWidth = Camera.main.aspect * cameraHeight;
        if(cameraWidth > 9)
        {
            Camera.main.orthographicSize = (expoPanelHeight / 7f) * 5f;
        }
        else
        {
            Camera.main.orthographicSize = (9f / Camera.main.aspect)/2f;
        }


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
        GameObject panel = Instantiate(panelPrefab, new Vector3(0,0,1), Quaternion.identity);
        panel.transform.localScale = new Vector2(panelWidth, panelHeight);
        panel.name = "DinoPanel";
        GameObject expoPanel = Instantiate(expoPanelPrefab, new Vector3(0, 0, 2), Quaternion.identity);
        expoPanel.transform.localScale = new Vector2(expoPanelWidth, expoPanelHeight);
        expoPanel.name = "ExpoPanel";

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
        _expositors.Add(expositorDown.GetComponent<ExpositorInstance>());
        _expositors.Add(expositorUpLeftCorner.GetComponent<ExpositorInstance>());
        _expositors.Add(expositorUpRightCorner.GetComponent<ExpositorInstance>());
        _expositors.Add(expositorDownLeftCorner.GetComponent<ExpositorInstance>());
        _expositors.Add(expositorDownRightCorner.GetComponent<ExpositorInstance>());
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
    }

    public void SetCells(int n)
    {
        ncells += n;
        SetCellNumber(ncells);
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
}
