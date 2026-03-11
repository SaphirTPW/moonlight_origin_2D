using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCounter : Passive
{
    #region Public Variables 

    #endregion

    #region Private Variables 
    private FearEmotion _fearEmotion;
    [SerializeField] private float _remainingDodges;
    [SerializeField] private float _maxDodges = 3f;
    [SerializeField] private Transform _dodgePoint;

    private bool _canShadowCounter;
    #endregion

    #region Unity Methods 

    protected override void OnEnable()
    {
        base.OnEnable();
        PlayerHealth.OnPlayerHit += HandlePlayerHit;
        Emotion.OnEmotionStateChanged += TurnOffShadowCounter;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PlayerHealth.OnPlayerHit -= HandlePlayerHit;
        Emotion.OnEmotionStateChanged -= TurnOffShadowCounter;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _fearEmotion = GetComponent<FearEmotion>();
    }

    #endregion

    #region Public Methods 

    public override void HandlePassiveOff()
    {
        UpdatePassiveState(PassiveState.Off);
    }

    public override void EnablePassive()
    {
        _remainingDodges = _maxDodges;
    }

    public override void DisablePassive()
    {
        _remainingDodges = 0;
    }

    public override void CheckCondition()
    {
        if (_fearEmotion.EmoState == Emotion.EmotionState.CrashOut)
        {
            _canShadowCounter = true;
            UpdatePassiveState(PassiveState.On);
        }
    }

    private bool HandlePlayerHit(float damage)
    {
        if (_canShadowCounter)
        {
            if (PassState == PassiveState.On && _remainingDodges > 0)
            {
                AutomaticDodge();

                _remainingDodges--;

                return true;
            }
        }
        return false;
    }

    private void AutomaticDodge()
    {
        transform.position = _dodgePoint.position;
    }

    public void TurnOffShadowCounter(Emotion.EmotionState state)
    {
        if (_fearEmotion.EmoState != Emotion.EmotionState.CrashOut)
        {
            _canShadowCounter = false;
            HandlePassiveOff();
        }
    }

    #endregion
}
