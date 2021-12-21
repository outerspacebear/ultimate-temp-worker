using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetComposition
{
    public PlanetComposition(Dictionary<Element, float> elementalComposition)
    {
        this.elementalComposition = elementalComposition;
    }

    public Dictionary<Element, float> elementalComposition { get; }
}

public class Planet
{
    PlanetComposition composition;
    Sprite sprite;
}
