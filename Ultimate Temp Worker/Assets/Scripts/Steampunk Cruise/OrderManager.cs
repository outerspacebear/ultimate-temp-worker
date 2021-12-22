using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    public GameObject orderedGlassPrefab;
    public List<GlassPosition> orderPositions;

    public GameObject currencyDisplayPrefabPlate;
    public GameObject currencyDisplayPrefabOrder;
    public Transform currencyUIPosition;

    public GameObject warningLimitReachedUI;

    private List<CocktailEnum> cocktailEnums;
    private List<Glass> orderedGlasses;
    private List<Glass> preparedGlasses;
    private List<GlassPosition> unusedOrderPositions;

    private void Awake()
    {
        SteampunkEvents.addGlassToOrderEvent.AddListener(AddGlassToBeServed);
    }

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
        preparedGlasses = new List<Glass>();
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

    private void DestroyPreparedGlassObjects()
    {
        foreach (var glass in preparedGlasses)
        {
            Destroy(glass.currentGlass);
        }
    }

    void AddGlassToBeServed(Glass glass)
    {
        preparedGlasses.Add(glass);
    }

    public void ServePreparedGlasses()
    {
        bool hasServedMoreThanAuthorised = preparedGlasses.Count > SteampunkGameData.MaxAmountOfDrinksAuthorised;

        if (hasServedMoreThanAuthorised)
        {
            warningLimitReachedUI.SetActive(true);
        }

        int amount = CalculateRewardForOrder();
        DisplayCurrencyEarnedUI(amount, currencyDisplayPrefabPlate);
        AddCurrencyToInventory(amount);
        ResetRound();
    }

    void DisplayCurrencyEarnedUI(int amountEarned, GameObject currencyPrefab)
    {
        var currencyDisplay = GameObject.Instantiate(currencyPrefab, currencyUIPosition.position, currencyPrefab.transform.rotation, currencyUIPosition);
        currencyDisplay.GetComponentInChildren<Text>().text = amountEarned.ToString();
        Destroy(currencyDisplay, 1);
    }

    public void DiscardOrder()
    {
        if (ShouldHaveDiscardedOrder())
        {
            AddCurrencyToInventory(SteampunkGameData.SingleEarningValue);
            DisplayCurrencyEarnedUI(SteampunkGameData.SingleEarningValue, currencyDisplayPrefabOrder);
        }
        DestroyOrderedGlassObjects();
        InitLists();
        ResetOrderPositions();
        GenerateNewOrder(UnityEngine.Random.Range(0, 2), false);
    }

    private bool ShouldHaveDiscardedOrder()
    {
        return orderedGlasses.Count > 2;
    }

    private void AddCurrencyToInventory(int amount)
    {
        CurrencyUtils.AddCurrencyForGame(SteampunkGameData.GameName, amount);
        Debug.Log(amount + " currency was added to inventory");
    }

    private int CalculateRewardForOrder()
    {
        int reward = 0;

        bool hasServedMoreThanAuthorised = preparedGlasses.Count > SteampunkGameData.MaxAmountOfDrinksAuthorised;

        if (hasServedMoreThanAuthorised)
        {
            return -SteampunkGameData.SingleEarningValue;
        }

        bool hasSameNumberOfGlasses = orderedGlasses.Count == preparedGlasses.Count;

        bool shouldGetReward = hasSameNumberOfGlasses && hasSameNumberOfGlasses;

        foreach (var order in orderedGlasses)
        {
            shouldGetReward &= preparedGlasses.Any(preparedGlass => order.cocktailColor == preparedGlass.cocktailColor && order.hasIce == preparedGlass.hasIce);
        }

        if (shouldGetReward)
        {
            ++reward;
        }

        return reward;
    }

    private void ResetRound()
    {
        DestroyOrderedGlassObjects();
        DestroyPreparedGlassObjects();
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
            randomCocktail = UnityEngine.Random.Range(0, 4);
        }
        else
        {
            numberOfCocktails = UnityEngine.Random.Range(1, 4);
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
            withIce &= cocktail != CocktailEnum.White;

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
