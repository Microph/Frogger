using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogData : MovableEntityData
{
    public FrogState State;

    public FrogData(MovableEntityData movableEntityData, FrogState currentStatus) : base(movableEntityData)
    {
        State = currentStatus;
    }
}

public enum FrogState
{
    Idle,
    Jumping,
    Die,
    InFinishLine
}