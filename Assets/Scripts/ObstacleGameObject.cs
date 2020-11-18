using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGameObject : MonoBehaviour
{
    public MovableEntityData MovableEntityData; //Gamestate modify

    private void Start()
    {
    }

    private void Update()
    {
        transform.position = MovableEntityData.CurrentPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collide with tag: " + other.tag);
    }
}
