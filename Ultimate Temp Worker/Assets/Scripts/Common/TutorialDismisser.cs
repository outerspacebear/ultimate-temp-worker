using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDismisser : MonoBehaviour
{
    bool wasEnabled = true;

    private void OnEnable()
    {
        wasEnabled = true;
    }

    private void OnDisable()
    {
        if(wasEnabled)
            MiniGameEvents.miniGameStartedEvent.Invoke();
    }
}
