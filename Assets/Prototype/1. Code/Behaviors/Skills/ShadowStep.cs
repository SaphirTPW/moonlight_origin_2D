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
    [SerializeField] private float _dashTime;
    private Vector2 _savedVelocity;
    public bool _isActive;
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
        HandleReturnToNormal();
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
        PC.CanJump = false;
        PM.Rb.linearVelocity = new Vector2(PM.Rb.linearVelocity.x * -_dashForce, PM.Rb.linearVelocity.y);
        PM.Rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        PM.PlayerCollider.enabled = false;
        _isActive = true;
    }

    private void HandleReturnToNormal()
    {
        if (_isActive)
        {
            _dashTime += Time.deltaTime * 3f;
            _savedVelocity = PM.Rb.linearVelocity;
        }

        if (_dashTime >= _maxDashTime)
        {
            Debug.Log("Dash Done");
            _isActive = false;
            PC.CanJump = true;
            PM.Rb.constraints = RigidbodyConstraints2D.None;
            PM.Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            PM.PlayerCollider.enabled = true;
            CurrentSkillState = SkillState.CoolDown;
            PM.Rb.linearVelocity = _savedVelocity;
            _dashTime = 0;
        }

    }
    #endregion

    #region Coroutines
    #endregion
}
