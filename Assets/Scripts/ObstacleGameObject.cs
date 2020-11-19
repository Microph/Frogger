using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGameObject : MonoBehaviour
{
    public MovableEntityData MovableEntityData; //Assigned by GameState
    public int RowIndex = -1;

    private void Update()
    {
        transform.position = MovableEntityData.CurrentPosition;
    }
}
