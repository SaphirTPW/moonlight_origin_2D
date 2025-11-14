using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChargePunch : Skill
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
    [SerializeField] private float _damage;

    [SerializeField] private float _chargePunchTime;
    [SerializeField] private float _maxChargePunchTime;
    [SerializeField] private ParticleSystem _gatherFX;
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
        if (CurrentSkillState == SkillState.Ready)
        {
            HandleCharge();
        }
    }
    #endregion

    #region Public Methods 
    public override void EnableSkill(float pSkillCost)
    {
        if (!_isActive)
        {
            base.EnableSkill(pSkillCost);
            _isActive = true;
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

    #region Public Methods 
    #endregion

    #region Private Methods 

    private void HandleCharge()
    {
        if (_isActive)
        {
            _chargePunchTime += Time.deltaTime;
            if (_chargePunchTime != _maxChargePunchTime)
            {
                PC.CanMove = false;
                PM.Rb.linearVelocity = Vector2.zero;
                _gatherFX.Play();
            }

            if (_chargePunchTime >= _maxChargePunchTime)
            {
                _gatherFX.Stop();
                ChargePunchAttack(_damage);
                PC.PlayerAnim.SetTrigger("Attack");
                PC.CanMove = true;
                _chargePunchTime = 0;
                CurrentSkillState = SkillState.CoolDown;
            }
        }
    }
    private void ChargePunchAttack(float pDamage)
    {
        if (_isActive)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(pDamage * PCom.AttackMod);
                enemy.GetComponent<DummyEnemy>().Knockback(transform, _knockBackForce * 2, _knockBackUp);
                PlayerAttackRecoil(enemy.transform, _recoilForce);
                _isActive = false;
            }

            _isActive = false;
        }
    }

    private void PlayerAttackRecoil(Transform pTransform, float pRecoilForce)
    {
        Vector2 direction = (transform.position - pTransform.position).normalized;
        PM.Rb.linearVelocity = direction * pRecoilForce;
    }
    #endregion

    #region Coroutines
    #endregion
}
