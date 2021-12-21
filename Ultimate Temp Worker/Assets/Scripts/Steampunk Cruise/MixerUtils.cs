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
                var newColor = MixerUtils.GetNewColor(glass.cocktailColor, MixerUtils.GetColorBasedOnName(colorName));
                glass.cocktailColor = newColor;
                renderer.material.color = glass.cocktailColor;
                renderer.enabled = true;
            }
        }
        return glass;
    }

    public static Color GetColorBasedOnName(string colorName)
    {
        switch (colorName)
        {
            case "Red":
                return Color.red;
            case "Yellow":
                return Color.yellow;
            case "Blue":
                return Color.blue;
            case "White":
                return Color.white;
            default:
                return Color.black;
        }
    }

    public static Color GetNewColor(Color baseColor, Color fillingColor)
    {
        if (baseColor == Color.black)
        {
            return fillingColor; // TODO
        }

        if ((baseColor == Color.red && fillingColor == Color.red) || (fillingColor == Color.red && baseColor == Color.red))
        {
            return Color.red;
        }
        else if ((baseColor == Color.red && fillingColor == Color.yellow) || (fillingColor == Color.red && baseColor == Color.yellow))
        {
            return new Color(1.0f, 0.64f, 0.0f); // orange
        }
        else if ((baseColor == Color.red && fillingColor == Color.blue) || (fillingColor == Color.red && baseColor == Color.blue))
        {
            return new Color(1.27f, 0.63f, 1.91f); // purple
        }
        else if ((baseColor == Color.red && fillingColor == Color.white) || (fillingColor == Color.red && baseColor == Color.white))
        {
            return new Color(1.91f, 0.63f, 1.91f); // pink
        }
        else if ((baseColor == Color.blue && fillingColor == Color.yellow) || (fillingColor == Color.blue && baseColor == Color.yellow))
        {
            return Color.green;
        }
        else if ((baseColor == Color.blue && fillingColor == Color.white) || (fillingColor == Color.blue && baseColor == Color.white))
        {
            return Color.cyan;
        }
        else if ((baseColor == Color.blue && fillingColor == Color.blue) || (fillingColor == Color.blue && baseColor == Color.blue))
        {
            return Color.blue;
        }
        else if ((baseColor == Color.white && fillingColor == Color.white) || (fillingColor == Color.white && baseColor == Color.white))
        {
            return Color.white;
        }
        else if ((baseColor == Color.yellow && fillingColor == Color.yellow) || (fillingColor == Color.yellow && baseColor == Color.yellow))
        {
            return Color.yellow;
        }
        else
        {
            return Color.black;
        }
    }
}
