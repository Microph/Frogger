using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableEntityData
{
    public Vector2 CurrentPosition;
    public SpriteRenderer SpriteRenderer;
    public FacingDirection FacingDirection;
    public int Width;

    public MovableEntityData()
    {
    }

    public MovableEntityData(Vector2 currentPosition, SpriteRenderer spriteRenderer, FacingDirection facingDirection)
    {
        CurrentPosition = currentPosition;
        SpriteRenderer = spriteRenderer;
        FacingDirection = facingDirection;
    }

    public MovableEntityData(MovableEntityData movableEntityData)
    {
        CurrentPosition = movableEntityData.CurrentPosition;
        SpriteRenderer = movableEntityData.SpriteRenderer;
        FacingDirection = movableEntityData.FacingDirection;
    }
}

public enum FacingDirection
{
    Right,
    Down,
    Left,
    Up
}