using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FacingDirection
{
    Right,
    Down,
    Left,
    Up
}

public class MovableEntityData
{
    public Vector2 CurrentPosition;
    public FacingDirection FacingDirection;
    public int Width;

    public MovableEntityData()
    {
    }

    public MovableEntityData(Vector2 currentPosition, FacingDirection facingDirection)
    {
        CurrentPosition = currentPosition;
        FacingDirection = facingDirection;
    }

    public MovableEntityData(MovableEntityData movableEntityData)
    {
        CurrentPosition = movableEntityData.CurrentPosition;
        FacingDirection = movableEntityData.FacingDirection;
    }
}
