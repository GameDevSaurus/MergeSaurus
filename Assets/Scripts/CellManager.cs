using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    [SerializeField]
    GameObject cellPrefab;
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
    float targetWidth;
    float targetHeight;
    int ncells = 4;

    List<GameObject> _cells;

    private void Awake()
    {
        panelWidth = 5 + (4 * horizontalDist);
        panelHeight = 5 + (4 * verticalDist);
        targetWidth = padding + panelWidth;
        targetHeight = panelHeight;
        Camera.main.GetComponent<CameraAutoSize>().SetWidth(targetWidth);

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

        SetCellNumber(4);
    }

    public void SetCellNumber(int nCells)
    {
        _cells = new List<GameObject>();
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        cellCount = nCells;
        rows = _cellPositionList[cellCount - 4].Count;
        Vector2 centerOfCells = new Vector2((panelWidth / 2f), (panelHeight / 2f));

        float verticalCellDistance = panelHeight / ((float)rows + 1f);

        for (int i = 0; i < rows; i++)
        {
            cols = _cellPositionList[cellCount - 4][i];
            float horizontalCellDistance = panelWidth / ((float)cols + 1f);
            for (int j = 0; j < cols; j++)
            {
                GameObject dinopanel = Instantiate(cellPrefab, new Vector2(((j + 1) * horizontalCellDistance) - panelWidth / 2f, panelHeight / 2f - ((i + 1) * verticalCellDistance)), Quaternion.identity);
                dinopanel.transform.SetParent(transform);
                dinopanel.name = i + " - " + j;
                _cells.Add(dinopanel);
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
}
