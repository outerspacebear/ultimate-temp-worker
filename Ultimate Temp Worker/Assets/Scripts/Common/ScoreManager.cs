using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MiniGameScoreDisplay
{
    public Text currencyEarnedText;
    public Image currencySymbol;
    public Text currencyConversionText;

    public Image finalCurrencySymbol;
    public Text finalCurrencyAmountText;
}

[System.Serializable]
public enum MiniGameType { Restaurant, Cocktails, Creatures};

[System.Serializable]
public struct MiniGameInfo
{
    public MiniGameType type;
    public string miniGameCurrencyStoreKey;
    public Sprite currencySprite;
    public float currencyToFinalCurrencyMultiplier;
}

public class ScoreManager : MonoBehaviour
{
    public List<MiniGameScoreDisplay> miniGameScoreDisplays;

    public Image totalFinalCurrencySymbol;
    public Text totalFinalCurrencyText;

    public List<MiniGameInfo> miniGameInfos;
    public Sprite finalCurrencySprite;

    private void Start()
    {
        int i = 0;

        foreach(var index in GameManager.loadedMiniGameSceneIndicesInOrder)
        {
            var gameType = sceneIndicesToGameType[index];
            var info = GetInfo(gameType);

            PopulateScoreLineAtIndexForInfo(i++, info);
        }

        PopulateTotalFinalCurrencyDisplay();
    }

    MiniGameInfo GetInfo(MiniGameType type)
    {
        foreach(var info in miniGameInfos)
        {
            if (info.type == type)
                return info;
        }

        return new MiniGameInfo{ };
    }

    void PopulateScoreLineAtIndexForInfo(int index, MiniGameInfo info)
    {
        var displayToPopulate = miniGameScoreDisplays[index];

        var amountEarned = PlayerPrefs.GetInt(info.miniGameCurrencyStoreKey);
        displayToPopulate.currencyEarnedText.text = amountEarned.ToString();

        displayToPopulate.currencySymbol.sprite = info.currencySprite;
        displayToPopulate.currencySymbol.SetAllDirty();

        displayToPopulate.currencyConversionText.text = info.currencyToFinalCurrencyMultiplier.ToString();

        displayToPopulate.finalCurrencySymbol.sprite = finalCurrencySprite;
        displayToPopulate.finalCurrencySymbol.SetAllDirty();

        float totalFinalCurrency = amountEarned * info.currencyToFinalCurrencyMultiplier;
        displayToPopulate.finalCurrencyAmountText.text = totalFinalCurrency.ToString();

        totalFinalCurrencyEarned += totalFinalCurrency;
    }

    float totalFinalCurrencyEarned = 0;

    void PopulateTotalFinalCurrencyDisplay()
    {
        totalFinalCurrencyText.text = totalFinalCurrencyEarned.ToString();
        totalFinalCurrencySymbol.sprite = finalCurrencySprite;
        totalFinalCurrencySymbol.SetAllDirty();
    }

    Dictionary<int, MiniGameType> sceneIndicesToGameType = new Dictionary<int, MiniGameType>
    {
        {1, MiniGameType.Restaurant },
        {2, MiniGameType.Cocktails },
        {3, MiniGameType.Creatures }
    };
}
