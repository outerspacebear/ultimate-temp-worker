using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mashable : MonoBehaviour
{
    public GameObject replacementPrefab;

    private void Update()
    {
        foreach(Touch touch in Input.touches)
        {
            if (IsTouchOnObject(touch) && IsTouchStationary(touch))
            {
                OnMash();
                break;
            }
        }
    }

    static bool IsTouchStationary(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Stationary:
            case TouchPhase.Began:
                return true;
        }

        return false;
    }

    bool IsTouchOnObject(Touch touch)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);

        if (hit.collider != null)
        {
            if(hit.collider.gameObject == gameObject)
                return true;    
        }

        return false;
    }

    void OnMash()
    {
        //Play sound
        CreateReplacementPrefab();
        Destroy(this.gameObject);
    }

    void CreateReplacementPrefab()
    {
        var spawnedPrefab = GameObject.Instantiate(replacementPrefab, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
        Destroy(spawnedPrefab, 1.5f);
    }
}