using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouristInstance : MonoBehaviour
{
    List<int> _touristRoute;
    StreetManager _streetManager;
    float moveDuration = 3f;
    [SerializeField]
    AnimationCurve _animationCurve;

    public void SetRoute(List<int> route, StreetManager sManager)
    {
        _touristRoute = route;
        _streetManager = sManager;
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        string finalP = "";
        for (int i = 0; i<_touristRoute.Count; i++)
        {
            finalP += _touristRoute[i];
        }

        Vector3 initialPos = transform.position;
        for (float j = 0; j < moveDuration; j += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(initialPos, _streetManager.GetExpositorTransformPosByCoords(_touristRoute[0], 0), _animationCurve.Evaluate(j / moveDuration));
            yield return null;
        }
        transform.position = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[0], 0);

        yield return new WaitForSeconds(0.2f);



        for (int i = 0; i<_touristRoute.Count -1; i++)
        {
            int startingPoint = _touristRoute[i];
            int targetPoint = _touristRoute[i+1];

            int initialSide = Random.Range(3,5);
            for(float j = 0; j<0.2f; j += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(_streetManager.GetExpositorTransformPosByCoords(_touristRoute[i], 0), _streetManager.GetExpositorTransformPosByCoords(_touristRoute[i], initialSide), (j / 0.2f));
                yield return null;
            }
            transform.position = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[i], initialSide);
            //Obtengo las direcciones para ir desde startPoint hasta targetPoint
            List <StreetManager.Directions> directionsList = _streetManager.GetDirectionsList(startingPoint, targetPoint);
            //Interpreto??¿¿??
            for (int j = 0; j<directionsList.Count; j++)
            {
                Vector2 initialMatrixCoord = _streetManager.GetExpoCoordsByIndex(startingPoint);
                Vector2 targetMatrixCoord = initialMatrixCoord;
                switch (directionsList[j])
                {
                    case StreetManager.Directions.Up:
                        targetMatrixCoord += new Vector2(-1,0);
                        break;
                    case StreetManager.Directions.Down:
                        targetMatrixCoord += new Vector2(1, 0);
                        break;
                    case StreetManager.Directions.Left:
                        targetMatrixCoord += new Vector2(0, -1);
                        break;
                    case StreetManager.Directions.Right:
                        targetMatrixCoord += new Vector2(0, 1);
                        break;
                }
                targetPoint = _streetManager.GetExpoIndexByCoords(targetMatrixCoord);

                for(float l = 0; l<1f; l += Time.deltaTime)
                {
                    transform.position = Vector3.Lerp(_streetManager.GetExpositorTransformPosByCoords(startingPoint,initialSide), _streetManager.GetExpositorTransformPosByCoords(targetPoint, initialSide), l/1f);
                    yield return null;
                }
                transform.position = _streetManager.GetExpositorTransformPosByCoords(targetPoint, initialSide);
                startingPoint = targetPoint; 
            }
            for (float k = 0; k < 0.2f; k += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(_streetManager.GetExpositorTransformPosByCoords(_touristRoute[i+1], initialSide), _streetManager.GetExpositorTransformPosByCoords(_touristRoute[i + 1], 0), (k / 0.2f));
                yield return null;
            }
            transform.position = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[i + 1], 0);
            yield return new WaitForSeconds(2f);
        }
        Destroy(gameObject,1f);
    }



}
