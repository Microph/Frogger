using System.Collections.Generic;

public enum RowMovingDirection
{
    Left = 1,
    Right = -1
}

public enum RowMovingSpeed
{
    Slow = 1,
    Medium = 2,
    High = 3
}

public class RowData
{
    public RowMovingDirection RowMovingDirection;
    public float RowMovingUnitPerSec;
    public List<Obstacle> ObstacleGameObjectList;

    public RowData(RowDataConfig rowDataConfig, GameConfig gameConfig, int currentRound)
    {
        RowMovingDirection = rowDataConfig.RowMovingDirection;
        RowMovingUnitPerSec = rowDataConfig.GetRowMovingUnitPerSec() * GameState.GetGameSpeedModifier(gameConfig, currentRound);
        ObstacleGameObjectList = new List<Obstacle>();
    }
    
}