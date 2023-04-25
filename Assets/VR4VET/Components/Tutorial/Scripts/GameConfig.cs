using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An object representing the configuration which is active for the current scenario.
/// </summary>
public sealed class GameConfig : MonoBehaviour
{
    public UnityEvent<GameMode> OnGameModeChanged;
    public UnityEvent OnGameModeArcade;
    public UnityEvent OnGameModeRealism;
    private static GameConfig s_current;
    private GameMode gameMode = GameMode.Arcade;

    [MaybeNull]
    public static GameConfig Current => s_current;

    public GameMode GameMode
    {
        get => gameMode;
        set
        {
            if (gameMode != value)
            {
                gameMode = value;
                OnGameModeChanged?.Invoke(value);
                (value == GameMode.Realism ? OnGameModeRealism : OnGameModeArcade)?.Invoke();
            }
        }
    }

    private void Awake()
    {
        if (s_current != null)
        {
            throw new Exception($"There is already an instance of {nameof(GameConfig)}." +
                $"Use {nameof(GameConfig)}.{nameof(Current)} instead.");
        }
        s_current = this;
        OnGameModeChanged?.Invoke(gameMode);
    }
}
