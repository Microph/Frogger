using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogGameObject : MonoBehaviour
{
    private FrogData _frogData; //ref from GameState

    private void Start()
    {
        _frogData = GameManager.Instance.GameState.CurrentFrogData;
    }

    private void Update()
    {
        transform.position = _frogData.CurrentPosition;
    }
}
