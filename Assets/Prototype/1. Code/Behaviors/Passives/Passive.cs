using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive : MonoBehaviour
{
    #region Public Variables 
    public PassiveState PassState { get => _passiveState; set => _passiveState = value; }
    public PlayerController PC { get => _pc; set => _pc = value; }
    public PlayerMovement Pm { get => _pm; set => _pm = value; }
    #endregion

    #region Private Variables 
    [SerializeField] private PassiveData _passiveData;
    private PlayerHealth _pH;
    private PlayerMovement _pm;
    private PlayerCombat _pCom;
    private EmotionController _ec;
    private PlayerController _pc;

    [SerializeField] private PassiveState _passiveState;
    #endregion

    #region Unity Methods 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _pH = GetComponent<PlayerHealth>();
        _pm = GetComponent<PlayerMovement>();
        _pCom = GetComponent<PlayerCombat>();
        _ec = GetComponent<EmotionController>();
        _pc = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        HandlePassiveOn();
        HandlePassiveOff();
        UpdatePassiveState(_passiveState);
    }
    #endregion

    #region Public Methods 

    public virtual void UpdatePassiveState(PassiveState pPassiveState)
    {
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
    }
    public enum PassiveState
    {
        On,
        Off
    }

    
    public virtual void HandlePassiveOn()
    {
        if ((int)_passiveState == (int)_ec.CurrentActiveEmotion)
        {
            if (_passiveData.passiveType == PassiveData.PassiveType.Auto)
            {
                _passiveState = PassiveState.On;
            }
            else if (_passiveData.passiveType == PassiveData.PassiveType.Conditional)
            {
                CheckCondition();
            }
        }
        else
            return;
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
