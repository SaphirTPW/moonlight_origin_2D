using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCrush : Skill
{
    #region Public Variables 
    #endregion

    #region Private Variables 
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _knockBackForce = 50f;
    [SerializeField] private float _knockBackUp = 10f;
    [SerializeField] private float _recoilForce;
    [SerializeField] private float _dashForce;
    [SerializeField] private float _maxDashTime;
    [SerializeField] private float _damage;
    private float _dashTime;
    private Vector2 _savedVelocity;
    [SerializeField] private bool _isActive;
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
        DashCrushAttack(_damage);
    }
    #endregion

    #region Public Methods 
    public override void EnableSkill(float pSkillCost)
    {
        base.EnableSkill(pSkillCost);
        if (CurrentSkillState == SkillState.Ready)
        {
            HandleDashCrush();
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
    private void HandleDashCrush()
    {
        _dashTime += Time.deltaTime * 3f;
        _savedVelocity = PM.Rb.linearVelocity;
        PM.Rb.linearVelocity = new Vector2(PM.Rb.linearVelocity.x * _dashForce, PM.Rb.linearVelocity.y);
        _isActive = true;
        CurrentSkillState = SkillState.CoolDown;

        if (_dashTime >= _maxDashTime)
        {
            PM.Rb.linearVelocity = _savedVelocity;
        }
    }

    private void DashCrushAttack(float pDamage)
    {
        if (_isActive)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(pDamage * PCom.AttackMod);
                enemy.GetComponent<DummyEnemy>().Knockback(transform, _knockBackForce, _knockBackUp);
                PlayerAttackRecoil(enemy.transform, _recoilForce);
                _isActive = false;
            }
        }
    }

    private void PlayerAttackRecoil(Transform pTransform, float pRecoilForce)
    {
        Vector2 direction = (transform.position - pTransform.position).normalized;
        PM.Rb.linearVelocity = direction * pRecoilForce;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }
    #endregion

    #region Coroutines
    #endregion
}
