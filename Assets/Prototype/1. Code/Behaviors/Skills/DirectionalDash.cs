using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalDash : Skill
{
    #region Public Variables 
    #endregion

    #region Private Variables 
    [SerializeField] private float _dashForce;
    private Vector2 _savedVelocity;
    private Vector2 _inputDirection;
    [SerializeField] private ParticleSystem _dashFX;
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
            //Handle Directional Dash 
            HandleDirectionalDash();
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
    private void HandleDirectionalDash()
    {
        //_inputDirection.x = Input.GetAxis("Horizontal");
        //_inputDirection.y = Input.GetAxis("Vertical");

        PM.Rb.linearVelocity = new Vector2(PC.InputDirection.x, PC.InputDirection.y / 2) * _dashForce;
        CurrentSkillState = SkillState.CoolDown;
    }
    #endregion

    #region Coroutines
    #endregion
}
