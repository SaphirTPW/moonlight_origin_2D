using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    public static bool isPaused;

    public PauseState pauseState;

    public static event Action<PauseState> OnGamePaused;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        isPaused = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause") && !isPaused)
        {
            UpdatePauseState(PauseState.Pause);
        }
        else if (Input.GetButtonDown("Pause") && isPaused)
        {
            UpdatePauseState(PauseState.Unpause);
        }
    }

    public void UpdatePauseState(PauseState pPauseState)
    {
        pauseState = pPauseState;

        switch (pPauseState)
        {
            case PauseState.Pause:
                HandlePause();
                break;
            case PauseState.Unpause:
                HandleUnPause();
                break;
        }

        OnGamePaused?.Invoke(pPauseState);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void HandlePause()
    {
        isPaused = true;
        Time.timeScale = 0;
    }

    private void HandleUnPause()
    {
        isPaused = false;
        Time.timeScale = 1f;
        GameManager.Instance.UpdateGameState(GameManager.GameState.Playing);
    }

    public enum PauseState
    {
        Pause,
        Unpause
    }
}
