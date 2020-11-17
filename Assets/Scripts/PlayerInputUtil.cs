using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputUtil
{
    public enum PlayerFrogActionEnum
    {
        None,
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight
    }

    public static PlayerFrogActionEnum ConvertVector2ToPlayerFrogActionEnum(Vector2 v)
    {
        if (v.y > 0)
        {
            return PlayerFrogActionEnum.MoveUp;
        }
        else if(v.y < 0)
        {
            return PlayerFrogActionEnum.MoveDown;
        }
        else if(v.x < 0)
        {
            return PlayerFrogActionEnum.MoveLeft;
        }
        else if(v.x > 0)
        {
            return PlayerFrogActionEnum.MoveRight;
        }
        else
        {
            return PlayerFrogActionEnum.None;
        }
    }

    public static FacingDirection ActionEnumToFacingDirection(PlayerFrogActionEnum playerFrogActionEnum)
    {
        switch (playerFrogActionEnum)
        {
            case PlayerFrogActionEnum.MoveUp: return FacingDirection.Up;
            case PlayerFrogActionEnum.MoveDown: return FacingDirection.Down;
            case PlayerFrogActionEnum.MoveLeft: return FacingDirection.Left;
            case PlayerFrogActionEnum.MoveRight: return FacingDirection.Right;
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
