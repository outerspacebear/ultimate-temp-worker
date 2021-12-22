using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDisplayer : MonoBehaviour
{
    public GameObject tutorial;

    // Start is called before the first frame update
    void Start()
    {
        if (!ScoreSaver.IsFirstTimePlaying())
            tutorial.SetActive(true);
        else
            MiniGameEvents.miniGameStartedEvent.Invoke();
    }
}
