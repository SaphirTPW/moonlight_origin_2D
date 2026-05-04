using System;
using System.Collections;
using System.Collections.Generic;
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
                HandlePause();
                break;
            case GameState.Pause:
                HandlePause();
                break;
            case GameState.Stop:
                break;
            case GameState.Dead:
                HandlePlayerDeath();
                break;
        }
        OnGameStateChanged?.Invoke(pGameState);
    }

    public void PlayerVoidOut()
    {
        player.transform.position = currentCheckpoint.transform.position;
        player.gameObject.SetActive(true);
        player.GetComponent<Rigidbody2D>().simulated = true;
        Time.timeScale = 1f;
    }

    public void UpdateCheckpoint(Transform pNewCheckpoint)
    {
        currentCheckpoint = pNewCheckpoint;
    }

    public void SetStartCheckPoint()
    {
        //currentCheckpoint = startPosition;
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
        StartCoroutine(PlayerDeathCo());
    }

    private void HandlePause()
    {
        pauseMenuObj.SetActive(PauseManager.isPaused);
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
    private IEnumerator PlayerDeathCo()
    {
        yield return null;
        player.gameObject.SetActive(false);
        player.GetComponent<Rigidbody2D>().simulated = false;
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0f;
        UIManager.Instance.EnableGameOverScreen();
    }
    #endregion
}
