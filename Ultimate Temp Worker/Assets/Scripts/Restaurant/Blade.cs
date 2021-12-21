using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    public GameObject bladePrefab;

    private void Start()
    {
        blade = GameObject.Instantiate(bladePrefab, Vector3.zero, gameObject.transform.rotation, gameObject.transform);
        DeInitBlade();
    }

    private void Update()
    {
        if(!DoesAnyTouchExist())
        {
            DeInitBlade();
            return;
        }

        if (bladeIsVisible)
        {
            if(HasTouchEnded())
            {
                DeInitBlade();
            }
            else
            {
                MakeBladeFollowTouch();
            }
        }
        else
        {
            var touch = Input.touches[0];
            if (IsSwipe(touch))
            {
                InitBlade(touch);
            }
        }
    }

    bool DoesAnyTouchExist()
    {
        return Input.touchCount > 0;
    }

    static bool HasTouchEnded()
    {
        return Input.touches[0].phase == TouchPhase.Ended;
    }

    static bool IsSwipe(Touch touch)
    {
        return touch.phase == TouchPhase.Moved;
    }

    void DeInitBlade()
    {
        blade.SetActive(false);
        bladeIsVisible = false;
    }

    void InitBlade(Touch touch)
    {
        var worldPosition = TouchUtils.GetWorldPosition(touch);
        worldPosition.z = -1;

        blade.transform.position = worldPosition;
        blade.SetActive(true);
        bladeIsVisible = true;
    }

    bool IsTouchOnObject(Touch touch)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject == gameObject)
                return true;
        }

        return false;
    }

    void MakeBladeFollowTouch()
    {
        var destination = Vector3.Lerp(blade.transform.position, TouchUtils.GetWorldPosition(Input.touches[0]), Time.deltaTime * 2f);
        destination.z = blade.transform.position.z;

        blade.transform.position = destination;
    }

    bool bladeIsVisible = false;
    GameObject blade;
}
