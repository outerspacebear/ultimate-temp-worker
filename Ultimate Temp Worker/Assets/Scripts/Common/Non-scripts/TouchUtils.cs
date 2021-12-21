using System;
using System.Collections.Generic;
using UnityEngine;

public static class TouchUtils
{
    public enum MoveType
    {
        Up,
        Down,
        Left,
        Right,
        None
    }

    private static float SwipeThreshold = 5f;

    public static bool DoesAnyTouchExist()
    {
        return Input.touchCount > 0;
    }

    public static bool HasTouchBegan()
    {
        return Input.touches[0].phase == TouchPhase.Began;
    }

    public static bool HasTouchEnded()
    {
        return Input.touches[0].phase == TouchPhase.Ended;
    }

    public static bool IsSwipe(Touch touch)
    {
        return touch.phase == TouchPhase.Moved;
    }

    public static MoveType GetMoveType(Touch startingTouch, Touch endingTouch)
    {
        return CalculateSwipeDirection(endingTouch.position.x - startingTouch.position.x, endingTouch.position.y - startingTouch.position.y);
    }

    private static MoveType CalculateSwipeDirection(float deltaX, float deltaY)
    {
        if (deltaX < SwipeThreshold && deltaY < SwipeThreshold)
        {
            return MoveType.None;
        }

        bool isHorizontalSwipe = Mathf.Abs(deltaX) >= (Mathf.Abs(deltaY) / 2);

        var moveType = MoveType.None;

        if (isHorizontalSwipe)
        {
            moveType = deltaX > 0 ? MoveType.Right : MoveType.Left;
        }
        else if (!isHorizontalSwipe)
        {
            moveType = deltaY > 0 ? MoveType.Up : MoveType.Down;
        }
        return moveType;
    }
}
