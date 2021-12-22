using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeGOEnabler : MonoBehaviour
{
    public float timeToEnableAfter = 2f;
    public GameObject objectToEnable;
    float timeElapsed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        timeElapsed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if(timeElapsed >= timeToEnableAfter)
        {
            objectToEnable.SetActive(true);
            Destroy(this);
        }
    }
}
