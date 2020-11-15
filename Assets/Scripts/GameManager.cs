using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;

#if UNITY_IOS || UNITY_ANDROID
        //mobileInputs.SetActive(true);
        Screen.orientation = ScreenOrientation.Portrait;
#endif

    }

    void Start()
    {
        //SetupNewGame
    }

    private void SetupNewGame()
    {
        //initize stuffs
    }

    void Update()
    {
        //main game loop
        //update GameState with methods
    }
}
