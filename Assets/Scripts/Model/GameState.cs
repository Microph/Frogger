using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public FrogData CurrentFrogData;
    public RowData[] GroundRowDatas;
    public RowData[] RiverRowDatas;
    public FinishSpotData[] FinishSpotDatas;

    //for quick debug
    public GameState()
    {
        CurrentFrogData = new FrogData(new MovableEntityData(new Vector2(0.5f, -7.5f), null, FacingDirection.Up), CurrentStatus.Idle);
    }

    /*
    public GameState(FrogData currentFrogData, RowData[] groundRowDatas, RowData[] riverRowDatas, FinishSpotData[] finishSpotDatas)
    {
        CurrentFrogData = currentFrogData;
        GroundRowDatas = groundRowDatas;
        RiverRowDatas = riverRowDatas;
        FinishSpotDatas = finishSpotDatas;
    }
    */

    public void MoveFrog(float dt, GameConfig gameConfig, PlayerInput.PlayerFrogActions playerFrogActions)
    {
        //MoveFrog toward player direction with speed specified in gameConfig and move pixel by ratio according to dt
        Vector2 inputVector2 = playerFrogActions.Move.ReadValue<Vector2>();
        Debug.Log($"inputVector2: {inputVector2}");

        CurrentFrogData.CurrentPosition += (inputVector2 * gameConfig.FrogMoveUnitPerSec * dt);
        Debug.Log($"CurrentPosition: {CurrentFrogData.CurrentPosition}");
    }
}
