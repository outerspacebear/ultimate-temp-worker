using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MiniGameTimer : MonoBehaviour
{
    private void Awake()
    {
        MiniGameEvents.miniGameStartedEvent.AddListener(SetHasGameStarted);
    }

    private void Update()
    {
        if(!hasGameStarted)
        {
            return;
        }

        totalTimeElapsed += Time.deltaTime;
        UpdateUI();

        if(totalTimeElapsed >= GlobalConsts.MiniGameDurationInSeconds)
        {
            OnTimeUp();
        }
    }

    private void OnTimeUp()
    {
        MiniGameEvents.miniGameEndedEvent.Invoke();
        Destroy(this);

        Debug.Log("rest: " + PlayerPrefs.GetInt(CurrencyUtils.RestaurantGameName).ToString() + "\ncocktails: " + PlayerPrefs.GetInt(SteampunkGameData.GameName).ToString()
            + "\nCreatures: " + PlayerPrefs.GetInt(CurrencyUtils.CreatureCreatorGameName).ToString());
    }

    private void SetHasGameStarted()
    {
        hasGameStarted = true;
    }

    bool hasGameStarted = false;
    float totalTimeElapsed = 0;

    //======================================

    public GameObject clockHand;

    void UpdateUI()
    {
        if (!clockHand)
            return;

        var elapsedTimePercent = (totalTimeElapsed * 100) / GlobalConsts.MiniGameDurationInSeconds;

        var targetClockHandAngle = 360 - ((elapsedTimePercent / 100) * 360);
        var currentClockHandAngle = clockHand.transform.rotation.eulerAngles.z;
        var angleToRotateBy = targetClockHandAngle - currentClockHandAngle;

        clockHand.transform.Rotate(transform.forward, angleToRotateBy);
    }
}
