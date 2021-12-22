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
                orderGlass.cocktailColor = cocktail;
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
                return new Color32(242, 217, 92, 255);

            case CocktailEnum.Orange:
                return new Color32(251, 172, 89, 255);

            case CocktailEnum.White:
                return new Color32(253, 252, 252, 255);

            case CocktailEnum.Blue:
                return new Color32(70, 161, 239, 255);

            case CocktailEnum.Cyan:
                return new Color32(156, 242, 251, 255);

            case CocktailEnum.Purple:
                return new Color32(201, 100, 217, 255);

            case CocktailEnum.Pink:
                return new Color32(251, 177, 172, 255);

            case CocktailEnum.Green:
                return new Color32(91, 229, 149, 255);

            case CocktailEnum.Black:
            default:
                return Color.black;
        }
    }
}
