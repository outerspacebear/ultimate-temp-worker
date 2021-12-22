using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassManager : MonoBehaviour
{
    public Transform startingPosition;
    public List<GlassPosition> possibleGlassPositions;
    public List<GlassPosition> possibleCounterPositions;
    public GlassPosition binPosition;
    public AudioSource glassBinnedSound;
    public GameObject prefabGlass;

    private List<GlassPosition> unusedCounterPositions;

    private Glass glass;

    bool isDraggingGlass = false;

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

        if (TouchUtils.HasTouchBegan() && IsTouchOnGlass(Input.touches[0]))
        {
            isDraggingGlass = true;
        }

        if(isDraggingGlass)
        {
            var touchPosition = TouchUtils.GetWorldPosition(Input.touches[0]);
            var destinationPosition = Vector3.Lerp(glass.currentGlass.transform.position, touchPosition, Time.deltaTime * 20f);

            destinationPosition.z = glass.currentGlass.transform.position.z;

            glass.currentGlass.transform.position = destinationPosition;
        }

        if(TouchUtils.HasTouchEnded() && isDraggingGlass)
        {
            isDraggingGlass = false;
            var closestGlassPosition = GetClosestGlassPosition(glass.currentGlass.transform);
            glass.currentGlass.transform.position = closestGlassPosition.position.position;
            glass.currentPosition = closestGlassPosition;

            if(closestGlassPosition.position.position == binPosition.position.position)
            {
                glassBinnedSound.Play();
                DiscardGlass();
            }
            else if(unusedCounterPositions.Contains(closestGlassPosition))
            {
                OnMoveGlassToCounter();
            }
        }
    }

    GlassPosition GetClosestGlassPosition(Transform sourcePosition)
    {
        float minDistance = Mathf.Infinity;
        GlassPosition nearestPosition = new GlassPosition();

        foreach(var potentialPosition in possibleGlassPositions)
        {
            var distance = Vector3.Distance(sourcePosition.position, potentialPosition.position.position);
            if(distance < minDistance)
            {
                minDistance = distance;
                nearestPosition = potentialPosition;
            }
        }
        foreach (var potentialPosition in unusedCounterPositions)
        {
            var distance = Vector3.Distance(sourcePosition.position, potentialPosition.position.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPosition = potentialPosition;
            }
        }

        {
            var distance = Vector3.Distance(sourcePosition.position, binPosition.position.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPosition = binPosition;
            }
        }

        return nearestPosition;
    }

    private void Init()
    {
        InitGlass();
        InitUnusedCounterPositions();
    }

    public void InitUnusedCounterPositions()
    {
        unusedCounterPositions = new List<GlassPosition>(possibleCounterPositions);
    }

    private bool IsTouchOnGlass(Touch touch)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject == glass.currentGlass)
                return true;
        }

        return false;
    }

    void InitGlass() 
    {
        GameObject currentGlass = Instantiate(prefabGlass, startingPosition.position, startingPosition.rotation);

        foreach (Transform child in currentGlass.transform)
        {
            child.GetComponent<Renderer>().enabled = false;
        }

        glass = new Glass {
            currentGlass = currentGlass,
            currentPosition = new GlassPosition { name = "Starting Position", position = startingPosition },
            cocktailColor = CocktailEnum.Empty,
            hasIce = false
        };
    }

    public void PourIce()
    {
        foreach (var renderer in glass.currentGlass.GetComponentsInChildren<Renderer>())
        {
            if (renderer.tag == "Ice" && glass.currentPosition.name == "Ice")
            {
                glass.hasIce = true;
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

    public void OnMoveGlassToCounter()
    {
        if (unusedCounterPositions.Count > 0)
        {
            RemoveGlassPositionFromUnusedCounterPositions();
            SubmitGlassToOrder();
            InitGlass();
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

    private GlassPosition GetRandomCounterGlassPosition()
    {
        int spawnIndex = UnityEngine.Random.Range(0, unusedCounterPositions.Count);
        return unusedCounterPositions[spawnIndex];
    }

    private void DiscardGlass()
    {
        Destroy(glass.currentGlass);
        InitGlass();
    }

    private void MoveGlassToPosition(GlassPosition newGlassPosition)
    {
        glass.currentGlass.transform.position = newGlassPosition.position.position;
        glass.currentPosition = newGlassPosition;
    }
}
