using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInputUtil;

public class FrogManager : MonoBehaviour
{
    public FrogData FrogData; //Assigned by GameState
    public Transform SpriteTransform;
    public Animator FrogAnimator;

    private Transform _frogTransform;

    public void Initialize(FrogData frogData)
    {
        FrogData = frogData;
        _frogTransform = GetComponent<Transform>();
    }

    public void ResetFrogToStartPosition(GameConfig gameConfig)
    {
        FrogData.State = FrogState.Idle;
        FrogData.CurrentPosition = gameConfig.FROG_START_POINT;
        _frogTransform.position = FrogData.CurrentPosition;
        SpriteTransform.eulerAngles = GetSpriteRotationValue(FacingDirection.Up);
        FrogAnimator.Play("Idle");
    }

    public FrogState TickUpdate(PlayerFrogAction inputFrogAction, GameStateSnapshot lastTickSnapshot, float dt, GameConfig gameConfig)
    {
        FrogData.UpdateFrogData(inputFrogAction, lastTickSnapshot, dt, gameConfig);
        _frogTransform.position = FrogData.CurrentPosition;
        FrogAnimator.SetBool("Jumping", FrogData.State == FrogState.Jumping);
        if (FrogData.State == FrogState.Jumping)
        {
            SpriteTransform.eulerAngles = GetSpriteRotationValue(FrogData.FacingDirection);
            FrogAnimator.SetBool("Jumping", true);
        }
        else if (FrogData.State == FrogState.Die)
        {
            SpriteTransform.eulerAngles = GetSpriteRotationValue(FacingDirection.Up);
            if (FrogData.IsDrown())
            {
                FrogAnimator.SetBool("DrownDie", true);
            }
            else
            {
                FrogAnimator.SetBool("CollisionDie", true);
            }
        }
        else if (FrogData.State == FrogState.InFinishLine)
        {
            FrogAnimator.SetBool("GoInvisible", true);
        }
        else
        {
            FrogAnimator.SetBool("GoInvisible", false);
            FrogAnimator.SetBool("CollisionDie", false);
            FrogAnimator.SetBool("DrownDie", false);
        }

        return FrogData.State;
    }

    private Vector3 GetSpriteRotationValue(FacingDirection facingDirection)
    {
        switch (facingDirection)
        {
            case FacingDirection.Up: return new Vector3(0, 0, 0);
            case FacingDirection.Down: return new Vector3(0, 0, 180);
            case FacingDirection.Left: return new Vector3(0, 0, 90);
            case FacingDirection.Right: return new Vector3(0, 0, -90);
            default: return new Vector3(0, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        FrogData.UpdateFrogDataTriggerEnter2D(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        FrogData.UpdateFrogDataTriggerOnExit2D(other);
    }
}