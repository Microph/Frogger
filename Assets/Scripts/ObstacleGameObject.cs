using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGameObject : MonoBehaviour
{
    public MovableEntityData MovableEntityData; //Assigned by GameState
    public int ObstacleWidth = 1;
    public int RowIndex = -1;

    private Transform _obstacleTransform;
    private Transform[] _childrenTransforms;
    private SpriteRenderer[] _childrenSpriteRenderers;

    public void Initialize(int rowIndex, Vector2 spawnPos, RowMovingDirection movingDirection)
    {
        if (MovableEntityData == null)
        {
            MovableEntityData = new MovableEntityData();
        }

        ObstacleWidth = _obstacleTransform.childCount;
        RowIndex = rowIndex;
        MovableEntityData.CurrentPosition = spawnPos;
        MovableEntityData.FacingDirection = movingDirection == RowMovingDirection.Left ? FacingDirection.Left : FacingDirection.Right;
        FlipChildrenSprites(MovableEntityData.FacingDirection);
    }

    public void UpdateFrame(float dt, GameConfig gameConfig)
    {
        Vector2 moveVector = new Vector2(dt * gameConfig.RowDataConfigs[RowIndex].RowMovingUnitPerSec, 0);
        if (gameConfig.RowDataConfigs[RowIndex].RowMovingDirection == RowMovingDirection.Right)
        {
            MovableEntityData.CurrentPosition += moveVector;
        }
        else
        {
            MovableEntityData.CurrentPosition -= moveVector;
        }

        _obstacleTransform.position = MovableEntityData.CurrentPosition;
    }

    private void Awake()
    {
        _obstacleTransform = GetComponent<Transform>();
        _childrenTransforms = _obstacleTransform.GetComponentsInChildren<Transform>();
        _childrenSpriteRenderers = _obstacleTransform.GetComponentsInChildren<SpriteRenderer>();
    }

    private void FlipChildrenSprites(FacingDirection facingDirection)
    {
        if(facingDirection == FacingDirection.Right)
        {
            for(int i = 0; i < _childrenSpriteRenderers.Length; i++)
            {
                _childrenSpriteRenderers[i].flipX = true;
                Vector2 currentlocalPos = _childrenTransforms[i + 1].localPosition;
                _childrenTransforms[i + 1].localPosition = new Vector2(-currentlocalPos.x, currentlocalPos.y);
            }
        }
        else
        {
            for (int i = 0; i < _childrenSpriteRenderers.Length; i++)
            {
                _childrenSpriteRenderers[i].flipX = true;
                Vector2 currentlocalPos = _childrenTransforms[i + 1].localPosition;
                _childrenTransforms[i + 1].localPosition = new Vector2(Mathf.Abs(currentlocalPos.x), currentlocalPos.y);
            }
        }
    }
}