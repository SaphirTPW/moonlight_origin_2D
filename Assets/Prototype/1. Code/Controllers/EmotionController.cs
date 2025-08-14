using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmotionController : MonoBehaviour
{
    #region Public Variables 
    public EmotionControllerState EmoControllerState { get => _emoControllerState; set => _emoControllerState = value; }
    public ActiveEmotionState CurrentActiveEmotion { get => _currentActiveEmotion; set => _currentActiveEmotion = value; }
    public bool CanSwitch { get => _canSwitch; set => _canSwitch = value; }
    public bool CoolDownIsOn { get => _coolDownIsOn; set => _coolDownIsOn = value; }
    public Emotion[] Emotions { get => _emotions; set => _emotions = value; }
    #endregion

    #region Private Variables 
    [SerializeField] private Emotion[] _emotions;
    [SerializeField] private EmotionControllerState _emoControllerState;
    [SerializeField] private ActiveEmotionState _currentActiveEmotion;
    [SerializeField] private float _startControllerCooldownTime;
    [SerializeField] private float _currControllerCooldown;
    public bool _canSwitch = false;
    [SerializeField] private bool _coolDownIsOn = false;
    private float _dPadH;
    private float _dPadV;
    private Coroutine neutralDelayCall = null;
    #endregion

    #region Debug Variables
    [SerializeField] private TMP_Text _joyValueText;
    [SerializeField] private TMP_Text _angerValueText;
    [SerializeField] private TMP_Text _sadnessValueText;
    [SerializeField] private TMP_Text _fearValueText;
    #endregion

    #region Unity Methods 
    private void Awake()
    {
        SetEmotionController();
    }

    private void OnDestroy()
    {
        StopCoroutine(neutralDelayCall);
        neutralDelayCall = null;
    }

    void Start()
    {
        EnableEmotion(_emotions[0], ActiveEmotionState.Neutral);
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
                EnableEmotion(_emotions[1], ActiveEmotionState.Joy);
            else if (_dPadV > 0)
                EnableEmotion(_emotions[3], ActiveEmotionState.Sadness);
            else if (_dPadH < 0)
                EnableEmotion(_emotions[2], ActiveEmotionState.Anger);
            else if (_dPadH > 0)
                EnableEmotion(_emotions[4], ActiveEmotionState.Fear);
            else if (Input.GetButtonDown("Neutral"))
                EnableEmotion(_emotions[0], ActiveEmotionState.Neutral);
                
        }
    }

    public void EnableEmotion(Emotion pEmotion, ActiveEmotionState pActiveEmoState)
    {
        for (int i = 0; i < _emotions.Length; i++)
        {
            _emotions[i].EmoState = Emotion.EmotionState.Sleep;
        }

        pEmotion.EmoState = Emotion.EmotionState.Awake;
        _currentActiveEmotion = pActiveEmoState;
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
        EnableEmotion(_emotions[0], ActiveEmotionState.Neutral);
    }
    #endregion
}
