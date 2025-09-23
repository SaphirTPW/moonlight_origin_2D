using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowStep : Skill
{
    #region Public Variables 
    #endregion

    #region Private Variables
    [SerializeField] private float _dashForce;
    [SerializeField] private float _maxDashTime;
    private float _dashTime;
    private Vector2 _savedVelocity;
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
    }
    #endregion

    #region Public Methods 
    public override void EnableSkill(float pSkillCost)
    {
        base.EnableSkill(pSkillCost);
        if (CurrentSkillState == SkillState.Ready)
        {
            HandleShadowStep();
        }
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
    private void HandleShadowStep()
    {
        _dashTime += Time.deltaTime * 3f;
        _savedVelocity = PM.Rb.linearVelocity;
        PC.CanJump = false;
        PM.Rb.linearVelocity = new Vector2(PM.Rb.linearVelocity.x * -_dashForce, PM.Rb.linearVelocity.y);
        CurrentSkillState = SkillState.CoolDown;

        if (_dashTime >= _maxDashTime)
        {
            PM.Rb.linearVelocity = _savedVelocity;
            PC.CanJump = false;
        }
    }
    #endregion

    #region Coroutines
    #endregion
}
