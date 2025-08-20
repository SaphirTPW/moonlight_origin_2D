using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emotion : MonoBehaviour
{
    #region Public Variables 
    public EmotionState EmoState { get => _emotionState; set => _emotionState = value; }
    public float CurrentEmotionEnergy { get => _currentEmotionEnergy; set => _currentEmotionEnergy = value; }
    #endregion

    #region Private Variables 
    [SerializeField] private EmotionData _emotionData;
    [SerializeField] private Passive _passive;
    [SerializeField] private Skill _skills;
    private PlayerHealth _pH;
    private PlayerMovement _pm;
    private PlayerCombat _pCom;
    private EmotionController _ec;

    [SerializeField] private EmotionState _emotionState;
    [SerializeField] private float _maxEmotionEnergy = 100f;
    [SerializeField] private float _currentEmotionEnergy;
    [SerializeField] private float _buildUpRate;
    [SerializeField] private float _coolDownRate;
    [SerializeField] private string _emotionName;
    private float _crashOutCooldownRate;

    private float _defenseModifier;
    private float _speedModifier;
    private float _attackModifier;

    private bool _isAwake;
    [SerializeField] private bool _canUseSkill = false;

    #endregion

    #region Unity Methods 

    private void Awake()
    {
        _pH = GetComponent<PlayerHealth>();
        _pm = GetComponent<PlayerMovement>();
        _pCom = GetComponent<PlayerCombat>();
        _ec = GetComponent<EmotionController>();
    }

    public virtual void Start()
    {
        SetEmotionStat();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        UpdateEmotionState(_emotionState);
        HandleSKill();
    }
    #endregion

    #region Public Methods 

    public virtual void SetEmotionStat()
    {
        _buildUpRate = _emotionData.buildUpRate;
        _coolDownRate = _emotionData.coolDownRate;
        _crashOutCooldownRate = _emotionData.crashCoolDownRate;
        _emotionName = _emotionData.emotionName;

        _defenseModifier = _emotionData.defenseModifier;
        _speedModifier = _emotionData.speedModifier;
        _attackModifier = _emotionData.powerModifier;

        _emotionState = EmotionState.Sleep;
    }

    public virtual void UpdateEmotionState(EmotionState pEmotionState)
    {
        _emotionState = pEmotionState;

        switch (pEmotionState)
        {
            case EmotionState.Awake:
                HandleAwakeEmotion();
                break;
            case EmotionState.Sleep:
                HandleSleepEmotion();
                break;
            case EmotionState.CrashOut:
                HandleCrashOut();
                break;
            default:
                break;
        }
    }

    public virtual void HandleAwakeEmotion()
    {
        _emotionState = EmotionState.Awake;
        _pH.DefenseMod = _defenseModifier;
        _pm.SpeedMod = _speedModifier;
        _pCom.AttackMod = _attackModifier;
        _isAwake = true;

        if (_isAwake)
        {
            _currentEmotionEnergy += Time.deltaTime * _buildUpRate;
            if (_currentEmotionEnergy >= _maxEmotionEnergy)
            {
                _currentEmotionEnergy = _maxEmotionEnergy;
                if (_emotionName != "Neutral")
                    _emotionState = EmotionState.CrashOut;
            }
        }

    }

    public virtual void HandleSleepEmotion()
    {
        _isAwake = false;
        _currentEmotionEnergy -= Time.deltaTime * _coolDownRate;
        if (_currentEmotionEnergy <= 0)
        {
            _currentEmotionEnergy = 0;
        }
    }

    public virtual void HandleCrashOut()
    {
        _ec.EmoControllerState = EmotionController.EmotionControllerState.NotReady;
        _currentEmotionEnergy -= Time.deltaTime * _crashOutCooldownRate;

        if (_currentEmotionEnergy <= 0)
        {
            _currentEmotionEnergy = 0;
            _ec.EmoControllerState = EmotionController.EmotionControllerState.Cooldown;
            _ec.EnableEmotion(_ec.Emotions[0], EmotionController.ActiveEmotionState.Neutral);
        }
    }

    public virtual void HandleSKill()
    {
        if((int)_emotionData.emotionType == (int)_ec.CurrentActiveEmotion)
        {
            if (Input.GetButton("LB"))
            {
                _canUseSkill = true;
            }
            else if (Input.GetButtonUp("LB"))
            {
                _canUseSkill = false;
            }

            if (_canUseSkill)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    _skills.EnableSkill(_skills.SkillCost);
                }
            }
        }
    }
    public enum EmotionState
    {
        Awake,
        Sleep,
        CrashOut
    }
    #endregion

    #region Protected Methods 
    #endregion

    #region Coroutines
    #endregion
}
