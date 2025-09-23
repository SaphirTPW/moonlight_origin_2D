using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParaShot : Skill
{
    #region Public Variables 
    #endregion

    #region Private Variables
    [SerializeField] private GameObject _paraShotObj;
    [SerializeField] private Transform _paraShotPoint;
    #endregion

    #region Unity Methods 
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
        HandleParaShot();
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
    private void HandleParaShot()
    {
        if (CurrentSkillState == SkillState.Ready)
        {
            Instantiate(_paraShotObj, _paraShotPoint.position, _paraShotPoint.rotation);
            CurrentSkillState = SkillState.CoolDown;
        }
    }
    #endregion

    #region Coroutines
    #endregion
}
