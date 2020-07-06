using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouristInstance : MonoBehaviour
{
    List<int> _touristRoute;
    StreetManager _streetManager;
    float moveDuration = 3f;
    [SerializeField]
    Animator _animator;

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

        Vector3 initialPos = Vector3.zero;
        float speed = Random.Range(1f,3f) * CurrentSceneManager.GetGlobalSpeed();
        _animator.SetFloat("Speed", speed);
        switch (_touristRoute[0])
        {
            case 6:
            case 0:
            case 2:
            case 8:
                initialPos = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[0], 0) + new Vector3(-2,0,0);
                break;
            case 7:
            case 1:
            case 3:
            case 9:
                initialPos = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[0], 0) + new Vector3(2, 0, 0);
                break;
            case 4:
            case 5:
                if (Random.value < 0.5f)
                {
                    initialPos = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[0], 0) + new Vector3(2, 0, 0);
                }
                else
                {
                    initialPos = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[0], 0) + new Vector3(-2, 0, 0);
                }
                break;
        }

        Vector3 startLerpPos = initialPos;
        Vector3 targetLerpPos = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[0], 0);
        float lerpDistance = Mathf.Abs((targetLerpPos - startLerpPos).magnitude);
        float lerpTime = lerpDistance / ((speed) * CurrentSceneManager.GetGlobalSpeed());
        float globalSpeed = CurrentSceneManager.GetGlobalSpeed();
        if(startLerpPos.x == targetLerpPos.x)
        {
            if(startLerpPos.y < targetLerpPos.y)
            {
                _animator.SetTrigger("Up");

            }
            else
            {
                if(startLerpPos.y > targetLerpPos.y)
                {
                    _animator.SetTrigger("Down");

                }
            }
        }
        else
        {
            if (startLerpPos.x < targetLerpPos.x)
            {
                _animator.SetTrigger("Right");
            }
            else
            {
                _animator.SetTrigger("Left");
            }
        }
        for (float j = 0; j < lerpTime; j += Time.deltaTime)
        {
            if(CurrentSceneManager.GetGlobalSpeed()  != globalSpeed)
            {
                lerpTime = lerpDistance / ((speed) * CurrentSceneManager.GetGlobalSpeed());
                j /= (CurrentSceneManager.GetGlobalSpeed() / globalSpeed);
                globalSpeed = CurrentSceneManager.GetGlobalSpeed();
            }
            transform.position = Vector3.Lerp(startLerpPos, targetLerpPos, j / lerpTime);
            yield return null;
        }
        transform.position = targetLerpPos;
        _animator.SetTrigger("Idle");
        float waitingTime = Random.Range(0.5f, 1.5f);
        yield return new WaitForSeconds(waitingTime/2f);
        GameEvents.TouristWatchDino.Invoke(_touristRoute[0]);
        yield return new WaitForSeconds(waitingTime/2f);


        for (int i = 0; i<_touristRoute.Count -1; i++)
        {
            int startingPoint = _touristRoute[i];
            int targetPoint = _touristRoute[i + 1];
            int initialSide = Random.Range(3,5);

            startLerpPos = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[i], 0);
            targetLerpPos = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[i],initialSide);
            lerpDistance = Mathf.Abs((targetLerpPos - startLerpPos).magnitude);
            lerpTime = lerpDistance / ((speed) * CurrentSceneManager.GetGlobalSpeed());
            if (startLerpPos.x == targetLerpPos.x)
            {
                if (startLerpPos.y < targetLerpPos.y)
                {
                    _animator.SetTrigger("Up");

                }
                else
                {
                    if (startLerpPos.y > targetLerpPos.y)
                    {
                        _animator.SetTrigger("Down");

                    }
                }
            }
            else
            {
                if (startLerpPos.x < targetLerpPos.x)
                {
                    _animator.SetTrigger("Right");
                }
                else
                {
                    _animator.SetTrigger("Left");
                }
            }
            globalSpeed = CurrentSceneManager.GetGlobalSpeed();
            for (float j = 0; j<lerpTime; j += Time.deltaTime)
            {
                if (CurrentSceneManager.GetGlobalSpeed() != globalSpeed)
                {
                    lerpTime = lerpDistance / ((speed) * CurrentSceneManager.GetGlobalSpeed());
                    j /= (CurrentSceneManager.GetGlobalSpeed() / globalSpeed);
                    globalSpeed = CurrentSceneManager.GetGlobalSpeed();
                }
                transform.position = Vector3.Lerp(startLerpPos, targetLerpPos, (j / lerpTime));
                yield return null;
            }
            transform.position = targetLerpPos;
            //Obtengo las direcciones para ir desde startPoint hasta targetPoint
            List <StreetManager.Directions> directionsList = _streetManager.GetDirectionsList(startingPoint, targetPoint);

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

                startLerpPos = _streetManager.GetExpositorTransformPosByCoords(startingPoint, initialSide);
                targetLerpPos = _streetManager.GetExpositorTransformPosByCoords(targetPoint, initialSide);
                lerpDistance = Mathf.Abs((targetLerpPos - startLerpPos).magnitude);
                lerpTime = lerpDistance / ((speed) * CurrentSceneManager.GetGlobalSpeed());
                if (startLerpPos.x == targetLerpPos.x)
                {
                    if (startLerpPos.y < targetLerpPos.y)
                    {
                        _animator.SetTrigger("Up");

                    }
                    else
                    {
                        if (startLerpPos.y > targetLerpPos.y)
                        {
                            _animator.SetTrigger("Down");

                        }
                    }
                }
                else
                {
                    if (startLerpPos.x < targetLerpPos.x)
                    {
                        _animator.SetTrigger("Right");
                    }
                    else
                    {
                        _animator.SetTrigger("Left");
                    }
                }
                globalSpeed = CurrentSceneManager.GetGlobalSpeed();
                for (float l = 0; l< lerpTime; l += Time.deltaTime)
                {
                    if (CurrentSceneManager.GetGlobalSpeed() != globalSpeed)
                    {
                        lerpTime = lerpDistance / ((speed) * CurrentSceneManager.GetGlobalSpeed());
                        l /= (CurrentSceneManager.GetGlobalSpeed() / globalSpeed);
                        globalSpeed = CurrentSceneManager.GetGlobalSpeed();
                    }
                    transform.position = Vector3.Lerp(startLerpPos, targetLerpPos, l/ lerpTime);
                    yield return null;
                }
                transform.position = targetLerpPos;
                startingPoint = targetPoint; 
            }

            startLerpPos = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[i + 1], initialSide);
            targetLerpPos = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[i + 1], 0);
            lerpDistance = Mathf.Abs((targetLerpPos - startLerpPos).magnitude);
            lerpTime = lerpDistance / ((speed) * CurrentSceneManager.GetGlobalSpeed());
            if (startLerpPos.x == targetLerpPos.x)
            {
                if (startLerpPos.y < targetLerpPos.y)
                {
                    _animator.SetTrigger("Up");

                }
                else
                {
                    if (startLerpPos.y > targetLerpPos.y)
                    {
                        _animator.SetTrigger("Down");

                    }
                }
            }
            else
            {
                if (startLerpPos.x < targetLerpPos.x)
                {
                    _animator.SetTrigger("Right");
                }
                else
                {
                    _animator.SetTrigger("Left");
                }
            }
            globalSpeed = CurrentSceneManager.GetGlobalSpeed();
            for (float k = 0; k < lerpTime; k += Time.deltaTime)
            {
                if (CurrentSceneManager.GetGlobalSpeed() != globalSpeed)
                {
                    lerpTime = lerpDistance / ((speed) * CurrentSceneManager.GetGlobalSpeed());
                    k /= (CurrentSceneManager.GetGlobalSpeed() / globalSpeed);
                    globalSpeed = CurrentSceneManager.GetGlobalSpeed();
                }
                transform.position = Vector3.Lerp(startLerpPos, targetLerpPos, (k / lerpTime));
                yield return null;
            }
            transform.position = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[i + 1], 0);
            _animator.SetTrigger("Idle");
            waitingTime = Random.Range(0.5f, 1.5f);
            yield return new WaitForSeconds(waitingTime / 2f);

            //LANZAR MONEDA
            GameEvents.TouristWatchDino.Invoke(_touristRoute[i+1]);
            yield return new WaitForSeconds(waitingTime / 2f);
            yield return new WaitForSeconds(0.2f);
        }

        switch (_touristRoute[_touristRoute.Count -1])
        {
            case 0:
                targetLerpPos = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[_touristRoute.Count - 1], 0) + new Vector3(-2, 0, 0);
                break;
            case 1:
                targetLerpPos = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[_touristRoute.Count - 1], 0) + new Vector3(2, 0, 0);
                break;
            case 2:
                targetLerpPos = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[_touristRoute.Count - 1], 0) + new Vector3(-2, 0, 0);
                break;
            case 3:
                targetLerpPos = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[_touristRoute.Count - 1], 0) + new Vector3(2, 0, 0);
                break;
        }

        startLerpPos = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[_touristRoute.Count - 1], 0);
        lerpDistance = Mathf.Abs((targetLerpPos - startLerpPos).magnitude);
        lerpTime = lerpDistance / ((speed) * CurrentSceneManager.GetGlobalSpeed());
        if (startLerpPos.x == targetLerpPos.x)
        {
            if (startLerpPos.y < targetLerpPos.y)
            {
                _animator.SetTrigger("Up");

            }
            else
            {
                if (startLerpPos.y > targetLerpPos.y)
                {
                    _animator.SetTrigger("Down");

                }
            }
        }
        else
        {
            if (startLerpPos.x < targetLerpPos.x)
            {
                _animator.SetTrigger("Right");
            }
            else
            {
                _animator.SetTrigger("Left");
            }
        }
        globalSpeed = CurrentSceneManager.GetGlobalSpeed();
        for (float j = 0; j < lerpTime; j += Time.deltaTime)
        {
            if (CurrentSceneManager.GetGlobalSpeed() != globalSpeed)
            {
                lerpTime = lerpDistance / ((speed) * CurrentSceneManager.GetGlobalSpeed());
                j /= (CurrentSceneManager.GetGlobalSpeed() / globalSpeed);
                globalSpeed = CurrentSceneManager.GetGlobalSpeed();
            }
            transform.position = Vector3.Lerp(startLerpPos, targetLerpPos, j / lerpTime);
            yield return null;
        }
        transform.position = targetLerpPos;
        Destroy(gameObject,1f);
    }

    private void Update()
    {
        _animator.speed = CurrentSceneManager.GetGlobalSpeed();
    }
}
