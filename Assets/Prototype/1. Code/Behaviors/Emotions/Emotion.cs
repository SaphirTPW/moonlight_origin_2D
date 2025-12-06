using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static EmotionController;

public class Emotion : MonoBehaviour
{
    #region Public Variables 
    public EmotionState EmoState { get => _emotionState; set => _emotionState = value; }
    public float CurrentEmotionEnergy { get => _currentEmotionEnergy; set => _currentEmotionEnergy = value; }
    public Passive Passive { get => _passive; set => _passive = value; }
    public float MaxEmotionEnergy { get => _maxEmotionEnergy; set => _maxEmotionEnergy = value; }
    #endregion

    #region Private Variables 
    [SerializeField] private EmotionData _emotionData;
    [SerializeField] private Passive _passive;
    [SerializeField] private Skill[] _skills;
    [SerializeField] private UniqueSkill _uskill;

    private PlayerHealth _pH;
    private PlayerMovement _pm;
    private PlayerCombat _pCom;
    private EmotionController _ec;
    private PlayerController _pc;

    [SerializeField] private EmotionState _emotionState;
    [SerializeField] private float _maxEmotionEnergy = 100f;
    [SerializeField] private float _currentEmotionEnergy;
    [SerializeField] private float _buildUpRate;
    [SerializeField] private float _coolDownRate;
    [SerializeField] private string _emotionName;
    [SerializeField] private ParticleSystem _emotionEffect;
    [SerializeField] private Color _emotionColor;
    private float _crashOutCooldownRate;

    private float _defenseModifier;
    private float _speedModifier;
    private float _attackModifier;
    private float _moveSmoothing;

    private float _coPowerModifier;
    private float _coDefenseModifier;
    private float _coSpeedModifier;
    private float _coMoveSmoothing;

    private bool _isAwake;
    [SerializeField] private bool _openSkillTab = false;
    [SerializeField] private bool _canUseSkill = true;
    [SerializeField] private bool _canUseUSkill = true;

    [SerializeField] private GameObject _skillIndicator;
    [SerializeField] private GameObject _skillListIndicator;

    [SerializeField] private TMP_Text[] _skillNames;
    [SerializeField] private TMP_Text[] _skillCooldowns;

    #endregion

    #region Unity Methods 

    private void Awake()
    {
        _pH = GetComponent<PlayerHealth>();
        _pm = GetComponent<PlayerMovement>();
        _pCom = GetComponent<PlayerCombat>();
        _ec = GetComponent<EmotionController>();
        _pc = GetComponent<PlayerController>();
    }

    public virtual void Start()
    {
        //CheckUSkill();
        SetEmotionStat();
        SetCoEmotionStat();
        var main = _emotionEffect.main;
        main.startColor = _emotionColor;
        _emotionEffect.gameObject.SetActive(false);
        _skillListIndicator.SetActive(false);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        UpdateEmotionState(_emotionState);
        HandleSKill();
        HandleUSkill();
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
        _moveSmoothing = _emotionData.moveSmoothing;

        _emotionState = EmotionState.Sleep;
    }

    public virtual void SetCoEmotionStat()
    {
        _coDefenseModifier = _emotionData.coDefenseModifier;
        _coPowerModifier = _emotionData.coPowerModifier;
        _coSpeedModifier = _emotionData.coSpeedModifier;
        _coMoveSmoothing = _emotionData.coMoveSmoothing;
    }

