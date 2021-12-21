using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrencyUtils
{
    static string CurrentMiniGameCurrency = "CurrentMiniGameCurrency";
    static string TotalCurrencyKey = "TotalCurrency";

    public static string RestaurantGameName = "RestGameCur";
    public static string CreatureCreatorGameName = "CreCreGameCur";

    public static void AddCurrencyForCurrentGame(int amount)
    {
        var currencyEarnedPreviously = PlayerPrefs.GetInt(CurrentMiniGameCurrency, 0);

        PlayerPrefs.SetInt(CurrentMiniGameCurrency, currencyEarnedPreviously + amount);
    }

    public static void AddCurrencyForGame(string gameName, int amount)
    {
        var currencyEarnedPreviously = PlayerPrefs.GetInt(gameName, 0);

        PlayerPrefs.SetInt(gameName, currencyEarnedPreviously + amount);
    }
}
