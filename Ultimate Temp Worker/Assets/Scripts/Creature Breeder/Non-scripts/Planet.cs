using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetComposition
{
    public static PlanetComposition GetRandomComposition()
    {
        float remainingPercent = 100f;

        Dictionary<Element, float> composition = new Dictionary<Element, float>();

        Shuffle(ElementUtil.AllElements);
        foreach(var element in ElementUtil.AllElements)
        {
            float percentForElement = Random.Range(0, remainingPercent);
            composition.Add(element, percentForElement);

            remainingPercent -= percentForElement;
        }

        if(remainingPercent > 0)
        {
            composition[ElementUtil.AllElements[0]] += remainingPercent;
        }

        return new PlanetComposition(composition);
    }

    private static void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, list.Count);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private PlanetComposition(Dictionary<Element, float> elementalComposition)
    {
        this.elementalComposition = elementalComposition;
    }

    public Dictionary<Element, float> elementalComposition { get; }
}

public class Planet
{
    public Planet(PlanetComposition composition, Sprite sprite)
    {
        this.composition = composition;
        this.sprite = sprite;
    }

    public PlanetComposition composition { get; }
    public Sprite sprite { get; }
}
