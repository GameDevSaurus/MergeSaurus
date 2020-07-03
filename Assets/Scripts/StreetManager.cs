using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetManager : MonoBehaviour
{
    [SerializeField]
    float _streetCellDistance;
    [SerializeField]
    GameObject _streetPointPrefab;
    [SerializeField]
    GameObject _touristPrefab;
    List<List<Vector3>> _streetPointsPositons;
    List<List<int>> _expositorsMatrix;
    List<Vector2> _expoMatrixCoordinates;

    public enum Directions { Up, Down, Left, Right };

    List<List<List<List<Directions>>>> _directionsManager; 

    private void Start()
    {
        _directionsManager = new List<List<List<List<Directions>>>>();
        _expositorsMatrix = new List<List<int>>();
        _expositorsMatrix.Add(new List<int>() { 6, 4, 7 });
        _expositorsMatrix.Add(new List<int>() { 0, -1, 1 });
        _expositorsMatrix.Add(new List<int>() { 2, -1, 3 });
        _expositorsMatrix.Add(new List<int>() { 8, 5, 9 });

        _expoMatrixCoordinates = new List<Vector2>();
        _expoMatrixCoordinates.Add(new Vector2(1,0));
        _expoMatrixCoordinates.Add(new Vector2(1,2));
        _expoMatrixCoordinates.Add(new Vector2(2,0));
        _expoMatrixCoordinates.Add(new Vector2(2,2));
        _expoMatrixCoordinates.Add(new Vector2(0,1));
        _expoMatrixCoordinates.Add(new Vector2(3,1));
        _expoMatrixCoordinates.Add(new Vector2(0,0));
        _expoMatrixCoordinates.Add(new Vector2(0,2));
        _expoMatrixCoordinates.Add(new Vector2(3,0));
        _expoMatrixCoordinates.Add(new Vector2(3, 2));

        // ZERO  / / / / / / /
        List<List<List<Directions>>> _directionsListManagerZero = new List<List<List<Directions>>>();
        for (int i = 0; i < 10; i++)
        {
            _directionsListManagerZero.Add(new List<List<Directions>>());
        }
        _directionsListManagerZero[0].Add(new List<Directions>());
        _directionsListManagerZero[1].Add(new List<Directions>() { Directions.Up, Directions.Right, Directions.Right, Directions.Down });
        _directionsListManagerZero[2].Add(new List<Directions>() { Directions.Down });
        _directionsListManagerZero[3].Add(new List<Directions>() { Directions.Up, Directions.Right, Directions.Right, Directions.Down, Directions.Down });
        _directionsListManagerZero[3].Add(new List<Directions>() { Directions.Down, Directions.Down, Directions.Right, Directions.Right, Directions.Up });
        _directionsListManagerZero[4].Add(new List<Directions>() { Directions.Up, Directions.Right });
        _directionsListManagerZero[5].Add(new List<Directions>() { Directions.Down, Directions.Down, Directions.Right });
        _directionsListManagerZero[6].Add(new List<Directions>() { Directions.Up });
        _directionsListManagerZero[7].Add(new List<Directions>() { Directions.Up, Directions.Right, Directions.Right });
        _directionsListManagerZero[8].Add(new List<Directions>() { Directions.Down, Directions.Down });
        _directionsListManagerZero[9].Add(new List<Directions>() { Directions.Down, Directions.Down, Directions.Right, Directions.Right });
        _directionsManager.Add(_directionsListManagerZero);
        // ONE  / / / / / / /

        List<List<List<Directions>>> _directionsListManagerOne = new List<List<List<Directions>>>();
        for (int i = 0; i < 10; i++)
        {
            _directionsListManagerOne.Add(new List<List<Directions>>());
        }

        _directionsListManagerOne[0].Add(new List<Directions>() { Directions.Up, Directions.Left, Directions.Left, Directions.Down });
        _directionsListManagerOne[1].Add(new List<Directions>());
        _directionsListManagerOne[2].Add(new List<Directions>() { Directions.Up, Directions.Left, Directions.Left, Directions.Down, Directions.Down });
        _directionsListManagerOne[2].Add(new List<Directions>() { Directions.Down, Directions.Down, Directions.Left, Directions.Left, Directions.Up });
        _directionsListManagerOne[3].Add(new List<Directions>() { Directions.Down });
        _directionsListManagerOne[4].Add(new List<Directions>() { Directions.Up, Directions.Left });
        _directionsListManagerOne[5].Add(new List<Directions>() { Directions.Down, Directions.Down, Directions.Left });
        _directionsListManagerOne[6].Add(new List<Directions>() { Directions.Up, Directions.Left, Directions.Left });
        _directionsListManagerOne[7].Add(new List<Directions>() { Directions.Up });
        _directionsListManagerOne[8].Add(new List<Directions>() { Directions.Down, Directions.Down, Directions.Left, Directions.Left });
        _directionsListManagerOne[9].Add(new List<Directions>() { Directions.Down, Directions.Down });
        _directionsManager.Add(_directionsListManagerOne);
        // TWO  / / / / / / /

        List<List<List<Directions>>> _directionsListManagerTwo = new List<List<List<Directions>>>();
        for (int i = 0; i < 10; i++)
        {
            _directionsListManagerTwo.Add(new List<List<Directions>>());
        }

        _directionsListManagerTwo[0].Add(new List<Directions>() { Directions.Up });
        _directionsListManagerTwo[1].Add(new List<Directions>() { Directions.Up, Directions.Up, Directions.Right, Directions.Right, Directions.Down });
        _directionsListManagerTwo[1].Add(new List<Directions>() { Directions.Down, Directions.Right, Directions.Right, Directions.Up, Directions.Up });
        _directionsListManagerTwo[2].Add(new List<Directions>());
        _directionsListManagerTwo[3].Add(new List<Directions>() { Directions.Down, Directions.Right, Directions.Right, Directions.Up });
        _directionsListManagerTwo[4].Add(new List<Directions>() { Directions.Up, Directions.Up, Directions.Right });
        _directionsListManagerTwo[5].Add(new List<Directions>() { Directions.Down, Directions.Right });
        _directionsListManagerTwo[6].Add(new List<Directions>() { Directions.Up, Directions.Up });
        _directionsListManagerTwo[7].Add(new List<Directions>() { Directions.Up, Directions.Up, Directions.Right, Directions.Right });
        _directionsListManagerTwo[8].Add(new List<Directions>() { Directions.Down });
        _directionsListManagerTwo[9].Add(new List<Directions>() { Directions.Down, Directions.Right, Directions.Right });
        _directionsManager.Add(_directionsListManagerTwo);
        // THREE  / / / / / / /

        List<List<List<Directions>>> _directionsListManagerThree = new List<List<List<Directions>>>();
        for (int i = 0; i < 10; i++)
        {
            _directionsListManagerThree.Add(new List<List<Directions>>());
        }

        _directionsListManagerThree[0].Add(new List<Directions>() { Directions.Up, Directions.Up, Directions.Left, Directions.Left, Directions.Down });
        _directionsListManagerThree[0].Add(new List<Directions>() { Directions.Down, Directions.Left, Directions.Left, Directions.Up, Directions.Up });
        _directionsListManagerThree[1].Add(new List<Directions>() { Directions.Up});
        _directionsListManagerThree[2].Add(new List<Directions>() { Directions.Down, Directions.Left, Directions.Left, Directions.Up});
        _directionsListManagerThree[3].Add(new List<Directions>());
        _directionsListManagerThree[4].Add(new List<Directions>() { Directions.Up, Directions.Up, Directions.Left });
        _directionsListManagerThree[5].Add(new List<Directions>() { Directions.Down, Directions.Left});
        _directionsListManagerThree[6].Add(new List<Directions>() { Directions.Up, Directions.Up, Directions.Left, Directions.Left});
        _directionsListManagerThree[7].Add(new List<Directions>() { Directions.Up, Directions.Up });
        _directionsListManagerThree[8].Add(new List<Directions>() { Directions.Down, Directions.Left, Directions.Left });
        _directionsListManagerThree[9].Add(new List<Directions>() { Directions.Down });
        _directionsManager.Add(_directionsListManagerThree);

        // FOUR  / / / / / / /

    }

    public void Init(List<ExpositorInstance> expositors)
    {
        _streetPointsPositons = new List<List<Vector3>>();
        for (int i = 0; i<expositors.Count; i++)
        {
            List<Vector3> aux = new List<Vector3>();
            GameObject botMidPoint = Instantiate(_streetPointPrefab, expositors[i].transform.position + (Vector3.down * _streetCellDistance), _streetPointPrefab.transform.rotation);
            GameObject botLeftPoint = Instantiate(_streetPointPrefab, expositors[i].transform.position + (Vector3.down * _streetCellDistance) + (Vector3.left * _streetCellDistance), _streetPointPrefab.transform.rotation);
            GameObject botRightPoint = Instantiate(_streetPointPrefab, expositors[i].transform.position + (Vector3.down * _streetCellDistance) + (Vector3.right * _streetCellDistance), _streetPointPrefab.transform.rotation);

            //GameObject leftMidPoint = Instantiate(_streetPointPrefab, expositors[i].transform.position + (Vector3.left * _streetCellDistance), _streetPointPrefab.transform.rotation);
            //GameObject rightMidPoint = Instantiate(_streetPointPrefab, expositors[i].transform.position + (Vector3.right * _streetCellDistance), _streetPointPrefab.transform.rotation);
            //GameObject topMidPoint = Instantiate(_streetPointPrefab, expositors[i].transform.position + (Vector3.up * _streetCellDistance), _streetPointPrefab.transform.rotation);
            //_directionsList = new List<Directions>().Add(Directions.Up, );
            GameObject topLeftPoint = Instantiate(_streetPointPrefab, expositors[i].transform.position + (Vector3.up * _streetCellDistance) + (Vector3.left * _streetCellDistance), _streetPointPrefab.transform.rotation);
            GameObject topRightPoint = Instantiate(_streetPointPrefab, expositors[i].transform.position + (Vector3.up * _streetCellDistance) + (Vector3.right * _streetCellDistance), _streetPointPrefab.transform.rotation);
            
            aux.Add(botMidPoint.transform.position);
            aux.Add(topLeftPoint.transform.position);
            aux.Add(topRightPoint.transform.position);
            aux.Add(botLeftPoint.transform.position);
            aux.Add(botRightPoint.transform.position);
            
            _streetPointsPositons.Add(aux);
        }
    }

    public List<int> CreateRoute()
    {
        List<int> streetPointsIndex = new List<int>();

        for (int i = 0; i < 4; i++)
        {
            streetPointsIndex.Add(i);
        }

        for(int i = 0; i<15; i++)
        {
            int firstIndex = Random.Range(0,streetPointsIndex.Count);
            int secondIndex = Random.Range(0, streetPointsIndex.Count);
            int aux = streetPointsIndex[firstIndex];
            streetPointsIndex[firstIndex] = streetPointsIndex[secondIndex];
            streetPointsIndex[secondIndex] = aux;
        }
        return streetPointsIndex;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnTourist();
        }
    }

    public void  SpawnTourist()
    {
        List<int> route = CreateRoute();
        GameObject nTourist = Instantiate(_touristPrefab, Vector3.zero, Quaternion.identity);
        nTourist.GetComponent<TouristInstance>().SetRoute(route, this);  
    }

    public Vector3 GetExpositorTransformPosByCoords(int expoIndex, int pointIndex)
    {
        return _streetPointsPositons[expoIndex][pointIndex];
    }

    public Vector2 GetExpoCoordsByIndex(int expositorIndex)
    {
        return _expoMatrixCoordinates[expositorIndex];
    }

    public int GetExpoIndexByCoords(Vector2 coords)
    {
        return _expositorsMatrix[(int)coords.x][(int)coords.y];
    }

    public List<Directions> GetDirectionsList(int origin, int dest)
    {
        int dirIndex = Random.Range(0, _directionsManager[origin][dest].Count);
        return _directionsManager[origin][dest][dirIndex];
    }
}
