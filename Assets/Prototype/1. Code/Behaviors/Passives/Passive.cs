using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Passive : MonoBehaviour
{
    #region Public Variables 
    public PassiveState PassState { get => _passiveState; set => _passiveState = value; }
    public PlayerController PC { get => _pc; set => _pc = value; }
    public PlayerMovement Pm { get => _pm; set => _pm = value; }
    public PlayerCombat PCom { get => _pCom; set => _pCom = value; }
    public PlayerHealth PH { get => _pH; set => _pH = value; }
    #endregion

    #region Private Variables 
    [SerializeField] private PassiveData _passiveData;
    private PlayerHealth _pH;
    private PlayerMovement _pm;
    private PlayerCombat _pCom;
    private EmotionController _ec;
    private PlayerController _pc;

    [SerializeField] private PassiveState _passiveState;
    [SerializeField] private PassiveMode _passiveMode;

    public static event Action<PassiveState> OnPassiveStateChanged;
    #endregion

    #region Unity Methods 

    protected virtual void OnEnable()
    {
        Emotion.OnEmotionStateChanged += HandlePassiveOn;
    }

    protected virtual void OnDisable()
    {
        Emotion.OnEmotionStateChanged -= HandlePassiveOn;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _pH = GetComponent<PlayerHealth>();
        _pm = GetComponent<PlayerMovement>();
        _pCom = GetComponent<PlayerCombat>();
        _ec = GetComponent<EmotionController>();
        _pc = GetComponent<PlayerController>();
    }
    #endregion

    #region Public Methods 

    public void UpdatePassiveState(PassiveState pPassiveState)
    {
        if (_passiveState == pPassiveState)
        {
            return;
        }
        _passiveState = pPassiveState;

        switch (pPassiveState)
        {
            case PassiveState.On:
                EnablePassive();
                break;
            case PassiveState.Off:
                DisablePassive();
                break;
            default:
                break;
        }
        OnPassiveStateChanged?.Invoke(pPassiveState);
    }

    public enum PassiveState
    {
        On,
        Off
    }

    public enum PassiveMode
    {
        Normal,
        CrashOut
    }

    public void HandlePassiveOn(Emotion.EmotionState pEmotionState)
    {
        if ((int)_passiveData.passiveEmoType != (int)_ec.CurrentActiveEmotion)
        {
            UpdatePassiveState(PassiveState.Off);
            return;
        }

        bool shouldBeActive = false;

        if (pEmotionState == Emotion.EmotionState.Awake && _passiveMode == PassiveMode.Normal)
        {
            shouldBeActive = true;
        }
        else if (pEmotionState == Emotion.EmotionState.CrashOut && _passiveMode == PassiveMode.CrashOut)
        {
            shouldBeActive = true;
        }

        if (!shouldBeActive)
        {
            UpdatePassiveState(PassiveState.Off);
            return;
        }

        // Activation
        if (_passiveData.passiveType == PassiveData.PassiveType.Auto)
        {
            UpdatePassiveState(PassiveState.On);
        }
        else if (_passiveData.passiveType == PassiveData.PassiveType.Conditional)
        {
            CheckCondition();
        }

        Debug.Log($"{name} - EmotionState reçu : p{pEmotionState}");
    }

    public virtual void HandlePassiveOff()
    {
        
    }

    public virtual void EnablePassive()
    {

    }

    public virtual void DisablePassive()
    {

    }

    public virtual void CheckCondition()
    {

    }
    #endregion

    #region Private Methods 
    #endregion

    #region Coroutines
    #endregion
}
