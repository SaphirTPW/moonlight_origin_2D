using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsolationClone : Skill
{
    #region Public Variables 
    #endregion

    #region Private Variables 
    [SerializeField] private float _recoilForce;
    [SerializeField] private GameObject _isolationClone;
    [SerializeField] private Transform _clonePosition;
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
        if (CurrentSkillState == SkillState.Ready)
        {
            //HandleIsolationClone();
        }
    }
    #endregion

    #region Public Methods 
    public override void EnableSkill(float pSkillCost)
    {
        base.EnableSkill(pSkillCost);
        HandleIsolationClone();
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
    private void HandleIsolationClone()
    {
        if(CurrentSkillState == SkillState.Ready)
        {
            Instantiate(_isolationClone, _clonePosition.position, _clonePosition.rotation);
            CurrentSkillState = SkillState.CoolDown;
        }
    }
    #endregion

    #region Coroutines
    #endregion
}
