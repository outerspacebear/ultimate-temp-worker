using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedGameObjectDisabler : MonoBehaviour
{
    private void OnEnable()
    {
        timeElapsedSinceEnabling = 0;
    }

    private float timeElapsedSinceEnabling = 0f;

    public float timeToDisableAfterInSeconds = 1f;

    private void Update()
    {
        if(isActiveAndEnabled)
        {
            timeElapsedSinceEnabling += Time.deltaTime;
            if(timeElapsedSinceEnabling >= timeToDisableAfterInSeconds)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
