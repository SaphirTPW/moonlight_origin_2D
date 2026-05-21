using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class EmotionController : MonoBehaviour
{
    #region Public Variables 
    public EmotionControllerState EmoControllerState { get => _emoControllerState; set => _emoControllerState = value; }
    public ActiveEmotionState CurrentActiveEmotion { get => _currentActiveEmotion; set => _currentActiveEmotion = value; }
    public bool CanSwitch { get => canSwitch; set => canSwitch = value; }
    public bool CoolDownIsOn { get => _coolDownIsOn; set => _coolDownIsOn = value; }
    public Emotion[] Emotions { get => _emotions; set => _emotions = value; }
    public TMP_Text EmotionIndacatorText { get => _emotionIndacatorText; set => _emotionIndacatorText = value; }

    public Color joyColor;
    public Color angerColor;
    public Color sadnessColor;
    public Color fearColor;
    public Color neutralColor;
    public Color fatigueColor;
    #endregion

    #region Private Variables
    private PlayerController _pc;
    private PlayerMovement _pm;
    [SerializeField] private Emotion[] _emotions;
    [SerializeField] private bool[] _emotionIsActive;
    [SerializeField] private EmotionControllerState _emoControllerState;
    [SerializeField] private ActiveEmotionState _currentActiveEmotion;
    [SerializeField] private float _startControllerCooldownTime;
    [SerializeField] private float _currControllerCooldown;
    [SerializeField] private float _switchCooldown = 0.2f;
    private float _switchCooldownTimer;

    public bool canSwitch = false;
    [SerializeField] private bool _isFusing = false;

    [SerializeField] private bool _coolDownIsOn = false;
    [SerializeField] private bool _hasFused = false;

    private float _dPadH;
    private float _dPadV;
    private int _currentEmotionIndex = 0;
    private Coroutine neutralDelayCall = null;
    
    [SerializeField] private CinemachineImpulseSource _impulseSource;

    [SerializeField] private ParticleSystem _gatherFX;
    [SerializeField] private ParticleSystem _burstFX;
    [SerializeField] private ParticleSystem _defusionFX;
    [SerializeField] private ParticleSystem _emoShiftFX;
    private SpriteRenderer _playerSprite;

    [SerializeField] private AudioClip _loadFusionSFX;
    [SerializeField] private AudioClip _triggerFusionSFX;
    [SerializeField] private AudioClip _triggerDefusionSFX;
    [SerializeField] private AudioClip _switchFusionSFX;

    #endregion

    #region Debug Variables
    [SerializeField] private TMP_Text _joyValueText;
    [SerializeField] private TMP_Text _angerValueText;
    [SerializeField] private TMP_Text _sadnessValueText;
    [SerializeField] private TMP_Text _fearValueText;
    [SerializeField] private TMP_Text _emotionIndacatorText;

    #endregion

    #region Unity Methods 
    private void Awake()
    {
        SetEmotionController();
        _pc = GetComponent<PlayerController>();
        _pm = GetComponent<PlayerMovement>();
        //_impulseSource = GetComponent<CinemachineImpulseSource>();
        _playerSprite = GetComponent<SpriteRenderer>();
    }

    private void OnDestroy()
    {
        StopCoroutine(neutralDelayCall);
        neutralDelayCall = null;
    }

    void Start()
    {
        //EnableEmotion(_emotions[0], ActiveEmotionState.Neutral, neutralColor, null);
        //_emotionIndacatorText.text = ActiveEmotionState.Neutral.ToString();
        DelayNeutralCall();
    }

    // Update is called once per frame
    void Update()
    {
        EmotionSwitch();
        //SetDebugTextValue();
        ControllerCooldown();
        StartControllerCoolDown();

        if(_switchCooldownTimer > 0)
        {
            _switchCooldownTimer -= Time.deltaTime;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    #endregion

    #region Public Methods 

    public void EmotionSwitch()
    {
        if (_emoControllerState == EmotionControllerState.Ready)
            canSwitch = true;
        else if (_emoControllerState == EmotionControllerState.NotReady)
            canSwitch = false;

        if (canSwitch)
        {
            if (_isFusing)
                return;
            if(InputDeviceManager.Instance.CurrentControl == InputDeviceManager.ControlType.Gamepad)
            {
                _dPadH = Input.GetAxisRaw("DPAD-H");
                _dPadV = Input.GetAxisRaw("DPAD-V");

                if (_dPadV < 0 && _currentActiveEmotion != ActiveEmotionState.Joy)
                {
                    EnableEmotion(_emotions[1], ActiveEmotionState.Joy, joyColor, _emoShiftFX);
                    //_emotionIndacatorText.text = ActiveEmotionState.Joy.ToString();
                }
                else if (_dPadV > 0 && _currentActiveEmotion != ActiveEmotionState.Sadness)
                {
                    EnableEmotion(_emotions[3], ActiveEmotionState.Sadness, sadnessColor, _emoShiftFX);
                    //_emotionIndacatorText.text = ActiveEmotionState.Sadness.ToString();
                }
                else if (_dPadH < 0 && _currentActiveEmotion != ActiveEmotionState.Anger)
                {
                    EnableEmotion(_emotions[2], ActiveEmotionState.Anger, angerColor, _emoShiftFX);
                    //_emotionIndacatorText.text = ActiveEmotionState.Anger.ToString();
                }
                else if (_dPadH > 0 && _currentActiveEmotion != ActiveEmotionState.Fear)
                {
                    EnableEmotion(_emotions[4], ActiveEmotionState.Fear, fearColor, _emoShiftFX);
                    //_emotionIndacatorText.text = ActiveEmotionState.Fear.ToString();
                }
                else if (Input.GetButtonDown("Neutral"))
                {
                    EnableEmotion(_emotions[0], ActiveEmotionState.Neutral, neutralColor, null);
                    //_emotionIndacatorText.text = ActiveEmotionState.Neutral.ToString();
                }
            }
            else if (InputDeviceManager.Instance.CurrentControl == InputDeviceManager.ControlType.Keyboard)
            {
                float scroll = Input.mouseScrollDelta.y;

                if (scroll > 0f)
                {
                    ChangeEmotion(1);
                }
                else if (scroll < 0f)
                {
                    ChangeEmotion(-1);
                }
                else if (Input.GetMouseButtonDown(2))
                {
                    _currentEmotionIndex = 0;
                    ApplyEmotion(0);
                }
            }
        }
    }

    public void EnableEmotion(Emotion pEmotion, ActiveEmotionState pActiveEmoState, Color pColor, ParticleSystem pFX)
    {
        if (_switchCooldownTimer > 0)
            return;

        for (int i = 0; i < _emotions.Length; i++)
        {
            //_emotions[i].EmoState = Emotion.EmotionState.Sleep;
            _emotions[i].UpdateEmotionState(Emotion.EmotionState.Sleep);
            _emotionIsActive[i] = false;
            _emotions[i].IsCrashOutAvailable = false;
            InputDeviceManager.Instance.DisablePrompt();

            if (_emotions[i].Passive != null)
            {
                _emotions[i].Passive.PassState = Passive.PassiveState.Off;
            }
        }

        _currentActiveEmotion = pActiveEmoState;

        if(!_hasFused && _currentActiveEmotion != ActiveEmotionState.Neutral)
        {
            AnimaFusion(pEmotion, pColor);
        }

        if(_hasFused && _currentActiveEmotion != ActiveEmotionState.Neutral)
        {
            _switchCooldownTimer = _switchCooldown;
            pEmotion.UpdateEmotionState(Emotion.EmotionState.Awake);
            AudioManager.Instance.PlaySFX(_switchFusionSFX);

            if(pFX != null)
            {
                pFX.startColor = pColor;
                pFX.Play();
            }
        }

        if (_hasFused && _currentActiveEmotion == ActiveEmotionState.Neutral)
        {
            AnimaDefusion(pEmotion);
        }

        if (!_hasFused && _currentActiveEmotion == ActiveEmotionState.Neutral)
        {
            //Debug.Log("isNeutral");
            pEmotion.UpdateEmotionState(Emotion.EmotionState.Awake);
        }
    }

    public void ControllerCooldown()
    {
        if (_coolDownIsOn)
        {
            _currControllerCooldown -= Time.deltaTime;
            if (_currControllerCooldown <= 0)
                EndControllerCoolDown();
                
        }
    }

    public void StartControllerCoolDown()
    {
        if(_emoControllerState == EmotionControllerState.Cooldown)
            _coolDownIsOn = true;
    }

    public void EndControllerCoolDown()
    {
        _currControllerCooldown = _startControllerCooldownTime;
        _emoControllerState = EmotionControllerState.Ready;
        EnableEmotion(_emotions[0], ActiveEmotionState.Neutral, neutralColor, null);
        //_emotionIndacatorText.text = "Neutral";
        _playerSprite.color = neutralColor;
        _coolDownIsOn = false;
    }

    public void SetEmotionController()
    {
        _emoControllerState = EmotionControllerState.Ready;
        canSwitch = true;
        _coolDownIsOn = false;
        _currControllerCooldown = _startControllerCooldownTime;
    }

    public enum EmotionControllerState
    {
        NotReady,
        Ready,
        Cooldown
    }

    public enum ActiveEmotionState
    {
        Neutral,
        Joy,
        Anger,
        Sadness,
        Fear,
        Fatigue,
        Ecstasy,
        Rage,
        Grief,
        Terror
    }
    #endregion

    #region Private Methods 

    private void ChangeEmotion(int direction)
    {
        _currentEmotionIndex += direction;

        // loop entre 1 et 4 uniquement
        if (_currentEmotionIndex < 1)
            _currentEmotionIndex = 4;

        if (_currentEmotionIndex > 4)
            _currentEmotionIndex = 1;

        ApplyEmotion(_currentEmotionIndex);
    }

    private void ApplyEmotion(int index)
    {
        if ((int)_currentActiveEmotion == _currentEmotionIndex)
            return;

        switch (index)
        {
            case 0:
                EnableEmotion(_emotions[0], ActiveEmotionState.Neutral, neutralColor, null);
                break;
            case 1:
                EnableEmotion(_emotions[1], ActiveEmotionState.Joy, joyColor, _emoShiftFX);
                break;
            case 2:
                EnableEmotion(_emotions[2], ActiveEmotionState.Anger, angerColor, _emoShiftFX);
                break;
            case 3:
                EnableEmotion(_emotions[3], ActiveEmotionState.Sadness, sadnessColor, _emoShiftFX);
                break;
            case 4:
                EnableEmotion(_emotions[4], ActiveEmotionState.Fear, fearColor, _emoShiftFX);
                break;
        }

        //_emotionIndacatorText.text = ((ActiveEmotionState)index).ToString();
    }
    //private void SetDebugTextValue()
    //{
    //    _joyValueText.text = Mathf.Round(_emotions[1].CurrentEmotionEnergy).ToString();
    //    _angerValueText.text = Mathf.Round(_emotions[2].CurrentEmotionEnergy).ToString();
    //    _sadnessValueText.text = Mathf.Round(_emotions[3].CurrentEmotionEnergy).ToString();
    //    _fearValueText.text = Mathf.Round(_emotions[4].CurrentEmotionEnergy).ToString();
    //}

    private void AnimaFusion(Emotion pEmotion, Color pColor)
    {
        _isFusing = true;
        _gatherFX.startColor = pColor;
        _burstFX.startColor = pColor;
        _gatherFX.Play();
        AudioManager.Instance.PlaySFX(_loadFusionSFX);
        StartCoroutine(AnimaFusionCo(_gatherFX, _burstFX, pEmotion));
    }

    private void AnimaDefusion(Emotion pEmotion)
    {
        _defusionFX.Play();
        AudioManager.Instance.PlaySFX(_triggerDefusionSFX);
        StartCoroutine(AnimaDefusionCo(_defusionFX, pEmotion));
    }

    private void DelayNeutralCall()
    {
        if(neutralDelayCall != null)
        {
            return;
        }

        neutralDelayCall = StartCoroutine(SetNeutralState());
    }
    #endregion

    #region Coroutines
    private IEnumerator SetNeutralState()
    {
        yield return new WaitForSeconds(0.1f);
        //Debug.Log("Neutral");
        EnableEmotion(_emotions[0], ActiveEmotionState.Neutral, neutralColor, null);
    }

    private IEnumerator AnimaFusionCo(ParticleSystem pStartFX, ParticleSystem pEndFX, Emotion pEmotion)
    {
        _pc.CanMove = false;
        _pc.CanJump = false;
        _pc.InputDirection = Vector2.zero;
        _pm.Rb.linearVelocity = Vector2.zero;
        _pm.Rb.simulated = false;
        yield return new WaitUntil(() => !pStartFX.IsAlive());
        pEndFX.Play();
        CameraShakeManager.instance.CameraShake(_impulseSource);
        AudioManager.Instance.PlaySFX(_triggerFusionSFX);
        yield return new WaitUntil(() => !pEndFX.IsAlive());
        pEmotion.UpdateEmotionState(Emotion.EmotionState.Awake);
        _pc.CanMove = true;
        _pc.CanJump = true;
        _pm.Rb.simulated = true;
        _hasFused = true;
        _isFusing = false;
    }

    private IEnumerator AnimaDefusionCo(ParticleSystem pDefusionFX, Emotion pEmotion)
    {
        _pc.CanMove = false;
        _pc.CanJump = false;
        _pc.InputDirection = Vector2.zero;
        _pm.Rb.linearVelocity = Vector2.zero;
        _pm.Rb.simulated = false;
        yield return new WaitUntil(() => !pDefusionFX.IsAlive());
        _pc.CanMove = true;
        _pc.CanJump = true;
        pEmotion.UpdateEmotionState(Emotion.EmotionState.Awake);
        _pm.Rb.simulated = true;
        _hasFused = false;
    }
    #endregion
}
