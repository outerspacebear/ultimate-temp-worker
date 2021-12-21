using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetGenerator : MonoBehaviour
{
    public List<Sprite> planetSprites;

    // Start is called before the first frame update
    void Start()
    {
        GenerateAndDisplayNewPlanet();
    }

    public void GenerateAndDisplayNewPlanet()
    {
        currentPlanet = GeneratePlanet();
        UpdatePlanetInfoDisplay();
    }

    Planet GeneratePlanet()
    {
        var sprite = SelectRandomSpriteFromList(planetSprites);
        var composition = PlanetComposition.GetRandomComposition();

        return new Planet(composition, sprite);
    }

    Sprite SelectRandomSpriteFromList(List<Sprite> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    private Planet currentPlanet;

    //===========================

    public Image planetImage;
    public Slider fireSlider;
    public Slider waterSlider;
    public Slider forestSlider;

    void UpdatePlanetInfoDisplay()
    {
        planetImage.sprite = currentPlanet.sprite;
        planetImage.SetAllDirty();

        foreach(var entry in currentPlanet.composition.elementalComposition)
        {
            UpdateSliderForElement(entry.Key, entry.Value / 100);
        }
    }

    void UpdateSliderForElement(Element element, float sliderPercent)
    {
        switch(element)
        {
            case Element.Fire:
                fireSlider.value = sliderPercent;
                //fireSlider.SetValueWithoutNotify(sliderPercent);
                break;
            case Element.Water:
                waterSlider.value = sliderPercent;
                break;
            case Element.Trees:
                forestSlider.value = sliderPercent;
                break;
        }
    }
}
