using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedBehaviourEnabler : MonoBehaviour
{
    public MonoBehaviour behaviour;
    public float timeInSeconds;

    public void SetBehaviourEnabledAfterTime(bool value)
    {
        shouldElapseTime = true;
        setActive = value;
    }

    float elapsedTime = 0;
    bool shouldElapseTime = false;
    bool setActive = true;

    private void Update()
    {
        if(shouldElapseTime)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= timeInSeconds)
            {
                behaviour.enabled = setActive;
                shouldElapseTime = false;
            }
        }
    }
}
