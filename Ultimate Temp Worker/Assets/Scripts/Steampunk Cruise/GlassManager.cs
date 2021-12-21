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

    public List<GlassPosition> possibleGlassPositions;
    public GameObject prefabGlass;

    private GameObject currentGlass;
    private GlassPosition currentPosition;
    private Color cocktailColor = Color.black;

    private Touch initialTouch;

    // Start is called before the first frame update
    void Start()
    {
        InitGlassAtRandom();
    }

    void Update()
    {
        if (!TouchUtils.DoesAnyTouchExist())
        {
            return;
        }

        if (TouchUtils.HasTouchBegan())
        {
            initialTouch = Input.touches[0];
        }

        var touch = Input.touches[0];
        if (TouchUtils.IsSwipe(touch))
        {
            var moveType = TouchUtils.GetMoveType(initialTouch, touch);
            MoveGlass(moveType);
        }
    }

    void InitGlassAtRandom() 
    {
        currentPosition = GetRandomSpawnableGlassPosition();

        currentGlass = Instantiate(prefabGlass, currentPosition.position.position, currentPosition.position.rotation);

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

    private void MoveGlass(TouchUtils.MoveType moveType)
    {
        switch (moveType)
        {
            case TouchUtils.MoveType.Up:
                MoveGlassUp();
                return;

            case TouchUtils.MoveType.Down:
                MoveGlassDownOrDiscard();
                return;

            case TouchUtils.MoveType.Left:
                MoveGlassToLeft();
                return;

            case TouchUtils.MoveType.Right:
                MoveGlassToRight();
                return;

            case TouchUtils.MoveType.None:
            default:
                return;
        }
    }

    public void MoveGlassUp()
    {
        var glassPosition = FindGlassPosition("Counter");
        if (glassPosition.name == "null" || glassPosition.position == currentPosition.position)
        {
            return;
        }
        MoveGlassToPosition(glassPosition);
    }

    public void MoveGlassDownOrDiscard()
    {
        var glassPosition = FindGlassPosition("Counter");
        if (glassPosition.name == "null")
        {
            return;
        }

        if (glassPosition.position == currentPosition.position)
        {
            MoveGlassToPosition(GetRandomSpawnableGlassPosition());
        }
        else
        {
            DiscardGlass();
        }
    }

    public void MoveGlassToLeft()
    {
        int leftIndex = FindIndexOfPositionToLeft();
        if (leftIndex != -1)
        {
            MoveGlassToPosition(possibleGlassPositions[leftIndex]);
        }
    }

    public void MoveGlassToRight()
    {
        int rightIndex = FindIndexOfPositionToRight();
        if (rightIndex != -1)
        {
            MoveGlassToPosition(possibleGlassPositions[rightIndex]);
        }
    }

    private int FindIndexOfPositionToLeft()
    {
        int currentPositionIndex = GetCurrentPositionIndex();
        if (currentPositionIndex == -1)
        {
            return currentPositionIndex;
        }

        int leftPositionIndex = currentPositionIndex - 1;
        if (leftPositionIndex < 0)
        {
            leftPositionIndex = possibleGlassPositions.Count - 1;
        }

        return leftPositionIndex;
    }

    private int FindIndexOfPositionToRight()
    {
        int currentPositionIndex = GetCurrentPositionIndex();
        if (currentPositionIndex == -1)
        {
            return currentPositionIndex;
        }

        int rightPositionIndex = currentPositionIndex + 1;
        if (rightPositionIndex >= possibleGlassPositions.Count)
        {
            rightPositionIndex = 0;
        }

        return rightPositionIndex;
    }

    private int GetCurrentPositionIndex()
    {
        int currentPositionIndex = -1;
        for (int i = 0; i < possibleGlassPositions.Count; ++i)
        {
            if (possibleGlassPositions[i].position == currentPosition.position)
            {
                currentPositionIndex = i;
                continue;
            }
        }

        return currentPositionIndex;
    }

    private GlassPosition GetRandomSpawnableGlassPosition()
    {
        if (possibleGlassPositions.Count == 0)
        {
            Debug.Log("Empty glass positions, check assets");
            return new GlassPosition { name = "null", position = null, isSpawnable = false };
        }

        int spawnIndex;
        Transform spawnPoint = possibleGlassPositions[0].position;

        while (true)
        {
            spawnIndex = UnityEngine.Random.Range(0, possibleGlassPositions.Count);
            if (possibleGlassPositions[spawnIndex].isSpawnable)
            {
                return possibleGlassPositions[spawnIndex];
            }
        }
    }

    private void DiscardGlass()
    {
        Destroy(currentGlass);
        InitGlassAtRandom();
    }

    private GlassPosition FindGlassPosition(string name)
    {
        foreach (var glassPosition in possibleGlassPositions)
        {
            if (glassPosition.name == name)
            {
                return glassPosition;
            }
        }
        return new GlassPosition{name = "null", position = null, isSpawnable = false};
    }

    private void MoveGlassToPosition(GlassPosition newGlassPosition)
    {
        currentGlass.transform.position = newGlassPosition.position.position;
        currentPosition = newGlassPosition;
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
