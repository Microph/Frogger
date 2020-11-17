using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputUtil
{
    public enum PlayerFrogAction
    {
        None,
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight
    }

    public static PlayerFrogAction ConvertVector2ToPlayerFrogActionEnum(Vector2 v)
    {
        if (v.y > 0)
        {
            return PlayerFrogAction.MoveUp;
        }
        else if(v.y < 0)
        {
            return PlayerFrogAction.MoveDown;
        }
        else if(v.x < 0)
        {
            return PlayerFrogAction.MoveLeft;
        }
        else if(v.x > 0)
        {
            return PlayerFrogAction.MoveRight;
        }
        else
        {
            return PlayerFrogAction.None;
        }
    }

    public static FacingDirection ActionEnumToFacingDirection(PlayerFrogAction playerFrogActionEnum)
    {
        switch (playerFrogActionEnum)
        {
            case PlayerFrogAction.MoveUp: return FacingDirection.Up;
            case PlayerFrogAction.MoveDown: return FacingDirection.Down;
            case PlayerFrogAction.MoveLeft: return FacingDirection.Left;
            case PlayerFrogAction.MoveRight: return FacingDirection.Right;
            default: return FacingDirection.Up;
        }
    }

    public static Vector2 GetNormalizedVector2FromFacingDirection(FacingDirection facingDirection)
    {
        switch (facingDirection)
        {
            case FacingDirection.Up: return new Vector2(0, 1);
            case FacingDirection.Down: return new Vector2(0, -1);
            case FacingDirection.Left: return new Vector2(-1, 0);
            case FacingDirection.Right: return new Vector2(1, 0);
            default: return new Vector2(0, 1);
        }
    }
}
