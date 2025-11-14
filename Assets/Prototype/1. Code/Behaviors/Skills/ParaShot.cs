using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParaShot : Skill
{
    #region Public Variables 
    public GameObject ParaShotObj { get => _paraShotObj; set => _paraShotObj = value; }
    #endregion

    #region Private Variables
    [SerializeField] private GameObject _paraShotObj;
    [SerializeField] private Transform _paraShotPoint;
    private float _dir;
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
            var _paraBullet = _paraShotObj.GetComponent<ParaShotBullet>();
            float dir = Mathf.Sign(transform.localScale.x);
            _paraBullet.SetDirection(new Vector2(dir, 0));

            var bullet = Instantiate(_paraShotObj, _paraShotPoint.position, Quaternion.identity);
            bullet.transform.localScale = Vector3.one;
            CurrentSkillState = SkillState.CoolDown;
        }
    }

    #endregion

    #region Coroutines
    #endregion
}
