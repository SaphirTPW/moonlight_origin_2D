using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Android;

public class GameManager : MonoBehaviour
{
    #region Public Variables 
    public static GameManager Instance;
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;

    public Transform currentCheckpoint;
    public Transform startPosition;
    public Transform player;

    public GameObject pauseMenuObj;
    #endregion

    #region Private Variables 
    #endregion

    #region Unity Methods 
    private void OnEnable()
    {
        PauseManager.OnGamePaused += PauseManagerOnGamePaused;
    }

    private void OnDestroy()
    {
        PauseManager.OnGamePaused -= PauseManagerOnGamePaused;
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetStartCheckPoint();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Public Methods 
    public void UpdateGameState(GameState pGameState)
    {
        State = pGameState;

        switch (pGameState)
        {
            case GameState.SetUp:
                break;
            case GameState.Start:
                break;
            case GameState.Playing:
                break;
            case GameState.Pause:
                break;
            case GameState.Stop:
                break;
            case GameState.Dead:
                break;
            default:
                break;
        }
        OnGameStateChanged?.Invoke(pGameState);
    }

    public void PlayerVoidOut()
    {
        player.transform.position = currentCheckpoint.transform.position;
    }

    public void UpdateCheckpoint(Transform pNewCheckpoint)
    {
        currentCheckpoint = pNewCheckpoint;
    }

    public void SetStartCheckPoint()
    {
        currentCheckpoint = startPosition;
        player.transform.position = startPosition.transform.position;
    }

    public enum GameState
    {
        SetUp,
        Start,
        Playing,
        Pause,
        Stop,
        Dead
    }
    #endregion

    #region Private Methods 
    private void HandlePlayerDeath()
    {

    }

    private void HandlePause()
    {

    }

    private void PauseManagerOnGamePaused(PauseManager.PauseState pauseState)
    {
        if(pauseState == PauseManager.PauseState.Pause)
        {
            UpdateGameState(GameState.Pause);
        }
    }
    #endregion

    #region Coroutines
    #endregion
}
