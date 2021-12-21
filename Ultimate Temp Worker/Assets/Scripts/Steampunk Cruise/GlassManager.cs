using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassManager : MonoBehaviour
{
    [Serializable]
    public struct GlassPosition
    {
        public string name;
        public Transform position;
        public bool isSpawnable;
    }

    public List<GlassPosition> spawnablePositions;
    public GameObject prefabGlass;

    private GameObject currentGlass;
    private GlassPosition currentPosition;
    private Color cocktailColor = Color.black;

    // Start is called before the first frame update
    void Start()
    {
        InitGlassAtRandom();
    }


    void InitGlassAtRandom() 
    {
        bool isSpawnable = false;
        int spawnIndex;
        Transform spawnPoint = spawnablePositions[0].position;

        while (!isSpawnable)
        {
            spawnIndex = UnityEngine.Random.Range(0, spawnablePositions.Count);
            isSpawnable = spawnablePositions[spawnIndex].isSpawnable;
            spawnPoint = spawnablePositions[spawnIndex].position;
            currentPosition = spawnablePositions[spawnIndex];
        }

        currentGlass = Instantiate(prefabGlass, spawnPoint.position, spawnPoint.rotation);

        foreach (Transform child in currentGlass.transform)
        {
            child.GetComponent<Renderer>().enabled = false;
        }
    }

    public void PourIce()
    {
        foreach (var renderer in currentGlass.GetComponentsInChildren<Renderer>())
        {
            if (renderer.tag == "Ice" && currentPosition.name == "Ice")
            {
                renderer.enabled = true;
            }
        }
    }

    public void FillWithRed()
    {
        FillWithColor("Red");
    }

    public void FillWithYellow()
    {
        FillWithColor("Yellow");
    }

    public void FillWithBlue()
    {
        FillWithColor("Blue");
    }

   
    public void FillWithWhite()
    {
        FillWithColor("White");
    }

    private void FillWithColor(string colorName)
    {
        foreach (var renderer in currentGlass.GetComponentsInChildren<Renderer>())
        {
            if (renderer.tag == "Cocktail" && currentPosition.name == colorName)
            {
                var newColor = GetNewColor(cocktailColor, GetColorBasedOnName(colorName));
                cocktailColor = newColor;
                renderer.material.color = cocktailColor;
                renderer.enabled = true;
            }
        }
    }

    private void MoveGlass()
    {

    }

    static Color GetColorBasedOnName(string colorName)
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

    private Color GetNewColor(Color baseColor, Color fillingColor)
    {
        if (baseColor == Color.black)
        {
            return fillingColor;
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
