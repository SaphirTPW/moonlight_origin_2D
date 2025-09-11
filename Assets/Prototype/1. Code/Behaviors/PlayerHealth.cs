using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    #region Public Variables 
    public float DefenseMod { get => _defenseModifier; set => _defenseModifier = value; }
    #endregion

    #region Private Variables 
    [SerializeField] private float _playerMaxHealth;
    [SerializeField] private float _playerCurrentHealth;
    [SerializeField] private TMP_Text _playerHealthText;
    private float _defenseModifier = 1f;
    private bool _isDead = false;
    #endregion

    #region Unity Methods 
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChange;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChange;
    }
    void Start()
    {
        GameManagerOnGameStateChange(GameManager.GameState.SetUp);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerHealth();
    }
    #endregion

    #region Public Methods 
    public void PlayerTakeDamage(float pDamage)
    {
        _playerCurrentHealth -= pDamage * _defenseModifier;
    }

    public void PlayerGainHealth(float pHealthAmount)
    {
        _playerCurrentHealth += pHealthAmount;
    }
    #endregion

    #region Private Methods 
    private void GameManagerOnGameStateChange(GameManager.GameState pState)
    {
        if (pState == GameManager.GameState.SetUp)
        {
            SetPlayerHealth();
        }
    }

    private void SetPlayerHealth()
    {
        _playerCurrentHealth = _playerMaxHealth;

        if (_playerCurrentHealth >= _playerMaxHealth)
            _playerCurrentHealth = _playerMaxHealth;
    }

    private void UpdatePlayerHealth()
    {
        _playerHealthText.text = Mathf.Round(_playerCurrentHealth).ToString();

        if(_playerCurrentHealth <= 0)
        {
            _playerCurrentHealth = 0f;
            _isDead = true;
        }

        if (_isDead)
            GameManager.Instance.UpdateGameState(GameManager.GameState.Dead);
    }
    #endregion

    #region Coroutines
    #endregion
}