    public virtual void UpdateEmotionState(EmotionState pEmotionState)
    {
        _emotionState = pEmotionState;

        switch (pEmotionState)
        {
            case EmotionState.Awake:
                HandleAwakeEmotion();
                var main = _emotionEffect.main;
                main.startColor = _emotionColor;
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
        _pm.PlayerMoveSmoothing = _moveSmoothing;
        _isAwake = true;
        //_emotionEffect.Play();
        _emotionEffect.gameObject.SetActive(true);

        if (_isAwake)
        {
            _currentEmotionEnergy += Time.deltaTime * _buildUpRate;
            if (_currentEmotionEnergy >= _maxEmotionEnergy)
            {
                _currentEmotionEnergy = _maxEmotionEnergy;
                if (_emotionName != "Neutral" || _emotionName != "Fatigue")
                {
                    HandleFatigueState();
                }
            }
        }

    }

    public virtual void HandleFatigueState()
    {
        Debug.Log("Hello");
        _currentEmotionEnergy = 0;
        _ec.EmoControllerState = EmotionController.EmotionControllerState.Cooldown;
        _ec.EnableEmotion(_ec.Emotions[5], EmotionController.ActiveEmotionState.Fatigue, _ec.fatigueColor, null);
        _ec.EmotionIndacatorText.text = "Fatigue";
        _ec.CanSwitch = false;
        HideSkill();
        _canUseSkill = true;
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
        if (_openSkillTab)
        {
            //Debug.Log("Menu is Open ?");
            _openSkillTab = false;
            HideSkill();
            _pc.CanJump = true;
        }

        _ec.EmotionIndacatorText.text = $"{_ec.CurrentActiveEmotion.ToString()} CRASH OUT";
        _pH.DefenseMod = _coDefenseModifier;
        _pm.SpeedMod = _coSpeedModifier;
        _pCom.AttackMod = _coPowerModifier;
        _pm.PlayerMoveSmoothing = _coMoveSmoothing;

        _ec.EmoControllerState = EmotionController.EmotionControllerState.NotReady;
        _currentEmotionEnergy -= Time.deltaTime * _crashOutCooldownRate;

        if (_currentEmotionEnergy <= 0)
        {
            _ec.EmotionIndacatorText.text = ActiveEmotionState.Neutral.ToString();
            _currentEmotionEnergy = 0;
            _ec.EmoControllerState = EmotionController.EmotionControllerState.Cooldown;
            _ec.EmotionIndacatorText.text = "CoolDown";
            _ec.EnableEmotion(_ec.Emotions[0], EmotionController.ActiveEmotionState.Neutral, _ec.neutralColor, null);
            _canUseSkill = true;
        }
    }

    public virtual void HandleUSkill()
    {
        if ((int)_emotionData.emotionType == 0)
        {
            return;
        }
        else if(_uskill == null)
        {
            return;
        }
        else if ((int)_emotionData.emotionType == (int)_ec.CurrentActiveEmotion && _canUseUSkill)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                _uskill.EnableUSkill();
            }
        }
    }

    public virtual void HandleSKill()
    {
        if((int)_emotionData.emotionType == 0)
        {
            return;
        }
        else if((int)_emotionData.emotionType == (int)_ec.CurrentActiveEmotion && _canUseSkill)
        {
            if (Input.GetButton("LB"))
            {
                _openSkillTab = true;
                ShowSKill();
            }
            else if (Input.GetButtonUp("LB"))
            {
                _openSkillTab = false;
                HideSkill();
            }

            if (_openSkillTab)
            {
                _pc.CanJump = false;
                _canUseUSkill = false;

                for (int i = 0; i < _skillNames.Length; i++)
                {
                    for (int j = 0; j < _skillCooldowns.Length; j++)
                    {
                        _skillNames[i].text = _skills[i].SkillName;
                        _skillCooldowns[j].text = Mathf.Round(_skills[j].CoolDownTime).ToString();
                    }
                }
                if (Input.GetButtonDown("Jump"))
                {
                    _skills[0].EnableSkill(_skills[0].SkillCost);
                }
                else if (Input.GetButtonDown("Cancel"))
                {
                    _skills[1].EnableSkill(_skills[1].SkillCost);
                }
            }
            else if (!_openSkillTab)
            {
                _pc.CanJump = true;
                _canUseUSkill = true;
            }
        }
    }

    private void CheckUSkill()
    {
        if (_uskill == null)
        {
            return;
        }
    }

    public virtual void ShowSKill()
    {
        _skillListIndicator.SetActive(true);
        _skillIndicator.SetActive(false);
    }

    public virtual void HideSkill()
    {
        _skillListIndicator.SetActive(false);
        _skillIndicator.SetActive(true);
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
