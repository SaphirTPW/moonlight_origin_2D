using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDash : Passive
{
    #region Public Variables 
    private JoyEmotion _joyEmotion;
    [SerializeField] private float _sprintTime;
    [SerializeField] private float _maxSprintTime;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _defaultSpeed;

    private float _newAnimSpeed;
    private float _defaultAnimSpeed;

    [SerializeField] private bool _canAutoDash = false;
    [SerializeField] private AudioClip _autoDashSFX;
    #endregion

    #region Private Variables 
    #endregion

    #region Unity Methods 

    protected override void OnEnable()
    {
        base.OnEnable();
        Emotion.OnEmotionStateChanged += TurnOffAutoDash;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Emotion.OnEmotionStateChanged -= TurnOffAutoDash;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _joyEmotion = GetComponent<JoyEmotion>();
        _sprintSpeed = Pm.PlayerSpeed * 2;
        _defaultSpeed = Pm.PlayerSpeed;

        _defaultAnimSpeed = 1f;
        _newAnimSpeed = 2f;
    }

    private void Update()
    {
        LoadAutoDash();
    }
    #endregion

    #region Public Methods 

    public override void HandlePassiveOff()
    {
        if (!PC.IsMoving)
        {
            UpdatePassiveState(PassiveState.Off);
        }
    }

    public override void EnablePassive()
    {
        Pm.PlayerSpeed = _sprintSpeed;
        PC.PlayerAnim.speed = _newAnimSpeed;
        AudioManager.Instance.PlaySFX(_autoDashSFX);
    }

    public override void DisablePassive()
    {
        Pm.PlayerSpeed = _defaultSpeed;
        PC.PlayerAnim.speed = _defaultAnimSpeed;
    }

    public override void CheckCondition()
    {
        if(_joyEmotion.EmoState == Emotion.EmotionState.Awake)
        {
            _canAutoDash = true;
        }
    }

    public void LoadAutoDash()
    {
        if (_canAutoDash)
        {
            if (PC.IsMoving && Pm.PlayerGrounded)
            {
                _sprintTime += Time.deltaTime;
                if (_sprintTime >= _maxSprintTime)
                {
                    UpdatePassiveState(PassiveState.On);
                }
            }
            else if (!PC.IsMoving)
            {
                _sprintTime = 0;
                HandlePassiveOff();
            }
        }
    }

    public void TurnOffAutoDash(Emotion.EmotionState state)
    {
        if (_joyEmotion.EmoState != Emotion.EmotionState.Awake)
            _canAutoDash = false;
    }

    #endregion

    #region Private Methods 
    #endregion

    #region Coroutines
    #endregion
}
