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

    private bool _isOnPlatform = false;
    private int _platformRowIndex = -1;

    public FrogData(MovableEntityData movableEntityData) : base(movableEntityData)
    {
    }

    public bool IsDrown()
    {
        bool isDrown = CurrentPosition.y >= -0.5f && State != FrogState.Jumping && !_isOnPlatform;
        Debug.Log("isDrown:" + isDrown);
        return isDrown;
    }

    public bool IsOnGround()
    {
        return CurrentPosition.y < -0.5f;
    }

    public void UpdateFrogDataTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Car"))
        {
            State = FrogState.Die;
        }
        else if (other.tag.Equals("Platform"))
        {
            _platformRowIndex = other.GetComponentInParent<ObstacleGameObject>().RowIndex;
        }
    }

    public void UpdateFrogDataTriggerOnStay2D(Collider2D other)
    {
        if (State == FrogState.Idle && other.tag.Equals("Platform"))
        {
            _isOnPlatform = true;
        }
    }

    public void UpdateFrogDataTriggerOnExit2D(Collider2D other)
    {
        if (other.tag.Equals("Platform"))
        {
            _isOnPlatform = false;
        }
    }

    public void UpdateFrogData(PlayerFrogAction inputFrogAction, GameStateSnapshot lastTickSnapshot, float dt, GameConfig gameConfig)
    {
        FacingDirection InputFrogActionDirection = PlayerInputUtil.ActionEnumToFacingDirection(inputFrogAction);

        switch (State)
        {
            case FrogState.Idle:
                if(IsDrown())
                {
                    State = FrogState.Die;
                    return;
                }

                if(_isOnPlatform)
                {
                    CurrentPosition = MoveWithPlatform(CurrentPosition, gameConfig, _platformRowIndex, dt);
                }

                _currentMoveCoolDown -= _currentMoveCoolDown > 0 ? dt : 0;
                if (inputFrogAction != PlayerFrogAction.None)
                {
                    if (_currentMoveCoolDown > 0)
                    {
                        //Debug.Log("MOVE COOLDOWN");
                        return;
                    }
                    
                    if (inputFrogAction == lastTickSnapshot.InputFrogAction && FacingDirection == InputFrogActionDirection)
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
                    CurrentPosition = Vector2.Lerp(_startPosition, _targetPosition, _elapsedJumpingTime / gameConfig.FROG_JUMP_TIME);
                }
                else
                {
                    CurrentPosition = _targetPosition;
                    _elapsedJumpingTime = 0;
                    State = FrogState.Idle;
                }

                if (IsOnGround())
                {
                    CurrentPosition.x = Mathf.Clamp(CurrentPosition.x, -6.5f, 6.5f);
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

    private Vector2 MoveWithPlatform(Vector2 currentPosition, GameConfig gameConfig, int rowIndex, float dt)
    {
        if (rowIndex == -1)
        {
            return currentPosition;
        }

        float unitPerSec = gameConfig.RowDataConfigs[rowIndex].RowMovingUnitPerSec;
        RowMovingDirection rowMovingDirection = gameConfig.RowDataConfigs[rowIndex].RowMovingDirection;
        if (rowMovingDirection == RowMovingDirection.Right)
        {
            return new Vector2(currentPosition.x + (unitPerSec * dt), currentPosition.y);
        }
        else
        {
            return new Vector2(currentPosition.x - (unitPerSec * dt), currentPosition.y);
        }
        
    }
}
