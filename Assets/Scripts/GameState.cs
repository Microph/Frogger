using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInputUtil;

public struct GameStateSnapshot
{
    public PlayerFrogAction InputFrogAction;
    public FrogState FrogState;
    public int CurrentScore;
    public float TimeLimit;
    public float TimeLeft;
    public int CurrentHealth;
    public int CurrentRound;
}

public class GameState : MonoBehaviour
{
    public FrogManager FrogManager;
    public ObstacleManager ObstacleManager;
    public FinishSpot[] FinishSpots;
    public int CurrentScore;
    public float TimeLimit, TimeLeft;
    public int CurrentHealth;
    public int CurrentRound = 0;

    private PlayerFrogAction _inputFrogAction = PlayerFrogAction.None;
    private FrogState _frogState;

    public static float GetGameSpeedModifier(GameConfig gameConfig, int currentRound)
    {
        return 1 + (currentRound * gameConfig.SPEED_INCREASE_PERCENT);
    }

    public GameStateSnapshot GetSnapshot()
    {
        GameStateSnapshot snapshot = new GameStateSnapshot();
        snapshot.InputFrogAction = _inputFrogAction;
        snapshot.FrogState = _frogState;
        snapshot.CurrentScore = CurrentScore;
        snapshot.TimeLimit = TimeLimit;
        snapshot.TimeLeft = TimeLeft;
        snapshot.CurrentHealth = CurrentHealth;
        snapshot.CurrentRound = CurrentRound;
        return snapshot;
    }

    public void Initialize(GameConfig gameConfig)
    {
        TimeLimit = gameConfig.GAME_TIME_LIMIT;
        TimeLeft = TimeLimit;
        CurrentHealth = gameConfig.HEALTH;
        CurrentRound = 0;
        FrogManager.Initialize(new FrogData(new MovableEntityData(gameConfig.FROG_START_POINT, FacingDirection.Up)));
        ObstacleManager.Initialize(gameConfig, CurrentRound);
    }

    private void StartNewLevel(GameConfig gameConfig)
    {
        TimeLeft = TimeLimit;
        CurrentRound += 1;
        FrogManager.ResetFrogToStartPosition(gameConfig);
        ObstacleManager.ResetToNewLevel(gameConfig, CurrentRound);
        foreach (var spot in FinishSpots)
        {
            spot.ResetState();
        }
    }

    public GameStateSnapshot UpdateToNextTick(GameStateSnapshot lastTickGameStateSnapshot, float dt, GameConfig gameConfig, PlayerInput.PlayerFrogActions currentTickPlayerFrogAction)
    {
        TimeLeft -= dt;
        if (TimeLeft <= 0)
        {
            FrogManager.FrogData.State = FrogState.Die;
        }

        ObstacleManager.TickUpdate(dt, gameConfig);
        Vector2 inputVector2 = currentTickPlayerFrogAction.Move.ReadValue<Vector2>();
        _inputFrogAction = PlayerInputUtil.ConvertVector2ToPlayerFrogActionEnum(inputVector2);
        _frogState = FrogManager.TickUpdate(_inputFrogAction, lastTickGameStateSnapshot, dt, gameConfig);
        if(lastTickGameStateSnapshot.FrogState == FrogState.Idle && _frogState == FrogState.Jumping)
        {
            if(_inputFrogAction == PlayerFrogAction.MoveUp)
            {
                CurrentScore += 10;
            }
        }

        if (_frogState == FrogState.WaitUpdateDie)
        {
            ProcessDie(gameConfig);
        }
        else if(_frogState == FrogState.WaitUpdateFinishLine)
        {
            CurrentScore += 50;
            CurrentScore += 10 * (int)(TimeLeft / 0.5f);
            ProcessFinishLine(gameConfig);
        }

        return GetSnapshot();
    }

    private void ProcessFinishLine(GameConfig gameConfig)
    {
        if(IsFinishLevel(FinishSpots))
        {
            CurrentScore += 1000;
            StartNewLevel(gameConfig);
        }
        else
        {
            FrogManager.ResetFrogToStartPosition(gameConfig);
            TimeLeft = TimeLimit;
        }
    }

    private void ProcessDie(GameConfig gameConfig)
    {
        if(CurrentHealth == 0)
        {
            GameManager.Instance.UIManager.ShowGameOver();
        }
        else
        {
            FrogManager.ResetFrogToStartPosition(gameConfig);
            TimeLeft = TimeLimit;
        }
    }

    private bool IsFinishLevel(FinishSpot[] FinishSpots)
    {
        for(int i=0; i<FinishSpots.Length; i++)
        {
            if(!FinishSpots[i].IsFinished)
            {
                return false;
            }
        }

        return true;
    }
}
