using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //TODO: Maybe call after button press or something, instead od in Start()?
        //StartGame();
    }

    public void StartGame()
    {
        MiniGameEvents.miniGameStartedEvent.Invoke();
    }
}
