using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NervousPrecision : Passive
{
    #region Public Variables 
    #endregion

    #region Private Variables 
    private FearEmotion _fearEmotion;
    [SerializeField] private ParaShot _paraShot;
    [SerializeField] private float _upgradedValue;
    [SerializeField] private float _defaultValue;
    [SerializeField] private float _focusTime;
    [SerializeField] private float _maxFocusTime;

    [SerializeField] private bool _canNervShot = false;
    #endregion

    #region Unity Methods 

    protected override void OnEnable()
    {
        base.OnEnable();
        Emotion.OnEmotionStateChanged += TurnOffNervShot;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Emotion.OnEmotionStateChanged -= TurnOffNervShot;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _fearEmotion = GetComponent<FearEmotion>();
        _upgradedValue = _paraShot.ParaShotObj.GetComponent<ParaShotBullet>().StunValue + 5;
        _defaultValue = _paraShot.ParaShotObj.GetComponent<ParaShotBullet>().StunValue;
    }

    private void Update()
    {
        LoadNervShot();
    }
    #endregion

    #region Public Methods 

    public override void HandlePassiveOff()
    {
        UpdatePassiveState(PassiveState.Off);
    }

    public override void EnablePassive()
    {
        _paraShot.ParaShotObj.GetComponent<ParaShotBullet>().StunValue = _upgradedValue;
    }

    public override void DisablePassive()
    {
        _paraShot.ParaShotObj.GetComponent<ParaShotBullet>().StunValue = _defaultValue;
    }

    public override void CheckCondition()
    {
        if(_fearEmotion.EmoState == Emotion.EmotionState.Awake)
        {
            _canNervShot = true;
        }
    }

    public void LoadNervShot()
    {
        if (_canNervShot)
        {
            if (!PC.IsMoving)
            {
                _focusTime += Time.deltaTime;
                if (_focusTime >= _maxFocusTime)
                {
                    PassState = PassiveState.On;
                    _focusTime = _maxFocusTime;
                }
            }
            else if (PC.IsMoving)
            {
                _focusTime = 0;
                HandlePassiveOff();
            }
        }
    }

    private void TurnOffNervShot(Emotion.EmotionState pState)
    {
        if(_fearEmotion.EmoState != Emotion.EmotionState.Awake)
            _canNervShot = false;
    }
    #endregion

    #region Private Methods 
    #endregion

    #region Coroutines
    #endregion
}
