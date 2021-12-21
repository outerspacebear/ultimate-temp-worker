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
        Init();
        GenerateNewOrder(1, true);
    }

    private void Init()
    {
        InitLists();
        ResetOrderPositions();
    }

    private void InitLists()
    {
        cocktailEnums = new List<CocktailEnum>();
        orderedGlasses = new List<Glass>();
    }

    private void ResetOrderPositions()
    {
        unusedOrderPositions = new List<GlassPosition>(orderPositions);
    }

    private void DestroyOrderedGlassObjects()
    {
        foreach (var glass in orderedGlasses)
        {
            Destroy(glass.currentGlass);
        }
    }

    public void DiscardOrder()
    {
        DestroyOrderedGlassObjects();
        InitLists();
        ResetOrderPositions();
        GenerateNewOrder(UnityEngine.Random.Range(0, 2), false);
    }

    void GenerateNewOrder(int numberOfCocktails, bool isFirstOrder)
    {
        AddToCocktailEnums(numberOfCocktails, isFirstOrder);

        bool withIce = UnityEngine.Random.value < 0.5;
        if (isFirstOrder)
        {
            withIce = !isFirstOrder;
        }
        GenerateOrderedGlasses(withIce);

    }

    void AddToCocktailEnums(int numberOfCocktails, bool isFirstOrder)
    {
        int randomCocktail = 0;

        if (isFirstOrder)
        {
            randomCocktail = UnityEngine.Random.Range(0, 3);
        }
        else
        {
            numberOfCocktails = UnityEngine.Random.Range(1, 3);
            randomCocktail = UnityEngine.Random.Range(0, Enum.GetNames(typeof(CocktailEnum)).Length - 2);
        }

        for (int i = 0; i < numberOfCocktails; ++i)
        {
            cocktailEnums.Add((CocktailEnum) randomCocktail);
        }
    }

    void GenerateOrderedGlasses(bool withIce)
    {
        foreach (var cocktail in cocktailEnums)
        {
            var position = PopRandomOrderedGlassPosition();
            var glassGameObject = GetGlassObject(position);

            Glass glass = new Glass
            {
                currentGlass = glassGameObject,
                currentPosition = position,
                cocktailColor = cocktail,
                hasIce = withIce
            };

            glass = MixerUtils.FillOrderWithColor(glass, cocktail);

            if (glass.hasIce)
            {
                glass = AddIce(glass);
            }

            glass.currentGlass.SetActive(true);
            orderedGlasses.Add(glass);
        }
    }

    GlassPosition PopRandomOrderedGlassPosition()
    {
        int randomCocktailIndex = UnityEngine.Random.Range(0, unusedOrderPositions.Count);
        var chosenGlassPosition = unusedOrderPositions[randomCocktailIndex];
        unusedOrderPositions.RemoveAll(position => position.position == chosenGlassPosition.position);
        return chosenGlassPosition;
    }

    GameObject GetGlassObject(GlassPosition position)
    {
        return Instantiate(orderedGlassPrefab, position.position.position, position.position.rotation);
    }

    Glass AddIce(Glass glass)
    {
        foreach (var renderer in glass.currentGlass.GetComponentsInChildren<Renderer>())
        {
            if (renderer.tag == "Ice")
            {
                renderer.enabled = true;
            }
        }
        return glass;
    }
}
