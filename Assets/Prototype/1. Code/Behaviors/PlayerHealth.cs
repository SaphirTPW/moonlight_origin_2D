using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class PlayerHealth : MonoBehaviour
{
    #region Public Variables 
    public float DefenseMod { get => _defenseModifier; set => _defenseModifier = value; }
    public bool IsHealing { get => _isHealing; set => _isHealing = value; }
    public float PlayerCurrentHealth { get => _playerCurrentHealth; set => _playerCurrentHealth = value; }
    public float PlayerMaxHealth { get => _playerMaxHealth; set => _playerMaxHealth = value; }
    public bool IsReceivingDamage { get => _isReceivingDamage; set => _isReceivingDamage = value; }
    public bool IsGriefFaceOn { get => _isGriefFaceOn; set => _isGriefFaceOn = value; }
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
    private float _griefDefendMod = 0.5f;
    private bool _isDead = false;
    [SerializeField] private bool _isHealing = false;
    [SerializeField] private bool _isReceivingDamage = false;
    [SerializeField] private bool _isGriefFaceOn = false;
    private bool _isLowHealth = false;

    [SerializeField] private AudioClip _lowHealthSFX;
    [SerializeField] private AudioClip _hitSFX;
    public static Action<bool> OnLowHealth;

    
    public delegate bool OnPlayerHitDelegate(float damage);
    public static event OnPlayerHitDelegate OnPlayerHit;
    #endregion

    #region Unity Methods 
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChange;
        OnLowHealth += HandleLowHealth;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChange;
        OnLowHealth -= HandleLowHealth;
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
        ProgressiveHealing(_healingAmount);
        ProgressiveDamage(_damageAmount);
    }
    #endregion

    #region Public Methods 
    public void PlayerTakeDamage(float pDamage)
    {
        bool cancelHit = false;

        if(OnPlayerHit != null)
        {
            foreach (OnPlayerHitDelegate subscriber in OnPlayerHit.GetInvocationList())
            {
                if (subscriber.Invoke(pDamage))
                {
                    cancelHit = true;
                    //CheckLowHealth();
                    break;
                }
            }
        }

        if (cancelHit)
            return;


        if (!IsGriefFaceOn)
        {
            _playerCurrentHealth -= pDamage * _defenseModifier;
            UpdatePlayerHealth();
            CheckLowHealth();
            AudioManager.Instance.PlaySFX(_hitSFX);
        }
        else
        {
            _playerCurrentHealth -= pDamage * _griefDefendMod;
            UpdatePlayerHealth();
            CheckLowHealth();
        }
    }

    public void PlayerGainHealth(float pHealthAmount)
    {
        _playerCurrentHealth += pHealthAmount;
        UpdatePlayerHealth();
        CheckLowHealth();
    }

    public void ProgressiveHealing(float pHealthAmount)
    {
        if (_isHealing && _playerCurrentHealth < _playerMaxHealth)
        {
            _healingRate -= Time.deltaTime;
            

            if (_healingRate <= 0f)
            {
                PlayerGainHealth(pHealthAmount);
                CheckLowHealth();
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
            //Debug.Log("Full Health");
            SetPlayerHealth();
        }
    }

    public void SetPlayerHealth()
    {
        _playerCurrentHealth = _playerMaxHealth;
        _playerHealthBar.fillAmount = _playerCurrentHealth / 100;

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
            GameManager.Instance.UpdateGameState(GameManager.GameState.Dead);
            _isDead = false;
        }
    }

    private void CheckLowHealth()
    {
        bool isLow = _playerCurrentHealth <= (_playerMaxHealth * 0.25f);

        if (isLow != _isLowHealth)
        {
            _isLowHealth = isLow;
            OnLowHealth?.Invoke(isLow);
        }
    }

    private void HandleLowHealth(bool pIsLow)
    {
        if (pIsLow)
            AudioManager.Instance.PlaySFX(_lowHealthSFX, true);
        else
        {
            AudioManager.Instance.StopLoopingSFX();
        }
    }
    #endregion

    #region Coroutines
    #endregion
}
