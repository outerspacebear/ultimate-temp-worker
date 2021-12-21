using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSubmitter : MonoBehaviour
{
    public PlanetGenerator planetGenerator;
    public CreaturePartsCombiner creatureGenerator;

    public void SubmitCreature()
    {
        var creature = creatureGenerator.GetCurrentCreature();
        var planet = planetGenerator.GetCurrentPlanet();

        bool willSurvive = WillCreatureSurvive(creature, planet);
        UpdateUI(willSurvive);

        if (willSurvive)
            RewardCurrencyOnSuccess();
    }

    private bool WillCreatureSurvive(Creature creature, Planet planet)
    {
        return true;
    }

    private void RewardCurrencyOnSuccess()
    {
        //TODO
    }

    public GameObject successUI;
    public GameObject failureUI;

    private void UpdateUI(bool didCreatureSurvive)
    {
        //Show currency?

        if (didCreatureSurvive)
            successUI.SetActive(true);
        else
            failureUI.SetActive(true);
    }
}
