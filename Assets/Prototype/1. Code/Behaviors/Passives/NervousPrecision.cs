using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NervousPrecision : Passive
{
    #region Public Variables 
    #endregion

    #region Private Variables 
    [SerializeField] private ParaShot _paraShot;
    [SerializeField] private float _upgradedValue;
    [SerializeField] private float _defaultValue;
    [SerializeField] private float _focusTime;
    [SerializeField] private float _maxFocusTime;
    #endregion

    #region Unity Methods 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _upgradedValue = _paraShot.ParaShotObj.GetComponent<ParaShotBullet>().StunValue + 5;
        _defaultValue = _paraShot.ParaShotObj.GetComponent<ParaShotBullet>().StunValue;
    }

    public override void Update()
    {
        base.Update();
    }
    #endregion

    #region Public Methods 
    public override void UpdatePassiveState(PassiveState pPassiveState)
    {
        base.UpdatePassiveState(pPassiveState);
    }
    public override void HandlePassiveOn()
    {
        base.HandlePassiveOn();
    }

    public override void HandlePassiveOff()
    {
        if (PC.IsMoving && PassState == PassiveState.On)
        {
            PassState = PassiveState.Off;
        }
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
        }
    }
    #endregion

    #region Private Methods 
    #endregion

    #region Coroutines
    #endregion
}
