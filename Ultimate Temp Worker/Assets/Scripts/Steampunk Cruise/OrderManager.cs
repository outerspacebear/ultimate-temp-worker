using System.Collections.Generic;
using UnityEngine;
using System;

public class OrderManager : MonoBehaviour
{
    public GameObject orderedGlassPrefab;
    public List<GlassPosition> orderPositions;

    private List<CocktailEnum> cocktailEnums;
    private List<Glass> orderedGlasses;
    private List<GlassPosition> unusedOrderPositions;

    // Start is called before the first frame update
    void Start()
    {
        cocktailEnums = new List<CocktailEnum>();
        orderedGlasses = new List<Glass>();
        unusedOrderPositions = orderPositions;
        GenerateNewOrder(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateNewOrder(int numberOfCocktails = 0)
    {
        AddToCocktailEnums(numberOfCocktails);
        GenerateOrderedGlasses();

    }

    void AddToCocktailEnums(int numberOfCocktails)
    {
        if (numberOfCocktails == 0)
        {
            numberOfCocktails = UnityEngine.Random.Range(1, 3);
        }

        for (int i = 0; i < numberOfCocktails; ++i)
        {
            int randomCocktail = UnityEngine.Random.Range(0, Enum.GetNames(typeof(CocktailEnum)).Length);
            cocktailEnums.Add((CocktailEnum) randomCocktail);
        }
    }

    void GenerateOrderedGlasses()
    {
        foreach (var cocktail in cocktailEnums)
        {
            var position = PopRandomOrderedGlassPosition();
            var glassGameObject = GetGlassObject(position);
            Glass glass = new Glass
            {
                currentGlass = glassGameObject,
                currentPosition = position,
                cocktailColor = cocktail
            };
            glass = MixerUtils.FillOrderWithColor(glass, cocktail);
            glass.currentGlass.SetActive(true);
            orderedGlasses.Add(glass);
        }
    }

    GlassPosition PopRandomOrderedGlassPosition()
    {
        int randomCocktailIndex = UnityEngine.Random.Range(0, cocktailEnums.Count);
        var chosenGlassPosition = unusedOrderPositions[randomCocktailIndex];
        unusedOrderPositions.RemoveAll(position => position.position == chosenGlassPosition.position);
        return chosenGlassPosition;
    }

    GameObject GetGlassObject(GlassPosition position)
    {
        return Instantiate(orderedGlassPrefab, position.position.position, position.position.rotation);
    }
}
