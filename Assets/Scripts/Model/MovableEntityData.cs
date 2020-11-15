using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableEntityData
{
    public Vector2 CurrentPosition;
    public SpriteRenderer SpriteRenderer;
    public FacingDirection FacingDirection;
}

public enum FacingDirection
{
    Right,
    Down,
    Left,
    Up
}