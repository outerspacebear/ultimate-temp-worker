using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrencyUtils
{
    static string CurrentMiniGameCurrency = "CurrentMiniGameCurrency";
    static string TotalCurrencyKey = "TotalCurrency";

    public static void AddCurrencyForCurrentGame(int amount)
    {
        var currencyEarnedPreviously = PlayerPrefs.GetInt(CurrentMiniGameCurrency, 0);

        PlayerPrefs.SetInt(CurrentMiniGameCurrency, currencyEarnedPreviously + amount);
    }
}
