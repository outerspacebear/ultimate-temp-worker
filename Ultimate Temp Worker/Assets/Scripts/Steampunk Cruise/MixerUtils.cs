using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerUtils : MonoBehaviour
{
    public static Glass FillWithColor(Glass glass, string colorName)
    {
        foreach (var renderer in glass.currentGlass.GetComponentsInChildren<Renderer>())
        {
            if (renderer.tag == "Cocktail" && glass.currentPosition.name == colorName)
            {
                var newColor = MixerUtils.GetNewCocktail(glass.cocktailColor, MixerUtils.GetCockatilBasedOnName(colorName));
                glass.cocktailColor = newColor;
                renderer.material.color = GetColor(glass.cocktailColor);
                renderer.enabled = true;
            }
        }
        return glass;
    }

    public static Glass FillOrderWithColor(Glass orderGlass, CocktailEnum cocktail)
    {
        foreach (var renderer in orderGlass.currentGlass.GetComponentsInChildren<Renderer>())
        {
            if (renderer.tag == "Cocktail")
            {
                var newColor = MixerUtils.GetNewCocktail(orderGlass.cocktailColor, cocktail);
                orderGlass.cocktailColor = newColor;
                renderer.material.color = GetColor(orderGlass.cocktailColor);
                renderer.enabled = true;
            }
        }
        return orderGlass;
    }

    public static CocktailEnum GetCockatilBasedOnName(string colorName)
    {
        switch (colorName)
        {
            case "Red":
                return CocktailEnum.Red;
            case "Yellow":
                return CocktailEnum.Yellow;
            case "Blue":
                return CocktailEnum.Blue;
            case "White":
                return CocktailEnum.White;
            default:
                return CocktailEnum.Black;
        }
    }

    public static CocktailEnum GetNewCocktail(CocktailEnum baseCocktail, CocktailEnum fillingCocktail)
    {
        if (baseCocktail == CocktailEnum.Empty)
        {
            return fillingCocktail;
        }

        if (baseCocktail == CocktailEnum.Red && fillingCocktail == CocktailEnum.Red)
        {
            return CocktailEnum.Red;
        }
        else if ((baseCocktail == CocktailEnum.Red && fillingCocktail == CocktailEnum.Yellow) || (fillingCocktail == CocktailEnum.Red && baseCocktail == CocktailEnum.Yellow))
        {
            return CocktailEnum.Orange;
        }
        else if ((baseCocktail == CocktailEnum.Red && fillingCocktail == CocktailEnum.Blue) || (fillingCocktail == CocktailEnum.Red && baseCocktail == CocktailEnum.Blue))
        {
            return CocktailEnum.Purple; 
        }
        else if ((baseCocktail == CocktailEnum.Red && fillingCocktail == CocktailEnum.White) || (fillingCocktail == CocktailEnum.Red && baseCocktail == CocktailEnum.White))
        {
            return CocktailEnum.Pink;
        }
        else if ((baseCocktail == CocktailEnum.Blue && fillingCocktail == CocktailEnum.Yellow) || (fillingCocktail == CocktailEnum.Blue && baseCocktail == CocktailEnum.Yellow))
        {
            return CocktailEnum.Green;
        }
        else if ((baseCocktail == CocktailEnum.Blue && fillingCocktail == CocktailEnum.White) || (fillingCocktail == CocktailEnum.Blue && baseCocktail == CocktailEnum.White))
        {
            return CocktailEnum.Cyan;
        }
        else if (baseCocktail == CocktailEnum.Blue && fillingCocktail == CocktailEnum.Blue)
        {
            return CocktailEnum.Blue;
        }
        else if (baseCocktail == CocktailEnum.White && fillingCocktail == CocktailEnum.White)
        {
            return CocktailEnum.White;
        }
        else if (baseCocktail == CocktailEnum.Yellow && fillingCocktail == CocktailEnum.Yellow)
        {
            return CocktailEnum.Yellow;
        }
        else
        {
            return CocktailEnum.Black;
        }
    }

    public static Color GetColor(CocktailEnum cocktail)
    {
        switch (cocktail)
        {
            case CocktailEnum.Red:
                return Color.red;

            case CocktailEnum.Yellow:
                return Color.yellow;

            case CocktailEnum.Orange:
                return new Color(1.0f, 0.64f, 0.0f);

            case CocktailEnum.White:
                return Color.white;

            case CocktailEnum.Blue:
                return Color.blue;

            case CocktailEnum.Cyan:
                return Color.cyan;

            case CocktailEnum.Purple:
                return new Color(1.27f, 0.63f, 1.91f);

            case CocktailEnum.Pink:
                return new Color(1.91f, 0.63f, 1.91f);

            case CocktailEnum.Green:
                return Color.green;

            case CocktailEnum.Black:
            default:
                return Color.black;
        }
    }
}
