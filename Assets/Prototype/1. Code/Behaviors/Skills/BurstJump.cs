using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstJump : Skill
{
    #region Public Variables 
    [SerializeField] private float _jumpForce;
    #endregion

    #region Private Variables 
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
            HandleBurstJump();
    }

    public override void SkillOnCoolDown()
    {
        base.SkillOnCoolDown();
    }

    public override void SetSkillInfo()
    {
        base.SetSkillInfo();
    }

    private void HandleBurstJump()
    {
        PM.Rb.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
        CurrentSkillState = SkillState.CoolDown;
    }
    #endregion

    #region Private Methods 
    #endregion

    #region Coroutines
    #endregion
}
