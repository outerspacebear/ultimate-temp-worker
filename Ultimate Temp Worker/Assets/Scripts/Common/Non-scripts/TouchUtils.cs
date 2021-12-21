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
        bool isHorizontalSwipe = Mathf.Abs(deltaX) > Mathf.Abs(deltaY);

        if (isHorizontalSwipe)
        {
            if (deltaX > 0)
                return MoveType.Right;
            else if (deltaX < 0)
                return MoveType.Left;
        }
        //vertical swipe
        else if (!isHorizontalSwipe)
        {
            //up
            if (deltaY > 0)
                return MoveType.Up;
            //down
            else if (deltaY < 0)
                return MoveType.Down;
        }
        return MoveType.None;
    }
}
