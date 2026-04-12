using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    Tutorial,
    Actual
}
public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance { get; private set; }

    // Static property so you can call GameModeManager.GameMode directly
    public static GameMode GameMode { get; private set; } = GameMode.Tutorial;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void SetMode(GameMode mode)
    {
        GameMode = mode;
    }

    public static void SetTutorial() => SetMode(GameMode.Tutorial);
    public static void SetActualGame() => SetMode(GameMode.Actual);
}
