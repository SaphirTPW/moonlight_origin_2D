using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static EmotionController;

public class Emotion : MonoBehaviour
{
    #region Public Variables 
    public EmotionState EmoState { get => _emotionState; set => _emotionState = value; }
    public float CurrentEmotionEnergy { get => _currentEmotionEnergy; set => _currentEmotionEnergy = value; }
    public Passive Passive { get => _passive; set => _passive = value; }
    public float MaxEmotionEnergy { get => _maxEmotionEnergy; set => _maxEmotionEnergy = value; }
    public bool IsCrashOutAvailable { get => _isCrashOutAvailable; set => _isCrashOutAvailable = value; }
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
    [SerializeField] private Image _emotionMeterPad;
    [SerializeField] private Image _emotionMeterKey;
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
    [SerializeField] private bool _isCrashOutAvailable;

    [SerializeField] private GameObject _skillIndicator;
    [SerializeField] private GameObject _skillListIndicator;

    [SerializeField] private TMP_Text[] _skillNamesPAD;
    [SerializeField] private TMP_Text[] _skillCooldownsPAD;

    [SerializeField] private TMP_Text[] _skillNamesKEY;
    [SerializeField] private TMP_Text[] _skillCooldownsKEY;

    public static event Action<EmotionState> OnEmotionStateChanged;
    public static event Action<bool> OnCrashOutAvailability;


    #endregion

    #region Unity Methods 

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

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
        SetEmotionStat();
        SetCoEmotionStat();
        var main = _emotionEffect.main;
        main.startColor = _emotionColor;
        _emotionMeterPad.color = _emotionColor;
        _emotionMeterKey.color = _emotionColor;
        _emotionEffect.gameObject.SetActive(false);
        _skillListIndicator.SetActive(false);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        _emotionMeterPad.fillAmount = _currentEmotionEnergy / 100;
        _emotionMeterKey.fillAmount = _currentEmotionEnergy / 100;

        //UpdateEmotionState(_emotionState);
        HandleSKill();
        HandleUSkill();
        UpdateSkillUI();
        EnableCrashOut();
        IncreaseEmotionEnergy();
        DecreaseEmotionEnergy();
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
        if (_emotionState == pEmotionState)
            return;

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
            case EmotionState.Fatigue:
                HandleFatigueState();
                break;
            default:
                break;
        }
        OnEmotionStateChanged?.Invoke(pEmotionState);
    }

    public virtual void HandleAwakeEmotion()
    {

        _emotionState = EmotionState.Awake;
        _pH.DefenseMod = _defenseModifier;
        _pm.SpeedMod = _speedModifier;
        _pCom.AttackMod = _attackModifier;
        _pm.PlayerMoveSmoothing = _moveSmoothing;
        _emotionEffect.Play();
        _emotionEffect.gameObject.SetActive(true);
    }

    public virtual void HandleFatigueState()
    {
        _isCrashOutAvailable = false;
        InputDeviceManager.Instance.DisablePrompt();
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
    }

    public void EnableCrashOut()
    {
        if(_isCrashOutAvailable && Input.GetButtonDown("Crash Out"))
        {
            UpdateEmotionState(EmotionState.CrashOut);
            _isCrashOutAvailable = false;
            _canUseSkill = false;
            InputDeviceManager.Instance.DisablePrompt();
            _pH.PlayerCurrentHealth += _pH.PlayerMaxHealth * 0.25f;
        }
    }

    public void CheckCrashOut()
    {
        bool newState = _currentEmotionEnergy >= 75 && _pH.PlayerCurrentHealth <= 25;

        if (newState == _isCrashOutAvailable)
            return;

        _isCrashOutAvailable = newState;
        OnCrashOutAvailability?.Invoke(newState);
    }

    public void IncreaseEmotionEnergy()
    {
        if (_emotionState == EmotionState.Awake)
        {
            _currentEmotionEnergy += Time.deltaTime * _buildUpRate;

            if (_currentEmotionEnergy >= _maxEmotionEnergy)
            {
                _currentEmotionEnergy = _maxEmotionEnergy;
                if (_emotionName != "Neutral" || _emotionName != "Fatigue")
                {
                    UpdateEmotionState(EmotionState.Fatigue);
                }
            }
        }

        CheckCrashOut();
    }

    public void DecreaseEmotionEnergy()
    {
        if (_emotionState == EmotionState.Sleep || _emotionState == EmotionState.CrashOut)
        {
            _currentEmotionEnergy -= Time.deltaTime * _coolDownRate;

            if (_currentEmotionEnergy <= 0)
            {
                if(_emotionState == EmotionState.Sleep)
                {
                    _currentEmotionEnergy = 0;
                }
                else if(_emotionState == EmotionState.CrashOut)
                {
                    UpdateEmotionState(EmotionState.Fatigue);
                }
            }
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
            if (Input.GetButtonDown("Cancel") || Input.GetKeyDown(KeyCode.G))
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
            if (_pc.OpenSkillTab)
            {
                ShowSKill();
            }
            else if (!_pc.OpenSkillTab)
            {
                HideSkill();
            }

            if (_pc.OpenSkillTab)
            {
                _pc.CanJump = false;
                _canUseUSkill = false;

                if(InputDeviceManager.Instance.CurrentControl == InputDeviceManager.ControlType.Gamepad)
                {
                    for (int i = 0; i < _skillNamesPAD.Length; i++)
                    {
                        for (int j = 0; j < _skillCooldownsPAD.Length; j++)
                        {
                            _skillNamesPAD[i].text = _skills[i].SkillName;
                            _skillCooldownsPAD[j].text = Mathf.Round(_skills[j].CoolDownTime).ToString();
                        }
                    }
                }
                else if(InputDeviceManager.Instance.CurrentControl == InputDeviceManager.ControlType.Keyboard)
                {
                    for (int i = 0; i < _skillNamesKEY.Length; i++)
                    {
                        for (int j = 0; j < _skillCooldownsKEY.Length; j++)
                        {
                            _skillNamesKEY[i].text = _skills[i].SkillName;
                            _skillCooldownsKEY[j].text = Mathf.Round(_skills[j].CoolDownTime).ToString();
                        }
                    }
                }

                if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Q))
                {
                    _skills[0].EnableSkill(_skills[0].SkillCost);
                    //Debug.Log("Input Called 1");
                }
                else if (Input.GetButtonDown("Cancel") || Input.GetKeyDown(KeyCode.E))
                {
                    _skills[1].EnableSkill(_skills[1].SkillCost);
                    //Debug.Log("Input Called 2");
                }
            }
            else if (!_openSkillTab)
            {
                _pc.CanJump = true;
                _canUseUSkill = true;
            }
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

    private void UpdateSkillUI()
    {
        if(InputDeviceManager.Instance.CurrentControl == InputDeviceManager.ControlType.Keyboard)
        {
            _skillIndicator = UIManager.Instance.KeySkillUI;
            _skillListIndicator = UIManager.Instance.KeySkillListUI;
        }
        else if(InputDeviceManager.Instance.CurrentControl == InputDeviceManager.ControlType.Gamepad)
        {
            _skillIndicator = UIManager.Instance.PadSkillUI;
            _skillListIndicator = UIManager.Instance.PadSkillListUI;
        }

    }
    public enum EmotionState
    {
        Awake,
        Sleep,
        CrashOut,
        Fatigue
    }
    #endregion

    #region Protected Methods 
    #endregion

    #region Coroutines
    #endregion
}
