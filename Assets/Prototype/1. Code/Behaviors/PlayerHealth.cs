using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    #region Public Variables 
    public float DefenseMod { get => _defenseModifier; set => _defenseModifier = value; }
    public bool IsHealing { get => _isHealing; set => _isHealing = value; }
    #endregion

    #region Private Variables 
    [SerializeField] private float _playerMaxHealth;
    private float _healingRate = 1f;
    [SerializeField] private float _healingAmount;
    [SerializeField] private float _startHealingRate = 1f;
    [SerializeField] private float _playerCurrentHealth;
    [SerializeField] private TMP_Text _playerHealthText;
    private float _defenseModifier = 1f;
    private bool _isDead = false;
    private bool _isHealing = false;
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
        _healingRate = _startHealingRate;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerHealth();
        ProgressiveHealing(_healingAmount);
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

    public void ProgressiveHealing(float pHealthAmount)
    {
        if (_isHealing)
        {
            _healingRate -= Time.deltaTime;
            if(_healingRate <= 0f)
            {
                PlayerGainHealth(pHealthAmount);
                _healingRate = _startHealingRate;
            }
        }
        else
        {
            return;
        }
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
