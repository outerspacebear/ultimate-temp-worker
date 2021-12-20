using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mashable : MonoBehaviour
{
    private void Update()
    {
        foreach(Touch touch in Input.touches)
        {
            if (IsTouchStationary(touch))
            {
                OnMash();
            }
        }
    }

    static bool IsTouchStationary(Touch touch)
    {
        if (touch.phase == TouchPhase.Stationary)
            return true;

        return false;
    }

    void OnMash()
    {
        Destroy(this.gameObject);
    }
}
