using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDash : Passive
{
    #region Public Variables 
    [SerializeField] private float _sprintTime;
    [SerializeField] private float _maxSprintTime;
    private float _sprintSpeed;
    private float _defaultSpeed;

    private float _newAnimSpeed;
    private float _defaultAnimSpeed;
    #endregion

    #region Private Variables 
    #endregion

    #region Unity Methods 
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _sprintSpeed = Pm.PlayerSpeed * 2;
        _defaultSpeed = Pm.PlayerSpeed;

        _defaultAnimSpeed = 1f;
        _newAnimSpeed = 2f;
    }

    // Update is called once per frame
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
        if (!PC.IsMoving && PassState == PassiveState.On)
        {
            PassState = PassiveState.Off;
        }
    }

    public override void EnablePassive()
    {
        Pm.PlayerSpeed = _sprintSpeed;
        PC.PlayerAnim.speed = _newAnimSpeed;
    }

    public override void DisablePassive()
    {
        Pm.PlayerSpeed = _defaultSpeed;
        PC.PlayerAnim.speed = _defaultAnimSpeed;
    }

    public override void CheckCondition()
    {
        if (PC.IsMoving)
        {
            _sprintTime += Time.deltaTime;
            if(_sprintTime >= _maxSprintTime)
            {
                PassState = PassiveState.On;
            }
        }
        else if (!PC.IsMoving)
        {
            _sprintTime = 0;
        }
    }
    #endregion

    #region Private Methods 
    #endregion

    #region Coroutines
    #endregion
}
