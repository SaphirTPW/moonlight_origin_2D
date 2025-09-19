using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsolationSheild : Skill
{
    #region Public Variables 
    #endregion

    #region Private Variables 
    [SerializeField] private float _maxSheildTime;
    private float _sheildTime;
    private bool _sheildisActive;
    [SerializeField] private GameObject _isolationSheildObj;
    #endregion

    #region Unity Methods 
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (CurrentSkillState == SkillState.Ready)
        {
            HandleActiveShield();
        }
    }
    #endregion

    #region Public Methods 
    public override void EnableSkill(float pSkillCost)
    {
        base.EnableSkill(pSkillCost);
        _sheildisActive = true;
    }

    public override void SkillOnCoolDown()
    {
        base.SkillOnCoolDown();
    }

    public override void SetSkillInfo()
    {
        base.SetSkillInfo();
    }
    #endregion

    #region Private Methods 
    private void HandleActiveShield()
    {
        if (_sheildisActive)
        {
            _sheildTime += Time.deltaTime;

            if(_sheildTime != _maxSheildTime)
            {
                PC.CanMove = false;
                PC.CanJump = false;   
                _isolationSheildObj.SetActive(true);
                PM.Rb.linearVelocity = Vector2.zero;
            }

            if(_sheildTime >= _maxSheildTime)
            {
                PC.CanJump = true;
                PC.CanMove = true;
                _sheildTime = 0;
                _isolationSheildObj.SetActive(false);
                _sheildisActive = false;
                CurrentSkillState = SkillState.CoolDown;
            }
        }
    }
    #endregion

    #region Coroutines
    #endregion
}
