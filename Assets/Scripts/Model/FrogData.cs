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
    public float _currentSameMoveDelay = 1f;
    public float _elapsedJumpingTime = 0f;
    public Vector2 _startPosition;
    public Vector2 _targetPosition;
    public FrogState State;

    public FrogData(MovableEntityData movableEntityData) : base(movableEntityData)
    {
    }

    public void UpdateFrogData(PlayerFrogAction inputFrogAction, GameStateSnapshot lastFrameSnapshot, float dt, GameConfig gameConfig)
    {
        FacingDirection InputFrogActionDirection = PlayerInputUtil.ActionEnumToFacingDirection(inputFrogAction);

        switch (State)
        {
            case FrogState.Idle:
                if (inputFrogAction != PlayerFrogAction.None)
                {
                    if (inputFrogAction == lastFrameSnapshot.InputFrogAction && FacingDirection == InputFrogActionDirection)
                    {
                        if (_currentSameMoveDelay > 0)
                        {
                            //Debug.Log("SAME MOVE DELAY");
                            _currentSameMoveDelay -= dt;
                        }
                        else
                        {
                            FacingDirection = InputFrogActionDirection;
                            _startPosition = CurrentPosition;
                            _targetPosition = _startPosition + (PlayerInputUtil.GetNormalizedVector2FromFacingDirection(FacingDirection) * gameConfig.FROG_JUMP_DISTANCE);
                            State = FrogState.Jumping;
                        }
                    }
                    else
                    {
                        _currentSameMoveDelay = gameConfig.SAME_MOVE_DELAY;
                        FacingDirection = InputFrogActionDirection;
                        _startPosition = CurrentPosition;
                        _targetPosition = _startPosition + (PlayerInputUtil.GetNormalizedVector2FromFacingDirection(FacingDirection) * gameConfig.FROG_JUMP_DISTANCE);
                        State = FrogState.Jumping;
                    }
                }
                else
                {
                    _currentSameMoveDelay = gameConfig.SAME_MOVE_DELAY;
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
