using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public MovableEntityData MovableEntityData; //Assigned by GameState
    public int ObstacleWidth = 1;
    public int RowIndex = -1;
    
    protected Transform _obstacleTransform;
    protected Animator[] _childrenAnimators;
    protected Transform[] _childrenTransforms;
    protected SpriteRenderer[] _childrenSpriteRenderers;
    protected Collider2D[] _childrenColliders;

    public void Initialize(int rowIndex, Vector2 spawnPos, RowMovingDirection movingDirection, GameConfig gameConfig)
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

    public virtual void UpdateTick(float dt, GameConfig gameConfig)
    {
        Vector2 moveVector = new Vector2(dt * gameConfig.RowDataConfigs[RowIndex].GetRowMovingUnitPerSec(), 0);
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
        _childrenAnimators = _obstacleTransform.GetComponentsInChildren<Animator>();
        _childrenColliders = _obstacleTransform.GetComponentsInChildren<Collider2D>();
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
                _childrenSpriteRenderers[i].flipX = false;
                Vector2 currentlocalPos = _childrenTransforms[i + 1].localPosition;
                _childrenTransforms[i + 1].localPosition = new Vector2(Mathf.Abs(currentlocalPos.x), currentlocalPos.y);
            }
        }
    }
}