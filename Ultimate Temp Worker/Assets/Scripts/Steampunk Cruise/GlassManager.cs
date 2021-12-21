using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassManager : MonoBehaviour
{
    public Transform startingPosition;
    public List<GlassPosition> possibleGlassPositions;
    public List<GlassPosition> possibleCounterPositions;
    public GameObject prefabGlass;

    private List<GlassPosition> unusedCounterPositions;

    private Glass glass;

    private Touch initialTouch;

    void Start()
    {
        Init();
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

    private void Init()
    {
        InitGlassAtRandom();
        unusedCounterPositions = possibleCounterPositions;
    }

    void InitGlassAtRandom() 
    {
        GameObject currentGlass = Instantiate(prefabGlass, startingPosition.position, startingPosition.rotation);

        foreach (Transform child in currentGlass.transform)
        {
            child.GetComponent<Renderer>().enabled = false;
        }

        glass = new Glass {
            currentGlass = currentGlass,
            currentPosition = new GlassPosition { name = "Starting Position", position = startingPosition },
            cocktailColor = CocktailEnum.Empty
        };
    }

    public void PourIce()
    {
        foreach (var renderer in glass.currentGlass.GetComponentsInChildren<Renderer>())
        {
            if (renderer.tag == "Ice" && glass.currentPosition.name == "Ice")
            {
                renderer.enabled = true;
            }
        }
    }

    public void FillWithRed()
    {
        glass = MixerUtils.FillWithColor(glass, "Red");
    }

    public void FillWithYellow()
    {
        glass = MixerUtils.FillWithColor(glass, "Yellow");
    }

    public void FillWithBlue()
    {
        glass = MixerUtils.FillWithColor(glass, "Blue");
    }

   
    public void FillWithWhite()
    {
        glass = MixerUtils.FillWithColor(glass, "White");
    }

    private void MoveGlass(TouchUtils.MoveType moveType)
    {
        switch (moveType)
        {
            case TouchUtils.MoveType.Up:
                OnMoveGlassUp();
                return;

            case TouchUtils.MoveType.Down:
                DiscardGlass();
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

    public void OnMoveGlassUp()
    {
        if (IsGlassInStartingPosition())
        {
            return;
        }

        if (unusedCounterPositions.Count > 0)
        {
            MoveGlassToCounter();
            RemoveGlassPositionFromUnusedCounterPositions();
            SubmitGlassToOrder();
            InitGlassAtRandom();
        }
    }

    private void MoveGlassToCounter()
    {
        MoveGlassToPosition(GetRandomCounterGlassPosition());
        
    }

    private void RemoveGlassPositionFromUnusedCounterPositions()
    {
        unusedCounterPositions.RemoveAll(position => glass.currentPosition.position == position.position);
    }

    private void SubmitGlassToOrder()
    {
        SteampunkEvents.addGlassToOrderEvent.Invoke(glass);
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
        if (glass.currentPosition.position.position == startingPosition.position)
        {
            return 0;
        }

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
            if (possibleGlassPositions[i].position == glass.currentPosition.position)
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
            return new GlassPosition { name = "null", position = null };
        }

        int spawnIndex = UnityEngine.Random.Range(0, possibleGlassPositions.Count);
        return possibleGlassPositions[spawnIndex];
    }

    private GlassPosition GetRandomCounterGlassPosition()
    {
        if (possibleCounterPositions.Count == 0)
        {
            Debug.Log("Empty counter positions, check assets");
            return new GlassPosition { name = "null", position = null };
        }

        int spawnIndex = UnityEngine.Random.Range(0, possibleCounterPositions.Count);
        return possibleCounterPositions[spawnIndex];
    }

    private void DiscardGlass()
    {
        Destroy(glass.currentGlass);
        InitGlassAtRandom();
    }

    private void MoveGlassToPosition(GlassPosition newGlassPosition)
    {
        glass.currentGlass.transform.position = newGlassPosition.position.position;
        glass.currentPosition = newGlassPosition;
    }

    private bool IsGlassInStartingPosition()
    {
        return glass.currentPosition.position.position == startingPosition.position;
    }
}
