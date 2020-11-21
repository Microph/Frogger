using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurtleState
{
    Initilize,
    Swimming_OnOffset,
    Swimming_PastOffset,
    StartDiving,
    Diving
}

public class Turtle : Obstacle
{
    public float StartOffsetTime = 1f;
    public float RepeatIntervalTime = 1f;
    public float StartDivingTime = 1f;
    public float DivingTime = 1f;

    private Animator[] _turtleAnimators;
    private float _currentOffsetTimer;
    private float _currentIntervalTimer;
    private float _currentStartDivingTimer;
    private float _currentDivingTimer;
    private TurtleState _currentTurtleState;

    private void OnEnable()
    {
        _currentTurtleState = TurtleState.Initilize;
    }

    private void InitilizeTimers(GameConfig gameConfig)
    {
        StartOffsetTime = UnityEngine.Random.Range(gameConfig.TURTLE_START_OFFSET_MIN, gameConfig.TURTLE_START_OFFSET_MAX);
        RepeatIntervalTime = UnityEngine.Random.Range(gameConfig.TURTLE_REPEAT_INTERVAL_MIN, gameConfig.TURTLE_REPEAT_INTERVAL_MAX);
    }

    private void PlayerAnimationState(Animator[] childrenAnimators, string str)
    {
        for (int i = 0; i < childrenAnimators.Length; i++)
        {
            childrenAnimators[i].Play(str);
        }
    }

    private void SetActiveColliders(Collider2D[] childrenColiiders, bool isEnabled)
    {
        for (int i = 0; i < childrenColiiders.Length; i++)
        {
            childrenColiiders[i].enabled = isEnabled;
        }
    }

    public override void UpdateTick(float dt, GameConfig gameConfig, RowData rowData)
    {
        base.UpdateTick(dt, gameConfig, rowData);

        switch (_currentTurtleState)
        {
            case TurtleState.Initilize:
                InitilizeTimers(gameConfig);
                _currentOffsetTimer = StartOffsetTime;
                _currentIntervalTimer = RepeatIntervalTime;
                _currentStartDivingTimer = StartDivingTime;
                _currentDivingTimer = DivingTime;
                _currentTurtleState = TurtleState.Swimming_OnOffset;
                PlayerAnimationState(_childrenAnimators, "Swimming");
                break;
            case TurtleState.Swimming_OnOffset:
                _currentOffsetTimer -= dt;
                if (_currentOffsetTimer < 0)
                {
                    _currentOffsetTimer = StartOffsetTime;
                    _currentTurtleState = TurtleState.StartDiving;
                    PlayerAnimationState(_childrenAnimators, "StartDiving");
                }
                break;
            case TurtleState.Swimming_PastOffset:
                _currentIntervalTimer -= dt;
                if (_currentIntervalTimer < 0)
                {
                    _currentIntervalTimer = RepeatIntervalTime;
                    _currentTurtleState = TurtleState.StartDiving;
                    PlayerAnimationState(_childrenAnimators, "StartDiving");
                }
                break;
            case TurtleState.StartDiving:
                _currentStartDivingTimer -= dt;
                if (_currentStartDivingTimer < 0)
                {
                    _currentStartDivingTimer = StartDivingTime;
                    SetActiveColliders(_childrenColliders, false);
                    _currentTurtleState = TurtleState.Diving;
                    PlayerAnimationState(_childrenAnimators, "Diving");
                }
                break;
            case TurtleState.Diving:
                _currentStartDivingTimer -= dt;
                if (_currentStartDivingTimer < 0)
                {
                    _currentStartDivingTimer = StartDivingTime;
                    SetActiveColliders(_childrenColliders, true);
                    _currentTurtleState = TurtleState.Swimming_PastOffset;
                    PlayerAnimationState(_childrenAnimators, "Swimming");
                }
                break;
            default: break;
        }
    }
}
