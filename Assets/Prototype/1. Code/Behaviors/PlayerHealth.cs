using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    #region Public Variables 
    public float DefenseMod { get => _defenseModifier; set => _defenseModifier = value; }
    public bool IsHealing { get => _isHealing; set => _isHealing = value; }
    public float PlayerCurrentHealth { get => _playerCurrentHealth; set => _playerCurrentHealth = value; }
    public float PlayerMaxHealth { get => _playerMaxHealth; set => _playerMaxHealth = value; }
    public bool IsReceivingDamage { get => _isReceivingDamage; set => _isReceivingDamage = value; }
    #endregion

    #region Private Variables 
    public static PlayerHealth Instance;
    [SerializeField] private float _playerMaxHealth;
    private float _healingRate = 1f;
    [SerializeField] private float _healingAmount;
    [SerializeField] private float _damageAmount;
    [SerializeField] private float _startHealingRate = 1f;
    [SerializeField] private float _playerCurrentHealth;
    [SerializeField] private TMP_Text _playerHealthText;
    [SerializeField] private Image _playerHealthBar;
    private float _defenseModifier = 1f;
    private bool _isDead = false;
    [SerializeField] private bool _isHealing = false;
    [SerializeField] private bool _isReceivingDamage = false;
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
    private void Awake()
    {
        Instance = this;
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
        ProgressiveDamage(_damageAmount);
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
        if (_isHealing && _playerCurrentHealth < _playerMaxHealth)
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

    public void ProgressiveDamage(float pDamageAmount)
    {
        if (_isReceivingDamage)
        {
            _healingRate -= Time.deltaTime;
            if (_healingRate <= 0f)
            {
                PlayerTakeDamage(pDamageAmount);
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
        _playerHealthBar.fillAmount = _playerCurrentHealth / 100;

        if(_playerCurrentHealth <= 0)
        {
            _playerCurrentHealth = 0f;
            _isDead = true;
        }

        if (_isDead)
        {
            //GameManager.Instance.UpdateGameState(GameManager.GameState.Dead);
            GameManager.Instance.PlayerVoidOut();
            SetPlayerHealth();
            _isDead = false;
        }
    }
    #endregion

    #region Coroutines
    #endregion
}
