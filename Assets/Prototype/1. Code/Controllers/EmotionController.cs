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
    public bool CanSwitch { get => _canSwitch; set => _canSwitch = value; }
    public bool CoolDownIsOn { get => _coolDownIsOn; set => _coolDownIsOn = value; }
    public Emotion[] Emotions { get => _emotions; set => _emotions = value; }
    public TMP_Text EmotionIndacatorText { get => _emotionIndacatorText; set => _emotionIndacatorText = value; }

    public Color joyColor;
    public Color angerColor;
    public Color sadnessColor;
    public Color fearColor;
    public Color neutralColor;
    #endregion

    #region Private Variables
    private PlayerController _pc;
    private PlayerMovement _pm;
    [SerializeField] private Emotion[] _emotions;
    [SerializeField] private EmotionControllerState _emoControllerState;
    [SerializeField] private ActiveEmotionState _currentActiveEmotion;
    [SerializeField] private float _startControllerCooldownTime;
    [SerializeField] private float _currControllerCooldown;
    public bool _canSwitch = false;
    [SerializeField] private bool _coolDownIsOn = false;
    [SerializeField] private bool _hasFused = false;
    private float _dPadH;
    private float _dPadV;
    private Coroutine neutralDelayCall = null;
    private CinemachineImpulseSource _impulseSource;

    [SerializeField] private ParticleSystem _gatherFX;
    [SerializeField] private ParticleSystem _burstFX;
    [SerializeField] private ParticleSystem _defusionFX;
    [SerializeField] private ParticleSystem _emoShiftFX;

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
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnDestroy()
    {
        StopCoroutine(neutralDelayCall);
        neutralDelayCall = null;
    }

    void Start()
    {
        EnableEmotion(_emotions[0], ActiveEmotionState.Neutral, neutralColor, null);
        DelayNeutralCall();
    }

    // Update is called once per frame
    void Update()
    {
        EmotionSwitch();
        SetDebugTextValue();
        ControllerCooldown();
        StartControllerCoolDown();
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
            _canSwitch = true;
        else if (_emoControllerState == EmotionControllerState.NotReady)
            _canSwitch = false;

        if (_canSwitch)
        {
            _dPadH = Input.GetAxisRaw("DPAD-H");
            _dPadV = Input.GetAxisRaw("DPAD-V");

            if (_dPadV < 0)
            {
                EnableEmotion(_emotions[1], ActiveEmotionState.Joy, joyColor, _emoShiftFX);
                _emotionIndacatorText.text = ActiveEmotionState.Joy.ToString();
            }
            else if (_dPadV > 0)
            {
                EnableEmotion(_emotions[3], ActiveEmotionState.Sadness, sadnessColor, _emoShiftFX);
                _emotionIndacatorText.text = ActiveEmotionState.Sadness.ToString();
            }
            else if (_dPadH < 0)
            {
                EnableEmotion(_emotions[2], ActiveEmotionState.Anger, angerColor, _emoShiftFX);
                _emotionIndacatorText.text = ActiveEmotionState.Anger.ToString();
            }
            else if (_dPadH > 0)
            {
                EnableEmotion(_emotions[4], ActiveEmotionState.Fear, fearColor, _emoShiftFX);
                _emotionIndacatorText.text = ActiveEmotionState.Fear.ToString();
            }
            else if (Input.GetButtonDown("Neutral"))
            {
                EnableEmotion(_emotions[0], ActiveEmotionState.Neutral, neutralColor, null);
                _emotionIndacatorText.text = ActiveEmotionState.Neutral.ToString();
            }
                
        }
    }

    public void EnableEmotion(Emotion pEmotion, ActiveEmotionState pActiveEmoState, Color pColor, ParticleSystem pFX)
    {
        for (int i = 0; i < _emotions.Length; i++)
        {
            _emotions[i].EmoState = Emotion.EmotionState.Sleep;
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
        else if(_hasFused && _currentActiveEmotion != ActiveEmotionState.Neutral)
        {
            pEmotion.EmoState = Emotion.EmotionState.Awake;
            pFX.startColor = pColor;
            pFX.Play();

        }
        else if(_hasFused && _currentActiveEmotion == ActiveEmotionState.Neutral)
        {
            AnimaDefusion(pEmotion);
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
        _coolDownIsOn = false;
    }

    public void SetEmotionController()
    {
        _emoControllerState = EmotionControllerState.Ready;
        _canSwitch = true;
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
        Ecstasy,
        Rage,
        Grief,
        Terror
    }
    #endregion

    #region Private Methods 
    private void SetDebugTextValue()
    {
        _joyValueText.text = Mathf.Round(_emotions[1].CurrentEmotionEnergy).ToString();
        _angerValueText.text = Mathf.Round(_emotions[2].CurrentEmotionEnergy).ToString();
        _sadnessValueText.text = Mathf.Round(_emotions[3].CurrentEmotionEnergy).ToString();
        _fearValueText.text = Mathf.Round(_emotions[4].CurrentEmotionEnergy).ToString();
    }

    private void AnimaFusion(Emotion pEmotion, Color pColor)
    {
        _gatherFX.startColor = pColor;
        _burstFX.startColor = pColor;
        _gatherFX.Play();
        StartCoroutine(AnimaFusionCo(_gatherFX, _burstFX, pEmotion));
    }

    private void AnimaDefusion(Emotion pEmotion)
    {
        _defusionFX.Play();
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
        EnableEmotion(_emotions[0], ActiveEmotionState.Neutral, neutralColor, null);
    }

    private IEnumerator AnimaFusionCo(ParticleSystem pStartFX, ParticleSystem pEndFX, Emotion pEmotion)
    {
        _pc.CanMove = false;
        _pc.CanJump = false;
        _pm.Rb.simulated = false;
        yield return new WaitUntil(() => !pStartFX.IsAlive());
        pEndFX.Play();
        CameraShakeManager.instance.CameraShake(_impulseSource);
        yield return new WaitUntil(() => !pEndFX.IsAlive());
        pEmotion.EmoState = Emotion.EmotionState.Awake;
        _pc.CanMove = true;
        _pc.CanJump = true;
        _pm.Rb.simulated = true;
        _hasFused = true;
    }

    private IEnumerator AnimaDefusionCo(ParticleSystem pDefusionFX, Emotion pEmotion)
    {
        _pc.CanMove = false;
        _pc.CanJump = false;
        _pm.Rb.linearVelocity = Vector2.zero;
        yield return new WaitUntil(() => !pDefusionFX.IsAlive());
        _pc.CanMove = true;
        _pc.CanJump = true;
        pEmotion.EmoState = Emotion.EmotionState.Awake;
        _hasFused = false;
    }
    #endregion
}
