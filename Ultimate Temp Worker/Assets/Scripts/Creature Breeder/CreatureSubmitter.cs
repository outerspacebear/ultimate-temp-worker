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

        planetGenerator.GenerateAndDisplayNewPlanet();
        creatureGenerator.InitSelectedParts();
    }

    private bool WillCreatureSurvive(Creature creature, Planet planet)
    {
        var creaturePercentages = GetCreatureElementalPercentages(creature);

        float survivalProbability = 100f;

        foreach(var element in ElementUtil.AllElements)
        {
            survivalProbability -= Mathf.Abs(planet.composition.elementalComposition[element] - creaturePercentages[element]);
        }

        float randomFloat = Random.Range(0, 100f);

        return randomFloat <= survivalProbability;
    }

    Dictionary<Element, float> GetCreatureElementalPercentages(Creature creature)
    {
        var percentages = new Dictionary<Element, float>() { { Element.Water, 0 }, { Element.Fire, 0 }, { Element.Trees, 0 } };

        foreach (var part in creature.parts)
        {
            percentages[part.elementalAffinity] += 33.33f;
        }

        return percentages;
    }

    private void RewardCurrencyOnSuccess()
    {
        CurrencyUtils.AddCurrencyForGame(CurrencyUtils.CreatureCreatorGameName, 1);
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
