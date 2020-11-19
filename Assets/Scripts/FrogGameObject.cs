using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInputUtil;

public class FrogGameObject : MonoBehaviour
{
    public FrogData FrogData; //Assigned by GameState
    public SpriteRenderer SpriteRenderer;
    public Animator FrogAnimator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collide with tag: " + other.tag);
    }

    public void ApplyActionToFrog(PlayerFrogAction inputFrogAction, GameStateSnapshot lastFrameSnapshot, float dt, GameConfig gameConfig)
    {
        FrogData.UpdateFrogData(inputFrogAction, lastFrameSnapshot, dt, gameConfig);
        transform.position = FrogData.CurrentPosition;
    }
}