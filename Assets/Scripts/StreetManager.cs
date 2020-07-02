using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetManager : MonoBehaviour
{
    [SerializeField]
    float _streetCellDistance;
    [SerializeField]
    GameObject _streetPointPrefab;
    List<Vector3> _streetPointsPositons;
    enum Directions { Up, Down, Left, Right };

    List<List<List<Directions>>> _directionsListManager;

    private void Start()
    {
        _directionsListManager = new List<List<List<Directions>>>();
        for (int i = 0; i < 10; i++)
        {
            _directionsListManager.Add(new List<List<Directions>>());
        }

        _directionsListManager[0].Add(new List<Directions>() { Directions.Up, Directions.Right, Directions.Right, Directions.Down });
        _directionsListManager[0].Add(new List<Directions>() { Directions.Down });




    }

















    public void Init(List<ExpositorInstance> expositors)
    {
        for (int i = 0; i<expositors.Count; i++)
        {
            GameObject botMidPoint = Instantiate(_streetPointPrefab, expositors[i].transform.position + (Vector3.down * _streetCellDistance), _streetPointPrefab.transform.rotation);
            GameObject botLeftPoint = Instantiate(_streetPointPrefab, expositors[i].transform.position + (Vector3.down * _streetCellDistance) + (Vector3.left * _streetCellDistance), _streetPointPrefab.transform.rotation);
            GameObject botRightPoint = Instantiate(_streetPointPrefab, expositors[i].transform.position + (Vector3.down * _streetCellDistance) + (Vector3.right * _streetCellDistance), _streetPointPrefab.transform.rotation);

            //GameObject leftMidPoint = Instantiate(_streetPointPrefab, expositors[i].transform.position + (Vector3.left * _streetCellDistance), _streetPointPrefab.transform.rotation);
            //GameObject rightMidPoint = Instantiate(_streetPointPrefab, expositors[i].transform.position + (Vector3.right * _streetCellDistance), _streetPointPrefab.transform.rotation);
            //GameObject topMidPoint = Instantiate(_streetPointPrefab, expositors[i].transform.position + (Vector3.up * _streetCellDistance), _streetPointPrefab.transform.rotation);
            //_directionsList = new List<Directions>().Add(Directions.Up, );
            GameObject topLeftPoint = Instantiate(_streetPointPrefab, expositors[i].transform.position + (Vector3.up * _streetCellDistance) + (Vector3.left * _streetCellDistance), _streetPointPrefab.transform.rotation);
            GameObject topRightPoint = Instantiate(_streetPointPrefab, expositors[i].transform.position + (Vector3.up * _streetCellDistance) + (Vector3.right * _streetCellDistance), _streetPointPrefab.transform.rotation);
        }
    }

    //public List<int> CreateRoute()
    //{
    //    List<int> streetPointsIndex = new List<int>();

    //}

    //IEnumerator AddTourist()
    //{
    //    List<int> route = CreateRoute();
    //    GameObject gameObject
    //}
}
