using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerInputUtil;

public struct GameStateSnapshot
{
    public PlayerFrogAction InputFrogAction;
    public FrogState FrogState;
    public int CurrentScore;
    public float TimeLimit;
    public float TimeLeft;
    public int MaxHealth;
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
    public int MaxHealth, CurrentHealth;
    public int CurrentRound = 0;
    public bool IsSuperHotMode = false;

    private PlayerFrogAction _inputFrogAction = PlayerFrogAction.None;
    private FrogState _frogState;
    private bool _isFirstTimeShow = true;
    private bool _isGameOver = false;

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
        snapshot.MaxHealth = MaxHealth;
        snapshot.CurrentHealth = CurrentHealth;
        snapshot.CurrentRound = CurrentRound;
        return snapshot;
    }

    public void Initialize(GameConfig gameConfig)
    {
        TimeLimit = gameConfig.GAME_TIME_LIMIT;
        TimeLeft = TimeLimit;
        MaxHealth = gameConfig.HEALTH;
        CurrentHealth = MaxHealth;
        CurrentRound = 0;
        FrogManager.Initialize(new FrogData(new MovableEntityData(gameConfig.FROG_START_POINT, FacingDirection.Up)));
        ObstacleManager.Initialize(gameConfig, CurrentRound);
        IsSuperHotMode = PlayerPrefs.GetInt("EnableSuperHotMode", 1) == 1 ? true : false;
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
        //Get player output
        Vector2 inputVector2 = currentTickPlayerFrogAction.Move.ReadValue<Vector2>();
        _inputFrogAction = PlayerInputUtil.ConvertVector2ToPlayerFrogActionEnum(inputVector2);
        
        //First time button hit will close Title
        if(_isFirstTimeShow)
        {
            if (_inputFrogAction != PlayerFrogAction.None)
            {
                _isFirstTimeShow = false;
                GameManager.Instance.UIManager.UpdateSuperHotModeValue();
                IsSuperHotMode = PlayerPrefs.GetInt("EnableSuperHotMode", 1) == 1 ? true : false;
                GameManager.Instance.UIManager.ShowTitle(false);
            }
            else
            {
                return GetSnapshot();
            }
        }

        //Game over screen receive button input to restart
        if (_isGameOver)
        {
            if (_inputFrogAction != PlayerFrogAction.None)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            return GetSnapshot();
        }

        //Super Hot Mode
        if (IsSuperHotMode && (_inputFrogAction == PlayerFrogAction.None))
        {
            return GetSnapshot();
        }

        //Update main timer in game
        TimeLeft -= dt;
        if (TimeLeft <= 0)
        {
            FrogManager.FrogData.State = FrogState.Die;
        }

        //Tick update frog
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

        //Tick update obstacle
        ObstacleManager.TickUpdate(dt, gameConfig);
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
        CurrentHealth -= 1;
        if (CurrentHealth == 0)
        {
            _isGameOver = true;
            GameManager.Instance.UIManager.ShowGameOver(true);
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
