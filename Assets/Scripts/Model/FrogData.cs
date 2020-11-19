using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInputUtil;

public enum FrogState
{
    Idle,
    Jumping,
    Die,
    InFinishLine
}

public class FrogData : MovableEntityData
{
    public FrogState State;
    public float _currentMoveCoolDown = 0.1f;
    public float _currentSameMovePenaltyTime = 0.15f;
    public float _elapsedJumpingTime = 0f;
    public Vector2 _startPosition;
    public Vector2 _targetPosition;
    public bool _isOnPlatform = false;

    public FrogData(MovableEntityData movableEntityData) : base(movableEntityData)
    {
    }

    public bool IsDrown()
    {
        return CurrentPosition.y >= -0.5f && !_isOnPlatform;
    }

    public void UpdateFrogDataTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Car"))
        {
            State = FrogState.Die;
        }
        else if (other.tag.Equals("Platform"))
        {
            _isOnPlatform = true;
        }
    }

    public void UpdateFrogDataTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Platform"))
        {
            _isOnPlatform = false;
        }
    }

    public void UpdateFrogData(PlayerFrogAction inputFrogAction, GameStateSnapshot lastFrameSnapshot, float dt, GameConfig gameConfig)
    {
        FacingDirection InputFrogActionDirection = PlayerInputUtil.ActionEnumToFacingDirection(inputFrogAction);

        switch (State)
        {
            case FrogState.Idle:
                //Check if on water
                if(IsDrown())
                {
                    State = FrogState.Die;
                    return;
                }

                _currentMoveCoolDown -= _currentMoveCoolDown > 0 ? dt : 0;
                if (inputFrogAction != PlayerFrogAction.None)
                {
                    if (_currentMoveCoolDown > 0)
                    {
                        //Debug.Log("MOVE COOLDOWN");
                        return;
                    }
                    
                    if (inputFrogAction == lastFrameSnapshot.InputFrogAction && FacingDirection == InputFrogActionDirection)
                    { 
                        if (_currentSameMovePenaltyTime > 0)
                        {
                            //Debug.Log("SAME MOVE PENALTY");
                            _currentSameMovePenaltyTime -= _currentSameMovePenaltyTime > 0 ? dt : 0;
                            return;
                        }
                    }
                    else
                    {
                        _currentSameMovePenaltyTime = gameConfig.SAME_MOVE_PENALTY_TIME;
                    }

                    FacingDirection = InputFrogActionDirection;
                    _startPosition = CurrentPosition;
                    _targetPosition = _startPosition + (PlayerInputUtil.GetNormalizedVector2FromFacingDirection(FacingDirection) * gameConfig.FROG_JUMP_DISTANCE);
                    _currentMoveCoolDown = gameConfig.MOVE_COOLDOWN;
                    State = FrogState.Jumping;
                }
                break;
            case FrogState.Jumping:
                _elapsedJumpingTime += dt;
                float lerpAmount = _elapsedJumpingTime / gameConfig.FROG_JUMP_TIME;
                if (lerpAmount < 1)
                {
                    //TODO: Check hitting corner first
                    CurrentPosition = Vector2.Lerp(_startPosition, _targetPosition, _elapsedJumpingTime / gameConfig.FROG_JUMP_TIME);
                    //TODO: check falling into water / hit obstacle
                }
                else
                {
                    CurrentPosition = _targetPosition;
                    _elapsedJumpingTime = 0;
                    State = FrogState.Idle;
                }
                //Debug.Log($"CurrentPosition: {CurrentPosition}");
                break;
            case FrogState.Die:
                break;
            case FrogState.InFinishLine:
                break;
            default: break;
        }
    }
}
