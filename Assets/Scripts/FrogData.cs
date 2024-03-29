﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInputUtil;

public enum FrogState
{
    Idle,
    Jumping,
    Die,
    InFinishLine,
    WaitUpdateDie,
    WaitUpdateFinishLine
}

public class FrogData : MovableEntityData
{
    public FrogState State;
    public float _currentMoveCoolDown = 0.1f;
    public float _currentSameMovePenaltyTime = 0.15f;
    public float _elapsedJumpingTime = 0f;
    public float _currentRestartGameFlowDelay = 2f;
    public Vector2 _startPosition;
    public Vector2 _targetPosition;

    public bool IsOnPlatform { get => _isOnPlatformCounter > 0; }
    private int _isOnPlatformCounter = 0;
    private int _platformRowIndex = -1;

    public FrogData(MovableEntityData movableEntityData) : base(movableEntityData)
    {
    }

    public bool IsDrown()
    {
        bool isDrown = CurrentPosition.y >= -0.5f && State != FrogState.Jumping && !IsOnPlatform;
        return isDrown;
    }

    public bool IsOnGround()
    {
        return CurrentPosition.y < -0.5f;
    }

    public void UpdateFrogDataTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Finish"))
        {
            State = FrogState.InFinishLine;
        }
        else if (other.tag.Equals("Car") && (State == FrogState.Idle || State == FrogState.Jumping))
        {
            State = FrogState.Die;
        }
        else if (other.tag.Equals("Platform"))
        {
            _isOnPlatformCounter++;
            _platformRowIndex = other.GetComponentInParent<Obstacle>().RowIndex;
        }
        
    }

    public void UpdateFrogDataTriggerOnExit2D(Collider2D other)
    {
        if (other.tag.Equals("Platform"))
        {
            _isOnPlatformCounter--;
        }
    }

    public void UpdateFrogData(PlayerFrogAction inputFrogAction, GameStateSnapshot lastTickSnapshot, float dt, GameConfig gameConfig)
    {
        FacingDirection InputFrogActionDirection = PlayerInputUtil.ActionEnumToFacingDirection(inputFrogAction);
        switch (State)
        {
            case FrogState.Idle:
                _currentMoveCoolDown -= _currentMoveCoolDown > 0 ? dt : 0;
                if (inputFrogAction != PlayerFrogAction.None)
                {
                    if (_currentMoveCoolDown > 0)
                    {
                        //MOVE COOLDOWN
                        return;
                    }
                    
                    if (inputFrogAction == lastTickSnapshot.InputFrogAction && FacingDirection == InputFrogActionDirection)
                    { 
                        if (_currentSameMovePenaltyTime > 0)
                        {
                            //"SAME MOVE PENALTY"
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
                else if (IsOnPlatform)
                {
                    CurrentPosition = MoveWithPlatform(CurrentPosition, gameConfig, _platformRowIndex, dt, lastTickSnapshot);
                }
                else if (IsDrown())
                {
                    State = FrogState.Die;
                }
                break;
            case FrogState.Jumping:
                _elapsedJumpingTime += dt;
                float lerpAmount = _elapsedJumpingTime / gameConfig.FROG_JUMP_TIME;
                if (lerpAmount < 1)
                {
                    CurrentPosition = Vector2.Lerp(_startPosition, _targetPosition, lerpAmount);
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
                    CurrentPosition.y = Mathf.Clamp(CurrentPosition.y, -7.5f, 7.5f);
                }
                break;
            case FrogState.Die:
                _currentRestartGameFlowDelay -= dt;
                if(_currentRestartGameFlowDelay <= 0)
                {
                    _currentRestartGameFlowDelay = gameConfig.RESTART_GAME_FLOW_DELAY;
                    State = FrogState.WaitUpdateDie;
                }
                break;
            case FrogState.InFinishLine:
                _currentRestartGameFlowDelay -= dt;
                if (_currentRestartGameFlowDelay <= 0)
                {
                    _currentRestartGameFlowDelay = gameConfig.RESTART_GAME_FLOW_DELAY;
                    State = FrogState.WaitUpdateFinishLine;
                }
                break;
            default: break;
        }
    }

    private Vector2 MoveWithPlatform(Vector2 currentPosition, GameConfig gameConfig, int rowIndex, float dt, GameStateSnapshot lastTick)
    {
        if (rowIndex == -1)
        {
            return currentPosition;
        }

        float unitPerSec = gameConfig.RowDataConfigs[rowIndex].GetRowMovingUnitPerSec() * GameState.GetGameSpeedModifier(gameConfig, lastTick.CurrentRound);
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
